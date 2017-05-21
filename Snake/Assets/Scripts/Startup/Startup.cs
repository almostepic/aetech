using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Init singletons
		PlayerStats.Initialize();
		PlayerStats.Instance.LoadStats ();

		// transition to the next scene
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
