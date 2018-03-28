using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

namespace Hunter
{
    public class DeviceManager : MonoBehaviour
    {
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

        /// <summary>
        /// This is the vertical value of the left stick.
        /// </summary>
        private float verticalInput;

        /// <summary>
        /// This is the horizontal value of the left stick.
        /// </summary>
        private float horizontalInput;

        /// <summary>
        /// This is the vertical value of the right stick.
        /// </summary>
        private float rightStickVertical;

        /// <summary>
        /// This is the horizontal value of the right stick.
        /// </summary>
        private float rightStickHorizontal;

        /// <summary>
        /// A boolean to determine if a controller is being used or not.
        /// </summary>
        [Tooltip("If true, the active device is a controller. If false, the active device is the keyboard / mouse.")]
        public bool isController;

        public InputDevice Device { get; set; }
        public Controls controls;

        private void Awake()
        {
            if (InputManager.Devices.Count == 0)
            {
                controls = Controls.KeyboardBindings();
                isController = false;
            }
            else if (InputManager.Devices.Count > 0)
            {
                controls = Controls.ControllerBindings();
                isController = true;
            }
            Device = InputManager.ActiveDevice;
        }

        private void Update()
        {
            Device = InputManager.ActiveDevice;

            HorizontalInput = controls.move.X;
            VerticalInput = controls.move.Y;
            RightStickHorizontal = controls.look.X;
            RightStickVertical = controls.look.Y;

            if (controls.pause.WasReleased)
            {
                SetBinding();
            }
        }

        public void SetBinding()
        {
            if (isController)
            {
                controls = Controls.KeyboardBindings();
                isController = false;
            }
            else if (!isController)
            {
                controls = Controls.ControllerBindings();
                isController = true;
            }
        }
    }
}
