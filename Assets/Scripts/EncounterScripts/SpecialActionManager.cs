using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class SpecialActionManager : MonoBehaviour
    {
        [SerializeField] private EncounterManager _encounterManager;

        private SpecialAction[] _selectedActions;
        private Player _player;

        private Game _game => Game.Instance;
        private GameEvents _events => _game.GameEvents;
        private Dictionary<Enums.InputType, Enums.InputState> _inputs => _game.GetFrameInputs();

        private void Awake()
        {
            _player = FindObjectOfType<Player>();
            _game.GlobalUpdate.AddListener(DoUpdate);
            _selectedActions = new SpecialAction[2]{new SpecialAction_MoveAttack(_player, 2), new SpecialAction_MultiAttack(_player)};
            _selectedActions[0].OnActionInvoked += ExecuteSpecialAction;
            _selectedActions[1].OnActionInvoked += ExecuteSpecialAction;
        }

        private void OnDestroy()
        {
            _game.GlobalUpdate.RemoveListener(DoUpdate);
        }

        private void DoUpdate()
        {
            if (_inputs.TryGetValue(Enums.InputType.Option1, out Enums.InputState input1) && input1 == Enums.InputState.Down)
            {
                if (CheckActionAvailable(_selectedActions[0]))
                    _encounterManager.PrepareSpecialAction(_selectedActions[0]);
                else
                    _events.NotEnoughEnergy.Invoke();
            }

            if (_inputs.TryGetValue(Enums.InputType.Option2, out Enums.InputState input2) && input2 == Enums.InputState.Down)
            {
                if (CheckActionAvailable(_selectedActions[1]))
                    _encounterManager.PrepareSpecialAction(_selectedActions[1]);
                else
                    _events.NotEnoughEnergy.Invoke();
            }

            if (_inputs.TryGetValue(Enums.InputType.Option3, out Enums.InputState input3) && input3 == Enums.InputState.Down)
            {
                if (CheckActionAvailable(_selectedActions[2]))
                    _encounterManager.PrepareSpecialAction(_selectedActions[2]);
                else
                    _events.NotEnoughEnergy.Invoke();
            }
        }

        private void ExecuteSpecialAction(EncounterAction action)
        {
            // TODO: Get spent energy smount per action type
            _player.SpendEnergy(20);
            _encounterManager.ResetSpecialActionTiles();
            _encounterManager.QueueAction(action, true);
        }

        private bool CheckActionAvailable(SpecialAction action)
        {
            // TODO: Get required energy per action type
            return _player.Energy >= 20;
        }
    }
}
