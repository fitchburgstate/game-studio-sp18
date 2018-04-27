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

        // Super General Character Traits
        [SerializeField]
        protected float health;
        public int totalHealth = 100;

        [SerializeField]
        private string displayName = "No Name";

        private Weapon currentWeapon = null;

        // Class specific vars
        private Transform rotationTransform, eyeLineTransform;

        protected NavMeshAgent agent;
        protected Animator anim;
        protected EffectsModule effectsModule;
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
                return health;
            }
            set
            {
                health = Mathf.Clamp(value, 0, totalHealth);
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
        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            effectsModule = GetComponentInChildren<EffectsModule>();
            if (effectsModule == null) { Debug.LogWarning($"{name} doesn't have an Effect Controller childed to it. No effects will play for it.", gameObject); }
            CurrentHealth = totalHealth;
        }

        protected virtual void Start()
        {

        }
        #endregion

        #region Combat Related Functions
        public void EquipWeaponToCharacter(Weapon weapon)
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

        public void EquipElementToWeapon(Element element)
        {
            if (CurrentWeapon != null)
            {
                currentWeapon.WeaponElement = element;
            }
        }

        public virtual void TakeDamage(string damage, bool isCritical, Weapon weaponAttackedWith)
        {
            TakeDamage(damage, isCritical, weaponAttackedWith.WeaponElement);
        }

        public virtual void TakeDamage (string damage, bool isCritical, Element damageElement)
        {
            if (invincible || IsDying) { return; }

            int parseDamage = -1;
            if (int.TryParse(damage, out parseDamage))
            {
                if(damageAction != null)
                {
                    StopCoroutine(damageAction);
                }
                damageAction = SubtractHealthFromCharacter(parseDamage, isCritical);
                StartCoroutine(damageAction);
            }

            if (effectsModule != null)
            {
                //Dont apply hits particles for dot effects or minor damage
                effectsModule.StartDamageEffects(damage, isCritical, damageElement, (parseDamage > 3));
            }
        }

        protected virtual IEnumerator SubtractHealthFromCharacter(int damage, bool isCritical)
        {
            CurrentHealth -= damage;
            yield return null;
        }

        public virtual IEnumerator RestoreHealthToCharacter(int restoreAmount, bool isCritical)
        {
            if (isCritical)
            {
                StopCoroutine("SubtractHealthFromCharacter");
            }
            CurrentHealth += restoreAmount;
            yield return null;
        }
        #endregion
    }
}
