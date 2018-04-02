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
        public TrailRenderer tipTrail;
        private Collider meleeHitBox;
        #endregion

        protected new void Start ()
        {
            base.Start();
            meleeHitBox = GetComponent<BoxCollider>();
            if (tipTrail != null)
            {
                tipTrail.enabled = false;
            }
            DisableHitbox();
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
            damageableObject.TakeDamage(totalDamage, isCritical, weaponElement);

            if (characterHoldingWeapon.tag == "Player")
            {
                Fabric.EventManager.Instance.PostEvent("Player Sword Hit", gameObject);
            }

            if (characterHoldingWeapon.tag == "Enemy")
            {
                Fabric.EventManager.Instance.PostEvent("Player Hit", gameObject);
            }
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
            if (tipTrail != null) { tipTrail.enabled = true; }
            EnableHitbox();
            for (int i = 0; i < hitBoxFrames; i++)
            {
                yield return new WaitForEndOfFrame();
            }
            DisableHitbox();
            if (tipTrail != null) {
                yield return new WaitForSeconds(0.5f);
                tipTrail.enabled = false;
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
