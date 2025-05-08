using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class EncounterGoalTest : MonoBehaviour
    {
        [SerializeField] private EncounterManager _encounterManager;

        private Tile _goalTile;
        private GameEvents _events => Game.Instance.GameEvents;

        private void Awake()
        {
            _events.EncounterStarted.AddListener(OnEncounterStarted);
        }

        private void Update()
        {
            if (_goalTile && !_goalTile.IsFree() && _goalTile.EntityOnTile as Player != null)
                _events.EncounterGoalReached.Invoke();
        }

        private void OnEncounterStarted()
        {
            _goalTile = _encounterManager.GetNearestTile(transform.position);
        }
    }
}
