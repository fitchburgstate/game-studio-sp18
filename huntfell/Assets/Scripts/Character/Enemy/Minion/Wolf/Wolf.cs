using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

namespace Hunter.Character
{
    public sealed class Wolf : Minion, IMoveable, IUtilityBasedAI
    {
        /// <summary>
        /// This is the speed at which the wolf walks.
        /// </summary>
        [Range(1, 10)]
        [Tooltip("The walking speed of the wolf when it is not in combat.")]
        public float walkSpeed = 2f;

        /// <summary>
        /// This is the speed at which the wolf runs.
        /// </summary>
        [Range(1, 10)]
        [Tooltip("The running speed of the wolf when it is in combat.")]
        public float runSpeed = 5f;

        private void Update()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject characterModel, NavMeshAgent agent)
        {
            // This function is not being used.
        }

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject characterModel, NavMeshAgent agent, Transform target)
        {
            var finalTarget = new Vector3(target.transform.position.x, characterModel.transform.localPosition.y, target.transform.position.z);
            agent.speed = runSpeed;
            agent.destination = finalTarget;
        }

        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented.
        }

        public void Idle(CharacterController controller, NavMeshAgent agent)
        {
            // This feature has not yet been implemented.
        }

        public void Wander(CharacterController controller, Vector3 target, NavMeshAgent agent)
        {
            agent.speed = walkSpeed;
            agent.destination = target;
        }
    }
}
