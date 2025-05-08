using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class SpecialAction_MoveAttack : SpecialAction
    {
        protected int _moveAmount;

        public SpecialAction_MoveAttack(Entity actionEntity, int moveAmount) : base(actionEntity)
        {
            _moveAmount = moveAmount;
        }

        public override TileData GetActionTileData()
        {
            TileData tileData = new();
            GetActionTileDataRecursive(_actor.Tile, Utils.AllDirections(), _moveAmount, ref tileData);
            tileData.TileAction += InvokeSpecialAction;
            return tileData;
        }

        protected void GetActionTileDataRecursive(Tile startTile, List<Enums.Direction> directions, int recursionAmount, ref TileData tileData)
        {
            foreach (Enums.Direction direction in directions)
            {
                startTile.Neighbors.TryGetValue(direction, out Tile neighbor);
                if (neighbor == null || startTile.Priority != neighbor.Priority)
                    continue;

                Enums.TileState tileState = GetTileState(neighbor, _moveAmount - recursionAmount);
                tileData.TileStates.Add((neighbor, tileState));
                if (!neighbor.IsFree())
                    continue;

                List<Enums.Direction> newDirections = GetRecursionDirections(direction);
                if (recursionAmount > 0)
                    GetActionTileDataRecursive(neighbor, newDirections, recursionAmount - 1, ref tileData);
            }
        }

        protected List<Enums.Direction> GetRecursionDirections(Enums.Direction currentDirection)
        {
            return new List<Enums.Direction> { currentDirection };
        }

        protected Enums.TileState GetTileState(Tile tile, int recursionNumber)
        {
            bool allowInput = recursionNumber != 0;
            // TODO: Check if entity on tile can actually be attacked
            if (allowInput && tile.EntityOnTile != null)
                return Enums.TileState.Highlighted;

            return Enums.TileState.Unavailable;
        }

        protected override void InvokeSpecialAction(Tile tile)
        {
            EncounterAction action = new EncounterAction_MoveAttack(_actor, tile.EntityOnTile);
            OnActionInvoked.Invoke(action);
        }
    }
}
