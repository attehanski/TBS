using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class EnemyAttackComponent : AttackComponent
    {
        [SerializeField]
        private string[] _attacks;

        public override string GetAttackName()
        {
            return _attacks[Random.Range(0, _attacks.Length)];
        }
    }
}
