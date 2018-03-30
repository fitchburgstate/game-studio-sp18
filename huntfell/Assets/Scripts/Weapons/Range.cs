using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Hunter.Elements;

namespace Hunter.Character
{
    public class Range : Weapon
    {
        /// <summary>
        /// Clip Size of the Weapon.
        /// </summary>
        public int clipSize;

        /// <summary>
        /// Reload Speed of the Weapon.
        /// </summary>
        public int reloadSpeed;

        /// <summary>
        /// Range of the Weapon (affects raycast length).
        /// </summary>
        public int weaponRange;

        /// <summary>
        /// Holds the distance between the weapon and enemy (used for falloff calculation).
        /// </summary>
        private float distanceBetweenWeaponAndEnemy;

        /// <summary>
        /// Holds the fall off ratio to determine damage based on distance travelled.
        /// </summary>
        private float damageFalloffRatio;

        /// <summary>
        /// Variable for holding the amount of shots fired.
        /// </summary>
        private int clipS = 0;

        /// <summary>
        /// Holds the distance between an enemy's "weapon" and the player (used for falloff calculation).
        /// </summary>
        private float playerRange;

        public Transform lastTarget;

        /// <summary>
        /// Performs a raycast to the range of the weapon and then calls the damage calculation method if an enemy is hit,
        /// also plays the animation for shooting.
        /// </summary>
        public override void StartAttackFromAnimationEvent()
        {
            var ray = new Ray();
            var hit = new RaycastHit();
            ray.origin = transform.position;
            ray.direction = characterHoldingWeapon.RotationTransform.forward;
            if (Physics.Raycast(ray, out hit, weaponRange))
            {
                var target = hit.transform;
                var damageableObject = target.GetComponent<IDamageable>();
                if (damageableObject == null) { return; }

                var enemy = target.GetComponent<Enemy>();
                Element enemyElementType = null;
                if (enemy != null) { enemyElementType = enemy.elementType; }

                var isCritical = ShouldAttackBeCritical(critPercent);
                var totalDamage = CalculateDamage(elementType, enemyElementType, isCritical);
                damageableObject.DealDamage(totalDamage, isCritical);

                Debug.Log(target.name + " hit!");
                lastTarget = target;
            }
            else
            {
                lastTarget = null;
            }
            CheckAmmo();
        }

        protected override int CalculateDamage(Element weaponElement, Element enemyElementType, bool isCritical)
        {
            var normalDamage = base.CalculateDamage(weaponElement, enemyElementType, isCritical);
            return normalDamage;
        }

        /// <summary>
        /// Increments clipS variable to determine when the reload happens.
        /// </summary>
        public void CheckAmmo()
        {
            clipS++;
            if (clipS == clipSize)
            {
                //reload animation
                //anim.Play(clip2.name);
                clipS = 0;
            }
        }

    }
}
