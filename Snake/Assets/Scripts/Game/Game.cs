using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Singletons;

namespace Snake
{
	
	public class Game : MonoBehaviour {

		Snake mSnake = new Snake ();
		Tile[] mTiles = null;
		int mLevelHeight = 0;
		int mLevelWidth = 0;

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

			mLevelHeight = levelTuning ["Levels"] [playerLevel] ["W"].AsInt;
			mLevelWidth = levelTuning ["Levels"] [playerLevel] ["H"].AsInt;

			mTiles = Tile.CreateMap (mLevelWidth, mLevelHeight);

			mSnake.SpawnSnake (mTiles, 0, 0);
		}
	}
}
