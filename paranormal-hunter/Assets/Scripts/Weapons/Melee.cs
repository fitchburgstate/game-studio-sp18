using System.Collections;
using UnityEngine;
using Hunter.Elements;

namespace Hunter.Character
{
    public class Melee : Weapon
    {
        public float windUpSpeed;
        public float hitBoxFrames;
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
            if (damageableObject == null)
            {
                return;
            }

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
        public void SwingMeleeWeapon ()
        {
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
