using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TBS
{
    public class Game : MonoBehaviour
    {
        public static Game Instance;

        [SerializeField]
        private GameEvents _gameEvents; // TODO: Handle this better instead of being a referenced gameobject

        protected EncounterManager _encounterManager;
        protected CombatManager _combatManager;
        protected PlayerInput _playerInput;
        protected bool _gamePaused = false;

        public GameEvents GameEvents => _gameEvents;

        public CombatValues CombatValues;
        [HideInInspector]
        public UnityEvent GlobalUpdateEarly;
        [HideInInspector]
        public UnityEvent GlobalUpdate;
        [HideInInspector]
        public UnityEvent GlobalUpdateLate;
        [HideInInspector]
        public UnityEvent GlobalFixedUpdate;

        private PlayerController _playerController;

        public Dictionary<Enums.InputType, Enums.InputState> GetFrameInputs()
        {
            return _playerInput.Inputs;
        }

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                _playerInput = new PlayerInput();
                //DontDestroyOnLoad(gameObject);
                if (_gameEvents)
                    AddEventListeners();
                return;
            }

            Destroy(gameObject);
        }

        private void Start()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _encounterManager = GetComponent<EncounterManager>();
            _combatManager = GetComponent<CombatManager>();
        }

        protected virtual void Update()
        {
            // TODO: Fix so playerInput only updates for relevant stuff while game is paused
            _playerInput.DoUpdate();
            if (_playerInput.Inputs.TryGetValue(Enums.InputType.Cancel, out Enums.InputState cancelInputState) && cancelInputState == Enums.InputState.Down)
                ChangeGamePaused(!_gamePaused);

            if (_gamePaused)
                return;

            _playerController.HandleInput(_playerInput.Inputs); // TODO: Add inEncounter bool
            GlobalUpdateEarly.Invoke();
            GlobalUpdate.Invoke();
            GlobalUpdateLate.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            if (_gamePaused)
                return;

            GlobalFixedUpdate.Invoke();
        }

        private void OnDestroy()
        {
            _gameEvents.ResetLevel.RemoveListener(ResetGame);
            _gameEvents.EncounterGoalReached.RemoveListener(OnEncounterFinished);
        }

        private void AddEventListeners()
        {
            _gameEvents.ResetLevel.AddListener(ResetGame);
            _gameEvents.EncounterGoalReached.AddListener(OnEncounterFinished);
        }

        /// <summary>
        /// Changes the game's paused state
        /// </summary>
        /// <param name="pause"></param>
        public void ChangeGamePaused(bool pause)
        {
            _gamePaused = pause;
            _gameEvents.PauseStateChanged.Invoke(pause);
        }

        public void QueueEncounterAction(EncounterAction action)
        {
            _encounterManager.QueueAction(action);
        }

        public void QueuePlayerAction(Entity player, Enums.Direction direction)
        {
            _encounterManager.QueuePlayerAction(player, direction);
        }

        public IEnumerator DoCombatAction(Entity attacker, Entity defender)
        {
            yield return _combatManager.DoCombatAction(attacker, defender);
        }

        public IEnumerator DoCombatAction(Entity attacker, Entity defender, string animationName)
        {
            yield return _combatManager.DoCombatAction(attacker, defender, animationName);
        }

        // TODO: This is kind of dirty, should be handled better
        public bool CurrentlyInCombat()
        {
            return _combatManager.CombatOnGoing;
        }

        private void OnEncounterFinished()
        {
            _gamePaused = true;
        }

        private void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
}
