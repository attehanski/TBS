using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class PlayerController : MonoBehaviour
    {
        private Entity _playerEntity;
        private PlayerAttackComponent _playerAttack;
        private bool _directionalInputPressed = false;

        private Game _game => Game.Instance;

        private void Awake()
        {
            _playerEntity = GetComponent<Entity>();
            _playerAttack = GetComponent<PlayerAttackComponent>();
            _game.GlobalUpdate.AddListener(DoUpdate);
        }

        private void OnDestroy()
        {
            _game.GlobalUpdate.RemoveListener(DoUpdate);
        }

        private void DoUpdate()
        {

        }

        // TODO: Remove default true from inEncounter
        public void HandleInput(Dictionary<Enums.InputType, Enums.InputState> inputs, bool inEncounter = true)
        {
            if (inEncounter)
                HandleEncounterInput(inputs);
            else
                HandleGenericInput(inputs);
        }

        private void HandleGenericInput(Dictionary<Enums.InputType, Enums.InputState> inputs)
        {

        }

        private void HandleEncounterInput(Dictionary<Enums.InputType, Enums.InputState> inputs)
        {
            if (!_directionalInputPressed)
            {
                if (inputs.TryGetValue(Enums.InputType.Right, out Enums.InputState rightInput) && rightInput == Enums.InputState.Held)
                    EncounterDirectionalInput(Enums.Direction.Right);
                else if (inputs.TryGetValue(Enums.InputType.Left, out Enums.InputState leftInput) && leftInput == Enums.InputState.Held)
                    EncounterDirectionalInput(Enums.Direction.Left);
                else if (inputs.TryGetValue(Enums.InputType.Up, out Enums.InputState upInput) && upInput == Enums.InputState.Held)
                    EncounterDirectionalInput(Enums.Direction.Up);
                else if (inputs.TryGetValue(Enums.InputType.Down, out Enums.InputState downInput) && downInput == Enums.InputState.Held)
                    EncounterDirectionalInput(Enums.Direction.Down);
            }
            else if (!inputs.ContainsKey(Enums.InputType.Right) && !inputs.ContainsKey(Enums.InputType.Left) && !inputs.ContainsKey(Enums.InputType.Up) &&
                !inputs.ContainsKey(Enums.InputType.Down))
                _directionalInputPressed = false; 

            if (inputs.TryGetValue(Enums.InputType.Attack, out Enums.InputState interactState) && interactState == Enums.InputState.Down)
            {
                if (_game.CurrentlyInCombat())
                    _playerAttack.CombatInput();
            }
        }

        private void EncounterDirectionalInput(Enums.Direction direction)
        {
            _directionalInputPressed = true;
            _game.QueuePlayerAction(_playerEntity, direction);
        }
    }
}
