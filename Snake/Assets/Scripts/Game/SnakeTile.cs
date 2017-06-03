using UnityEngine;
using System;
using System.IO;

namespace Snake
{
	public class SnakeTile
	{
		const float SNAKE_Z = -1.0f;
		public enum BodyType
		{
			Head = 0,
			Body,
			Tail
		};
		public enum Direction
		{
			Up = 0,
			Down,
			Left,
			Right
		};

		GameObject mSprite;
		float mSpeed;
		public Direction CurrentDirection { get; private set; }
		public Vector3 CurrentPosition { get; private set; }
		public Vector3 CurrentRotation { get; private set; } // euler
		public int CurrentTileNumber { get; private set; }

		public Direction TargetDirection { get; private set; }
		public Vector3 TargetPosition { get; private set; }
		public Vector3 TargetRotation { get; private set; } // euler
		public int TargetTileNumber { get; private set; }

		public SnakeTile (BodyType bodyType, Direction direction, Tile tile, float speed)
		{
			string bodyString;
			switch(bodyType)
			{
				case BodyType.Head:
					bodyString = "snake_head";
					break;
				case BodyType.Body:
					bodyString = "snake_body";
					break;
				default:
					bodyString = "snake_tail";
					break;
			};
			Vector3 globalPos = new Vector3 (tile.XPos, tile.YPos, SNAKE_Z);
			mSprite = UnityEngine.Object.Instantiate(Resources.Load(Path.Combine("Prefabs",  bodyString)),
				globalPos, 
				Quaternion.identity) as GameObject;

			TargetDirection = CurrentDirection = direction;
			TargetPosition = CurrentPosition = globalPos;
			TargetTileNumber = CurrentTileNumber = tile.TileNumber;
			TargetRotation = GetRotationOnDirection (direction);
			FlipSpriteOnDirection (direction);
			mSpeed = speed;
		}

		~SnakeTile()
		{
			if (mSprite != null)
			{
				UnityEngine.Object.Destroy (mSprite);
				mSprite = null;
			}
		}

		void FlipSpriteOnDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.Up:
					mSprite.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
					break;
				case Direction.Down:
					mSprite.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
					break;
				case Direction.Left:
					mSprite.transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
					break;
				case Direction.Right:
					mSprite.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
					break;
			};
		}

		Vector3 GetRotationOnDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.Up:
					return Vector3.forward * 90.0f;
				case Direction.Down:
					return Vector3.forward * -90.0f;
				default:
					return Vector3.forward * 0.0f;
			};
		}

		public bool ReachedTarget()
		{
			// epsilon floats?
			return mSprite.transform.position == TargetPosition;
		}

		public bool SetNextTarget(TileMap map, Direction dir)
		{
			// we assume we made it to the target, and that our previous target is now our actual current
			Tile currentTile = map.GetTile (TargetTileNumber);
			Tile targetTile = null;

			// determine the next tile in the map based off of the direction given
			switch (dir)
			{
				case Direction.Up:
					targetTile = map.GetTile (currentTile.X, currentTile.Y - 1);
					break;
				case Direction.Left:
					targetTile = map.GetTile (currentTile.X - 1, currentTile.Y);
					break;
				case Direction.Right:
					targetTile = map.GetTile (currentTile.X + 1, currentTile.Y);
					break;
				case Direction.Down:
					targetTile = map.GetTile (currentTile.X, currentTile.Y + 1);
					break;
			};
			// we dead
			if (targetTile == null)
				return false;

			// set current to our target and set a new target tile
			CurrentDirection = TargetDirection;
			CurrentPosition = TargetPosition;
			CurrentTileNumber = TargetTileNumber;
			CurrentRotation = TargetRotation;

			TargetDirection = dir;
			TargetPosition = new Vector3 (targetTile.XPos, targetTile.YPos, SNAKE_Z);
			TargetTileNumber = targetTile.TileNumber;
			TargetRotation = GetRotationOnDirection (TargetDirection);

			FlipSpriteOnDirection (TargetDirection);
			return true;
		}

		public void SetNextTarget(SnakeTile tile)
		{
			// set next target tile to be the given snake tile
			// set current to our target and set a new target tile
			CurrentDirection = TargetDirection;
			CurrentPosition = TargetPosition;
			CurrentTileNumber = TargetTileNumber;
			CurrentRotation = TargetRotation;

			TargetDirection = tile.CurrentDirection;
			TargetPosition = tile.CurrentPosition;
			TargetTileNumber = tile.CurrentTileNumber;
			TargetRotation = tile.CurrentRotation;

			FlipSpriteOnDirection (TargetDirection);
		}

		public void Update()
		{
			mSprite.transform.position = Vector3.MoveTowards (mSprite.transform.position, TargetPosition, Time.deltaTime * mSpeed);
			mSprite.transform.rotation = Quaternion.Euler (TargetRotation);
			//mSprite.transform.rotation = Quaternion.RotateTowards (mSprite.transform.rotation, Quaternion.Euler (TargetRotation), Time.deltaTime * 720.0f);
		}

		static public SnakeTile.Direction GetOppositeDirection(Direction dir)
		{
			switch (dir)
			{
				case Direction.Up:
					return Direction.Down;
				case Direction.Down:
					return Direction.Up;
				case Direction.Left:
					return Direction.Right;
				default:
					return Direction.Left;
			};
		}

		static public bool AreDirectionsOpposites(Direction dir1, Direction dir2)
		{
			return GetOppositeDirection (dir1) == dir2;
		}

	}
}
