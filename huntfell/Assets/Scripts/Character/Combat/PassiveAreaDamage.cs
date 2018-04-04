using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter.AI
{
    public class PassiveAreaDamage : MonoBehaviour
    {
        #region Properties
        public Character.Character AiCharacter
        {
            get
            {
                if (aiCharacter == null)
                {
                    aiCharacter = transform.parent.GetComponent<Character.Character>();
                }
                return aiCharacter;
            }
        }
        #endregion

        #region Variables
        [Header("Damage Radius Variables")]
        [Range(0.1f, 100f)]
        [Tooltip("The size of the circle.")]
        public float damageRadius = 1.0f;

        [Tooltip("The amount of damage the enemy should do to the player every tick.")]
        public int damageAmount = 1;

        [Tooltip("How often the enemy should deal damage to the player.")]
        public int damageInterval = 1;

        [Header("Line Renderer Variables")]
        [Range(3, 256)]
        [Tooltip("The number of segments the circle has, for a circle make at least 100.")]
        public int numSegments = 100;

        [Tooltip("The height of the circle")]
        public float lineHeight = -1f;

        public Gradient lineColorGradient;
        private float startWidth = 0.15f;
        private float endWidth = 0.15f;

        private IEnumerator areaEffectDamageCR;
        private Character.Character aiCharacter;
        #endregion

        private void Start()
        {
            DoRenderer();
            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.radius = damageRadius;
            sphereCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if ((other.tag == "Player") && (damageable != null))
            {
                var dotComponent = other.transform.gameObject.AddComponent<HealthDrainDot>();
                dotComponent.InitializeDot(damageAmount, damageInterval, damageable);
                Fabric.EventManager.Instance.PostEvent("Bat Aggro", gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var batDot = other.gameObject.GetComponent<HealthDrainDot>();
            if (batDot != null)
            {
                Destroy(batDot);
            }
        }

        private void DoRenderer()
        {
            var batLine = gameObject.AddComponent<LineRenderer>();
            batLine.colorGradient = lineColorGradient;
            batLine.material = new Material(Shader.Find("Particles/Additive"));
            batLine.startWidth = startWidth;
            batLine.endWidth = endWidth;
            batLine.positionCount = numSegments + 1;
            batLine.useWorldSpace = false;

            var deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
            var theta = 0f;

            for (var i = 0; i < numSegments + 1; i++)
            {
                var x = damageRadius * Mathf.Cos(theta);
                var z = damageRadius * Mathf.Sin(theta);
                var pos = new Vector3(x, lineHeight, z);
                batLine.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
            {
                var theta = 0f;
                Gizmos.color = Color.red;
                var x = damageRadius * Mathf.Cos(theta);
                var z = damageRadius * Mathf.Sin(theta);
                var pos = AiCharacter.eyeLine.position + new Vector3(x, lineHeight, z);
                var newPos = pos;
                var lastPos = pos;
                for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
                {
                    x = damageRadius * Mathf.Cos(theta);
                    z = damageRadius * Mathf.Sin(theta);
                    newPos = AiCharacter.eyeLine.position + new Vector3(x, lineHeight, z);
                    Gizmos.DrawLine(pos, newPos);
                    pos = newPos;
                }
                Gizmos.DrawLine(pos, lastPos);
            }
        }
    }
}
