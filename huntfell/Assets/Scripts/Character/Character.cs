using System.Collections;
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
        protected float currentHealth, targetHealth;
        [Tooltip("The multiplier for how fast the wound bar should subtract health from the Player."), Range(0.1f, 10f)]
        public float healthModificationSpeed = 1;

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
                //if (IsDying) { return; }
                currentHealth = Mathf.Clamp(value, TargetHealth, totalHealth);

                if (currentHealth <= 0)
                {
                    Kill();
                }
            }
        }

        public virtual float TargetHealth
        {
            get
            {
                return targetHealth;
            }
            set
            {
                //if (IsDying) { return; }
                targetHealth = Mathf.Clamp(value, 0, totalHealth);
                if (targetHealth > currentHealth)
                {
                    CurrentHealth = targetHealth;
                }
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
            effectsModule = GetComponentInChildren<VisualEffectsModule>();
            if (effectsModule == null) { Debug.LogWarning($"{name} doesn't have an Effect Controller childed to it. No effects will play for it.", gameObject); }
        }

        protected virtual void Start()
        {
            TargetHealth = totalHealth;
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

        public void Damage(int damage, bool isCritical, Weapon weaponAttackedWith)
        {
            //If you are attacked with a weapon (not things like dots) and your target health is already at 0, critical hit you deadski
            if (TargetHealth == 0) { isCritical = true; }
            if (isCritical && damage > 0) { StartCoroutine(SlowTimeCritical()); }
            Damage(damage, isCritical, weaponAttackedWith.WeaponElement);
        }

        private IEnumerator SlowTimeCritical()
        {
            Time.timeScale = 0.25f;
            yield return new WaitForSecondsRealtime(0.35f);

            // If the player were to pause in the middle of this, we want the time scale to not be reset because then the game will resume while they are still paused
            if (PauseManager.instance != null && PauseManager.instance.IsGamePaused) { yield break; }
            Time.timeScale = 1;
        }

        public void Damage(int damage, bool isCritical, Element damageElement)
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

        protected virtual IEnumerator SubtractHealthFromCharacter(int damage, bool isCritical)
        {
            TargetHealth -= damage;

            if (isCritical || healthModificationSpeed == 0)
            {
                CurrentHealth = TargetHealth;
                yield break;
            }

            while (CurrentHealth > TargetHealth)
            {
                CurrentHealth -= Time.deltaTime * healthModificationSpeed;
                yield return null;
            }
        }

        public void Heal(int restore, bool isCritical)
        {
            if (IsDying || CurrentHealth == totalHealth) { return; }

            if (isCritical && damageAction != null)
            {
                StopCoroutine(damageAction);
            }

            restoreAction = AddHealthToCharacter(restore, isCritical);
            StartCoroutine(restoreAction);

            if (effectsModule != null)
            {
                effectsModule.StartHealEffects(restore, isCritical);
            }

        }

        protected virtual IEnumerator AddHealthToCharacter(int restoreAmount, bool isCritical)
        {
            var healTarget = restoreAmount + TargetHealth;
            var cachedTarget = TargetHealth;

            if (isCritical || healthModificationSpeed == 0)
            {
                TargetHealth = healTarget;
                yield break;
            }

            while (cachedTarget < healTarget)
            {
                var step = Time.deltaTime * healthModificationSpeed;
                TargetHealth += step;
                cachedTarget += step;
                yield return null;
            }
        }

        public void Kill()
        {
            if (IsDying) { return; }

            deathAction = KillCharacter();
            StartCoroutine(deathAction);
        }

        protected virtual IEnumerator KillCharacter()
        {
            yield return new WaitForSeconds(5);
            gameObject.SetActive(false);
            deathAction = null;
        }
        #endregion
    }
}
