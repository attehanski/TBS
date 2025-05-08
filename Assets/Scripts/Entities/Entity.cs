using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TBS
{
    public class Entity : MonoBehaviour
    {
        [SerializeField]
        protected Animator _animator;
        [SerializeField]
        protected EntityBehaviour _behaviour;
        [SerializeField]
        protected int _health = 100;
        [SerializeField]
        protected UIHealthBar _healthbar;

        protected GameEvents _events => Game.Instance.GameEvents;
        protected int _maxHealth;
        protected Tile _tile;

        public Animator Animator => _animator;
        public Tile Tile => _tile;

        public virtual void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
                OnHealthReachedZero();
            if (_healthbar)
                _healthbar.SetHealthBarValue((float)_health / _maxHealth);
        }

        public virtual void OnHealthReachedZero()
        {
            DestroyEntity();
            _events.EntityDestroyed.Invoke(this);
        }

        public virtual void DestroyEntity()
        {
            _tile.RemoveEntity();
            Destroy(gameObject);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetToTile(Tile tile)
        {
            _tile = tile;
            Vector3 pos = tile.Position();
            SetPosition(pos);
        }

        public IEnumerator TurnEntity(Enums.Direction direction)
        {
            Quaternion startRotation = transform.rotation;
            Quaternion newRotation = Quaternion.Euler(Utils.DirectionToRotation(direction));

            if (startRotation == newRotation)
                yield break;

            float turnTime = 0.2f; // TODO: Move somewhere for easy editing
            float timeTurned = 0f;
            while (timeTurned < turnTime)
            {
                transform.rotation = Quaternion.Slerp(startRotation, newRotation, timeTurned / turnTime);
                timeTurned += Time.deltaTime;
                yield return null;
            }

            transform.rotation = newRotation;
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
                        //currentPathTile.DebugSetAsPath();
                        currentPathTile = currentPathTile.Connection;
                        count--;
                    }

                    //targetTile.DebugSetAsTarget();
                    return path;
                }

                foreach (var neighbor in current.Neighbors.Where(t => !processed.Contains(t.Value)))
                {
                    Tile neighborTile = neighbor.Value;
                    if (neighborTile != targetTile && (!current.CanMoveTo(neighborTile) || neighborTile.Priority < current.Priority - 1))
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

        public virtual EncounterAction GetAction()
        {
            if (!_behaviour)
                return null;

            return _behaviour.GetAction(this);
        }

        protected virtual void Start()
        {
            _maxHealth = _health;
        }
    }
}
