using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class Player : Character
    {
        [SerializeField]
        private Transform _cameraFollowTarget;

        public Transform CameraFollowTarget => _cameraFollowTarget;
        public int Energy => _energy;

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _events.PlayerHealthChanged.Invoke((float)_health / _maxHealth);
        }

        public override void AddEnergy(int addition)
        {
            base.AddEnergy(addition);
            _events.PlayerEnergyChanged.Invoke((float)_energy / _combatValues.MaxEnergy);
        }

        public override void SpendEnergy(int spent)
        {
            base.SpendEnergy(spent);
            _events.PlayerEnergyChanged.Invoke((float)_energy / _combatValues.MaxEnergy);
        }
        public override void OnHealthReachedZero()
        {
            base.OnHealthReachedZero();
            _events.OnPlayerDeath.Invoke();
        }

        protected override void Start()
        {
            base.Start();
            _events.PlayerEnergyChanged.Invoke((float)_energy / _combatValues.MaxEnergy);
        }
    }
}
