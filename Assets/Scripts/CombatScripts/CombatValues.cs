using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    [CreateAssetMenu(fileName = "CombatValues", menuName = "TBS/CombatValues")]
    public class CombatValues : ScriptableObject
    {
        [Header("Player Combat Values")]
        [SerializeField]
        private float _parryDuration;
        [SerializeField]
        private float _blockDuration;
        [SerializeField]
        private float _critDuration;
        [SerializeField]
        private int _energyLostOnBlock;
        [SerializeField]
        private int _energyLostOnDamage;
        [SerializeField]
        private int _energyGainedOnParry;
        [SerializeField]
        private int _energyGainedOnHit;
        [SerializeField]
        private int _energyGainedOnCrit;

        [Header("Generic Combat Values")]
        [SerializeField]
        private int _maxEnergy;


        public float ParryDuration => FramesToTime(_parryDuration);
        public float BlockDuration => FramesToTime(_blockDuration);
        public float CritDuration => FramesToTime(_critDuration);
        public int EnergyLostOnBlock => _energyLostOnBlock;
        public int EnergyLostOnDamage => _energyLostOnDamage;
        public int EnergyGainedOnParry => _energyGainedOnParry;
        public int EnergyGainedOnHit => _energyGainedOnHit;
        public int EnergyGainedOnCrit => _energyGainedOnCrit;
        public int MaxEnergy => _maxEnergy;

        private float FramesToTime(float frames)
        {
            return frames / 60;
        }
    }
}
