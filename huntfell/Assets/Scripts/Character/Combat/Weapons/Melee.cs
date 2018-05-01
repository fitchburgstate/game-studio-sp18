using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Hunter.Characters
{
    [RequireComponent(typeof(BoxCollider))]
    public class Melee : Weapon
    {
        [Serializable]
        public struct EquipEffect
        {
            public ElementOption elementEffect;
            public List<Material> equipMaterials;
            public ParticleSystem equipParticleSystem;
        }

        #region Variables
        [Header("Melee Options")]
        public float hitBoxFrames = 5;
        public ParticleSystem swingParticleSystem;

        [Space]
        public List<EquipEffect> equipEffects = new List<EquipEffect>()
        {
            //new EquipEffect()
            //{
            //    elementEffect = ElementOption.None,
            //    equipMaterials = null,
            //    equipParticleSystem = null
            //}
        };

        public override Element WeaponElement
        {
            get
            {
                return weaponElement;
            }

            set
            {
                weaponElement = value;
                ActivateMeleeEffect(Utility.ElementToElementOption(weaponElement));
            }
        }


        private EquipEffect currentMeleeEffect;

        private MeshRenderer weaponRenderer;
        private List<Material> originalMaterials;

        private Collider meleeHitBox;
        #endregion

        protected void Awake ()
        {
            weaponRenderer = GetComponentInChildren<MeshRenderer>();
            if (weaponRenderer != null)
            {
                originalMaterials = new List<Material>(weaponRenderer.materials);
            }

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
            var totalDamage = CalculateDamage(WeaponElement, enemyElementType, isCritical);
            damageableObject.TakeDamage(totalDamage, isCritical, this);

            
        }

        /// <summary>
        /// Animation Event for Melee Weapon
        /// </summary>
        public override void StartAttackFromAnimationEvent ()
        {
            //Debug.Log("Swinging Melee Weapon.");
            StartCoroutine(OpenAndCloseHitBox());
        }

        private IEnumerator OpenAndCloseHitBox ()
        {
            EnableHitbox();
            for (var i = 0; i < hitBoxFrames; i++)
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

        private void ActivateMeleeEffect (ElementOption elementOption)
        {
            //Stop the current effects before we start the new ones
            if (currentMeleeEffect.equipParticleSystem != null) { currentMeleeEffect.equipParticleSystem.Stop(); }

            //Get the effect that matches the newly equipped element
            var newEquipEffect = equipEffects.FirstOrDefault(o => o.elementEffect == elementOption);
            currentMeleeEffect = newEquipEffect;
            
            //Start the new effects
            if(currentMeleeEffect.equipParticleSystem != null) { currentMeleeEffect.equipParticleSystem.Play(); }

            //This is the long / way more readable form of the code below
            //if(currentMeleeEffect.equipMaterials != null && currentMeleeEffect.equipMaterials.Count > 0)
            //{
            //    weaponRenderer.materials = currentMeleeEffect.equipMaterials.ToArray();
            //}
            //else
            //{
            //    weaponRenderer.materials = initialMaterials.ToArray();
            //}
            if (weaponRenderer != null)
            {
                weaponRenderer.materials = (currentMeleeEffect.equipMaterials != null && currentMeleeEffect.equipMaterials.Count > 0) ?
                    currentMeleeEffect.equipMaterials.ToArray() : originalMaterials.ToArray();
            }
        }
    }
}
