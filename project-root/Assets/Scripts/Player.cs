﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

namespace CharacterScripts
{
    public class Player : Character, IMoveable<CharacterController, InputDevice>
    {
        #region Variables
        // ---------- SET THESE IN THE INSPECTOR ---------- \\
        [Range(0, 20)] public float speed;
        [Range(0, 20)] public float rotateChar;
        // ------------------------------------------------ \\ 

        private GameObject playerBody;
        private GameObject playerParent;
        private CharacterController controller;
        private NavMeshAgent agent;
        public Camera mainCamera;

        private Vector3 moveDirection = Vector3.zero;
        private Vector3 headDirection = Vector3.zero;

        int floorMask;
        float cameraRayLength = 100f;
        DeviceManager myDeviceManager;

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
            if (myDeviceManager.Device != null)
            {
                Move(controller, myDeviceManager.Device);
            }
            else
            {
                Move(controller, myDeviceManager.Device);
            }
        }

        public void Move(CharacterController controller, InputDevice device)
        {
            moveDirection = new Vector3(myDeviceManager.HorizontalInput, 0, myDeviceManager.VerticalInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            agent.destination = playerParent.transform.position;
            agent.updateRotation = false;

            if (device != null)
            {
                headDirection = new Vector3(myDeviceManager.RightStickHorizontal, 0, myDeviceManager.RightStickVertical);

                if (moveDirection != Vector3.zero && headDirection == Vector3.zero) // If the left stick is being used and the right stick is not, adjust the character body to align with the left stick
                {
                    playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateChar);
                }
                else if (headDirection != Vector3.zero) // If the right stick is being used, override the character body's rotation to align with the right stick
                {
                    playerBody.transform.parent = playerParent.transform;
                    playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(headDirection), Time.deltaTime * rotateChar);
                }
            }

            else if (device == null)
            {
                var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                var floorHit = new RaycastHit();

                if (Physics.Raycast(cameraRay, out floorHit, cameraRayLength, floorMask))
                {
                    var playerToMouse = floorHit.point - transform.position;
                    playerToMouse.y = 0f;
                    var newRotation = Quaternion.LookRotation(playerToMouse);
                    playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(playerToMouse), Time.deltaTime * rotateChar);
                }
            }
            controller.Move(moveDirection * Time.deltaTime);
        }

        public void Dash(CharacterController controller, InputDevice device)
        {

        }
    }
}
