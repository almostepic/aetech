using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Snake
{
	public class TileMap
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Count { get; private set; }

		Tile[] mTiles;
		GameObject mSnakeHead;
		GameObject mSnakeTail;
		List<GameObject> mSnakeBody;

		public TileMap ()
		{
		}

		public void InitMap(SimpleJSON.JSONNode level)
		{
			Width = level ["W"].AsInt;
			Height = level ["H"].AsInt;
			Count = Width * Height;

			mTiles = new Tile[Count];

			// figure out the position of the top left side
			int xOffset =  -(Width / 2);
			int yOffset = -(Height / 2);

			// init the tiles
			for (int i = 0; i < Count; ++i)
			{
				// handle 'Dead' tiles
				mTiles [i] = new Tile ();
				mTiles [i].SetTile ((i % Width) + xOffset, (i / Width) + yOffset, i);
			}

			// init the snake
			int snakeSpawnX = level["Spawn"]["X"].AsInt;
			int snakeSpawnY = level ["Spawn"] ["Y"].AsInt;

			// todo: error check that the snake can actually fit in a straight line
			// todo: move to snake body class
			int snakeTileNumber = (snakeSpawnX * Width) + snakeSpawnY;
			mSnakeBody = new List<GameObject> ();


			mSnakeHead = UnityEngine.Object.Instantiate (Resources.Load (Path.Combine ("Prefabs", "snake_head")), 
				new Vector3(mTiles[snakeTileNumber].XPos, mTiles[snakeTileNumber].YPos, -1.0f), 
				Quaternion.identity) as GameObject;
			--snakeTileNumber;
			mSnakeBody.Add(UnityEngine.Object.Instantiate (Resources.Load (Path.Combine ("Prefabs", "snake_body")), 
				new Vector3(mTiles[snakeTileNumber].XPos, mTiles[snakeTileNumber].YPos, -1.0f), 
				Quaternion.identity) as GameObject);
			--snakeTileNumber;
			mSnakeTail = UnityEngine.Object.Instantiate (Resources.Load (Path.Combine ("Prefabs", "snake_tail")), 
				new Vector3(mTiles[snakeTileNumber].XPos, mTiles[snakeTileNumber].YPos, -1.0f), 
				Quaternion.identity) as GameObject;
		}

		public void MoveSnake()
		{
		}

	}

}
