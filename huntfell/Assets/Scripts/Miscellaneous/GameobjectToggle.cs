using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class GameobjectToggle : MonoBehaviour
    {
        #region Variables
        [Header("Minion Management Settings")]
        public bool disableMinionsAtStart = true;
        public List<GameObject> minionSet = new List<GameObject>();

        private bool hasToggledMinions = false;

        [Header("Room Management Settings")]
        public bool floorToggle;
        public bool disableSecondFloorAtStart = true;
        public List<GameObject> roomSetA = new List<GameObject>();
        public List<GameObject> roomSetB = new List<GameObject>();

        [Header("Debug Options")]
        public bool showDebugLogs = false;

        private bool firstFloor = true;
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
            hasToggledMinions = false;

            if (minionSet != null && disableMinionsAtStart)
            {
                foreach (var minion in minionSet)
                {
                    minion.SetActive(false);
                }
            }

            if (roomSetB != null && disableSecondFloorAtStart)
            {
                foreach (var room in roomSetB)
                {
                    room.SetActive(false);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == Player)
            {
                if (floorToggle)
                {
                    RoomToggle();
                    firstFloor = !firstFloor;
                }

                if (!hasToggledMinions) { MinionToggle(); }
            }
        }
        #endregion

        #region Room Toggle Functions
        private void RoomToggle()
        {
            if (firstFloor)
            {
                if (roomSetA != null)
                {
                    foreach (var room in roomSetA)
                    {
                        room.SetActive(false);
                    }
                }

                if (roomSetB != null)
                {
                    foreach (var room in roomSetB)
                    {
                        room.SetActive(true);
                    }
                }
                if (showDebugLogs) { Debug.Log("Shut off the first floor, and activated the second floor."); }
            }
            else if (!firstFloor)
            {
                if (roomSetA != null)
                {
                    foreach (var room in roomSetA)
                    {
                        room.SetActive(true);
                    }
                }

                if (roomSetB != null)
                {
                    foreach (var room in roomSetB)
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
            if (minionSet != null)
            {
                foreach (var minion in minionSet)
                {
                    minion.SetActive(true);
                }
            }
            hasToggledMinions = true;
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
