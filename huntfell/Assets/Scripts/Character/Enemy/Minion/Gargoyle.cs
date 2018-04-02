using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character.AI;

namespace Hunter.Character
{
    public class Gargoyle : Minion
    {
        #region Properties
        public override int CurrentHealth
        {
            get
            {
                return health;
            }
            set
            {
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
                health = value;
            }
        }
        #endregion

        #region Variables
        [SerializeField]
        private Range rangedWeapon;

        private AIDetection gargoyleDetection;

        private GameObject target;
        private Transform targetEyeline;
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
                rangedWeapon.StartAttackFromAnimationEvent();
            }
        }

        #region Unused Functions

        #endregion
    }
}
