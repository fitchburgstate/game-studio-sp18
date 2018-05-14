using System.Collections.Generic;
using UnityEngine;

namespace Hunter
{
    public class GameobjectToggle : MonoBehaviour
    {
        #region Variables
        [Header("Global Settings")]
        public List<GameObject> gameobjectSet = new List<GameObject>();
        [Tooltip("If true, the set will be toggled each time the player passed through the trigger. If false, the set will be activated and then the trigger will be destroyed.")]
        public bool toggleGameobjectSet = true;
        public bool disableGameobjectSetAtStart = false;

        private bool setIsActive;
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
            if (gameobjectSet != null && disableGameobjectSetAtStart)
            {
                foreach (var objectToChange in gameobjectSet)
                {
                    objectToChange.SetActive(false);
                }
                setIsActive = false;
            }
            else if (gameobjectSet != null)
            {
                setIsActive = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Player)
            {
                if (toggleGameobjectSet)
                {
                    ToggleSet();
                }
                else
                {
                    SingleActivation();
                }
            }
        }
        #endregion

        #region Toggle Functions
        private void ToggleSet()
        {
            if (setIsActive) // Set to inactive
            {
                foreach (var objectToChange in gameobjectSet)
                {
                    objectToChange.SetActive(false);
                }
            }
            else // Set to active
            {
                foreach (var objectToChange in gameobjectSet)
                {
                    objectToChange.SetActive(true);
                }
            }
        }
        #endregion

        #region Single Activation Functions
        private void SingleActivation()
        {
            // Set gameobjects to active, then destroy this trigger
            foreach (var objectToChange in gameobjectSet)
            {
                objectToChange.SetActive(true);
            }

            Destroy(gameObject);
        }
        #endregion

        #region Gizmos Function
        private void OnDrawGizmos()
        {
            var toggleColor = new Color(0f, 1f, 0f, .5f);
            var disableAtStartToggleColor = new Color(1f, 0f, 0f, .5f);

            var singleActivationColor = new Color(0f, 0f, 1f, .5f);
            var disableAtStartSingleActivationColor = new Color(1f, 0f, 1f, .5f);

            var size = Vector3.one;

            if (toggleGameobjectSet && !disableGameobjectSetAtStart) { Gizmos.color = toggleColor; }
            else if (toggleGameobjectSet && disableGameobjectSetAtStart) { Gizmos.color = disableAtStartToggleColor; }
            else if (!toggleGameobjectSet && !disableGameobjectSetAtStart) { Gizmos.color = singleActivationColor; }
            else { Gizmos.color = disableAtStartSingleActivationColor; }

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawCube(Vector3.zero, size);
        }
        #endregion
    }
}
