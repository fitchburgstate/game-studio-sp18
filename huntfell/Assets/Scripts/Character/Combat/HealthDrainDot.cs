using System;
using System.Collections;
using UnityEngine;

namespace Hunter.Character
{
    class HealthDrainDot : MonoBehaviour
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
            dotCR = HealthDrain();
            GetComponent<Character>()?.effectsController?.poisonDamageSystem?.Play();
            StartCoroutine(dotCR);
        }

        private void OnDestroy()
        {
            StopCoroutine(dotCR);
            GetComponent<Character>()?.effectsController?.poisonDamageSystem?.Stop();
        }

        private IEnumerator HealthDrain()
        {
            while (true)
            {
                if (characterToDamage != null)
                {
                    characterToDamage.TakeDamage(damageAmount, false, null);
                }
                yield return new WaitForSeconds(damageInterval);
            }
        }
    }
}
