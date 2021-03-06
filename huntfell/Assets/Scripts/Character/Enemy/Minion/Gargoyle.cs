﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters.AI;

namespace Hunter.Characters
{
    public class Gargoyle : Minion
    {
        #region Variables
        [SerializeField, Header("Combat Options")]
        private Ranged rangedWeapon;

        [Header("Death Options")]
        [SerializeField]
        private ParticleSystem deathParticle;

        private GameObject target;
        private AIDetection gargoyleDetection;
        private IEnumerator gargoyleAttackCR;
        #endregion

        #region Unity Functions
        protected override void Start()
        {
            base.Start();
            gargoyleDetection = GetComponent<AIDetection>();
            EquipWeaponToCharacter(rangedWeapon);
            target = GameObject.FindGameObjectWithTag("Player");
        }

        private void FixedUpdate()
        {
            var enemyInLOS = gargoyleDetection.DetectPlayer();

            if (enemyInLOS)
            {
                RotateTowardsTarget(target.transform.position, turnSpeed);
                Attack();
            }
        }
        #endregion

        #region Gargoyle Combat
        private void Attack()
        {
            if (IsDying) { return; }
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

        protected override IEnumerator KillCharacter()
        {
            deathParticle?.Play();
            return base.KillCharacter();
        }
        #endregion
    }
}
