using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class EncounterAction_Attack : EncounterAction
    {
        protected Entity _attacker;
        protected Entity _defender;
        protected Enums.Direction _attackDirection;
        protected string _attackAnimationName;

        public EncounterAction_Attack(Entity attacker, Entity defender, string attackAnimationName = "")
        {
            _attacker = attacker;
            _defender = defender;
            _attackAnimationName = attackAnimationName;
            _attackDirection = Utils.GetDirectionFromTileToTile(_attacker.Tile, _defender.Tile);
        }

        public override IEnumerator Execute()
        {
            yield return _attacker.TurnEntity(_attackDirection);
            yield return Game.Instance.DoCombatAction(_attacker, _defender, _attackAnimationName);
        }
    }
}
