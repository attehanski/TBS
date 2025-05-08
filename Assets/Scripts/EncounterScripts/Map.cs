using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class Map
    {
        public Tile[][] Tiles;

        public Tile GetTileFromMap(Vector2Int coordinates)
        {
            if (coordinates.x < 0 || coordinates.x >= Tiles.Length ||
                coordinates.y < 0 || coordinates.y >= Tiles[coordinates.x].Length)
                return null;

            return Tiles[coordinates.x][coordinates.y];
        }

        public Tile FindEntityOnMap(Entity entity)
        {
            for (int i = 0; i < Tiles.Length; i++)
            {
                for (int j = 0; j < Tiles[i].Length; j++)
                {
                    if (Tiles[i][j].EntityOnTile == entity)
                        return Tiles[i][j];
                }
            }
            return null;
        }

        public bool TileExists(Vector2Int coordinates)
        {
            if (coordinates.x >= 0 && coordinates.x < Tiles.Length && coordinates.y >= 0 && coordinates.y < Tiles[coordinates.x].Length)
                return true;

            return false;
        }
    }
}
