using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Character.AI
{
    /// <summary>
    /// This class determines what action should be taken next based on an "urge".
    /// </summary>
    public abstract class UtilityBasedAI
    {
        public abstract void Act();
    }

    #region Attack Action
    /// <summary>
    /// This class controls when and how the AI will attack another character.
    /// </summary>
    public sealed class Attack : UtilityBasedAI
    {
        private GameObject aiGameObject;
        private IAttack aiIAttackComponent;

        public Attack(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
            aiIAttackComponent = aiGameObject.GetComponent<IAttack>();
        }

        public override void Act()
        {
            AttackAction(aiGameObject);
        }

        /// <summary>
        /// This function will calculate the urge to attack.
        /// </summary>
        public float CalculateAttack(float attackRange, float distanceToTarget, bool enemyInVisionCone, bool inCombat)
        {
            var attackUrgeTotal = 0f;

            if (inCombat)
            {
                if (distanceToTarget < attackRange)
                {
                    attackUrgeTotal += 100f;
                    if (!enemyInVisionCone)
                    {
                        attackUrgeTotal -= 20f;
                    }
                }
            }

            Mathf.Clamp(attackUrgeTotal, 0f, 100f);
            return attackUrgeTotal;
        }

        public void AttackAction(GameObject aiGameObject)
        {
            if (aiGameObject != null)
            {
                if (aiIAttackComponent != null)
                {
                    aiIAttackComponent.Attack();
                }
                else
                {
                    Debug.LogError("There is no IAttack component on this AI gameobject.", aiGameObject);
                }
            }
        }
    }
    #endregion

    #region Turn Action
    // This action exists outside of Attack because some mobs might exclusively use the turn feature, i.e. the Gargoyle
    public sealed class Turn : UtilityBasedAI
    {
        private GameObject aiGameObject;
        private AIInputModule aiInputModuleComponent;
        private IUtilityBasedAI aiIUtilityBasedAIComponent;

        public Turn(GameObject aiGameObject, Transform target)
        {
            this.aiGameObject = aiGameObject;
            aiInputModuleComponent = aiGameObject.GetComponent<AIInputModule>();
            aiIUtilityBasedAIComponent = aiGameObject.GetComponent<IUtilityBasedAI>();
        }

        public override void Act()
        {
            TurnAction(aiGameObject);
        }

        /// <summary>
        /// This function will calculate the urge to turn to a target.
        /// </summary>
        public float CalculateTurn(float attackRange, float distanceToTarget, bool enemyInVisionCone, bool inCombat)
        {
            var turnUrgeTotal = 0f;

            if (inCombat)
            {
                if (distanceToTarget < attackRange)
                {
                    turnUrgeTotal += 100f;
                    if (enemyInVisionCone)
                    {
                        turnUrgeTotal -= 20f;
                    }
                }
            }

            Mathf.Clamp(turnUrgeTotal, 0f, 100f);
            return turnUrgeTotal;
        }

        public void TurnAction(GameObject aiGameObject)
        {
            if (aiInputModuleComponent != null)
            {
                if (aiIUtilityBasedAIComponent != null)
                {
                    aiIUtilityBasedAIComponent.Turn(aiInputModuleComponent.Target);
                }
                else
                {
                    Debug.LogError("There is no IUtilityBasedAI component on this AI gameobject!", aiGameObject);
                }
            }
            else
            {
                Debug.LogError("There is no AIInputModule component on this AI gameobject!", aiGameObject);
            }
        }
    }
    #endregion

    #region MoveTo Action
    /// <summary>
    /// This class controls when and how the AI will move towards another character.
    /// </summary>
    public sealed class MoveTo : UtilityBasedAI
    {
        private GameObject aiGameObject;
        private AIInputModule aiInputModuleComponent;
        private IMoveable aiIMoveableComponent;

        public MoveTo(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
            aiInputModuleComponent = aiGameObject.GetComponent<AIInputModule>();
            aiIMoveableComponent = aiGameObject.GetComponent<IMoveable>();
        }

        public override void Act()
        {
            MoveToAction(aiGameObject);
        }

        /// <summary>
        /// This function will calculate the urge to move a target.
        /// </summary>
        public float CalculateMoveTo(float distanceToTarget, float distanceToTargetMin, float distanceToTargetMax, bool inCombat)
        {
            var moveToUrgeTotal = 0f;

            if (inCombat)
            {
                moveToUrgeTotal += Mathf.Clamp(distanceToTarget, distanceToTargetMin, distanceToTargetMax);
            }

            Mathf.Clamp(moveToUrgeTotal, 0f, 100f);
            return moveToUrgeTotal;
        }

        public void MoveToAction(GameObject aiGameObject)
        {
            if (aiInputModuleComponent != null)
            {
                if (aiIMoveableComponent != null)
                {
                    aiIMoveableComponent.Move(aiInputModuleComponent.Target);
                }
                else
                {
                    Debug.LogError("There is no IMoveable component on this AI gameobject!", aiGameObject);
                }
            }
            else
            {
                Debug.LogError("There is no AIInputModule component on this AI gameobject!", aiGameObject);
            }
        }
    }
    #endregion

    #region Retreat Action
    /// <summary>
    /// This class controls when and how the AI will retreat away from another character.
    /// </summary>
    public sealed class Retreat : UtilityBasedAI
    {
        private GameObject aiGameObject;

        public Retreat(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
        }

        public override void Act()
        {
            RetreatAction(aiGameObject);
        }

        /// <summary>
        /// This function will calculate the urge to retreat away from a target.
        /// </summary>
        public float CalculateRetreat(float currentHealth, bool inCombat)
        {
            var retreatUrgeTotal = 0f;

            if (inCombat)
            {

            }

            Mathf.Clamp(retreatUrgeTotal, 0f, 100f);
            return retreatUrgeTotal;
        }

        public void RetreatAction(GameObject aiGameObject)
        {
            // The retreat action will go here!
        }
    }
    #endregion

    #region Idle Action
    /// <summary>
    /// This class controls when and how the AI will idle.
    /// </summary>
    public sealed class Idle : UtilityBasedAI
    {
        private GameObject aiGameObject;
        private AIInputModule aiInputModuleComponent;

        public Idle(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
            aiInputModuleComponent = aiGameObject.GetComponent<AIInputModule>();
        }

        public override void Act()
        {
            IdleAction(aiGameObject);
        }

        /// <summary>
        /// This function will calculate the urge to idle when not in combat.
        /// </summary>
        public float CalculateIdle(float distanceToPoint, float distanceToPointMax, bool inCombat)
        {
            var idleUrgeTotal = 0f;

            if (!inCombat)
            {
                if (distanceToPoint < distanceToPointMax)
                {
                    idleUrgeTotal += 100f;
                }
            }

            Mathf.Clamp(idleUrgeTotal, 0f, 100f);
            return idleUrgeTotal;
        }

        public void IdleAction(GameObject aiGameObject)
        {
            if (aiInputModuleComponent != null)
            {
                aiInputModuleComponent.PointTarget = aiInputModuleComponent.FindPointOnNavmesh();
            }
            else
            {
                Debug.LogError("There is no AIInputModule component on this AI gameobject!", aiGameObject);
            }
        }
    }
    #endregion

    #region Wander Action
    /// <summary>
    /// This class controls when and how the AI will wander around.
    /// </summary>
    public sealed class Wander : UtilityBasedAI
    {
        private GameObject aiGameObject;
        private AIInputModule aiInputModuleComponent;
        private IUtilityBasedAI aiUtilityBasedAIComponent;

        public Wander(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
            aiInputModuleComponent = aiGameObject.GetComponent<AIInputModule>();
            aiUtilityBasedAIComponent = aiGameObject.GetComponent<IUtilityBasedAI>();
        }

        public override void Act()
        {
            WanderAction(aiGameObject);
        }

        /// <summary>
        /// This function will calculate the urge to wander around when not in combat.
        /// </summary>
        public float CalculateWander(float distanceToPoint, float distanceToPointMax, bool inCombat)
        {
            var wanderUrgeTotal = 0f;

            if (!inCombat)
            {
                if (distanceToPoint > distanceToPointMax)
                {
                    wanderUrgeTotal += 100f;
                }
            }

            Mathf.Clamp(wanderUrgeTotal, 0f, 100f);
            return wanderUrgeTotal;
        }

        public void WanderAction(GameObject aiGameObject)
        {
            if (aiUtilityBasedAIComponent != null)
            {
                if (aiInputModuleComponent != null)
                {
                    aiUtilityBasedAIComponent.Wander(aiInputModuleComponent.PointTarget);
                }
                else
                {
                    Debug.LogError("There is no IUtilityBasedAI component on this AI gameobject!", aiGameObject);
                }
            }
            else
            {
                Debug.LogError("There is no AIInputModule component on this AI gameobject!", aiGameObject);
            }
        }
    }
    #endregion
}
