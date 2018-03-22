using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Hunter.Elements;

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
        public Element elementType;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown
        /// </summary>
        public ElementOptions inspectorElementType;


        /// <summary>
        /// Sets the element type of the weapon based upon the given options variable
        /// </summary>
        /// <param name="elementTypeOption">Option for the Element Type</param>
        public void SetElementType(ElementOptions elementTypeOption)
        {
            switch (elementTypeOption)
            {
                case ElementOptions.Fire:
                    elementType = new Fire();
                    break;
                case ElementOptions.Ice:
                    elementType = new Ice();
                    break;
                case ElementOptions.Silver:
                    elementType = new Silver();
                    break;
                case ElementOptions.Lightning:
                    elementType = new Lightning();
                    break;
                case ElementOptions.Nature:
                    elementType = new Nature();
                    break;

            }
        }
    }
}
