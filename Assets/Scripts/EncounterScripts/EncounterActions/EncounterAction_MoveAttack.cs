using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class EncounterAction_MoveAttack : EncounterAction
    {
        private Entity _target;

        public EncounterAction_MoveAttack(Entity actor, Entity target)
        {
            _actingEntity = actor;
            _target = target;
        }

        public override IEnumerator Execute()
        {
            int moveAmount = Utils.GetDistanceFromTileToTile(_actingEntity.Tile, _target.Tile);
            Enums.Direction moveDirection = Utils.GetDirectionFromTileToTile(_actingEntity.Tile, _target.Tile);
            for (int i = 0; i < moveAmount; i++)
            {
                EncounterAction_Move moveAction = new EncounterAction_Move(_actingEntity, moveDirection);
                yield return moveAction.Execute();
            }
            EncounterAction_Attack attackAction = new EncounterAction_Attack(_actingEntity, _target);
            yield return attackAction.Execute();
        }
    }
}
