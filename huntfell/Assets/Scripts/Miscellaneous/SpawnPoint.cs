using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public class SpawnPoint : MonoBehaviour
    {
        #region Variables
        public bool activated;

        public LayerMask validLayers;

        [HideInInspector]
        public Vector3 respawnPosition;

        private Player playerScript;
        #endregion

        private void Start()
        {
            activated = false;
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            var hit = new RaycastHit();
            var ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out hit, validLayers))
            {
                //Debug.DrawLine(transform.position, hit.point, Color.blue, 5);
                respawnPosition = Utility.GetClosestPointOnNavMesh(hit.point, playerScript.gameObject.GetComponent<NavMeshAgent>(), transform);
            }
            else
            {
                Debug.LogError("Could not Raycast downwards from the spawn point's center.", gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((other.gameObject == playerScript.gameObject) && (activated == false))
            {
                activated = true;
            }
        }

        #region Helper Functions
        private void OnDrawGizmos()
        {
            var activatedColor = new Color(0f, 1f, 0f, .5f);
            var deactivatedColor = new Color(1f, 0f, 0f, .5f);
            var size = Vector3.one;

            if (activated) { Gizmos.color = activatedColor; }

            else { Gizmos.color = deactivatedColor; }

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(Vector3.zero, size);
        }
        #endregion
    }
}
