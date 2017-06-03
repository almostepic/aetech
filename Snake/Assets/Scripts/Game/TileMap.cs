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

		public Tile GetTile(int tileNumber)
		{
			if(tileNumber >= Count || tileNumber < 0)
				return null;
			return mTiles[tileNumber];
		}

		public void Init(JSONNode level)
		{
			Width = level ["Width"].AsInt;
			Height = level ["Height"].AsInt;
			const float tileWidth = 1;
			const float tileHeight = 1;
			Count = Width * Height;

			mTiles = new Tile[Count];

			// figure out the position of the top left side
			float xOffset =  -((float)Width * 0.5f) + tileWidth * 0.5f;
			float yOffset = ((float)Height * 0.5f) - tileHeight * 0.5f;



			for (int i = 0; i < Count; ++i)
			{
				mTiles [i] = new Tile (
					xOffset + ((i % Width) * tileWidth),
					yOffset - ((i / Width) * tileHeight),
					i % Width,
					i / Width,
					i
				);
			}
		}
	}

}
