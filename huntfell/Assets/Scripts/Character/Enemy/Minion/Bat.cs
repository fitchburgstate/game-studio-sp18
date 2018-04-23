using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public class Bat : Minion, IMoveable, IUtilityBasedAI
    {
        #region Properties
        public override float CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0 && !isDying)
                {
                    //TODO Change this to reflect wether the death anim should be cinematic or not later
                    StartCoroutine(KillBat(true));
                    isDying = true;
                }
            }
        }
        #endregion

        #region Variables
        /// <summary>
        /// This is the speed at which the character runs.
        /// </summary>
        [Range(0, 20), Tooltip("The running speed of the character when it is in combat.")]
        public float speed = 2.5f;

        [Range(1, 250)]
        public float turnSpeed = 175f;
        #endregion

        protected override void Start()
        {
            base.Start();
            Fabric.EventManager.Instance?.PostEvent("Bat Start Wing Loop", gameObject);
            agent.updateRotation = false;
        }

        #region Bat Movement
        public void Move(Vector3 target, float finalSpeed)
        {
            if (isDying) { return; }
            var finalTarget = new Vector3(target.x, RotationTransform.localPosition.y, target.z);

            MoveToCalculations(turnSpeed, finalSpeed, finalTarget);
        }

        public void Wander(Vector3 target)
        {
            if (isDying) { return; }
            if (target != Vector3.zero)
            {
                Move(target, speed);
            }
        }
        #endregion

        #region Bat Combat
        private IEnumerator KillBat(bool isCinematic)
        {
            agent.speed = 0;
            agent.destination = transform.position;
            anim.SetTrigger("death");
            Fabric.EventManager.Instance?.PostEvent("Bat Stop Wing Loop", gameObject);
            minionHealthBarParent?.gameObject.SetActive(false);
            //TODO Change this later to reflect the animation time
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
        #endregion

        #region Unused Functions
        public void Idle()
        {
            if (isDying) { return; }
            // This should stay empty.
        }

        public void Move(Transform target)
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Turn(Transform target)
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Move(Vector3 moveDirection, Vector3 lookDirection, Vector3 animLookDirection)
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Dash()
        {
            if (isDying) { return; }
            // This feature will not be implemented.
        }

        public void Interact()
        {
            //Wolves cannot interact with stuff!
            return;
        }
        #endregion
    }
}
