using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter.Character.AI
{
    public class AIDetection : MonoBehaviour
    {
        /// <summary>
        /// Determines if a target is too close to the AI. If so, the target should be automatically found.
        /// </summary>
        public float minDetectionDistance = 3f;

        /// <summary>
        /// Determines the maximum distance that the AI can "see" the player. If the player is outside of this range they will be undetectable.
        /// </summary>
        public float maxDetectionDistance = 15f;

        /// <summary>
        /// Determines how wide the "arc" is of the AI's vision. This value represents one "eye" so it will be doubled later.
        /// </summary>
        public float fieldOfViewRange = 48f;

        public LayerMask detectionLayers;

        /// <summary>
        /// The distance between the AI and the target.
        /// </summary>
        private float distanceToTarget;

        private float cachedFOV;

        private Character aiCharacter;
        public Character AiCharacter
        {
            get
            {
                if (aiCharacter == null)
                {
                    aiCharacter = transform.GetComponent<Character>();
                }
                return aiCharacter;
            }
        }

        private Character playerCharacter;
        public Character PlayerCharacter
        {
            get
            {
                if (playerCharacter == null)
                {
                    var pcGO = GameObject.FindGameObjectWithTag("Player");
                    if (pcGO == null)
                    {
                        //Debug.LogWarning("Could not find a GameObject tagged 'Player' in the scene.");
                        return null;
                    }

                    playerCharacter = pcGO.GetComponent<Character>();
                    if (playerCharacter == null)
                    {
                        //Debug.LogWarning("The Player does not have the proper Character script attached to them.", pcGO);
                        return null;
                    }
                }
                return playerCharacter;
            }
        }

        /// <summary>
        /// The AI searches for a gameobject tagged "Player" and returns true when the player has been found.
        /// </summary>
        /// <returns></returns>
        public bool DetectPlayer ()
        {
            var rayHit = new RaycastHit();
            //Dont do this every frame, cache the player or something
            var aiCharacterEyeLine = AiCharacter.eyeLine;
            var rayDirection = PlayerCharacter.eyeLine.position - aiCharacterEyeLine.position;

            var wolfComponent = AiCharacter.GetComponent<Wolf>();
            //var playerComponent = PlayerCharacter.GetComponent<Player>();

            if (Vector3.Angle(rayDirection, aiCharacter.RotationTransform.forward) <= fieldOfViewRange * 0.5f)
            {
                //Debug.DrawRay(aiCharacterEyeLine.position, rayDirection, Color.red, 5);
                if (Physics.Raycast(aiCharacterEyeLine.position, rayDirection, out rayHit, maxDetectionDistance, detectionLayers)) // Detects to see if the player is within the field of view
                {
                    if (wolfComponent != null && !wolfComponent.justFound)
                    {
                        Fabric.EventManager.Instance.PostEvent("Wolf Aggro", gameObject);
                        wolfComponent.justFound = true;
                    }
                    return true;
                }
            }

            var collidersInRadius = Physics.OverlapSphere(aiCharacterEyeLine.position, minDetectionDistance, detectionLayers);
            if (collidersInRadius.Length > 0)
            {
                if (wolfComponent != null && !wolfComponent.justFound)
                {
                    Fabric.EventManager.Instance.PostEvent("Wolf Aggro", gameObject);
                    wolfComponent.justFound = true;
                }
                return true;
            }
            return false;
        }

        // TODO Fix this jank-ass Gizmo Draw Call
        private void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.blue;

            var direction = AiCharacter.eyeLine.TransformDirection(Vector3.forward) * maxDetectionDistance;

            var leftRayRotation = Quaternion.AngleAxis(-(fieldOfViewRange / 2), Vector3.up);
            var leftRayDirection = leftRayRotation * AiCharacter.RotationTransform.forward;

            var rightRayRotation = Quaternion.AngleAxis((fieldOfViewRange / 2), Vector3.up);
            var rightRayDirection = rightRayRotation * AiCharacter.RotationTransform.forward;

            Gizmos.DrawRay(AiCharacter.eyeLine.position, direction);
            Gizmos.DrawRay(AiCharacter.eyeLine.position, leftRayDirection * maxDetectionDistance);
            Gizmos.DrawRay(AiCharacter.eyeLine.position, rightRayDirection * maxDetectionDistance);
        }
    }
}
