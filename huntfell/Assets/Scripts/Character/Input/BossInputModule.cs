using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEditor;

namespace Hunter.Characters.AI
{
    public class BossInputModule : AIInputModule
    {
        [HideInInspector]
        public bool isAttacking = false;

        #region Unity Functions
        protected override void Start()
        {
            base.Start();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        #endregion

        public override UtilityBasedAI FindNextState(float distanceToTarget, float distanceToPoint)
        {
            var attackValue = 0f;
            var idleValue = 0f;
            var wanderValue = 0f;
            var turnValue = 0f;
            var moveToValue = 0f;
            var retreatValue = 0f;

            if (attack) { attackValue = attackAction.CalculateAttack(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (!isAttacking)
            {
                if (idle) { idleValue = idleAction.CalculateIdle(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
                if (wander) { wanderValue = wanderAction.CalculateWander(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
                if (turn) { turnValue = turnAction.CalculateTurn(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
                if (moveTo) { moveToValue = moveToAction.CalculateMoveTo(distanceToTarget, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat); }
                if (retreat) { retreatValue = retreatAction.CalculateRetreat(enemy.CurrentHealth, inCombat); }
            }

            #region Debug Logs
#if UNITY_EDITOR
            if (Selection.Contains(gameObject))
            {
                //Debug.Log("Attack Value: " + attackValue);
                //Debug.Log("Idle Value: " + idleValue);
                //Debug.Log("Wander Value: " + wanderValue);
                //Debug.Log("Turn Value: " + turnValue);
                //Debug.Log("MoveTo Value: " + moveToValue);
                //Debug.Log("Retreat Value: " + retreatValue);
            }
#endif
            #endregion

            var largestValue = new Dictionary<UtilityBasedAI, float>
            {
                { attackAction, attackValue },
                { idleAction, idleValue },
                { wanderAction, wanderValue },
                { turnAction, turnValue },
                { moveToAction, moveToValue },
                { retreatAction, retreatValue }
            };
            var max = largestValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return max;
        }
    }
}
