using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public class Bat : Minion, IUtilityBasedAI
    {
        private void Update()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
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
