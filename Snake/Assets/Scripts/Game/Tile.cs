using UnityEngine;
using System;
using System.IO;


namespace Snake
{
	public class Tile
	{
		GameObject mSprite;
		public float XPos { get; protected set; } // world x position of gameobject
		public float YPos { get; protected set; } // world y position of gameobject
		public int X  { get; protected set; } // cache the x and y in a 2d array
		public int Y { get; protected set; } // cache the x and y in a 2d array
		public int TileNumber { get; protected set; } // cache the tile number in a 1d array

		public Tile (float xPos, float yPos, int tileX, int tileY, int tileNum) 
		{
			XPos = xPos;
			YPos = yPos;
			TileNumber = tileNum;
			X = tileX;
			Y = tileY;
			mSprite = UnityEngine.Object.Instantiate (Resources.Load (Path.Combine ("Prefabs", "empty_tile")), new Vector3(XPos, YPos), Quaternion.identity) as GameObject;
		}

		~Tile() 
		{
			if (mSprite)
			{
				UnityEngine.Object.Destroy (mSprite); 
			}
		}
	}
}

