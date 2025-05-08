using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class UIGameOverScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _goalReachedText;
        [SerializeField] private GameObject _deathText;

        private Canvas _gameOverCanvas;

        private GameEvents _events => Game.Instance.GameEvents;

        public void ResetPressed()
        {
            _gameOverCanvas.enabled = false;
            _goalReachedText.SetActive(false);
            _deathText.SetActive(false);
            _events.ResetLevel.Invoke();
        }

        private void Start()
        {
            _gameOverCanvas = GetComponent<Canvas>();
            _events.EncounterGoalReached.AddListener(OnGoalReached);
            _events.OnPlayerDeath.AddListener(OnPlayerDeath);
        }

        private void OnGoalReached()
        {
            _gameOverCanvas.enabled = true;
            _goalReachedText.SetActive(true);
        }

        private void OnPlayerDeath()
        {
            _gameOverCanvas.enabled = true;
            _deathText.SetActive(true);
        }
    }
}
