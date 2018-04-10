using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hunter.Characters
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

        public ParticleSystem muzzleFlash;

        public LayerMask rangedValidLayers;
        [Header("Weapon Trail Options")]
        public bool usesTrail;
        public Material lineMaterial;
        public float weaponTrailDuration = .05f;

        public Gradient weaponTrailColor;

        private LineRenderer weaponTrail;
        //private IEnumerator weaponTrailCR;

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
            var endOfRay = Vector3.zero;

            Debug.DrawRay(ray.origin, ray.direction * weaponRange, Color.green, 5);

            if (Physics.Raycast(ray, out hit, weaponRange, rangedValidLayers))
            {
                var target = hit.transform;
                var damageableObject = target.GetComponent<IDamageable>();
                if (damageableObject != null)
                {
                    var enemy = target.GetComponent<Enemy>();
                    Element enemyElementType = null;
                    if (enemy != null) { enemyElementType = enemy.elementType; }

                    var isCritical = ShouldAttackBeCritical(critPercent);
                    var totalDamage = CalculateDamage(weaponElement, enemyElementType, isCritical);
                    damageableObject.TakeDamage(totalDamage, isCritical, this);
                }
                endOfRay = hit.point;
            }
            else
            {
                endOfRay = ray.GetPoint(weaponRange);
            }
            muzzleFlash?.Play();
            //if (usesTrail && weaponTrailCR == null)
            //{
            //    var startPoint = (muzzleFlash != null) ? muzzleFlash.transform.position : transform.position;
            //    weaponTrailCR = MakeGunTrail(startPoint, endOfRay);
            //    StartCoroutine(weaponTrailCR);
            //}
            //CheckAmmo();
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
                clipS = 0;
            }
        }

        private IEnumerator MakeGunTrail(Vector3 startPoint, Vector3 endPoint)
        {
            if(weaponTrail == null)
            {
                weaponTrail = gameObject.AddComponent<LineRenderer>();
            }
            else
            {
                weaponTrail.enabled = true;
            }
            var width = 0.05f;
            var initWeaponTrailPositions = new Vector3[2] { startPoint, endPoint };
            weaponTrail.SetPositions(initWeaponTrailPositions);
            weaponTrail.colorGradient = weaponTrailColor;

            weaponTrail.material = lineMaterial;
            weaponTrail.startWidth = width;
            weaponTrail.endWidth = width;
            weaponTrail.positionCount = 2;
            weaponTrail.numCapVertices = 1;
            weaponTrail.useWorldSpace = true;

            yield return new WaitForSeconds(weaponTrailDuration);
            //weaponTrailCR = null;
            weaponTrail.enabled = false;
        }
    }
}
