using UnityEngine;
using System;
using System.IO;


namespace Snake
{
	public class Tile
	{
		GameObject mBackground;
		public int XPos { get; protected set; }
		public int YPos { get; protected set; }
		public int TileNumber { get; protected set; }


		public Tile () {}
		public void SetTile(int xPos, int yPos, int tileNumber)
		{
			Debug.Log ("X: " + xPos + " Y: " + yPos);
			XPos = xPos;
			YPos = yPos;
			TileNumber = tileNumber;
			mBackground = UnityEngine.Object.Instantiate (Resources.Load (Path.Combine ("Prefabs", "Tile")), new Vector3(XPos, YPos), Quaternion.identity) as GameObject;
		}


		public static Tile[] CreateMap(int width, int height)
		{
			int totalCount = width * height;
		
			Tile[] tiles = new Tile[totalCount];

			// figure out the position of the top left side
			int xOffset =  -(width / 2);
			int yOffset = -(height / 2);

			for (int i = 0; i < totalCount; ++i)
			{
				// handle 'Dead' tiles
				tiles [i] = new Tile ();
				tiles [i].SetTile ((i % width) + xOffset, (i / width) + yOffset, i);
			}

			return tiles;
		}
	}
}

