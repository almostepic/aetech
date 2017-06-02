using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;
using Singletons;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Init singletons
		Tuning.Initialize ();
		Tuning.Instance.LoadTuningFromDisk (); // should come from the web
		PlayerStats.Initialize();
		PlayerStats.Instance.LoadStats ();

		// transition to the next scene
		SceneManager.LoadScene("Game", LoadSceneMode.Single);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
