using System;
using UnityEngine;

public class HandScript : MonoBehaviour
{
	public bool isColliding = false;
	void Start () 
	{
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		isColliding = true;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		isColliding = false;
	}
}

