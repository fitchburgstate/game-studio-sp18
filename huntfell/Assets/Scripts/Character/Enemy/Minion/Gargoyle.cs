using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters.AI;

namespace Hunter.Characters
{
    public class Gargoyle : Minion
    {
        #region Variables
        [SerializeField]
        private Ranged rangedWeapon;

        private AIDetection gargoyleDetection;

        private GameObject target;
        private Transform targetEyeline;

        private IEnumerator gargoyleAttackCR;
        #endregion

        #region Properties
        public override float CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        #endregion

        protected override void Start()
        {
            base.Start();
            gargoyleDetection = GetComponent<AIDetection>();
            EquipWeaponToCharacter(rangedWeapon);
            target = GameObject.FindGameObjectWithTag("Player");
            targetEyeline = target.GetComponent<Character>().eyeLine;
        }

        private void FixedUpdate()
        {
            var enemyInLOS = gargoyleDetection.DetectPlayer();

            if (enemyInLOS)
            {
                transform.LookAt(targetEyeline.transform.position);
                Attack();
            }
        }

        private void Attack()
        {
            if (gargoyleAttackCR != null) { return; }
            gargoyleAttackCR = GargoyleAttack();
            StartCoroutine(gargoyleAttackCR);
        }

        private IEnumerator GargoyleAttack()
        {
            rangedWeapon.StartAttackFromAnimationEvent();
            Fabric.EventManager.Instance?.PostEvent("Gargoyle Attack", gameObject);

            yield return new WaitForSeconds(CurrentWeapon.recoverySpeed);
            gargoyleAttackCR = null;
        }
    }
}
