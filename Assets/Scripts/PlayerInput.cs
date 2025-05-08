using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class PlayerInput
    {
        private Dictionary<Enums.InputType, Enums.InputState> _inputs = new Dictionary<Enums.InputType, Enums.InputState>();

        public Dictionary<Enums.InputType, Enums.InputState> Inputs => _inputs;

        public float GetHorizontalInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }

        public void DoUpdate()
        {
            _inputs.Clear();
            // TODO: Handle buttonDown and buttonUp for axes?
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            if (horizontalInput > 0)
                _inputs.Add(Enums.InputType.Right, Enums.InputState.Held);
            else if (horizontalInput < 0)
                _inputs.Add(Enums.InputType.Left, Enums.InputState.Held);

            float verticalInput = Input.GetAxisRaw("Vertical");
            if (verticalInput > 0)
                _inputs.Add(Enums.InputType.Up, Enums.InputState.Held);
            else if (verticalInput < 0)
                _inputs.Add(Enums.InputType.Down, Enums.InputState.Held);

            UpdateInput("Cancel", Enums.InputType.Cancel);
            UpdateInput("Interact", Enums.InputType.Interact);
            UpdateInput("Attack", Enums.InputType.Attack);
            UpdateInput("Option1", Enums.InputType.Option1);
            UpdateInput("Option2", Enums.InputType.Option2);
            UpdateInput("Option3", Enums.InputType.Option3);
        }

        private void UpdateInput(string inputButtonName, Enums.InputType inputType)
        {
            if (Input.GetButtonDown(inputButtonName))
                _inputs.Add(inputType, Enums.InputState.Down);
            else if (Input.GetButton(inputButtonName))
                _inputs.Add(inputType, Enums.InputState.Held);
            else if (Input.GetButtonUp(inputButtonName))
                _inputs.Add(inputType, Enums.InputState.Up);
        }
    }
}
