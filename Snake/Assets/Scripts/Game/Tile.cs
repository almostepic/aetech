using UnityEngine;
using System;
using System.IO;


namespace Snake
{
	public class Tile
	{
		GameObject mSprite;
		public int XPos { get; protected set; }
		public int YPos { get; protected set; }
		public int TileNumber { get; protected set; }


		public Tile () {}
		~Tile() 
		{
			if (mSprite)
			{
				UnityEngine.Object.Destroy (mSprite); 
			}
		}
		public void SetTile(int xPos, int yPos, int tileNumber)
		{
			XPos = xPos;
			YPos = yPos;
			TileNumber = tileNumber;
			mSprite = UnityEngine.Object.Instantiate (Resources.Load (Path.Combine ("Prefabs", "empty_tile")), new Vector3(XPos, YPos), Quaternion.identity) as GameObject;
		}
	}
}

