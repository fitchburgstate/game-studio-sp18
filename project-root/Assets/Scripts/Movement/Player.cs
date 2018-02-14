using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

namespace CharacterScripts
{
    public class Player : Character, IMoveable<CharacterController>
    {
        #region Variables
        // ---------- SET THESE IN THE INSPECTOR ---------- \\
        [Tooltip("Controls the speed at which the character is moving. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)] public float speed = 5f;

        [Tooltip("Controls the speed at which the character is turning. Can be adjusted between a value of 0 and 20.")]
        [Range(0, 20)] public float rotateChar = 12f;
        // ------------------------------------------------ \\ 

        // Variables that must be set at Start
        private GameObject playerBody;
        private GameObject playerParent;
        private CharacterController controller;
        private NavMeshAgent agent;
        private Camera mainCamera;

        // Player movement variables
        private Vector3 moveDirection = Vector3.zero;
        private Vector3 headDirection = Vector3.zero;

        // Mouse turning variables
        private int floorMask;
        private float cameraRayLength = 100f;
        private DeviceManager myDeviceManager;

        #endregion

        private void Start()
        {
            playerBody = gameObject.transform.GetChild(0).gameObject;
            playerParent = gameObject;
            controller = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();

            mainCamera = GameObject.FindObjectOfType<Camera>();
            myDeviceManager = mainCamera.GetComponent<DeviceManager>();

            floorMask = LayerMask.GetMask("Floor");
            playerParent.transform.forward = mainCamera.transform.forward;
        }

        private void Update()
        {
            Move(controller);
        }

        public void Move(CharacterController controller)
        {
            moveDirection = new Vector3(myDeviceManager.HorizontalInput, 0, myDeviceManager.VerticalInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            agent.destination = playerParent.transform.position;
            agent.updateRotation = false;

            if (!myDeviceManager.isController)
            {
                // If the player is using a mouse and keyboard, use raycasting to find the location of the mouse
                // The player will ALWAYS face the mouse direction, even when the mouse is not active
                var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                var floorHit = new RaycastHit();

                if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
                {
                    var playerToMouse = floorHit.point - transform.position;
                    playerToMouse.y = 0f;
                    playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(playerToMouse), Time.deltaTime * rotateChar);
                }
            }

            if (myDeviceManager.isController)
            {
                headDirection = new Vector3(myDeviceManager.RightStickHorizontal, 0, myDeviceManager.RightStickVertical);

                // If the left stick is being used and the right stick is not, adjust the character body to align with the left 
                if (moveDirection != Vector3.zero && headDirection == Vector3.zero)                {
                    playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateChar);
                }
                // If the right stick is being used, override the character body's rotation to align with the right stick
                else if (headDirection != Vector3.zero)
                {
                    playerBody.transform.parent = playerParent.transform;
                    playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(headDirection), Time.deltaTime * rotateChar);
                }
            }

            controller.Move(moveDirection * Time.deltaTime);
        }

        public void Dash(CharacterController controller)
        {
            // This feature has not yet been implemented
        }
    }
}
