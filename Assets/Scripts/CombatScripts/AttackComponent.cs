using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    [RequireComponent(typeof(Animator))]
    public abstract class AttackComponent : MonoBehaviour
    {
        [HideInInspector]
        public bool Blocking;

        protected CombatAction _currentCombatAction;
        protected Character _character;

        public virtual CombatAction GetAttackAction(Entity attacker, Entity defender, string animationName = "")
        {
            if (animationName == "")
                animationName = GetAttackName();
            return new CombatAction(attacker, defender, animationName);
        }

        public void SetCombatAction(CombatAction combatAction)
        {
            _currentCombatAction = combatAction;
        }

        public void AttackFinished()
        {
            if (_currentCombatAction != null)
                _currentCombatAction.ActionFinished();
        }

        // TODO: Doesn't really make sense to pass the entity here
        public virtual void ReceiveDamage(int damage, Entity targetEntity)
        {
            ApplyDefenceModifiers(ref damage);
            if (damage > 0)
                targetEntity.TakeDamage(damage);
        }

        protected virtual void AttackHit(int damage)
        {
            if (_currentCombatAction != null)
            {
                ApplyAttackModifiers(ref damage);
                _currentCombatAction.DealDamage(damage);
            }
        }

        protected void Start()
        {
            _character = GetComponent<Character>();
        }

        public abstract string GetAttackName();
        public virtual void SweetSpotHit() { }
        protected virtual void ApplyAttackModifiers(ref int damage) { }
        protected virtual void ApplyDefenceModifiers(ref int damage) { }
    }
}
