using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour {
	public GameObject puckObject;
	public GameObject targetObject;
	float direction = 1.0f;
	float linePosition = 0;
	float speed = 0;
	float speedIncrease = 0.0001f;
	bool prevCollision = false;
	bool curCollision = false;

	static float LINE_EIGHTH_LENGTH = 2.4f;
	static float LINE_QUARTER_LENGTH = 4.8f;
	static float LINE_LENGTH = LINE_QUARTER_LENGTH * 4;
	static float MIN_SPACING = 1.0f;

	enum Lines { Top, Right, Bottom, Left };

	void Start () {
		Reset();
	}

	void Reset () {
		linePosition = 0;
		direction = 1.0f;
		speed = 0.01f;
		speedIncrease = 0.0001f;

		targetObject.transform.position = new Vector3(2.4f, 2.4f, 0.0f);
	}
	
	void Update () {
		if (Input.GetKeyDown("space")) {
			OnAction();
		} else {
			prevCollision = curCollision;
			curCollision = CheckCollision();
			if (prevCollision && !curCollision) {
				Reset();
			}
		}

		linePosition += direction * speed;

		if (linePosition < 0) {
			linePosition += LINE_LENGTH;
		} else if (linePosition > LINE_LENGTH) {
			linePosition -= LINE_LENGTH;
		}

		SetPuckTransformPosition();
		
		speed += speedIncrease;
	}

	void SetPuckTransformPosition () {
		puckObject.transform.position = ConvertLinePositionToPosition(linePosition);
	}

	void SetNewTargetPosition() {
		// Pick a new line position with padding so the new target position isn't
		// instantly overlapping the puck
		float linePos = Random.Range(
			linePosition + MIN_SPACING,
			linePosition + LINE_LENGTH - MIN_SPACING
		);
		targetObject.transform.position = ConvertLinePositionToPosition(linePos);
	}

	Vector3 ConvertLinePositionToPosition (float linePos) {
		// Normalize
		if (linePos < 0) {
			linePos += LINE_LENGTH;
		} else if (linePos > LINE_LENGTH) {
			linePos -= LINE_LENGTH;
		}

		// Convert the line position into Top, Right, Bottom, or Left
		Lines line = (Lines)(linePos / LINE_QUARTER_LENGTH);

		Vector3 newPosition = new Vector3(0, 0, 0);

		switch (line) {
			case Lines.Top:
				newPosition.x = -LINE_EIGHTH_LENGTH + linePos;
				newPosition.y = LINE_EIGHTH_LENGTH;
				break;
			case Lines.Right:
				newPosition.x = LINE_EIGHTH_LENGTH;
				newPosition.y = LINE_EIGHTH_LENGTH - (linePos - LINE_QUARTER_LENGTH);
				break;
			case Lines.Bottom:
				newPosition.x = LINE_EIGHTH_LENGTH - (linePos - (2 * LINE_QUARTER_LENGTH));
				newPosition.y = -LINE_EIGHTH_LENGTH;
				break;
			case Lines.Left:
				newPosition.x = -LINE_EIGHTH_LENGTH;
				newPosition.y = -LINE_EIGHTH_LENGTH + (linePos - (3 * LINE_QUARTER_LENGTH));
				break;
		}

		return newPosition;
	}

	bool CheckCollision () {
		return puckObject.GetComponent<BoxCollider2D>().bounds
			.Intersects(targetObject.GetComponent<BoxCollider2D>().bounds);
	}

	void OnAction () {
		if (CheckCollision()) {
			direction *= -1.0f;

			SetNewTargetPosition();

			prevCollision = false;
			curCollision = false;
		} else {
			Reset();
		}
	}
}
