using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Hunter.Characters
{
    public class SpawnPoint : MonoBehaviour
    {
        #region Variables
        public Vector3 respawnPosition;
        public LayerMask validLayers;
        public bool activated;

        private Player playerScript;
        #endregion

        #region Unity Functions
        private void Start()
        {
            activated = false;
            playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            var hit = new RaycastHit();
            var ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out hit, validLayers))
            {
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
        #endregion

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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 1f, 1f, .5f);
            Gizmos.DrawLine(transform.position, respawnPosition);
            Gizmos.DrawWireSphere(respawnPosition, 1f);
        }
        #endregion
    }
}
