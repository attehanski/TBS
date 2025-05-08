using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField]
        private CombatManager _combatManager;
        [SerializeField] [Tooltip("Deprecated")]
        private GameObject _tilePrefab; 
        [SerializeField] [Tooltip("Deprecated")]
        private Transform _mapParent;
        /// <summary>
        /// Used for instantiating a whole map.
        /// </summary>
        [SerializeField] [Tooltip("Deprecated")]
        private GameObject _mapPrefab;
        [SerializeField]
        private Encounter _encounter;

        [SerializeField]
        private List<ListWrapper> _mapDebugList = new List<ListWrapper>();

        private Map _map;
        private Queue<EncounterAction> _actionQueue = new Queue<EncounterAction>();
        private Coroutine _actionQueueCoroutine;
        private bool _playingActionQueue = false;
        private bool _cameraMoveOnGoing = false;
        private List<Tile> _specialActionMarkedTiles = new List<Tile>();

        private GameEvents _events => Game.Instance.GameEvents;

        [System.Serializable]
        public class ListWrapper
        {
            public ListWrapper(Tile[] newList)
            {
                list = newList;
            }

            public Tile[] list;
        }

        [ContextMenu("StartEncounter")]
        public void StartEncounter()
        {
            _map = _encounter.GetEncounterMap();
            foreach (Tile[] row in _map.Tiles)
            {
                _mapDebugList.Add(new ListWrapper(row));
                foreach (Tile tile in row)
                    tile.InitalizeNeighbors(_map);
            }
            Player player = FindObjectOfType<Player>();
            if (player)
                SetNearestTileForEntity(player);
            foreach (Entity entity in _encounter.Entities)
                SetNearestTileForEntity(entity);

            _events.EncounterStarted.Invoke();
            Debug.Log("Encounter started");
        }

        public void QueuePlayerAction(Entity player, Enums.Direction direction)
        {
            if (_playingActionQueue)
                return;

            Vector2Int playerPos = player.Tile.Coordinates;
            Vector2Int directionCoordinates = Utils.DirectionToCoordinates(direction);
            Vector2Int targetPos = new Vector2Int(playerPos.x + directionCoordinates.x, playerPos.y + directionCoordinates.y);
            Tile targetTile = _map.GetTileFromMap(targetPos);

            if (targetTile == null)
                return;
            else if (targetTile.IsFree())
                QueueAction(new EncounterAction_Move(player, direction));
            else if (targetTile.EntityOnTile != null && player.Tile.Priority == targetTile.Priority)
                QueueAction(new EncounterAction_Attack(player, targetTile.EntityOnTile));

            if (_actionQueue.Count > 0)
                _actionQueueCoroutine = StartCoroutine(PlayQueue());
        }

        public void QueueAction(EncounterAction action, bool playQueue = false)
        {
            if (action == null)
                return;

            _actionQueue.Enqueue(action);

            if (playQueue)
                _actionQueueCoroutine = StartCoroutine(PlayQueue());
        }

        public void ResetTileDebugValues()
        {
            foreach (Tile[] row in _map.Tiles)
                foreach (Tile tile in row)
                    tile.DebugReset();
        }

        public Tile GetNearestTile(Vector3 position)
        {
            Tile nearestTile = null;
            float nearestDistance = Mathf.Infinity;
            float distance;
            for (int i = 0; i < _map.Tiles.Length; i++)
            {
                for (int j = 0; j < _map.Tiles[i].Length; j++)
                {
                    distance = Vector3.Distance(_map.Tiles[i][j].Position(), position);
                    if (distance < nearestDistance)
                    {
                        nearestTile = _map.Tiles[i][j];
                        nearestDistance = distance;
                    }
                }
            }

            return nearestTile;
        }

        public void PrepareSpecialAction(SpecialAction action)
        {
            if (_specialActionMarkedTiles.Count > 0)
            {
                ResetSpecialActionTiles();
                return;
            }

            SpecialAction.TileData tileData = action.GetActionTileData();
            foreach ((Tile, Enums.TileState) element in tileData.TileStates)
            {
                element.Item1.SetTileState(element.Item2);
                if (element.Item2 == Enums.TileState.Highlighted)
                    element.Item1.SubscribeToAction(tileData.TileAction);
                _specialActionMarkedTiles.Add(element.Item1);
            }
        }

        public void ResetSpecialActionTiles()
        {
            foreach (Tile tile in _specialActionMarkedTiles)
                tile.SetTileState(Enums.TileState.Default);
            _specialActionMarkedTiles.Clear();
        }

        // TODO: Clean up, NPC actions should be handled in a neater manner, perhaps with prioritiy queue for actions
        private IEnumerator PlayQueue()
        {
            _playingActionQueue = true;
            while (_actionQueue.Count > 0)
                yield return _actionQueue.Dequeue().Execute();

            QueueNPCActions();

            while (_actionQueue.Count > 0)
                yield return _actionQueue.Dequeue().Execute();

            _cameraMoveOnGoing = true;
            _events.CameraTargetChanged.AddListener(OnCameraMoveFinished);
            _events.ActionQueueFinished.Invoke();
            while (_cameraMoveOnGoing)
                yield return null;
            _events.CameraTargetChanged.RemoveListener(OnCameraMoveFinished);

            _playingActionQueue = false;
        }

        private void LoadMap()
        {
            GameObject map = Instantiate(_mapPrefab);
            map.name = "Map";

        }

        private void Start()
        {
            StartEncounter();

            _events.EntityDestroyed.AddListener(RemoveEntityFromEncounter);
        }

        private void SortCharacterList()
        {

        }

        private void SetNearestTileForEntity(Entity entity)
        {
            Vector3 entityPosition = entity.transform.position;
            Tile nearestTile = GetNearestTile(entityPosition);
            SetTileToEntity(entity, nearestTile);
        }

        private void SetTileToEntity(Entity entity, Tile tile)
        {
            entity.SetToTile(tile);
            tile.SetEntity(entity);
        }

        private void QueueNPCActions()
        {
            foreach (Entity entity in _encounter.Entities)
                QueueAction(entity.GetAction());
        }

        private void RemoveEntityFromEncounter(Entity entity)
        {
            _encounter.Entities.Remove(entity);
            entity.Tile.RemoveEntity();
        }

        private void OnCameraMoveFinished()
        {
            _cameraMoveOnGoing = false;
        }
    }
}
