using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class CombatManager : MonoBehaviour
    {
        private bool _combatOnGoing = false;
        private CombatAction _currentAction;

        public bool CombatOnGoing => _combatOnGoing;

        public IEnumerator DoCombatAction(Entity attacker, Entity defender, string animationName = "")
        {
            _combatOnGoing = true;

            CombatAction action = attacker.GetComponent<AttackComponent>().GetAttackAction(attacker, defender, animationName);
            _currentAction = action;
            yield return StartCoroutine(action.Execute());

            _combatOnGoing = false;
        }

        //public IEnumerator DoCombatAction(Entity attacker, Entity defender, string animationName)
        //{
        //    _combatOnGoing = true;
        //    CombatAction action = attacker.GetComponent<AttackComponent>().GetAttackAction(attacker, defender, animationName);
        //    _currentAction = action;
        //    yield return StartCoroutine(action.Execute());
        //    _combatOnGoing = false;
        //}

        #region LEGACY
        //[SerializeField]
        //private PlayerAttackComponent _player;
        //[SerializeField]
        //private EnemyAttackComponent _enemy;

        //public void DoCombatAction(Entity attacker, Entity defender)
        //{
        //    StartCoroutine(ExecuteCombatAction(attacker, defender));
        //}

        //[ContextMenu("Start Combat")]
        //private void StartCombat()
        //{
        //    _combatOnGoing = true;
        //    StartCoroutine(DoCombat(_player.GetComponent<Entity>(), _enemy.GetComponent<Entity>()));
        //}

        //private IEnumerator DoCombat(Entity actor1, Entity actor2)
        //{
        //    while (actor1.Health > 0 && actor2.Health > 0)
        //    {
        //        if (actor1.Health > 0)
        //        {
        //            PlayerAttackAction action1 = new PlayerAttackAction(actor1, actor2);
        //            _currentAction = action1;
        //            yield return StartCoroutine(action1.Execute());
        //        }

        //        if (actor2.Health > 0)
        //        {
        //            EnemyAttackAction action2 = new EnemyAttackAction(actor2, actor1);
        //            _currentAction = action2;
        //            yield return StartCoroutine(action2.Execute());
        //        }
        //    }
        //    _combatOnGoing = false;
        //}

        //private void Start()
        //{
        //    StartCombat();
        //}


        #endregion
    }
}
