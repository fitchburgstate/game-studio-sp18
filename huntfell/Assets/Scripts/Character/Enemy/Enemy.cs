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
