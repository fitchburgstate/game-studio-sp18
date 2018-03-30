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
        #endregion

        protected override void Start()
        {
            base.Start();
            gargoyleDetection = GetComponent<AIDetection>();
            target = GameObject.FindGameObjectWithTag("Player");
        }

        private void FixedUpdate()
        {
            var enemyInLOS = gargoyleDetection.DetectPlayer();

            transform.LookAt(target.transform);

            if (enemyInLOS)
            {
                rangedWeapon.StartAttackFromAnimationEvent();
            }
        }

        #region Unused Functions

        #endregion
    }
}
