using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
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
        private GameObject controller;

        public Attack(GameObject controller)
        {
            this.controller = controller;
        }

        public override void Act()
        {
            AttackAction(controller);
        }

        /// <summary>
        /// This function will calculate the urge to attack.
        /// </summary>
        /// <param name="attackRange">The range that the target must be before it can attack.</param>
        /// <param name="inCombat">Boolean to determine if the AI is currently in combat.</param>
        /// <returns></returns>
        public float CalculateAttack(float attackRange, bool inCombat)
        {
            var attackUrgeTotal = 0f;

            if (inCombat)
            {
                attackUrgeTotal = attackRange;
            }

            Mathf.Clamp(attackUrgeTotal, 0f, 100f);

            return attackUrgeTotal;
        }

        public void AttackAction(GameObject controller)
        {
            //var aiComponentModule = controller.GetComponent<AIInputModule>();
            //aiComponentModule.GetComponent<IAttack>().Attack();
        }
    }

    /// <summary>
    /// This class controls when and how the AI will move towards another character.
    /// </summary>
    public sealed class MoveTo : UtilityBasedAI
    {
        private GameObject controller;

        public MoveTo(GameObject controller)
        {
            this.controller = controller;
        }

        public override void Act()
        {
            MoveToAction(controller);
        }

        /// <summary>
        /// This function will calculate the urge to move a target.
        /// </summary>
        /// <param name="distanceToTarget">The distance the AI is from the target.</param>
        /// <param name="distanceToTargetMin">The minimum distance the AI can be from the target.</param>
        /// <param name="distanceToTargetMax">The maximum distance the AI can be from the target.</param>
        /// <param name="inCombat">Boolean to determine if the AI is currently in combat.</param>
        /// <returns></returns>
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

        public void MoveToAction(GameObject controller)
        {
            var aiComponentModule = controller.GetComponent<AIInputModule>();
            controller.GetComponent<IMoveable>().Move(aiComponentModule.Controller, aiComponentModule.MoveDirection, aiComponentModule.LookDirection, aiComponentModule.EnemyModel, aiComponentModule.Agent, aiComponentModule.Target);
        }
    }

    /// <summary>
    /// This class controls when and how the AI will retreat away from another character.
    /// </summary>
    public sealed class Retreat : UtilityBasedAI
    {
        private GameObject controller;

        public Retreat(GameObject controller)
        {
            this.controller = controller;
        }

        public override void Act()
        {
            RetreatAction(controller);
        }

        /// <summary>
        /// This function will calculate the urge to retreat away from a target.
        /// </summary>
        /// <param name="canMoveAwayFromTarget">Is the AI able to move further away from a target, i.e. is it cornered or not?</param>
        /// <param name="canMoveAwayFromTargetValue">The amount that the urge total will go down if the AI is cornered.</param>
        /// <param name="currentHealth">The current health of the AI.</param>
        /// <param name="inCombat">Is the AI in combat with a target?</param>
        /// <returns></returns>
        public float CalculateRetreat(bool canMoveAwayFromTarget, float canMoveAwayFromTargetValue, float currentHealth, bool inCombat)
        {
            var retreatUrgeTotal = 0f;

            if (inCombat)
            {
                if (!canMoveAwayFromTarget)
                {
                    retreatUrgeTotal -= canMoveAwayFromTargetValue;
                }
            }

            Mathf.Clamp(retreatUrgeTotal, 0f, 100f);

            return retreatUrgeTotal;
        }

        public void RetreatAction(GameObject controller)
        {
            //var aiComponentModule = controller.GetComponent<AIInputModule>();
        }
    }

    /// <summary>
    /// This class controls when and how the AI will idle.
    /// </summary>
    public sealed class Idle : UtilityBasedAI
    {
        private GameObject controller;

        public Idle(GameObject controller)
        {
            this.controller = controller;
        }

        public override void Act()
        {
            IdleAction(controller);
        }

        /// <summary>
        /// This function will calculate the urge to idle when not in combat.
        /// </summary>
        /// <param name="hasJustIdled">Has the AI just idled as the last action?</param>
        /// <param name="hasJustIdledValue">The amount that the urge total will go down if the AI has just idled.</param>
        /// <param name="inCombat">Is the AI in combat with a target?</param>
        /// <returns></returns>
        public float CalculateIdle(bool hasJustIdled, float hasJustIdledValue, bool inCombat)
        {
            var idleUrgeTotal = 0f;

            if (!inCombat)
            {
                if (hasJustIdled)
                {
                    idleUrgeTotal -= hasJustIdledValue;
                }
                else if (!hasJustIdled)
                {
                    idleUrgeTotal += hasJustIdledValue;
                }
            }

            Mathf.Clamp(idleUrgeTotal, 0f, 100f);

            return idleUrgeTotal;
        }

        public void IdleAction(GameObject controller)
        {
            // The action for idling will go here.
        }
    }

    /// <summary>
    /// This class controls when and how the AI will wander around.
    /// </summary>
    public sealed class Wander : UtilityBasedAI
    {
        private GameObject controller;

        public Wander(GameObject controller)
        {
            this.controller = controller;
        }

        public override void Act()
        {
            WanderAction(controller);
        }

        /// <summary>
        /// This function will calculate the urge to wander around when not in combat.
        /// </summary>
        /// <param name="hasJustWandered">Has the AI just wandered as the last action?</param>
        /// <param name="hasJustWanderedValue">The amount that the urge total will go down if the AI has just wandered.</param>
        /// <param name="inCombat">Is the AI in combat with a target?</param>
        /// <returns></returns>
        public float CalculateWander(bool hasJustWandered, float hasJustWanderedValue, bool inCombat)
        {
            var wanderUrgeTotal = 0f;

            if (!inCombat)
            {
                if (hasJustWandered)
                {
                    wanderUrgeTotal -= hasJustWanderedValue;
                }
                else if (!hasJustWandered)
                {
                    wanderUrgeTotal += hasJustWanderedValue;
                }
            }

            Mathf.Clamp(wanderUrgeTotal, 0f, 100f);

            return wanderUrgeTotal;
        }

        public void WanderAction(GameObject controller)
        {
            // The action for wandering will go here.
        }
    }
}
