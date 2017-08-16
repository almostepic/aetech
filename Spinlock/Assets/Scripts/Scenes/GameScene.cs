using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour {
	public GameObject lockObject;
	public GameObject handObject;
	public GameObject targetObject;
	Vector3 speed;
	float speedIncrease = 0.0f;

	bool prevCollision = false;
	bool curCollision = false;
	// Use this for initialization
	void Start () 
	{
		SimpleJSON.JSONNode gameTuning = Cache.Tuning.Instance.GetJSON ("Game");
		speed = new Vector3 (0, 0, gameTuning["Speed"].AsFloat);
		speedIncrease = gameTuning["SpeedIncrease"].AsFloat;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			OnClick ();
		} 
		else
		{
			prevCollision = curCollision;
			curCollision = CheckCollision ();
			if (prevCollision && !curCollision)
			{
				// we lose
				SceneManager.LoadScene ("LossScene");
			}
		}

		handObject.transform.Rotate (speed * Time.deltaTime);
	}

	void SetNewTargetPosition()
	{
		SimpleJSON.JSONNode gameTuning = Cache.Tuning.Instance.GetJSON ("Game");
		float currentRotation = handObject.transform.eulerAngles.z + 180.0f; // from +180 to -180
		float newRotation = (Random.Range (gameTuning["RotationMin"].AsInt, gameTuning["RotationMax"].AsInt) * (speed.z < 0 ? 1.0f : -1.0f)) + currentRotation;

		// wrap it around 0-360
		if (newRotation > 360.0f)
		{
			newRotation = ((int)newRotation % 360);
		} 
		else if(newRotation < 0.0f)
		{
			newRotation = 360.0f + newRotation;
		}

		newRotation -= 180.0f;
		targetObject.transform.rotation = Quaternion.Euler (Vector3.forward * newRotation);
	}

	bool CheckCollision()
	{
		return handObject.GetComponent<HandScript>().isColliding;
	}

	void OnClick()
	{
		if (CheckCollision ())
		{
			SetNewTargetPosition ();

			if (speed.z < 0)
				speed.z -= speedIncrease;
			else
				speed.z += speedIncrease;
			// flip the direction
			speed *= -1.0f;

			prevCollision = false;
			curCollision = false;
			handObject.GetComponent<HandScript> ().isColliding = false;
		} else
		{
			// we lose, clicked too early
			SceneManager.LoadScene ("LossScene");
		}
	}
}
