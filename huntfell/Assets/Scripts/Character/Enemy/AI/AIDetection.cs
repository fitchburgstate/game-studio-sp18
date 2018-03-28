using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter.Character;

namespace Hunter.AI
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

        /// <summary>
        /// The distance between the AI and the target.
        /// </summary>
        private float distanceToTarget;


        private Character.Character aiCharacter;
        public Character.Character AiCharacter
        {
            get
            {
                if(aiCharacter == null)
                {
                    aiCharacter = transform.GetComponent<Character.Character>();
                }
                return aiCharacter;
            }
        }

        private Character.Character playerCharacter;
        public Character.Character PlayerCharacter
        {
            get
            {
                if(playerCharacter == null)
                {
                    var pcGO = GameObject.FindGameObjectWithTag("Player");
                    if(pcGO == null)
                    {
                        Debug.LogWarning("Could not find a GameObject tagged 'Player' in the scene.");
                        return null;
                    }

                    playerCharacter = pcGO.GetComponent<Character.Character>();
                    if(playerCharacter == null)
                    {
                        Debug.LogWarning("The Player does not have the proper Character script attached to them.", pcGO);
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
        public bool DetectPlayer()
        {
            var rayHit = new RaycastHit();
            //Dont do this every frame, cache the player or something
            var aiCharacterEyeLine = AiCharacter.eyeLine;
            var rayDirection = PlayerCharacter.eyeLine.position - aiCharacterEyeLine.position;

            if (Vector3.Angle(rayDirection, aiCharacter.RotationTransform.forward) <= fieldOfViewRange * 0.5f)
            {
                Debug.DrawRay(aiCharacterEyeLine.position, rayDirection, Color.red, 5);
                if (Physics.Raycast(aiCharacterEyeLine.position, rayDirection, out rayHit, maxDetectionDistance)) // Detects to see if the player is within the field of view
                {
                    if (rayHit.transform.tag == "Player") // Returns true if the raycast has hit the player
                    {
                        //Debug.Log("The player has been found!");
                        return true;
                    }
                    else // Returns false if the raycast has hit anything (or nothing) BUT the player
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        // TODO Fix this jank-ass Gizmo Draw Call
        /// <summary>
        /// This function draws lines in the scene view to let us developers know the cone of vision that the clicked mob has.
        /// </summary>
        private void OnDrawGizmosSelected()
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

