using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

namespace Hunter.Characters
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
        public Element weaponElement = null;

        /// <summary>
        /// Options variable for Unity Inspector Dropdown.
        /// </summary>
        public ElementOption inspectorElementType;

        /// <summary>
        /// Critical Percentage Given to the Weapon.
        /// </summary>
        public int critPercent = 10;

        [HideInInspector]
        public Character characterHoldingWeapon;
        #endregion

        protected virtual void Start()
        {
            weaponElement = Utility.ElementOptionToElement(inspectorElementType);
        }

        public abstract void StartAttackFromAnimationEvent();

        protected virtual int CalculateDamage(Element weaponElement, Element enemyElementType, bool isCritical)
        {
            var critMult = isCritical ? 1.3f : 1.0f;
            var elementMult = 1.0f;

            if (enemyElementType != null && weaponElement != null)
            {
                var weaponType = weaponElement.GetType();
                var enemyType = enemyElementType.GetType();
                var enemyWeaknessType = enemyElementType.Weakness;

                if (weaponType.Equals(enemyType))
                {
                    elementMult = 0;
                }
                else if (weaponType.Equals(enemyWeaknessType))
                {
                    elementMult = 1.3f;
                }
            }
            var randomInt = UnityEngine.Random.Range(-1, 3);

            return (int)((baseDamage + randomInt) * critMult * elementMult);
        }

        /// <summary>
        /// Calculates whether or not the player crits based on crit percentage that is given to the function.
        /// </summary>
        /// <param name="percent"></param>
        protected bool ShouldAttackBeCritical(int percent)
        {
            if (percent == 100) { return true; }
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[4];

            rng.GetBytes(buffer);

            System.Random r = new System.Random();
            var num = r.Next(1, 100);
            return (num >= (100 - percent));
        }
    }
}
