﻿using System.Collections;
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

        private float verticalInput;
        private float horizontalInput;
        private float rightStickVertical;
        private float rightStickHorizontal;

        [Tooltip("If true, the active device is a controller. If false, the active device is the keyboard / mouse.")]
        public bool isController;

        public InputDevice Device { get; set; }
        public Controls controls;

        private void Start()
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
