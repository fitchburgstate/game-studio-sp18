using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Characters.AI
{
    public class AIDetection : MonoBehaviour
    {
        #region Variables
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

        /// <summary>
        /// Determines the layer(s) that the NPC can see through. For example, the "Floor" layer is not a layer that will return true if any raycasting is done on it.
        /// </summary>
        public LayerMask detectionLayers;

        /// <summary>
        /// Determines whether the editor should display the vision based gizmos or not.
        /// </summary>
        public bool showGizmos = true;

        /// <summary>
        /// The distance between the AI and the target.
        /// </summary>
        private float distanceToTarget;

        /// <summary>
        /// The transform of the AI character's EyeLineTransform component.
        /// </summary>
        private Transform aiCharacterEyeLineTransform;

        /// <summary>
        ///  The AI character itself.
        /// </summary>
        private Character aiCharacter;

        /// <summary>
        /// The player character that the AI will be interacting with.
        /// </summary>
        private Character playerCharacter;

        /// <summary>
        /// Is the target in the Conic field of View?
        /// </summary>
        private bool inVisionCone;
        #endregion

        #region Properties
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

        public bool InVisionCone
        {
            get
            {
                return inVisionCone;
            }
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            aiCharacterEyeLineTransform = AiCharacter.EyeLineTransform;
        }
        #endregion

        #region DetectPlayer Function
        /// <summary>
        /// The AI searches for a gameobject tagged "Player" and returns true when the player has been found.
        /// </summary>
        /// <returns></returns>
        public bool DetectPlayer()
        {
            var rayHit = new RaycastHit();
            var rayDirection = PlayerCharacter.transform.position - AiCharacter.transform.position;
            rayDirection.Normalize();

            var wolfComponent = AiCharacter.GetComponent<Wolf>();

            //Debug.Log($"Difference: {Vector3.Angle(rayDirection, aiCharacter.RotationTransform.forward)}, fov: {fieldOfViewRange * 0.5f}");

            if (Vector3.Angle(rayDirection, aiCharacter.RotationTransform.forward) <= fieldOfViewRange * 0.5f)
            {
                if (Physics.Raycast(aiCharacterEyeLineTransform.position, rayDirection, out rayHit, maxDetectionDistance, detectionLayers)) // Detects to see if the player is within the field of view
                {
                    inVisionCone = true;
                    //Debug.Log(rayHit.transform.name, rayHit.transform.gameObject);
                    if (wolfComponent != null && !wolfComponent.justFound)
                    {
                        Fabric.EventManager.Instance?.PostEvent("Wolf Aggro", gameObject);
                        wolfComponent.justFound = true;
                    }
                    return true;
                }
                else
                {
                    inVisionCone = false;
                }
            }
            else
            {
                inVisionCone = false;
            }

            var collidersInRadius = Physics.OverlapSphere(aiCharacterEyeLineTransform.position, minDetectionDistance, detectionLayers);
            if (collidersInRadius.Length > 0)
            {
                if (wolfComponent != null && !wolfComponent.justFound)
                {
                    Fabric.EventManager.Instance?.PostEvent("Wolf Aggro", gameObject);
                    wolfComponent.justFound = true;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Editor Gizmos
        private void OnDrawGizmosSelected()
        {
            if (showGizmos)
            {
                Gizmos.color = Color.blue;

                var lineHeight = 0f;
                var theta = 0f;
                var x = minDetectionDistance * Mathf.Cos(theta);
                var z = minDetectionDistance * Mathf.Sin(theta);
                var pos = AiCharacter.EyeLineTransform.position + new Vector3(x, lineHeight, z);
                var newPos = pos;
                var lastPos = pos;

                var direction = aiCharacter.RotationTransform.forward * maxDetectionDistance;
                var leftRayRotation = Quaternion.AngleAxis(-(fieldOfViewRange / 2), Vector3.up);
                var leftRayDirection = leftRayRotation * AiCharacter.RotationTransform.forward;
                var rightRayRotation = Quaternion.AngleAxis((fieldOfViewRange / 2), Vector3.up);
                var rightRayDirection = rightRayRotation * AiCharacter.RotationTransform.forward;

                Gizmos.DrawRay(AiCharacter.EyeLineTransform.position, direction);
                Gizmos.DrawRay(AiCharacter.EyeLineTransform.position, leftRayDirection * maxDetectionDistance);
                Gizmos.DrawRay(AiCharacter.EyeLineTransform.position, rightRayDirection * maxDetectionDistance);

                for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
                {
                    x = minDetectionDistance * Mathf.Cos(theta);
                    z = minDetectionDistance * Mathf.Sin(theta);
                    newPos = AiCharacter.EyeLineTransform.position + new Vector3(x, lineHeight, z);
                    Gizmos.DrawLine(pos, newPos);
                    pos = newPos;
                }
                Gizmos.DrawLine(pos, lastPos);
            }
        }
        #endregion
    }
}
