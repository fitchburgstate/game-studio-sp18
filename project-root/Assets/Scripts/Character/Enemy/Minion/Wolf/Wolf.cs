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
        public float rotateChar = 12f;

        private float speedRamp;

        public AnimationCurve rotateAnimation;
        // ------------------------------------------------ \\ 

        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 finalDirection, GameObject playerRoot, NavMeshAgent agent)
        {

        }

        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented
        }
    }
}

