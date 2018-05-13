using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class GameobjectToggle : MonoBehaviour
    {
        #region Variables
        [Header("Single Activation Set Settings")]
        public bool disableSetAtStart = true;
        public List<GameObject> singleActivationGameobjectSet = new List<GameObject>();

        private bool hasToggledSet = false;

        [Header("Toggle between Set A and B Settings")]
        public bool toggleBetweenSets;
        public bool disableSetBAtStart = true;
        public List<GameObject> gameobjectSetA = new List<GameObject>();
        public List<GameObject> gameobjectSetB = new List<GameObject>();

        [Header("Debug Options")]
        public bool showDebugLogs = false;

        private bool setA = true;
        private GameObject player;
        #endregion

        #region Properties
        public GameObject Player
        {
            get
            {
                if (player == null) { player = GameObject.FindGameObjectWithTag("Player"); }
                return player;
            }
        }
        #endregion

        #region Unity Functions
        private void Start()
        {
            hasToggledSet = false;

            if (singleActivationGameobjectSet != null && disableSetAtStart)
            {
                foreach (var minion in singleActivationGameobjectSet)
                {
                    minion.SetActive(false);
                }
            }

            if (gameobjectSetB != null && disableSetBAtStart)
            {
                foreach (var room in gameobjectSetB)
                {
                    room.SetActive(false);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == Player)
            {
                if (toggleBetweenSets)
                {
                    RoomToggle();
                    setA = !setA;
                }

                if (!hasToggledSet) { MinionToggle(); }
            }
        }
        #endregion

        #region Room Toggle Functions
        private void RoomToggle()
        {
            if (setA)
            {
                if (gameobjectSetA != null)
                {
                    foreach (var room in gameobjectSetA)
                    {
                        room.SetActive(false);
                    }
                }

                if (gameobjectSetB != null)
                {
                    foreach (var room in gameobjectSetB)
                    {
                        room.SetActive(true);
                    }
                }
                if (showDebugLogs) { Debug.Log("Shut off the first floor, and activated the second floor."); }
            }
            else if (!setA)
            {
                if (gameobjectSetA != null)
                {
                    foreach (var room in gameobjectSetA)
                    {
                        room.SetActive(true);
                    }
                }

                if (gameobjectSetB != null)
                {
                    foreach (var room in gameobjectSetB)
                    {
                        room.SetActive(false);
                    }
                }
                if (showDebugLogs) { Debug.Log("Shut off the second floor, and activated the first floor."); }
            }
        }
        #endregion

        #region Minions to Spawn Functions
        private void MinionToggle()
        {
            if (singleActivationGameobjectSet != null)
            {
                foreach (var minion in singleActivationGameobjectSet)
                {
                    minion.SetActive(true);
                }
            }
            hasToggledSet = true;
        }
        #endregion

        #region Gizmos Function
        private void OnDrawGizmos()
        {
            var gizmoColor = new Color(0f, 0f, 1f, .5f);
            var size = Vector3.one;

            Gizmos.color = gizmoColor;
            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(Vector3.zero, size);
        }
        #endregion
    }
}
