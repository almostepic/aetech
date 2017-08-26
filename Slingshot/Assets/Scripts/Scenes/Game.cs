using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using Cache;

public class Game : MonoBehaviour {

	static public Game  Instance;
	public GameObject Ball;
	public GameObject Target;
	public GameObject SpawnQuad;

	public Text InputText;
	public Text HighestScoreText;
	public Text CurrentScoreText;

	private enum InputType {
		Flick = 0,
		Sling = 1,
		NUM_TYPES
	}
	private enum GameState {
		ReadyForInput = 0,
		WaitingForCollision
	};

	private int currentScore;
	private int highestScore;

	// ball properties
	private Rigidbody ballRigid;
	private Vector3 spawnPoint;
	private Vector3 spawnRotation;

	// input properties
	private Vector3 startTouch;
	private Vector3 endTouch;
	private bool isTouching = false;
	private GameState state = GameState.ReadyForInput;
	private InputType inputType = InputType.Sling;


	void Awake() {
		ballRigid = Ball.GetComponent<Rigidbody> ();

		// temp hack until event system
		Instance = this;

		// init singletons
		Tuning.Initialize ();
		Tuning.Instance.LoadTuningFromDisk ();

		SimpleJSON.JSONNode tuning = Tuning.Instance.GetJSON ("game");
		// Cache starting position/rotation 
		spawnPoint = new Vector3 (tuning ["BallSpawn"] [0].AsFloat,
			tuning ["BallSpawn"] [1].AsFloat, 
			tuning ["BallSpawn"] [2].AsFloat);
		spawnRotation = new Vector3 (tuning ["BallRotation"] [0].AsFloat,
			tuning ["BallRotation"] [1].AsFloat,
			tuning ["BallRotation"] [2].AsFloat);
	}
	// Use this for initialization
	void Start () {
		// seed random
		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		UnityEngine.Random.InitState((int)(System.DateTime.UtcNow - epochStart).TotalSeconds);

		state = GameState.ReadyForInput;
		ResetBall ();
		UpdateText ();
		MoveTarget (true);
	}

	// Update is called once per frame
	void Update () {

		switch (state)
		{
			case GameState.ReadyForInput:
				CheckInput ();
				break;
		};

		CheckInputType ();
	}

	void CheckFlick() {
		if (Input.GetMouseButton (0) && !isTouching) {
			isTouching = true;
			startTouch = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp (0) && isTouching) {
			JSONNode tuning = Tuning.Instance.GetJSON ("game");
			isTouching = false;
			endTouch = Input.mousePosition;

			Vector3 vec = endTouch - startTouch;
			Vector3 dir = vec.normalized;
			float len = vec.magnitude; // distance in pixels

			if (dir.y <= 0.0f || len == 0.0f)
				return;

			float screenHeightPct = len / Screen.height;
			print (screenHeightPct);
			if (screenHeightPct <= tuning ["MinFlickScreenPct"].AsFloat)
				return;
			
			screenHeightPct = Mathf.Min (screenHeightPct, tuning ["MaxFlickScreenPct"].AsFloat);
			float power = Mathf.Max(tuning ["MaxFlickPower"].AsFloat * (screenHeightPct / tuning ["MaxFlickScreenPct"].AsFloat), tuning["MinFlickPower"].AsFloat);
			ballRigid.useGravity = true;

			//print (power);
			dir.z = dir.y;
			dir.y *= power * tuning["FlickYBias"].AsFloat;
			dir.x *= power;
			ballRigid.AddRelativeForce (dir + Vector3.forward * power);

			state = GameState.WaitingForCollision;
		}
	}

	void CheckSling() {
		if (Input.GetMouseButton (0) && !isTouching)
		{
			isTouching = true;
			startTouch = Input.mousePosition;
		} 

		if (isTouching)
		{
			JSONNode tuning = Tuning.Instance.GetJSON("game");
			Vector3 vec = Input.mousePosition - startTouch;
			Vector3 dir = vec.normalized;
			float yDiff = startTouch.y - Input.mousePosition.y;
			float xDiff = startTouch.x - Input.mousePosition.x;
			if (dir.y > 0.0f)
				yDiff = 0.0f;


			float yScreenPct = Mathf.Clamp (Mathf.Abs (yDiff) / Screen.height, 0.0f, tuning ["SlingMaxYScreenPct"].AsFloat);
			float xScreenPct = Mathf.Clamp (Mathf.Abs (xDiff) / Screen.width, 0.0f, tuning ["SlingMaxXScreenPct"].AsFloat);

			float yPct = yScreenPct / tuning ["SlingMaxYScreenPct"].AsFloat;
			float zMoveBy = (tuning ["SlingMaxPullPosZ"].AsFloat - spawnPoint.z) * yPct;

			float xPct = xScreenPct / tuning ["SlingMaxXScreenPct"].AsFloat;
			float yRotateBy = (tuning ["SlingMaxRotateY"].AsFloat * xPct);
			if (xDiff < 0.0f)
				yRotateBy *= -1.0f;

			// reset position
			Ball.transform.position = spawnPoint;
			Ball.transform.localRotation = Quaternion.Euler (spawnRotation.x, yRotateBy, 0.0f);

			Ball.transform.Translate (Vector3.forward * zMoveBy);

			if (Input.GetMouseButtonUp (0))
			{
				isTouching = false;
				if (yDiff > 0.0f)
				{
					// launch that bitch
					state = GameState.WaitingForCollision;
					ballRigid.useGravity = true;
					ballRigid.AddRelativeForce (Vector3.forward * Mathf.Max (tuning ["SlingMaxPower"].AsFloat * yPct, tuning ["SlingMinPower"].AsFloat));
				} else
				{
					ResetBall ();
				}
			}
		}
	}

	void CheckInput() {
		switch (inputType)
		{
			case InputType.Flick:
				CheckFlick ();
				break;
			case InputType.Sling:
				CheckSling ();
				break;
		}
	}

	/// <summary>
	/// Input checks from the user for input type change
	/// </summary>
	void CheckInputType() {
		if (Input.GetKeyUp ("left") ||  Input.GetKeyUp("right")) {
			inputType = inputType == InputType.Flick ? InputType.Sling : InputType.Flick;
			ResetInputData ();
			UpdateText ();
		}
	}

	/// <summary>
	/// Updates input type text field
	/// </summary>
	void UpdateText() {
		if(inputType == InputType.Flick)
			InputText.text = "Input Type (left/right): Flick";
		else
			InputText.text = "Input Type (left/right): Sling";

		HighestScoreText.text = "Highest: " + highestScore;
		CurrentScoreText.text = "Current: " + currentScore;
	}

	/// <summary>
	/// Resets the input data when the user selects a different input type.
	/// </summary>
	void ResetInputData() {
		startTouch = Vector3.zero;
		endTouch = Vector3.zero;
		isTouching = false;
	}

	/// <summary>
	/// Resets ball position, rotation, and physics properties to default
	/// </summary>
	void ResetBall()  {
		ballRigid.useGravity = false;
		ballRigid.velocity = Vector3.zero;
		ballRigid.angularVelocity = Vector3.zero;
		Ball.transform.position = spawnPoint;
		Ball.transform.localRotation = Quaternion.Euler (spawnRotation);
	}

	/// <summary>
	/// Moves the target to a random position on the spawn quad
	/// </summary>
	/// <param name="origin">If set to <c>true</c> origin.</param>
	public void MoveTarget(bool origin) {
		if (origin)
		{
			// spawn in center of the quad
			// assume spawn quad position is the center of the spawn quad??
			Target.transform.position = SpawnQuad.transform.position;
		} 
		else
		{
			Vector3 pos = SpawnQuad.transform.position;
			Vector3 halfScale = SpawnQuad.transform.localScale * 0.5f;
			Target.transform.position = new Vector3(
				UnityEngine.Random.Range (pos.x - halfScale.x, pos.x + halfScale.x),
				UnityEngine.Random.Range (pos.y - halfScale.y, pos.y + halfScale.y),
				pos.z);
		}
	}

	/// <summary>
	/// Event when ball collides with something
	/// </summary>
	/// <param name="col">Col.</param>
	public void OnBallCollision (Collision col) {
		if (col.gameObject.name == "Wall")
		{
			highestScore = Mathf.Max (currentScore, highestScore);
			currentScore = 0;
			MoveTarget (true);
		} 
		else if (col.gameObject.name == "Target")
		{
			currentScore++;
			MoveTarget (false);
		}

		state = GameState.ReadyForInput;
		UpdateText ();
		ResetBall ();
		ResetInputData ();
	}
}
