using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cache;
using SimpleJSON;

public class StartupScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Tuning.Initialize ();
		Tuning.Instance.LoadTuningFromDisk ();

		JSONNode gameTuning = Tuning.Instance.GetJSON ("game");

		Debug.Log ("Tuning value: " + gameTuning ["Test"].AsInt);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
