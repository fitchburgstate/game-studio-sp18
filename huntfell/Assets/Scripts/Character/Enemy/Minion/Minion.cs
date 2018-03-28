using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Character
{
    public abstract class Minion : Enemy
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


        //Take this out of update and do the logic in the CurrentHealth Property
        private void Update ()
        {
            if (CurrentHealth <= 0)
            {
                //Call death function later on
                Destroy(gameObject);
            }
        }
    }
}
