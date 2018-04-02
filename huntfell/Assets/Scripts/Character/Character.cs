using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Character
{
    [RequireComponent(typeof(CharacterController), typeof(NavMeshAgent), typeof(Animator))]
    public abstract class Character : MonoBehaviour, IDamageable
    {
        #region Variables / Properties
        /// <summary>
        /// Name of the Player, to be set in the inspector
        /// </summary>
        [SerializeField]
        private string displayName = "Nameless Being";
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// How much health the character has
        /// </summary>
        //This needs to be a float for when we do the health bar
        protected float health;
        public virtual float CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }
        [SerializeField]
        protected int totalHealth = 100;

        private Weapon currentWeapon = null;
        public Weapon CurrentWeapon
        {
            get
            {
                return currentWeapon;
            }
        }

        // Variables for handeling character rotation
        public const string ROTATION_TRANSFORM_TAG = "Rotation Transform";
        private Transform rotationTransform;
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

        public Transform eyeLine;

        protected CharacterController characterController;
        protected NavMeshAgent agent;
        protected Animator anim;
        #endregion

        protected virtual void Awake ()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            characterController = GetComponent<CharacterController>();
            CurrentHealth = totalHealth;
        }

        protected virtual void Start ()
        {

        }

        public void EquipWeaponToCharacter (Weapon weapon)
        {
            if (weapon != null)
            {
                currentWeapon = weapon;
                currentWeapon.characterHoldingWeapon = this;
            }
        }

        public void EquipElementToCharacter (Element element)
        {
            if (CurrentWeapon != null)
            {
                currentWeapon.weaponElement = element;
            }
        }

        public void TakeDamage (int damage, bool isCritical, Weapon weaponAttackedWith)
        {
            // This is also where we'll do the damage number pop up
            StartCoroutine(SubtractHealthFromCharacter(damage, isCritical));
        }

        protected virtual IEnumerator SubtractHealthFromCharacter (int damage, bool isCritical)
        {
            CurrentHealth -= damage;
            yield return null;
        }
    }
}
