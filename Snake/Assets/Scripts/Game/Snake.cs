using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Snake
{
	public class Snake
	{
		float mSpeed;
		const float SNAKE_Z = -1.0f;
		const int SNAKE_BODY_SIZE = 3;

		SnakeTile mHead;
		List<SnakeTile> mBody = new List<SnakeTile> ();
		SnakeTile mTail;

		public Snake ()
		{
		}

		~Snake()
		{
			Clear ();
		}

		public void Init(JSONNode level, JSONNode difficulty, TileMap map)
		{
			Clear();

			// init the snake
			int tileX = level["Spawn"]["X"].AsInt;
			int tileY = level["Spawn"]["Y"].AsInt;

			// create the starting body objects
			mHead = TileInitHelper (map, tileX, tileY, SnakeTile.BodyType.Head, SnakeTile.Direction.Right);
			for (int i = 0; i < SNAKE_BODY_SIZE; ++i)
			{
				mBody.Add (TileInitHelper (map, --tileX, tileY, SnakeTile.BodyType.Body, SnakeTile.Direction.Right));
			}
			mTail = TileInitHelper (map, --tileX, tileY, SnakeTile.BodyType.Tail, SnakeTile.Direction.Right);
		}

		SnakeTile TileInitHelper(TileMap map, int x, int y, SnakeTile.BodyType type, SnakeTile.Direction dir)
		{
			Tile t = map.GetTile (x, y);
			if (t != null)
			{
				return new SnakeTile (type, dir, new Vector3 (t.XPos, t.YPos, SNAKE_Z));
			}
			Debug.Assert (t != null, "Failed to get Tile X:" + x + " Y: " + y);
			return null;
		}


		void Clear()
		{
			mHead = null;
			mTail = null;
			mBody.Clear ();
		}

	}
}

