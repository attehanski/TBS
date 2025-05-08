using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class BlockStateBehaviour : StateMachineBehaviour
    {
        private AttackComponent _attackComponent;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _attackComponent = animator.GetComponent<AttackComponent>();
            _attackComponent.Blocking = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            _attackComponent.Blocking = false;
        }
    }
}
