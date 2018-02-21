using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public sealed class Wolf : Minion, IMoveable
    {
        // ---------- SET THESE IN THE INSPECTOR ---------- \\
        [Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)]
        public float speed = 5f;

        [Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 2,000.")]
        [Range(0, 2000)]
        public float rotateEnemy = 12f;

        private float speedRamp;
        public AnimationCurve rotateAnimation;
        // ------------------------------------------------ \\ 

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject enemyRoot, NavMeshAgent agent)
        {
            //moveDirection = transform.TransformDirection(moveDirection);
            //moveDirection *= speed;

            //agent.destination = enemyRoot.transform.position;
            //agent.updateRotation = false;

            //if (moveDirection.magnitude != 0 || finalDirection.magnitude != 0)
            //{
            //    var targetRotation = new Vector3(enemyRoot.transform.localEulerAngles.x, Mathf.Atan2(finalDirection.x, finalDirection.z) * Mathf.Rad2Deg, enemyRoot.transform.localEulerAngles.z);

            //    speedRamp = Mathf.Clamp(speedRamp + Time.deltaTime, 0, 1);
            //    var changeChar = rotateAnimation.Evaluate(speedRamp) * rotateEnemy;

            //    enemyRoot.transform.localRotation = Quaternion.RotateTowards(enemyRoot.transform.localRotation, Quaternion.Euler(targetRotation), changeChar);
            //}
            //else
            //{
            //    speedRamp = 0;
            //}

            //controller.Move(moveDirection * Time.deltaTime);
        }

        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented
        }
    }
}
