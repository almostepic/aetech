using UnityEngine;
using System;
using System.IO;

namespace Snake
{
	public class SnakeTile
	{
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

		public SnakeTile (BodyType bodyType, Direction direction, Vector3 globalPos)
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
			mSprite = UnityEngine.Object.Instantiate(Resources.Load(Path.Combine("Prefabs",  bodyString)),
				globalPos, 
				Quaternion.identity) as GameObject;
		}

		~SnakeTile()
		{
			if (mSprite)
			{
				UnityEngine.Object.Destroy (mSprite);
				mSprite = null;
			}
		}

		public void SetPosition(Direction  direction, Vector3 globalPos)
		{
		}

		public void SetTargetPosition(Direction targetDirection, Vector3 targetGlobalPos)
		{
		}


	}
}

