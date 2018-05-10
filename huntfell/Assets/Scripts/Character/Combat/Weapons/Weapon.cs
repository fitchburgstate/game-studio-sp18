using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

namespace Hunter.Characters
{
    public enum Finisher
    {
        Thrust = 0,
        Spin = 1,
        Smash = 2
    }

    public abstract class Weapon : MonoBehaviour
    {
        protected const float CRITICAL_HIT_MULTIPLIER = 1.5f;
        protected const float ELEMENTAL_WEAKNESS_MULTIPLIER = 1.5f;

        #region Variables
        [Header("Debug")]
        public ElementOption inspectorElementType;

        [Header("General Options")]
        public int baseDamage = 10;

        public int critPercent = 10;

        public float attackSpeed = 1;

        public float finisherAttackSpeed = 1.5f;

        public float recoverySpeed = 0.5f;

        public float finisherCooldown = 2;

        public Finisher finishingMove;

        public bool bigAttackEffect = false;

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

        protected virtual int CalculateDamage(Element weaponElement, Element enemyElementType, bool isCritical)
        {
            var critMult = isCritical ? CRITICAL_HIT_MULTIPLIER : 1.0f;
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
                    elementMult = ELEMENTAL_WEAKNESS_MULTIPLIER;
                }
            }
            var randomInt = UnityEngine.Random.Range(-2, 3);
            if (isCritical) { randomInt = Mathf.Abs(randomInt); }

            return ((int)((baseDamage + randomInt) * critMult * elementMult));
        }

        /// <summary>
        /// Calculates whether or not the player crits based on crit percentage that is given to the function.
        /// </summary>
        protected bool ShouldAttackBeCritical(int percent)
        {
            if (bigAttackEffect || percent == 100) { return true; }
            else if(percent == 0) { return false; }

            var rng = new RNGCryptoServiceProvider();
            var buffer = new byte[4];

            rng.GetBytes(buffer);

            var r = new System.Random();
            var num = r.Next(1, 100);
            return (num >= (100 - percent));
        }
    }
}
