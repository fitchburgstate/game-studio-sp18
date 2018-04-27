using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Characters;

namespace Hunter
{
    public class Aura : MonoBehaviour
    {
        #region Variables
        [Header("Aura Options"), Range(0.1f, 100f), Tooltip("The size of the circle.")]
        public float auraRadiusSize = 1.0f;
        [Range(2, 256), Tooltip("The number of segments the circle has, for a circle make at least 100.")]
        public int auraRadiusSegments = 100;
        [Tooltip("The height of the circle")]
        public float auraRadiusHeight = -1f;
        public Gradient auraRadiusGradient;
        public ParticleSystem auraParticleSystem;

        [Header("Effect Options")]
        public ElementOption effectElement;
        [Tooltip("Depending on the effect, how much of whatever you want the effect to do. For example, if it was a damage effect, this would be the amount of damage."), Range(1, 50)]
        public int effectAmount = 1;
        [Tooltip("How often the effect should be activated."), Range(0.1f, 10)]
        public float effectInterval = 1;
        [Tooltip("How long the effect should last before it is destroyed. Leave this at 0 for the effect to never be destroyed based on time."), Range(0, 20)]
        public float effectTotalTime = 0;
        public bool leavingAuraDestroysEffect = true;

        private List<Character> charactersInRadius = new List<Character>();
        private Dictionary<Character, HealthDrainDot> affectedCharacters = new Dictionary<Character, HealthDrainDot>();
        private LineRenderer auraRadius;
        private float startWidth = 0.1f;
        private float endWidth = 0.1f;

        #endregion

        #region Properties
        public bool AuraActive { get; private set; } = true;
        #endregion

        private void Start()
        {
            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = auraRadiusSize;
            sphereCollider.isTrigger = true;

            InitalizeAuraRadius();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!AuraActive) { return; }
            var character = other.gameObject.GetComponent<Character>();
            if (other.tag == "Player" && character != null)
            {
                charactersInRadius.Add(character);
                

                Fabric.EventManager.Instance?.PostEvent("Bat Aggro", gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!AuraActive) { return; }
            //if (other.gameObject == affectedCharacter)
            //{
            //    RemoveEffect();
            //}
        }

        private void ApplyEffect (Character affectedCharacter)
        {
            var dotComponent = affectedCharacter.gameObject.AddComponent<HealthDrainDot>();
            var damageElement = Utility.ElementOptionToElement(effectElement);
            //dotComponent.InitializeDot(effectAmount, effectInterval, damageElement, damageable);
        }

        private void ApplyEffect ()
        {

        }

        private void RemoveEffect (Character affectedCharacter)
        {
            var batDot = affectedCharacter.GetComponent<HealthDrainDot>();
            if (batDot != null) { Destroy(batDot); }
        }

        public void EnableAura ()
        {
            AuraActive = true;
            if(auraRadius == null){ InitalizeAuraRadius(); }
            auraRadius.enabled = true;

            if(auraParticleSystem != null) { auraParticleSystem.Play(); }
        }

        public void DisableAura ()
        {
            AuraActive = false;
            if(charactersInRadius.Count > 0)
            {
                if (leavingAuraDestroysEffect)
                {
                    foreach (var character in charactersInRadius)
                    {
                        RemoveEffect(character);
                    }
                }
                charactersInRadius.Clear();
            }
            if(auraRadius != null) { auraRadius.enabled = false; }
            if(auraParticleSystem != null) { auraParticleSystem.Stop(); }
        }

        private void InitalizeAuraRadius()
        {
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

        private void OnDrawGizmosSelected()
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
