using System.Collections;
using UnityEngine;

namespace Hunter.Characters
{
    public abstract class StatusEffect : MonoBehaviour
    {
        protected IEnumerator effectAction;

        private Aura homeAura;
        private Character affectedCharacter;

        public virtual bool InitializeEffect (int effectAmount, float effectInterval, float effectLifeTime, Element effectElement, Character characterToAffect, Aura causingAura)
        {
            if (effectAction != null) { return false; }

            affectedCharacter = characterToAffect;
            homeAura = causingAura;

            effectAction = StatusEffectAction(effectAmount, effectInterval, effectElement, characterToAffect);
            StartCoroutine(effectAction);

            if(effectLifeTime > 0)
            {
                gameObject.AddComponent<TimedDestroy>().InitiateDestroy(this, effectLifeTime);
            }
            return true;
        }

        protected abstract IEnumerator StatusEffectAction (int effectAmount, float effectInterval, Element effectElement, Character characterToAffect);

        protected virtual void OnDisable ()
        {
            StopCoroutine(effectAction);
            if(homeAura != null)
            {
                homeAura.RemoveEffect(affectedCharacter);
                affectedCharacter = null;
                homeAura = null;
            }
        }
    }
}
