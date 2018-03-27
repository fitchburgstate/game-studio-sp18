using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public sealed class Wolf : Minion, IMoveable, IUtilityBasedAI
    {
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
