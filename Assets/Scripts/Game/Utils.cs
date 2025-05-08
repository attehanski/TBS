using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class Utils
    {
        public static Vector2Int DirectionToCoordinates(Enums.Direction direction)
        {
            return direction switch
            {
                Enums.Direction.Right => new Vector2Int(1, 0),
                Enums.Direction.Left => new Vector2Int(-1, 0),
                Enums.Direction.Up => new Vector2Int(0, 1),
                Enums.Direction.Down => new Vector2Int(0, -1),
                _ => Vector2Int.zero
            };
        }

        public static Vector3 DirectionToRotation(Enums.Direction direction)
        {
            return direction switch
            {
                Enums.Direction.Right => new Vector3(0f, 0f, 0f),
                Enums.Direction.Left => new Vector3(0f, 180f, 0f),
                Enums.Direction.Up => new Vector3(0f, 270f, 0f),
                Enums.Direction.Down => new Vector3(0f, 90f, 0f),
                _ => Vector3.zero
            };
        }

        public Vector2Int GetNewPosition(Vector2Int oldPosition, Enums.Direction direction)
        {
            return direction switch
            {
                Enums.Direction.Right => oldPosition + new Vector2Int(1, 0),
                Enums.Direction.Left => oldPosition + new Vector2Int(-1, 0),
                Enums.Direction.Up => oldPosition + new Vector2Int(0, 1),
                Enums.Direction.Down => oldPosition + new Vector2Int(0, -1),
                _ => oldPosition
            };
        }

        public static Enums.Direction GetDirectionFromTileToTile(Tile from, Tile to)
        {
            Enums.Direction result = Enums.Direction.None;

            if (from == null || to == null)
                return result;

            if (from.Coordinates.x < to.Coordinates.x)
                result = Enums.Direction.Right;
            else if (from.Coordinates.x > to.Coordinates.x)
                result = Enums.Direction.Left;
            else if (from.Coordinates.y < to.Coordinates.y)
                result = Enums.Direction.Up;
            else if (from.Coordinates.y > to.Coordinates.y)
                result = Enums.Direction.Down;

            return result;
        }


        public static int GetDistanceFromTileToTile(Tile fromTile, Tile toTile)
        {
            Enums.Direction direction = Utils.GetDirectionFromTileToTile(fromTile, toTile);

            fromTile.Neighbors.TryGetValue(direction, out Tile neighbor);
            if (!neighbor)
                return -100000;
            else if (neighbor == toTile)
                return 1;
            else
                return GetDistanceFromTileToTile(neighbor, toTile) + 1;
        }

        public static List<Enums.Direction> AllDirections()
        {
            return new List<Enums.Direction> { Enums.Direction.Down, Enums.Direction.Up, Enums.Direction.Right, Enums.Direction.Left };
        }
    }
}
