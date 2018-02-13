using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

namespace CharacterScripts
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

        public InputDevice Device { get; set; }
        private Controls controls;

        public bool isController;
        public bool isMouseKeyboard;

        private void OnEnable()
        {
            controls = Controls.DefaultBindings();
        }

        private void Update()
        {
            Device = InputManager.ActiveDevice;

            HorizontalInput = controls.move.X;
            VerticalInput = controls.move.Y;
            RightStickHorizontal = controls.look.X;
            RightStickVertical = controls.look.Y;

            InputManager.OnActiveDeviceChanged += Device => Debug.Log("Switched: " + Device.Name);
            Debug.Log("Active Device is " + Device.DeviceStyle);
        }
    }
}
