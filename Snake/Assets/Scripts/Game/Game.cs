using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using Singletons;

namespace Snake
{
	
	public class Game : MonoBehaviour {

		TileMap mTiles = new TileMap();
		Snake mSnake = new Snake();

		// Use this for initialization
		void Start () {
			LoadLevel ();
		}

		// Update is called once per frame
		void Update () {
			mSnake.HandleInput ();
			mSnake.Update (mTiles);
		}

		void LoadLevel() {
			int playerLevel = PlayerStats.Instance.level;

			JSONNode levelTuning = Tuning.Instance.Get ("levels");
			JSONNode gameTuning = Tuning.Instance.Get ("game");

			int levelCount = levelTuning ["Levels"].Count;
			Debug.Assert (playerLevel <= levelCount);

			JSONNode level = levelTuning ["Levels"] [playerLevel];
			mTiles.Init (level);
			mSnake.Init (level, gameTuning["Difficulty"][PlayerStats.Instance.difficulty], mTiles);
		}
	}
}
