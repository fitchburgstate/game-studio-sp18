using System.Collections;
using UnityEngine;
using Hunter.Elements;

namespace Hunter.Character
{
    [RequireComponent(typeof(BoxCollider))]
    public class Melee : Weapon
    {
        //public float windUpSpeed;
        public float hitBoxFrames = 5;
        private Collider meleeHitBox;

        protected new void Start ()
        {
            base.Start();
            meleeHitBox = GetComponent<BoxCollider>();
            DisableHitbox();
        }

        private void OnTriggerEnter (Collider target)
        {
            var damageableObject = target.GetComponent<IDamageable>();
            //We do not want to apply damage to any object that doesnt extend IDamageable, as well as whoever is holding the weapon
            if (damageableObject == null || target.gameObject == characterHoldingWeapon.gameObject)
            {
                return;
            }

            //Checking to see if the target is an Enemy because of elemental weaknesses
            Enemy enemy = target.GetComponent<Enemy>();
            Element enemyElementType = null;
            if (enemy != null) { enemyElementType = enemy.elementType; }

            bool isCritical = ShouldAttackBeCritical(critPercent);
            int totalDamage = CalculateDamage(elementType, enemyElementType, isCritical);
            damageableObject.DealDamage(totalDamage, isCritical);
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
