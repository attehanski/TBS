using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace TBS
{
    public class PlayerAttackComponent : AttackComponent
    {
        [SerializeField]
        protected string _attackAnimationName = "Attack1";
        [SerializeField]
        private ParticleSystem _critParticle;

        private bool _parrying = false;
        private bool _blocking = false;
        private bool _critting = false;

        private CombatValues _combatValues => Game.Instance.CombatValues;

        public override void SweetSpotHit()
        {
            _critParticle.Play();
        }

        public override string GetAttackName()
        {
            return _attackAnimationName;
        }

        public void CombatInput()
        {
            if (_currentCombatAction == null)
                return;

            if (_currentCombatAction.IsActionActor(this))
            {
                StartCoroutine(DoCrit());
            }
            else
            {
                StartCoroutine(DoBlocking());
            }
        }

        protected override void ApplyAttackModifiers(ref int damage)
        {
            if (_critting)
            {
                damage = (int)(damage * 2f);
                SweetSpotHit();
                _character.AddEnergy(_combatValues.EnergyGainedOnCrit);
            }
            _character.AddEnergy(_combatValues.EnergyGainedOnHit);
        }

        protected override void ApplyDefenceModifiers(ref int damage)
        {
            if (_parrying)
            {
                damage = 0;
                SweetSpotHit();
                _character.AddEnergy(_combatValues.EnergyGainedOnParry);
            }
            else if (_blocking)
            {
                damage = 0;
                _character.SpendEnergy(_combatValues.EnergyLostOnBlock);
            }
            _character.SpendEnergy(_combatValues.EnergyLostOnDamage);
        }

        private IEnumerator DoBlocking()
        {
            _character.Animator.SetTrigger("Block");
            _blocking = false;
            _parrying = true;
            yield return new WaitForSeconds(_combatValues.ParryDuration);
            _parrying = false;
            _blocking = true;
            yield return new WaitForSeconds(_combatValues.BlockDuration);
            _blocking = false;
        }

        private IEnumerator DoCrit()
        {
            _critting = true;
            yield return new WaitForSeconds(_combatValues.CritDuration);
            _critting = false;
        }
    }
}
