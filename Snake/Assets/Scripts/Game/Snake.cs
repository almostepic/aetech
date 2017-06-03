using System;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace Snake
{
	public class Snake
	{
		float mSpeed;
		const int SNAKE_BODY_SIZE = 4;

		SnakeTile mHead;
		List<SnakeTile> mBody = new List<SnakeTile> ();
		SnakeTile mTail;
		SnakeTile.Direction mPendingDirection;

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
			float snakeSpeed = difficulty ["SnakeSpeed"].AsFloat;
			mPendingDirection = SnakeTile.Direction.Right;

			// create the starting body objects
			mHead = TileInitHelper (map, tileX, tileY, SnakeTile.BodyType.Head, mPendingDirection, snakeSpeed);
			for (int i = 0; i < SNAKE_BODY_SIZE; ++i)
			{
				mBody.Add (TileInitHelper (map, --tileX, tileY, SnakeTile.BodyType.Body, mPendingDirection, snakeSpeed));
			}
			mTail = TileInitHelper (map, --tileX, tileY, SnakeTile.BodyType.Tail, mPendingDirection, snakeSpeed);
		}

		public void Update(TileMap map)
		{
			// update the body part positions
			mHead.Update ();
			foreach (SnakeTile t in mBody)
				t.Update ();
			mTail.Update ();

			// set new target positions for the snake parts if the head has reached its destination
			if (mHead.ReachedTarget ())
			{
				// we assume that there is always a head and a tail, body is optional
				// set new targets update
				// assume all snake  parts reach the target at the same time
				if (mHead.SetNextTarget (map, mPendingDirection))
				{
					if (mBody.Count >= 1)
					{
						mBody [0].SetNextTarget (mHead);
						for (int i = 1; i < mBody.Count; ++i)
						{
							mBody [i].SetNextTarget (mBody [i - 1]);
						}

						mTail.SetNextTarget (mBody [mBody.Count - 1]);
					} else
					{
						mTail.SetNextTarget (mHead);
					}
				}

			}
		}

		public void HandleInput()
		{
			if (Input.GetButton ("Up") && !SnakeTile.AreDirectionsOpposites(mHead.CurrentDirection, SnakeTile.Direction.Up))
				mPendingDirection = SnakeTile.Direction.Up;
			if (Input.GetButton ("Down") && !SnakeTile.AreDirectionsOpposites(mHead.CurrentDirection, SnakeTile.Direction.Down))
				mPendingDirection = SnakeTile.Direction.Down;
			if (Input.GetButton ("Left") && !SnakeTile.AreDirectionsOpposites(mHead.CurrentDirection, SnakeTile.Direction.Left))
				mPendingDirection = SnakeTile.Direction.Left;
			if (Input.GetButton ("Right") && !SnakeTile.AreDirectionsOpposites(mHead.CurrentDirection, SnakeTile.Direction.Right))
				mPendingDirection = SnakeTile.Direction.Right;
		}
			
		SnakeTile TileInitHelper(TileMap map, int x, int y, SnakeTile.BodyType type, SnakeTile.Direction dir, float snakeSpeed)
		{
			Tile t = map.GetTile (x, y);
			if (t != null)
			{
				return new SnakeTile (type, dir, t, snakeSpeed);
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

