using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class EncounterAction_Move : EncounterAction
    {
        protected Enums.Direction _moveDirection;

        public EncounterAction_Move(Entity actor, Enums.Direction moveDirection)
        {
            _actingEntity = actor;
            _moveDirection = moveDirection;
        }

        public override IEnumerator Execute()
        {
            yield return _actingEntity.TurnEntity(_moveDirection);
            yield return MoveEntity();
        }

        // TODO: Handle animations etc
        protected IEnumerator MoveEntity()
        {
            Tile currentTile = _actingEntity.Tile;
            Tile newTile = currentTile.GetNeighbor(_moveDirection);
            if (newTile == null || !currentTile.CanMoveTo(newTile))
                yield break;

            float moveDuration = 0.3f; // TODO: Move somewhere for easy editing
            float timeMoved = 0f;
            while (timeMoved < moveDuration)
            {
                _actingEntity.SetPosition(Vector3.Lerp(currentTile.Position(), newTile.Position(), timeMoved / moveDuration));
                timeMoved += Time.deltaTime;
                yield return null;
            }

            newTile.SetEntity(_actingEntity);
            currentTile.RemoveEntity();
            _actingEntity.SetToTile(newTile);
        }

    }
}
