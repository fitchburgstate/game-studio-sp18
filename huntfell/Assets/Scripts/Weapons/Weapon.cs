using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using Hunter.Elements;

namespace Hunter.Character
{
    public abstract class Weapon : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Attack Speed of the Weapon.
        /// </summary>
        public float attackSpeed = 1;

        /// <summary>
        /// Recovery Speed of the Weapon a.k.a how fast before you can attack again.
        /// </summary>
        public float recoverySpeed = 0.5f;

        /// <summary>
        /// Base Damage number of the weapon.
        /// </summary>
        public int baseDamage = 10;

        /// <summary>
        /// Element type of the weapon.
        /// </summary>
        public Element elementType = null;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown.
        /// </summary>
        public ElementOptions inspectorElementType;

        /// <summary>
        /// Critical Percentage Given to the Weapon.
        /// </summary>
        public int critPercent = 10;

        [HideInInspector]
        public Character characterHoldingWeapon;
        #endregion

        protected void Start()
        {
            SetElementType(inspectorElementType);
        }

        /// <summary>
        /// Sets the element type of the weapon based upon the given options variable.
        /// </summary>
        /// <param name="elementType">Option for the Element Type</param>
        private void SetElementType(ElementOptions elementOption)
        {
            switch (elementOption)
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

        public abstract void StartAttackFromAnimationEvent ();

        protected virtual int CalculateDamage (Element weaponElement, Element enemyElementType, bool isCritical)
        {
            var critMult = 1;
            var elementMult = 1;

            if (enemyElementType != null)
            {
                Type weaponType = weaponElement.GetType();
                Type enemyType = enemyElementType.GetType();
                Type enemyWeaknessType = enemyElementType.Weakness;

                if (weaponType.Equals(enemyType))
                {
                    elementMult = 0;
                }
                else if (weaponType.Equals(enemyWeaknessType))
                {
                    elementMult = 2;
                }
            }

            return baseDamage * critMult * elementMult;
        }

        /// <summary>
        /// Calculates whether or not the player crits based on crit percentage that is given to the function.
        /// </summary>
        /// <param name="percent"></param>
        protected bool ShouldAttackBeCritical(int percent)
        {
            if(percent == 100) { return true; }
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[4];

            rng.GetBytes(buffer);

            System.Random r = new System.Random();
            var num = r.Next(1, 100);
            return (num >= (100 - percent));
        }
    }
}
