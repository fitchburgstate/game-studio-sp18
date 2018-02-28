using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter;

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

    /// <summary>
    /// A boolean to determine whether the AI is actively searching for a target.
    /// </summary>
    private bool isBlind;

    private void Start()
    {
        isBlind = false;
    }

    //private void FixedUpdate()
    //{
    //    if (!isBlind)
    //    {
    //        DetectPlayer();
    //    }
    //}

    /// <summary>
    /// The AI searches for a gameobject tagged "Player" and returns true when the player has been found.
    /// </summary>
    /// <returns></returns>
    public bool DetectPlayer()
    {
        var rayHit = new RaycastHit();
        var playerObject = GameObject.FindGameObjectWithTag("Player");
        var rayDirection = playerObject.transform.position - transform.position;

        //if (Physics.Raycast(transform.position, rayDirection, out rayHit))
        //{
        //    if ((rayHit.transform.tag == "Player") && (distanceToPlayer <= minDetectionDistance))
        //    {
        //        Debug.Log("The player has been hit!");
        //        return true;
        //    }
        //}

        if (Vector3.Angle(rayDirection, transform.forward) <= fieldOfViewRange * 0.5f)
        {
            if (Physics.Raycast(transform.position, rayDirection, out rayHit, maxDetectionDistance)) // Detects to see if the player is within the field of view
            {
                if (rayHit.transform.tag == "Player") // Returns true if the raycast has hit the player
                {
                    Debug.Log("The player has been hit!");
                    return true;
                }
                else // Returns false if the raycast has hit anything (or nothing) but the player
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
        var direction = transform.TransformDirection(Vector3.forward) * maxDetectionDistance;
        var leftRayRotation = Quaternion.AngleAxis(-(fieldOfViewRange / 2), Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        var rightRayRotation = Quaternion.AngleAxis((fieldOfViewRange / 2), Vector3.up);
        var rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, direction);
        Gizmos.DrawRay(transform.position, leftRayDirection * maxDetectionDistance);
        Gizmos.DrawRay(transform.position, rightRayDirection * maxDetectionDistance);
    }
}
