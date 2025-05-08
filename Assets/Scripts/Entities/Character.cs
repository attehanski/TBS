using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class Character : Entity
    {
        [SerializeField]
        protected int _energy = 0;

        protected CombatValues _combatValues => Game.Instance.CombatValues;

        public void MoveInDirection(Enums.Direction direction, int steps = 1)
        {

        }

        public void ExecuteTurn()
        {

        }

        public override void OnHealthReachedZero()
        {
            Game.Instance.GameEvents.EntityDestroyed.Invoke(this);
            Animator.SetBool("Death", true);
        }

        public virtual void AddEnergy(int addition)
        {
            _energy = Mathf.Min(_energy + addition, _combatValues.MaxEnergy);
        }

        public virtual void SpendEnergy(int spent)
        {
            _energy = Mathf.Max(0, _energy - spent);
        }
    }
}
