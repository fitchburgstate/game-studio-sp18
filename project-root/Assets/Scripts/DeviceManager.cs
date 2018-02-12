using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

namespace CharacterScripts
{
    public class DeviceManager : MonoBehaviour
    {
        public InputDevice Device { get; set; }

        private Controls controls;

        public float VerticalInput
        {
            get
            {
                return verticalInput;
            }

            set
            {
                verticalInput = value;
            }
        }

        public float HorizontalInput
        {
            get
            {
                return horizontalInput;
            }

            set
            {
                horizontalInput = value;
            }
        }

        public float RightStickVertical
        {
            get
            {
                return rightStickVertical;
            }

            set
            {
                rightStickVertical = value;
            }
        }

        public float RightStickHorizontal
        {
            get
            {
                return rightStickHorizontal;
            }

            set
            {
                rightStickHorizontal = value;
            }
        }

        private float verticalInput;
        private float horizontalInput;

        private float rightStickVertical;
        private float rightStickHorizontal;

        private void OnEnable()
        {
            controls = Controls.DefaultBindings();
        }

        private void Update()
        {
            Device = InputManager.ActiveDevice; // Since the game is singleplayer, we don't need to assign a specific controller, we can just call on the active device

            if (Device != null)
            {
                HorizontalInput = Device.LeftStick.X;
                VerticalInput = Device.LeftStick.Y;

                RightStickHorizontal = Device.RightStick.X;
                RightStickVertical = Device.RightStick.Y;
            }
            else 
            {
                HorizontalInput = Input.GetAxis("Horizontal");
                VerticalInput = Input.GetAxis("Vertical");
            }
        }
    }
}
