using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

namespace Snake
{
	public class TileMap
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Count { get; private set; }

		Tile[] mTiles;


		public TileMap ()
		{
		}

		~TileMap()
		{
		}

		public Tile GetTile(int x, int y)
		{
			if(x >= Width || x < 0)
				return null;
			if(y >= Height || y < 0)
				return null;
			return mTiles [y * Width + x];
		}

		public Tile GetTile(Vector2 xy)
		{
			return GetTile ((int)xy.x, (int)xy.y);
		}

		public Tile GetTile(int tileNumber)
		{
			if(tileNumber >= Count || tileNumber < 0)
				return null;
			return mTiles[tileNumber];
		}

		public void Init(JSONNode level)
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
		}
	}

}
