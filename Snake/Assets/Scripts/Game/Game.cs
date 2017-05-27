using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Singletons;

namespace Snake
{
	
	public class Game : MonoBehaviour {

		TileMap mTiles = new TileMap();

		// Use this for initialization
		void Start () {
			LoadLevel ();
		}

		// Update is called once per frame
		void Update () {
			
		}

		void LoadLevel() {
			int playerLevel = PlayerStats.Instance.level;

			JSONNode levelTuning = Tuning.Instance.Get ("levels");

			int levelCount = levelTuning ["Levels"].Count;
			Debug.Assert (playerLevel <= levelCount);

			mTiles.InitMap (levelTuning ["Levels"] [playerLevel]);
		}
	}
}
