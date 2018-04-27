using System;
using System.Collections;
using UnityEngine;

namespace Hunter.Characters
{
    class HealthDrainDot : MonoBehaviour
    {
        private IEnumerator dotCR;

        public void InitializeDot(int damageAmount, float damageInterval, Element damageElement, IDamageable characterToDamage)
        {
            if (dotCR != null)
            {
                return;
            }
            dotCR = HealthDrain(damageAmount, damageInterval, damageElement, characterToDamage);
            StartCoroutine(dotCR);
        }

        private void OnDestroy()
        {
            StopCoroutine(dotCR);
        }

        private IEnumerator HealthDrain(int damageAmount, float damageInterval, Element damageElement, IDamageable characterToDamage)
        {
            if(characterToDamage == null) { yield break; }
            while (true)
            {
                characterToDamage.TakeDamage(damageAmount.ToString(), false, damageElement);
                yield return new WaitForSeconds(damageInterval);
            }
        }
    }
}
