using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TBS
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private float _positionYOffset;
        [SerializeField] private bool _impassable = false;
        [SerializeField] private int _priority;
        [SerializeField] private TileStateIndicator _stateIndicator;

        [Header("Debug")]
        [SerializeField] private Renderer _debugRenderer;
        [SerializeField] private Color _debugPathColor;
        [SerializeField] private Color _debugTargetColor;

        public Vector2 _debugCoordinates;

        public Vector2Int Coordinates => _coordinates;
        public Entity EntityOnTile => _entityOnTile;
        public int Priority => _priority;
        public Dictionary<Enums.Direction, Tile> Neighbors => _neighbors;
        public Tile Connection { get; private set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;

        private Vector2Int _coordinates;
        private bool _free = true;
        private Entity _entityOnTile = null;
        private Dictionary<Enums.Direction, Tile> _neighbors = new Dictionary<Enums.Direction, Tile>();
        private UnityAction<Tile> _onClickAction;

        public bool IsFree()
        {
            return !_impassable && _free;
        }

        public void SetCoordinates(int x, int z)
        {
            _coordinates = new Vector2Int(x, z);
            _debugCoordinates = new Vector2(x, z);
        }

        public void RemoveEntity()
        {
            _entityOnTile = null;
            _free = true;
        }

        public void SetEntity(Entity entity)
        {
            _entityOnTile = entity;
            _free = false;
        }

        public Vector3 Position()
        {
            return transform.position + Vector3.up * _positionYOffset;
        }

        public void InitalizeNeighbors(Map map)
        {
            foreach (Enums.Direction direction in Utils.AllDirections())
            {
                Tile neighbor = map.GetTileFromMap(Coordinates + Utils.DirectionToCoordinates(direction));
                if (neighbor != null)
                    _neighbors[direction] = neighbor;
            }
        }

        public bool IsNeighborOf(Tile tile)
        {
            return _neighbors.ContainsValue(tile);
        }

        public Tile GetNeighbor(Enums.Direction direction)
        {
            Tile result;
            _neighbors.TryGetValue(direction, out result);
            return result;
        }

        public bool CanMoveTo(Tile other)
        {
            if (!other.IsFree() || other.Priority > _priority + 1)
                return false;

            return true;
        }

        public void SetTileState(Enums.TileState state)
        {
            _stateIndicator.SetIndicatorState(state);
        }

        public void SubscribeToAction(UnityAction<Tile> action)
        {
            _onClickAction = action;
        }

        private void OnClick()
        {
            if (_onClickAction != null)
                _onClickAction.Invoke(this);
        }

        private void Start()
        {
            _stateIndicator.OnClickEvent.AddListener(OnClick);
        }


        #region Pathfinding
        public void SetConnection(Tile tile)
        {
            Connection = tile;
        }

        public void SetG(float g)
        {
            G = g;
        }

        public void SetH(float h)
        {
            H = h;
        }

        public float GetPathfindingDistance(Tile other)
        {
            var dist = new Vector2Int(Mathf.Abs(Coordinates.x - other.Coordinates.x), Mathf.Abs(Coordinates.y - other.Coordinates.y));

            var lowest = Mathf.Min(dist.x, dist.y);
            var highest = Mathf.Max(dist.x, dist.y);

            var horizontalMovesRequired = highest - lowest;

            return lowest * 14 + horizontalMovesRequired * 10;
        }

        public void DebugSetAsTarget()
        {
            if (!_debugRenderer)
                return;

            _debugRenderer.enabled = true;
            _debugRenderer.material.color = _debugTargetColor;
        }

        public void DebugSetAsPath()
        {
            if (!_debugRenderer)
                return;

            _debugRenderer.enabled = true;
            _debugRenderer.material.color = _debugPathColor;
        }

        public void DebugReset()
        {
            if (!_debugRenderer)
                return;

            _debugRenderer.enabled = false;
        }

        public void DebugSetColor(Color color)
        {

            if (!_debugRenderer)
                return;

            _debugRenderer.enabled = true;
            _debugRenderer.material.color = color;
        }
        #endregion
    }
}
