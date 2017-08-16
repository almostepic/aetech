using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cache;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class StartupScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Tuning.Initialize ();
		Tuning.Instance.LoadTuningFromDisk ();

		SceneManager.LoadScene ("MainMenuScene");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
