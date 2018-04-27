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
        [Header("Debug")]
        public ElementOption inspectorElementType;

        [Header("General Options")]
        public int baseDamage = 10;

        public int critPercent = 10; 

        public float attackSpeed = 1;

        public float recoverySpeed = 0.5f;

        [HideInInspector]
        public Character characterHoldingWeapon;
        protected Element weaponElement = null;

        public virtual Element WeaponElement
        {
            get
            {
                return weaponElement;
            }

            set
            {
                weaponElement = value;
            }
        }
        #endregion

        protected virtual void Start()
        {
            WeaponElement = Utility.ElementOptionToElement(inspectorElementType);
        }

        public abstract void StartAttackFromAnimationEvent();

        protected virtual string CalculateDamage(Element weaponElement, Element enemyElementType, bool isCritical)
        {
            var critMult = isCritical ? 1.5f : 1.0f;
            var elementMult = 1.0f;

            if (enemyElementType != null && weaponElement != null)
            {
                var weaponType = weaponElement.GetType();
                var enemyType = enemyElementType.GetType();
                var enemyWeaknessType = enemyElementType.Weakness;

                if (weaponType.Equals(enemyType))
                {
                    return "Immune";
                }
                else if (weaponType.Equals(enemyWeaknessType))
                {
                    elementMult = 1.5f;
                }
            }
            var randomInt = UnityEngine.Random.Range(-1, 2);

            return ((int)((baseDamage + randomInt) * critMult * elementMult)).ToString();
        }

        /// <summary>
        /// Calculates whether or not the player crits based on crit percentage that is given to the function.
        /// </summary>
        protected bool ShouldAttackBeCritical(int percent)
        {
            if (percent == 100) { return true; }
            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[4];

            rng.GetBytes(buffer);

            var r = new System.Random();
            var num = r.Next(1, 100);
            return (num >= (100 - percent));
        }
    }
}
