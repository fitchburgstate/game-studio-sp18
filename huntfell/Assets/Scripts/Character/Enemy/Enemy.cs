using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Hunter.Characters
{
    public abstract class Enemy : Character
    {
        /// <summary>
        /// Element Type of the Enemy
        /// </summary>
        public Element elementType;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown
        /// </summary>
        public ElementOption enemyElementOption;

        protected override void Start ()
        {
            elementType = Utility.ElementOptionToElement(enemyElementOption);
        }

        #region RotateTowardsTarget Function
        public void RotateTowardsTarget(Vector3 targetPoint, float turnSpeed)
        {
            if (isDying) { return; }
            var characterRoot = RotationTransform;
            var dir = targetPoint - transform.position;
            dir.Normalize();

            var yRotEuler = Quaternion.RotateTowards(characterRoot.localRotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime).eulerAngles.y;
            characterRoot.localRotation = Quaternion.Euler(0, yRotEuler, 0);
        }
        #endregion

        #region MoveToCalculations Function
        public void MoveToCalculations(float turnSpeed, float finalSpeed, Vector3 finalTarget)
        {
            var navMeshPath = new NavMeshPath();

            RotateTowardsTarget(agent.steeringTarget, turnSpeed);

            agent.CalculatePath(finalTarget, navMeshPath);
            if (navMeshPath != null)
            {
                if (navMeshPath.status == NavMeshPathStatus.PathComplete)
                {
                    agent.speed = finalSpeed;
                    agent.destination = finalTarget;
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathPartial)
                {
                    // Put code here to perform something as a backup
                    return;
                }
                else if (navMeshPath.status == NavMeshPathStatus.PathInvalid)
                {
                    return;
                }
            }
            else
            {
                Debug.LogError("The navmeshpath is null.", gameObject);
            }
        }
        #endregion
    }
}
