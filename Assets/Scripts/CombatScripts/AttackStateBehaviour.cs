using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class AttackStateBehaviour : StateMachineBehaviour
    {
        private AttackComponent _attackComponent;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            _attackComponent = animator.GetComponent<AttackComponent>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            _attackComponent.AttackFinished();
        }
    }
}
