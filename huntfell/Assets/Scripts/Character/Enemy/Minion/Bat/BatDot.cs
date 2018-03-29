using System;
using System.Collections;
using UnityEngine;

namespace Hunter.Character
{
    class BatDot : MonoBehaviour
    {
        private int damageAmount = 1;
        private float damageInterval = 1;

        private IDamageable characterToDamage;
        private IEnumerator dotCR;

        public void InitializeDot(int damageAmount, float damageInterval, IDamageable characterToDamage)
        {
            this.characterToDamage = characterToDamage;
            this.damageInterval = damageInterval;
            this.damageAmount = damageAmount;

            if (dotCR != null)
            {
                return;
            }
            dotCR = AreaEffectDamage();
            StartCoroutine(dotCR);
        }

        private void OnDestroy()
        {
            StopCoroutine(dotCR);
        }

        private IEnumerator AreaEffectDamage()
        {
            while (true)
            {
                if (characterToDamage != null)
                {
                    characterToDamage.DealDamage(damageAmount, false);
                }
                yield return new WaitForSeconds(damageInterval);
            }
        }
    }
}
