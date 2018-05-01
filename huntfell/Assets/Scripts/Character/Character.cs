using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    [RequireComponent(typeof(CharacterController), typeof(NavMeshAgent), typeof(Animator))]
    public abstract class Character : MonoBehaviour, IDamageable
    {
        #region Variables
        public const string ROTATION_TRANSFORM_TAG = "Rotation Transform";
        public const string EYELINE_TRANSFORM_TAG = "EyeLine Transform";

        [SerializeField]
        private string displayName = "No Name";
        // Super General Character Traits
        public int totalHealth = 100;
        [SerializeField]
        protected float currentHealth;

        private Weapon currentWeapon = null;

        // Class specific vars
        private Transform rotationTransform, eyeLineTransform;

        protected NavMeshAgent agent;
        protected Animator anim;
        protected VisualEffectsModule effectsModule;
        protected CharacterController characterController;
        protected bool invincible = false;

        // Actions
        protected IEnumerator deathAction;
        protected IEnumerator damageAction;
        protected IEnumerator restoreAction;

        #endregion

        #region Properties
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        public virtual float CurrentHealth
        {
            get
            {
                return currentHealth;
            }
            set
            {
                currentHealth = Mathf.Clamp(value, 0, totalHealth);
            }
        }

        public Weapon CurrentWeapon
        {
            get
            {
                return currentWeapon;
            }
        }

        public Transform RotationTransform
        {
            get
            {
                if (rotationTransform == null)
                {
                    foreach (Transform child in transform)
                    {
                        if (child.tag == ROTATION_TRANSFORM_TAG) { rotationTransform = child; }
                    }
                    // Fallback for if the tag isn't set
                    if (rotationTransform == null)
                    {
                        Debug.LogWarning("GameObject: " + gameObject.name + " has no rotational transform set. Check the tag of the first childed GameObject underneath this GameObject.", gameObject);
                        rotationTransform = transform.GetChild(0);
                    }
                }
                return rotationTransform;
            }
        }

        public Transform EyeLineTransform
        {
            get
            {
                if (eyeLineTransform == null)
                {
                    foreach (Transform child in RotationTransform)
                    {
                        if (child.tag == EYELINE_TRANSFORM_TAG) { eyeLineTransform = child; }
                    }
                    // Fallback for if the tag isn't set
                    if (eyeLineTransform == null)
                    {
                        Debug.LogWarning("GameObject: " + gameObject.name + " has no eyeline transform set. Check the tag of the first childed GameObject underneath the rotation root.", gameObject);
                        eyeLineTransform = RotationTransform.GetChild(0);
                    }
                }
                return eyeLineTransform;
            }
        }

        public virtual bool PerformingMajorAction
        {
            get
            {
                return false;
            }
        }

        public virtual bool PerformingMinorAction
        {
            get
            {
                return false;
            }
        }

        public bool IsDying
        {
            get
            {
                return deathAction != null;
            }
        }

        #endregion

        #region Unity Functions
        protected virtual void Awake ()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            effectsModule = GetComponentInChildren<VisualEffectsModule>();
            if (effectsModule == null) { Debug.LogWarning($"{name} doesn't have an Effect Controller childed to it. No effects will play for it.", gameObject); }
            currentHealth = totalHealth;
        }

        protected virtual void Start ()
        {

        }
        #endregion

        #region Combat Related Functions
        public void EquipWeaponToCharacter (Weapon weapon)
        {
            if (weapon != null)
            {
                if (currentWeapon != null)
                {
                    currentWeapon.gameObject.SetActive(false);
                }

                currentWeapon = weapon;
                currentWeapon.characterHoldingWeapon = this;
                currentWeapon.gameObject.SetActive(true);
            }
        }

        public void EquipElementToWeapon (Element element)
        {
            if (CurrentWeapon != null)
            {
                currentWeapon.WeaponElement = element;
            }
        }

        public void Damage (int damage, bool isCritical, Weapon weaponAttackedWith)
        {
            Damage(damage, isCritical, weaponAttackedWith.WeaponElement);
        }

        public void Damage (int damage, bool isCritical, Element damageElement)
        {
            if (invincible || IsDying) { return; }

            if (damage > 0)
            {
                if (damageAction != null)
                {
                    StopCoroutine(damageAction);
                }

                damageAction = SubtractHealthFromCharacter(damage, isCritical);
                StartCoroutine(damageAction);
            }

            if (effectsModule != null)
            {
                //Dont apply hits particles for dot effects or immunity
                effectsModule.StartDamageEffects(damage, isCritical, damageElement, (damage > 3));
            }
        }

        protected virtual IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            CurrentHealth -= damage;
            yield return null;
        }

        public void Heal (int restore, bool isCritical)
        {
            if (invincible || IsDying || CurrentHealth == totalHealth) { return; }

            if (isCritical && damageAction != null)
            {
                StopCoroutine(damageAction);
            }

            //if (restoreAction != null)
            //{
            //    StopCoroutine(restoreAction);
            //}

            restoreAction = AddHealthToCharacter(restore, isCritical);
            StartCoroutine(restoreAction);

            if (effectsModule != null)
            {
                effectsModule.StartHealEffects(restore, isCritical);
            }

        }

        protected virtual IEnumerator AddHealthToCharacter (int restoreAmount, bool isCritical)
        {
            CurrentHealth += restoreAmount;
            yield return null;
        }
        #endregion
    }
}
