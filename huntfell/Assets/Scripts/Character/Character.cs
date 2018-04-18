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
        /// <summary>
        /// How much health the character has
        /// </summary>
        //This needs to be a float for when we do the health bar
        [SerializeField]
        protected float health;

        [SerializeField]
        public int totalHealth = 100;

        /// <summary>
        /// Name of the Player, to be set in the inspector
        /// </summary>
        [SerializeField]
        private string displayName = "No Name";

        private Weapon currentWeapon = null;

        // Variables for handling character rotation
        public const string ROTATION_TRANSFORM_TAG = "Rotation Transform";

        private Transform rotationTransform;

        public Transform eyeLine;

        protected CharacterController characterController;

        protected NavMeshAgent agent;

        protected Animator anim;
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
        //Effects
        [HideInInspector]
        public EffectsController effectsController;
        protected bool invinvible = false;
        protected bool isDying = false;
        #endregion

        #region Unity Functions
        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            effectsController = GetComponentInChildren<EffectsController>();
            if (effectsController == null) { Debug.LogWarning($"{name} doesn't have an Effect Controller childed to it. No effects will play for it.", gameObject); }
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
                currentWeapon.weaponElement = element;
            }
        }

        public void TakeDamage(int damage, bool isCritical, Weapon weaponAttackedWith)
        {
            if (invinvible || isDying) { return; }
            if (effectsController != null)
            {
                //Dont apply hits particles for Dot Effects, kinda jank
                if (damage > 3)
                {
                    effectsController.StartDamageEffects(damage, isCritical, weaponAttackedWith?.weaponElement);
                }
                else
                {
                    effectsController.StartDamageEffects(damage);
                }
            }
            if (tag == "Player")
            {
                Fabric.EventManager.Instance.PostEvent("Player Hit", gameObject);
            }
            else if (tag == "Enemy")
            {
                Fabric.EventManager.Instance.PostEvent("Player Sword Hit", gameObject);
            }
            StartCoroutine(SubtractHealthFromCharacter(damage, isCritical));
        }

        protected virtual IEnumerator SubtractHealthFromCharacter(int damage, bool isCritical)
        {
            CurrentHealth -= damage;
            yield return null;
        }

        public virtual void RestoreHealthToCharacter(int restoreAmount)
        {
            StopCoroutine("SubtractHealthFromCharacter");
            CurrentHealth += restoreAmount;
        }
        #endregion
    }
}
