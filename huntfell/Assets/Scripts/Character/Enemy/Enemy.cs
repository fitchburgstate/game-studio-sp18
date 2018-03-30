using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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
        public ElementOption inspectorElementType;


        /// <summary>
        /// Sets the element type of the weapon based upon the given options variable
        /// </summary>
        /// <param name="elementTypeOption">Option for the Element Type</param>
        public void SetElementType(ElementOption elementTypeOption)
        {
            switch (elementTypeOption)
            {
                case ElementOption.Fire:
                    elementType = new Element.Fire();
                    break;
                case ElementOption.Ice:
                    elementType = new Element.Ice();
                    break;
                case ElementOption.Silver:
                    elementType = new Element.Silver();
                    break;
                case ElementOption.Lightning:
                    elementType = new Element.Lightning();
                    break;
                case ElementOption.Nature:
                    elementType = new Element.Nature();
                    break;

            }
        }
    }
}
