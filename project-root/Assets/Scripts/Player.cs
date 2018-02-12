using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace CharacterScripts
{
    public class Player : Character, IMoveable<CharacterController>
    {
        public InputDevice Device { get; set; }

        #region Variables
        // ---------- SET THESE IN THE INSPECTOR ---------- \\
        [Range(0, 20)] public float speed;
        [Range(0, 20)] public float rotateChar;
        // ------------------------------------------------ \\ 

        private GameObject playerBody;
        private GameObject playerParent;
        private Vector3 moveDirection = Vector3.zero;
        private Vector3 headDirection = Vector3.zero;
        private CharacterController controller;
        #endregion

        private void Awake()
        {
            playerBody = gameObject.transform.GetChild(0).gameObject;
            playerParent = gameObject;
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Device = InputManager.ActiveDevice; // Since the game is singleplayer, we don't need to assign a specific controller, we can just call on the active device

            if (Device != null)
            {
                Move(controller);
            }
            else
            {
                Debug.Log("There is no Device attached!");
            }
        }

        public void Move(CharacterController controller)
        {
            moveDirection = new Vector3(Device.LeftStick.X, 0, Device.LeftStick.Y);
            headDirection = new Vector3(Device.RightStick.X, 0, Device.RightStick.Y);

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (moveDirection != Vector3.zero && headDirection == Vector3.zero) // If the left stick is being used and the right stick is not, adjust the character body to align with the left stick
            {
                playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * rotateChar);
            }
            else if (headDirection != Vector3.zero) // If the right stick is being used, override the character body's rotation to align with the right stick
            {
                playerBody.transform.parent = playerParent.transform;
                playerBody.transform.rotation = Quaternion.Slerp(playerBody.transform.rotation, Quaternion.LookRotation(headDirection), Time.deltaTime * rotateChar);
            }

            controller.Move(moveDirection * Time.deltaTime);
        }

        public void Dash(CharacterController controller)
        {

        }
    }
}

