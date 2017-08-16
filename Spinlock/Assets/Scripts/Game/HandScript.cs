using System;
using UnityEngine;

public class HandScript : MonoBehaviour
{
	public bool isColliding = false;
	void Start () 
	{
	}

	void OnTriggerEnter(Collider other)
	{
		isColliding = true;
	}

	void OnTriggerExit(Collider other)
	{
		isColliding = false;
	}
}

