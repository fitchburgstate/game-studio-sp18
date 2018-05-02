using System;
using System.Collections;
using UnityEngine;

namespace Hunter.Characters
{
    class DamageOverTime : StatusEffect
    {

        protected override IEnumerator StatusEffectAction (int effectAmount, float effectInterval, Element effectElement, Character characterToAffect)
        {
            if(characterToAffect == null) {
                Debug.LogWarning("Cannot add an effect to a null character.", gameObject);
                yield break;
            }

            var damageable = characterToAffect.GetComponent<IDamageable>();
            if(damageable == null)
            {
                Debug.LogWarning("Cannot drain the health of a character that is not damageable.", gameObject);
                yield break;
            }

            while (true)
            {
                damageable.Damage(effectAmount, false, effectElement);
                yield return new WaitForSeconds(effectInterval);
            }
        }
    }
}
