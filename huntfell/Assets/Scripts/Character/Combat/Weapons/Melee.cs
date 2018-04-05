using System.Collections;
using UnityEngine;

namespace Hunter.Character
{
    [RequireComponent(typeof(BoxCollider))]
    public class Melee : Weapon
    {
        #region Variables
        //public float windUpSpeed;
        public float hitBoxFrames = 5;
        public ParticleSystem swingParticleSystem;
        private Collider meleeHitBox;
        #endregion

        protected void Awake ()
        {
            meleeHitBox = GetComponent<BoxCollider>();
            DisableHitbox();
        }

        protected override void Start ()
        {
            base.Start();
            if (swingParticleSystem != null)
            {
                swingParticleSystem.Stop();
            }
        }

        private void OnTriggerEnter (Collider target)
        {
            var damageableObject = target.GetComponent<IDamageable>();
            // We do not want to apply damage to any object that doesnt extend IDamageable, as well as whoever is holding the weapon
            if (damageableObject == null || target.gameObject == characterHoldingWeapon.gameObject)
            {
                return;
            }

            // Checking to see if the target is an Enemy because of elemental weaknesses
            var enemy = target.GetComponent<Enemy>();
            Element enemyElementType = null;
            if (enemy != null) { enemyElementType = enemy.elementType; }

            var isCritical = ShouldAttackBeCritical(critPercent);
            var totalDamage = CalculateDamage(weaponElement, enemyElementType, isCritical);
            damageableObject.TakeDamage(totalDamage, isCritical, this);

            
        }

        /// <summary>
        /// Animation Event for Melee Weapon
        /// </summary>
        public override void StartAttackFromAnimationEvent ()
        {
            Debug.Log("Swinging Melee Weapon.");
            StartCoroutine(OpenAndCloseHitBox());
        }

        private IEnumerator OpenAndCloseHitBox ()
        {
            EnableHitbox();
            for (int i = 0; i < hitBoxFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            DisableHitbox();
        }

        public void StartStopParticleSystem ()
        {
            if (swingParticleSystem.isStopped)
            {
                swingParticleSystem.Play();
            }
            else if (swingParticleSystem.isPlaying)
            {
                swingParticleSystem.Stop();
            }
        }

        /// <summary>
        /// Enables the hitbox of the equipped melee weapon.
        /// </summary>
        public void EnableHitbox ()
        {
            meleeHitBox.enabled = true;
        }

        /// <summary>
        /// Disables the hitbox of the equipped melee weapon.
        /// </summary>
        public void DisableHitbox ()
        {
            meleeHitBox.enabled = false;
        }
    }
}
