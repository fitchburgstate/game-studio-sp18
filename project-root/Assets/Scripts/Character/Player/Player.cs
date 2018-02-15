using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Hunter.Character
{
    public sealed class Player : Character, IMoveable
    {
        #region Variables
        // ---------- SET THESE IN THE INSPECTOR ---------- \\
        [Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)] public float speed = 5f;

        [Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)] public float rotateChar = 12f;
        // ------------------------------------------------ \\ 

        // Variables that must be set at Start
        private GameObject player;
        private GameObject playerRoot;
        private NavMeshAgent agent;
        private Camera mainCamera;
        #endregion

        private void Start()
        {
            player = gameObject.transform.GetChild(0).gameObject; // This will find the player-root gameobject, which means that the only child of this gameobject should be player-root
            playerRoot = gameObject;
            agent = GetComponent<NavMeshAgent>();
            mainCamera = GameObject.FindObjectOfType<Camera>();
            playerRoot.transform.forward = mainCamera.transform.forward;
        }


        public void Move(CharacterController controller, Vector3 moveDirection, Vector3 lookDirection, GameObject playerRoot)
        {
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            agent.destination = this.playerRoot.transform.position;
            agent.updateRotation = false;

            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * rotateChar);

            controller.Move(moveDirection * Time.deltaTime);
        }

        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented
        }
    }
}
