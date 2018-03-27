using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public class Bat : Minion, IUtilityBasedAI
    {
        
        public void Idle()
        {
            // This feature has not yet been implemented.
        }

        public void Wander(Vector3 target)
        {
            agent.speed = walkSpeed;
            agent.destination = target;
        }
    }
}
