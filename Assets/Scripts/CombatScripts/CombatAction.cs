using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class CombatAction
    {
        protected Entity _actor;
        protected Entity _target;
        protected string _animationName;

        protected AttackComponent _actorAttackComponent;
        protected AttackComponent _targetAttackComponent;
        protected bool _actionInProgress = false;
        protected bool _cameraMoveInProgress = true;
        protected float _damageMultiplier = 1f;

        private GameEvents _events => Game.Instance.GameEvents;

        public CombatAction(Entity actor, Entity target, string animationName)
        {
            _actor = actor;
            _actorAttackComponent = _actor.GetComponent<AttackComponent>();
            _actorAttackComponent.SetCombatAction(this);

            _target = target;
            _targetAttackComponent = target.GetComponent<AttackComponent>();
            _targetAttackComponent.SetCombatAction(this);

            _animationName = animationName;
            _events.CameraTargetChanged.AddListener(OnCameraTargetChanged);
        }

        public void ActionFinished()
        {
            _actionInProgress = false;
        }

        public virtual IEnumerator Execute()
        {
            _actionInProgress = true;

            _events.CombatActionStarted.Invoke(_actor, _target);
            while (_cameraMoveInProgress)
                yield return null;

            _actor.Animator.SetTrigger(_animationName);

            while (_actionInProgress)
                yield return null;

            _actorAttackComponent.SetCombatAction(null);
            _targetAttackComponent.SetCombatAction(null);
        }

        public void DealDamage(int damage)
        {
            _targetAttackComponent.ReceiveDamage(damage, _target);
        }

        public bool IsActionActor(AttackComponent attacker)
        {
            return _actorAttackComponent == attacker;
        }

        protected void OnCameraTargetChanged()
        {
            _cameraMoveInProgress = false;
        }
    }
}
