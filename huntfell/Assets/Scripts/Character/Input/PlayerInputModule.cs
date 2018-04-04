using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hunter.Character
{
    public class PlayerInputModule : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// Represents which direction the character should move in. Think of it as the input of the left stick.
        /// </summary>
        private Vector3 moveDirection = Vector3.zero;

        /// <summary>
        /// Represents which direction the character should look in. Think of it as the input of the right stick.
        /// </summary>
        private Vector3 lookDirection = Vector3.zero;
        private Vector3 animLookDirection = Vector3.zero;

        private DeviceManager myDeviceManager;

        private IAttack attackCharacter;
        private IMoveable moveCharacter;
        #endregion

        private void Start ()
        {
            //Whoever this script is on is being controlled by the Player, so naturally they should be tagged as such
            transform.tag = "Player";

            var mainCamera = Camera.main;
            transform.forward = mainCamera.transform.forward;
            myDeviceManager = DeviceManager.instance;

            attackCharacter = GetComponent<IAttack>();
            moveCharacter = GetComponent<IMoveable>();
        }

        private void Update ()
        {
            moveDirection = new Vector3(myDeviceManager.MoveAxis_Horizontal, 0, myDeviceManager.MoveAxis_Vertical);

            lookDirection = new Vector3(myDeviceManager.LookAxis_Horizontal, 0, myDeviceManager.LookAxis_Vertical);
            animLookDirection = lookDirection;
            // If the left axis is being used and the right axis is not, adjust the character body to align with the left 
            if (moveDirection != Vector3.zero && lookDirection == Vector3.zero)
            {
                lookDirection = moveDirection;
            }
            

            if (moveCharacter != null)
            {
                moveCharacter.Move(moveDirection, lookDirection, animLookDirection);
            }


            if (myDeviceManager.PressedAttack && attackCharacter != null)
            {
                attackCharacter.Attack();
            }
            else if (myDeviceManager.PressedWeaponSwitchLeft && attackCharacter != null)
            {
                attackCharacter.SwitchWeapon(myDeviceManager.PressedWeaponSwitchLeft, myDeviceManager.PressedWeaponSwitchRight);
            }
            else if (myDeviceManager.PressedWeaponSwitchRight && attackCharacter != null)
            {
                attackCharacter.SwitchWeapon(myDeviceManager.PressedWeaponSwitchLeft, myDeviceManager.PressedWeaponSwitchRight);
            }
            else if((myDeviceManager.PressedElementDown || myDeviceManager.PressedElementUp) && attackCharacter != null)
            {
                attackCharacter.SwitchElement(myDeviceManager.PressedElementUp, myDeviceManager.PressedElementDown);
            }
            else if (myDeviceManager.PressedDash && moveCharacter != null)
            {
                moveCharacter.Dash();
            }
            //else if (myDeviceManager.PressedAim && attackCharacter != null)
            //{

            //}
            else if (myDeviceManager.PressedPause)
            {

            }
        }
    }
}
