using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;
using System;

namespace Hunter
{
    public enum EffectOptions
    {
        DamageOverTime,
        HealOverTime
    }

    public class Aura : MonoBehaviour
    {
        #region Variables
        public bool activateOnStart = true;

        [Header("Visual Effect Options")]
        [Range(0.1f, 100f), Tooltip("The size of the circle.")]
        public float auraRadiusSize = 1.0f;
        [Range(2, 256), Tooltip("The number of segments the circle has, for a circle make at least 100.")]
        public int auraRadiusSegments = 100;
        [Tooltip("The height of the circle")]
        public float auraRadiusHeight = -1f;
        public Gradient auraRadiusGradient;
        public ParticleSystem auraParticleSystem;

        [Header("Effect Options")]
        public EffectOptions effectType;
        [Tooltip("Depending on the effect, how much of whatever you want the effect to do. For example, if it was a damage effect, this would be the amount of damage."), Range(1, 50)]
        public int effectAmount = 1;
        public ElementOption effectElement;
        [Tooltip("How often the effect should be activated."), Range(0.1f, 10)]
        public float effectInterval = 1;
        [Tooltip("How long the effect should last before it is destroyed. Leave this at 0 for the effect to never be destroyed based on time."), Range(0, 20)]
        public float effectLifeTime = 0;
        public bool leavingAuraDestroysEffect = true;

        private List<Character> charactersInRadius = new List<Character>();
        private Dictionary<Character, StatusEffect> affectedCharacters = new Dictionary<Character, StatusEffect>();
        private LineRenderer auraRadius;
        private float startWidth = 0.1f;
        private float endWidth = 0.1f;

        #endregion

        #region Properties
        public bool AuraActive { get; private set; } = false;
        #endregion

        private void Start ()
        {
            InitalizeAuraRadius();
            if (activateOnStart) { EnableAura(); }
        }

        private void OnTriggerEnter (Collider other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character == null) { return; }
            if (!charactersInRadius.Contains(character))
            {
                charactersInRadius.Add(character);
            }

            if (!AuraActive) { return; }

            if (other.tag == "Player")
            {
                ApplyEffect(character);
                //Fabric.EventManager.Instance?.PostEvent("Bat Aggro", gameObject);
            }
        }

        private void OnTriggerExit (Collider other)
        {
            var character = other.gameObject.GetComponent<Character>();
            if (character == null) { return; }
            if (charactersInRadius.Contains(character))
            {
                charactersInRadius.Remove(character);
            }

            if (!AuraActive) { return; }

            if (affectedCharacters.ContainsKey(character) && leavingAuraDestroysEffect)
            {
                RemoveEffect(character);
            }
        }

        public void ApplyEffect (Character characterToAffect)
        {
            if (affectedCharacters.ContainsKey(characterToAffect)) { return; }

            StatusEffect addedEffect = null;
            switch (effectType)
            {
                case EffectOptions.DamageOverTime:
                    addedEffect = characterToAffect.gameObject.AddComponent<DamageOverTime>();
                    break;
                case EffectOptions.HealOverTime:
                    addedEffect = characterToAffect.gameObject.AddComponent<HealOverTime>();
                    break;
                default:
                    Debug.LogWarning("You tried to add an effect that isn't handled by the aura yet.", gameObject);
                    return;
            }
            var effectElement = Utility.ElementOptionToElement(this.effectElement);

            addedEffect.InitializeEffect(effectAmount, effectInterval, effectLifeTime, effectElement, characterToAffect, this);
            affectedCharacters.Add(characterToAffect, addedEffect);
        }

        private void ApplyEffectAll ()
        {
            foreach (var character in charactersInRadius)
            {
                if(character == null)
                {
                    charactersInRadius.Remove(character);
                    continue;
                }
                ApplyEffect(character);
            }
        }

        public void RemoveEffect (Character affectedCharacter)
        {
            if (!affectedCharacters.ContainsKey(affectedCharacter)) { return; }

            StatusEffect effectToRemove = null;
            affectedCharacters.TryGetValue(affectedCharacter, out effectToRemove);
            if (effectToRemove != null)
            {
                Destroy(effectToRemove);
            }

            affectedCharacters.Remove(affectedCharacter);
        }

        private void RemoveEffectAll ()
        {
            foreach (var character in charactersInRadius)
            {
                if (character == null)
                {
                    charactersInRadius.Remove(character);
                    continue;
                }
                RemoveEffect(character);
            }
        }

        public void EnableAura ()
        {
            AuraActive = true;
            ApplyEffectAll();

            if (auraRadius != null) { auraRadius.enabled = true; }
            if (auraParticleSystem != null) { auraParticleSystem.Play(); }
        }

        public void DisableAura ()
        {
            AuraActive = false;
            if (leavingAuraDestroysEffect) { RemoveEffectAll(); }

            if (auraRadius != null) { auraRadius.enabled = false; }
            if (auraParticleSystem != null) { auraParticleSystem.Stop(); }
        }

        private void InitalizeAuraRadius ()
        {
            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = auraRadiusSize;
            sphereCollider.isTrigger = true;

            auraRadius = gameObject.AddComponent<LineRenderer>();
            auraRadius.enabled = false;
            auraRadius.colorGradient = auraRadiusGradient;
            auraRadius.material = new Material(Shader.Find("Particles/Additive"));
            auraRadius.startWidth = startWidth;
            auraRadius.endWidth = endWidth;
            auraRadius.positionCount = auraRadiusSegments + 1;
            auraRadius.useWorldSpace = false;

            var deltaTheta = (float)(2.0 * Mathf.PI) / auraRadiusSegments;
            var theta = 0f;

            for (var i = 0; i < auraRadiusSegments + 1; i++)
            {
                var x = auraRadiusSize * Mathf.Cos(theta);
                var z = auraRadiusSize * Mathf.Sin(theta);
                var pos = new Vector3(x, auraRadiusHeight, z);
                auraRadius.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }

        private void OnDrawGizmosSelected ()
        {
            if (!Application.isPlaying)
            {
                var theta = 0f;
                Gizmos.color = Color.red;
                var x = auraRadiusSize * Mathf.Cos(theta);
                var z = auraRadiusSize * Mathf.Sin(theta);
                var pos = gameObject.transform.position + new Vector3(x, auraRadiusHeight, z);
                var newPos = pos;
                var lastPos = pos;
                for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
                {
                    x = auraRadiusSize * Mathf.Cos(theta);
                    z = auraRadiusSize * Mathf.Sin(theta);
                    newPos = gameObject.transform.position + new Vector3(x, auraRadiusHeight, z);
                    Gizmos.DrawLine(pos, newPos);
                    pos = newPos;
                }
                Gizmos.DrawLine(pos, lastPos);
            }
        }
    }
}
