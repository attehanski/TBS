using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    [CreateAssetMenu(fileName = "EnemyBehaviour_Basic", menuName = "TBS/Behaviours/EnemyBehaviour_Basic")]
    public class EnemyBehaviour_Basic : EntityBehaviour
    {
        [SerializeField]
        private float _aggroDistance;

        public override EncounterAction GetAction(Entity actor)
        {
            Enemy enemyActor = actor as Enemy;
            Tile currentTile = enemyActor.Tile;
            Tile playerTile = enemyActor.Player.Tile;
            if (enemyActor.Tile.IsNeighborOf(playerTile) && currentTile.Priority == playerTile.Priority)
            {
                return new EncounterAction_Attack(enemyActor, enemyActor.Player);
            }
            else if (Vector3.Distance(currentTile.Position(), playerTile.Position()) < _aggroDistance)
            {
                Stack<Tile> path = enemyActor.FindPath(currentTile, playerTile);
                if (path == null)
                    return null;

                Tile nextTileOnPath = path.Pop();
                Enums.Direction moveDirection = Utils.GetDirectionFromTileToTile(currentTile, nextTileOnPath);
                return new EncounterAction_Move(actor, moveDirection);
            }

            return null;
        }
    }
}
