using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class SpecialAction_MultiAttack : SpecialAction
    {
        public SpecialAction_MultiAttack(Entity actingEntity) : base(actingEntity) { }

        public override TileData GetActionTileData()
        {
            TileData tileData = new();
            tileData.TileAction += InvokeSpecialAction;

            foreach (Enums.Direction direction in Utils.AllDirections())
            {
                _actor.Tile.Neighbors.TryGetValue(direction, out Tile neighbor);
                if (neighbor == null)
                    continue;

                Enums.TileState tileState;
                if (neighbor.EntityOnTile != null && _actor.Tile.Priority == neighbor.Priority)
                    tileState = Enums.TileState.Highlighted;
                else
                    tileState = Enums.TileState.Unavailable;

                tileData.TileStates.Add((neighbor, tileState));
            }

            return tileData;
        }

        protected override void InvokeSpecialAction(Tile tile)
        {
            EncounterAction action = new EncounterAction_Attack(_actor, tile.EntityOnTile, "Attack3"); // Add multi attack here
            OnActionInvoked.Invoke(action);
        }
    }
}
