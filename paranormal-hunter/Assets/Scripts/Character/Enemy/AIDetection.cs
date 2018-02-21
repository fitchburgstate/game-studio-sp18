using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hunter;

public class AIDetection : MonoBehaviour
{
    public float minDetectionDistance = 3f;
    public float maxDetectionDistance = 15f;
    public float fieldOfViewRange = 48f;
    private float distanceToPlayer;

    private bool isBlind;

    private void Start()
    {
        isBlind = false;
    }

    private void FixedUpdate()
    {
        if (!isBlind)
        {
            DetectPlayer();
        }
    }

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
