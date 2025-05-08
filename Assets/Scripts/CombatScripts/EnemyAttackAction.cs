using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class EnemyAttackAction : CombatAction
    {

        public EnemyAttackAction(Entity actor, Entity target, string animationName) : base(actor, target, animationName)
        {

        }

        public override IEnumerator Execute()
        {
            return base.Execute();
        }
    }
}
