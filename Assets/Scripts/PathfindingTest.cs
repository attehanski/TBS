using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TBS
{
    public class PathfindingTest : MonoBehaviour
    {
        [SerializeField]
        private Entity _actor;
        [SerializeField]
        private Tile _targetTile;
        [SerializeField]
        private Stack<Tile> _path;

        public void GetPath()
        {
            _path = FindPath(_actor.Tile, _targetTile);
        }

        public Stack<Tile> FindPath(Tile startTile, Tile targetTile)
        {
            var toSearch = new List<Tile>() { startTile };
            var processed = new List<Tile>();

            while (toSearch.Any())
            {
                var current = toSearch[0];
                foreach (var t in toSearch)
                    if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

                processed.Add(current);
                toSearch.Remove(current);

                if (current == targetTile)
                {
                    var currentPathTile = targetTile;
                    var path = new Stack<Tile>();
                    var count = 100;
                    while (currentPathTile != startTile)
                    {
                        path.Push(currentPathTile);
                        currentPathTile.DebugSetAsPath();
                        currentPathTile = currentPathTile.Connection;
                        count--;
                    }

                    targetTile.DebugSetAsTarget();
                    return path;
                }

                foreach (var neighbor in current.Neighbors.Where(t => !processed.Contains(t.Value)))
                {
                    Tile neighborTile = neighbor.Value;
                    if (!current.CanMoveTo(neighborTile) || neighborTile.Priority < current.Priority - 1)
                        continue;

                    var inSearch = toSearch.Contains(neighborTile);

                    var costToNeighbor = current.G + current.GetPathfindingDistance(neighborTile);

                    if (!inSearch || costToNeighbor < neighborTile.G)
                    {
                        neighborTile.SetG(costToNeighbor);
                        neighborTile.SetConnection(current);

                        if (!inSearch)
                        {
                            neighborTile.SetH(neighborTile.GetPathfindingDistance(targetTile));
                            toSearch.Add(neighborTile);
                        }
                    }
                }
            }
            return null;
        }

        public void Move()
        {
            if (_path.Count <= 0)
                return;

            _actor.Tile.RemoveEntity();
            _actor.SetToTile(_path.Pop());
        }
    }
}
