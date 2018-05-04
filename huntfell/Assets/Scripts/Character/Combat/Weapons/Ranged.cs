using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Hunter.Characters
{
    public class Ranged : Weapon
    {
        #region Variables
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

        public float weaponTrailDuration = .05f;

        public Material gunTrailMaterial;

        public Gradient weaponTrailColor;

        private IEnumerator weaponTrailCR;

        [Tooltip("If left blank, the trail will use Particles/Additive on it's own.")]
        private LineRenderer lineMaterial;

        private Vector3 endOfRay;

        private Vector3 startPoint;
        #endregion

        protected override void Start()
        {
            base.Start();
            startPoint = transform.position;
        }

        #region StartAttackFromAnimationEvent Function
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
                    var totalDamage = CalculateDamage(WeaponElement, enemyElementType, isCritical);
                    damageableObject.Damage(totalDamage, isCritical, this);
                }
                endOfRay = hit.point;
            }
            else
            {
                endOfRay = ray.GetPoint(weaponRange);
            }

            muzzleFlash?.Play();

            if (usesTrail && weaponTrailCR == null)
            {
                weaponTrailCR = MakeGunTrail(startPoint, endOfRay);
                StartCoroutine(weaponTrailCR);
            }
        }
        #endregion

        #region CalculateDamage Function
        protected override int CalculateDamage(Element weaponElement, Element enemyElementType, bool isCritical)
        {
            var normalDamage = base.CalculateDamage(weaponElement, enemyElementType, isCritical);
            return normalDamage;
        }
        #endregion

        #region CheckAmmo Function
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
        #endregion

        #region MakeGunTrail Function
        private IEnumerator MakeGunTrail(Vector3 startPoint, Vector3 endPoint)
        {
            var weaponTrail = gameObject.AddComponent<LineRenderer>();
            var width = 0.05f;
            var initWeaponTrailPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            weaponTrail.SetPositions(initWeaponTrailPositions);

            startPoint = transform.position;
            endPoint = Vector3.zero;

            if (endOfRay != Vector3.zero)
            {
                endPoint = endOfRay;
            }
            else
            {
                endPoint = startPoint + (weaponRange * transform.forward);
            }

            weaponTrail.SetPosition(0, startPoint);
            weaponTrail.SetPosition(1, endPoint);

            weaponTrail.colorGradient = weaponTrailColor;

            if (gunTrailMaterial == null)
            {
                weaponTrail.material = new Material(Shader.Find("Particles/Additive"));
            }
            else
            {
                weaponTrail.material = gunTrailMaterial;
            }

            weaponTrail.startWidth = width;
            weaponTrail.endWidth = width;
            weaponTrail.positionCount = 2;
            weaponTrail.numCapVertices = 1;
            weaponTrail.useWorldSpace = true;

            yield return new WaitForSeconds(weaponTrailDuration);
            Destroy(weaponTrail);
            weaponTrailCR = null;
            endOfRay = Vector3.zero;

            if (weaponTrailCR != null)
            {
                StopCoroutine(weaponTrailCR);
            }
        }
        #endregion
    }
}
