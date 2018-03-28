using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public sealed class Wolf : Minion, IMoveable, IUtilityBasedAI
    {

        public void Move(Transform target)
        {
            var finalTarget = new Vector3(target.transform.position.x, RotationTransform.transform.localPosition.y, target.transform.position.z);
            agent.speed = runSpeed;
            agent.destination = finalTarget;
        }

        public void Idle()
        {
            // This feature has not yet been implemented.
        }

        public void Wander(Vector3 target)
        {
            agent.speed = walkSpeed;
            agent.destination = target;
        }

        public void Move (Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            //fuck you
        }

        public void Dash ()
        {
            // no dash for wolfie boi
        }
    }
}
