using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class Encounter : MonoBehaviour
    {
        [SerializeField]
        private List<Entity> _entities = new List<Entity>();

        public List<Entity> Entities => _entities;

        private Tile[] _tiles;

        private void Awake()
        {
            _tiles = GetComponentsInChildren<Tile>();
        }

        public Map GetEncounterMap()
        {
            (int, int, int, int) mapBounds = GetMapMatrixBounds();
            Map map = new Map();
            map.Tiles = new Tile[mapBounds.Item2 - mapBounds.Item1 + 1][];
            for (int i = 0; i < map.Tiles.Length; i++)
                map.Tiles[i] = new Tile[mapBounds.Item4 - mapBounds.Item3 + 1];

            InitializeMap(ref map, mapBounds);

            return map;
        }

        private (int, int, int, int) GetMapMatrixBounds()
        {
            (int, int, int, int) bounds = (int.MaxValue, int.MinValue, int.MaxValue, int.MinValue);
            foreach (Tile tile in _tiles)
            {
                Vector3 position = tile.transform.position;
                int posX = Mathf.RoundToInt(position.x);
                int posZ = Mathf.RoundToInt(position.z);
                bounds.Item1 = posX < bounds.Item1 ? posX : bounds.Item1;
                bounds.Item2 = posX > bounds.Item2 ? posX : bounds.Item2;
                bounds.Item3 = posZ < bounds.Item3 ? posZ : bounds.Item3;
                bounds.Item4 = posZ > bounds.Item4 ? posZ : bounds.Item4;
            }

            return bounds;
        }

        private void InitializeMap(ref Map map, (int, int, int, int) mapBounds)
        {
            foreach (Tile tile in _tiles)
            {
                Vector3 tilePosition = tile.transform.position;
                int posX = Mathf.RoundToInt(tilePosition.x);
                int posZ = Mathf.RoundToInt(tilePosition.z);
                NormalizeMapPosition(ref posX, ref posZ, (mapBounds.Item1, mapBounds.Item3));
                map.Tiles[posX][posZ] = tile;
                tile.SetCoordinates(posX, posZ);
            }
        }

        private void NormalizeMapPosition(ref int posX, ref int posZ, (int, int) origo)
        {
            posX += origo.Item1 * -1;
            posZ += origo.Item2 * -1;
        }
    }
}
