using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class PlayerAttackAction : CombatAction
    {
        public PlayerAttackAction(Entity actor, Entity target, string animationName) : base(actor, target, animationName) { }

        public override IEnumerator Execute()
        {
            yield return base.Execute();
        }
    }
}
