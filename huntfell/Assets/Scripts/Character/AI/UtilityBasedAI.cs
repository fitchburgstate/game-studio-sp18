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

    /// <summary>
    /// This class controls when and how the AI will attack another character.
    /// </summary>
    public sealed class Attack : UtilityBasedAI
    {
        private GameObject aiGameObject;

        public Attack(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
        }

        public override void Act()
        {
            AttackAction(aiGameObject);
        }

        /// <summary>
        /// This function will calculate the urge to attack.
        /// </summary>
        public float CalculateAttack(float attackRange, float distanceToTarget, bool inCombat)
        {
            var attackUrgeTotal = 0f;

            if (inCombat)
            {
                if (distanceToTarget < attackRange)
                {
                    attackUrgeTotal += 100f;
                }
            }

            Mathf.Clamp(attackUrgeTotal, 0f, 100f);
            return attackUrgeTotal;
        }

        public void AttackAction(GameObject aiGameObject)
        {
            var aiComponentModule = aiGameObject.GetComponent<AIInputModule>();
            aiComponentModule.GetComponent<IAttack>().Attack();
        }
    }

    /// <summary>
    /// This class controls when and how the AI will move towards another character.
    /// </summary>
    public sealed class MoveTo : UtilityBasedAI
    {
        private GameObject aiGameObject;

        public MoveTo(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
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
            var aiComponentModule = aiGameObject.GetComponent<AIInputModule>();
            aiGameObject.GetComponent<IMoveable>().Move(aiComponentModule.Target);
        }
    }

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
            //var aiComponentModule = aiGameObject.GetComponent<AIInputModule>();
        }
    }

    /// <summary>
    /// This class controls when and how the AI will idle.
    /// </summary>
    public sealed class Idle : UtilityBasedAI
    {
        private GameObject aiGameObject;

        public Idle(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
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
            var aiInputModule = aiGameObject.GetComponent<AIInputModule>();
            aiInputModule.PointTarget = aiInputModule.FindPointOnNavmesh();
        }
    }

    /// <summary>
    /// This class controls when and how the AI will wander around.
    /// </summary>
    public sealed class Wander : UtilityBasedAI
    {
        private GameObject aiGameObject;

        public Wander(GameObject aiGameObject)
        {
            this.aiGameObject = aiGameObject;
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
            var aiInputModule = aiGameObject.GetComponent<AIInputModule>();
            aiGameObject.GetComponent<IUtilityBasedAI>().Wander(aiInputModule.PointTarget);
        }
    }
}
