using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    public abstract class Enemy : Character
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

        /// <summary>
        /// Element Type of the Enemy
        /// </summary>
        public ElementType type;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown
        /// </summary>
        public OPTIONS elementType;

        /// <summary>
        /// Sets the element type of the weapon based upon the given options variable
        /// </summary>
        /// <param name="elementTypeOption">Option for the Element Type</param>
        public void SetElementType(OPTIONS elementTypeOption)
        {
            switch (elementTypeOption)
            {
                case OPTIONS.Fire:
                    type = new Elements.Fire();
                    break;
                case OPTIONS.Ice:
                    type = new Elements.Ice();
                    break;
                case OPTIONS.Disease:
                    type = new Elements.Disease();
                    break;
                case OPTIONS.Silver:
                    type = new Elements.Silver();
                    break;
                case OPTIONS.Blood:
                    type = new Elements.Blood();
                    break;
                case OPTIONS.Lightning:
                    type = new Elements.Lightning();
                    break;
                case OPTIONS.Mechanical:
                    type = new Elements.Mechanical();
                    break;
                case OPTIONS.Stone:
                    type = new Elements.Stone();
                    break;
            }
        }
    }
}
