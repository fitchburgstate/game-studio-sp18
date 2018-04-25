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
        #region Variables
        /// <summary>
        /// Determines whether the boss is already attacking or not.
        /// </summary>
        public bool isAttacking = false;
        #endregion

        #region Unity Functions
        protected override void FixedUpdate()
        {
            if (!inCombat)
            {
                if (enemy.CurrentHealth < tempHealth)
                {
                    inCombat = true;
                }
            }

            var distanceToPoint = DistanceToTarget(RandomPointTarget);
            var distanceToTarget = DistanceToTarget(Target.position);

            if (AiDetection != null)
            {
                enemyInLOS = AiDetection.DetectPlayer();
                enemyInVisionCone = AiDetection.InVisionCone;

                if (enemyInLOS)
                {
                    inCombat = true;
                }
                else if ((distanceToTarget > (AiDetection.maxDetectionDistance * 2)) && (!enemyInLOS))
                {
                    inCombat = false;
                }
            }

            var currentState = FindNextState(distanceToTarget, distanceToPoint);

#if UNITY_EDITOR
            Debug.Log(currentState);
            //Debug.Log("enemyInVisionCone: " + enemyInVisionCone);
#endif

            currentState.Act();

            #region State Switchers
            if (currentState is Attack)
            {
                inCombat = true;
            }
            else if (currentState is MoveTo)
            {
                inCombat = true;
            }
            else if (currentState is Turn)
            {
                inCombat = true;
            }
            else if (currentState is Retreat)
            {
                inCombat = true;
            }
            else if (currentState is Idle)
            {
                inCombat = false;
            }
            else if (currentState is Wander)
            {
                inCombat = false;
            }
            #endregion

            tempHealth = enemy.CurrentHealth;
        }
        #endregion

        #region FindNextState Function
        /// <summary>
        /// Finds the next most desired state by determining multiple factors present within the environment.
        /// </summary>
        public override UtilityBasedAI FindNextState(float distanceToTarget, float distanceToPoint)
        {
            var attackValue = 0f;
            var idleValue = 0f;
            var wanderValue = 0f;
            var turnValue = 0f;
            var moveToValue = 0f;

            if (idle) { idleValue = idleAction.CalculateIdle(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
            if (attack) { attackValue = attackAction.CalculateAttack(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (wander) { wanderValue = wanderAction.CalculateWander(distanceToPoint, urgeWeights.distanceToPointMax, inCombat); }
            if (turn) { turnValue = turnAction.CalculateTurn(urgeWeights.attackRangeMin, distanceToTarget, enemyInVisionCone, inCombat); }
            if (moveTo) { moveToValue = moveToAction.CalculateMoveTo(distanceToTarget, urgeWeights.distanceToTargetMin, urgeWeights.distanceToTargetMax, inCombat); }

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
            };
            var max = largestValue.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return max;
        }
        #endregion
    }
}
