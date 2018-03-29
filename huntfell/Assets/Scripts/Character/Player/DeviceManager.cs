using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

namespace Hunter
{
    public class DeviceManager : MonoBehaviour
    {
        #region Properties
        public float MoveAxis_Vertical
        {
            get
            {
                return moveAxis_Vertical;
            }

            set
            {
                moveAxis_Vertical = value;
            }
        }

        public float MoveAxis_Horizontal
        {
            get
            {
                return moveAxis_Horizontal;
            }

            set
            {
                moveAxis_Horizontal = value;
            }
        }

        public float LookAxis_Vertical
        {
            get
            {
                return lookAxis_Vertical;
            }

            set
            {
                lookAxis_Vertical = value;
            }
        }

        public float LookAxis_Horizontal
        {
            get
            {
                return lookAxis_Horizontal;
            }

            set
            {
                lookAxis_Horizontal = value;
            }
        }

        public float AltAxis_Horizontal
        {
            get
            {
                return altAxis_Horizontal;
            }

            set
            {
                altAxis_Horizontal = value;
            }
        }

        public float AltAxis_Vertical
        {
            get
            {
                return altAxis_Vertical;
            }

            set
            {
                altAxis_Vertical = value;
            }
        }

        public bool PressedConfirm
        {
            get
            {
                return pressedConfirm;
            }

            set
            {
                pressedConfirm = value;
            }
        }

        public bool PressedCancel
        {
            get
            {
                return pressedCancel;
            }

            set
            {
                pressedCancel = value;
            }
        }

        public bool PressedPause
        {
            get
            {
                return pressedPause;
            }

            set
            {
                pressedPause = value;
            }
        }

        public bool PressedAttack
        {
            get
            {
                return pressedAttack;
            }

            set
            {
                pressedAttack = value;
            }
        }

        public bool PressedAim
        {
            get
            {
                return pressedAim;
            }

            set
            {
                pressedAim = value;
            }
        }

        public bool PressedDash
        {
            get
            {
                return pressedDash;
            }

            set
            {
                pressedDash = value;
            }
        }

        public bool PressedElementUp
        {
            get
            {
                return pressedElementUp;
            }

            set
            {
                pressedElementUp = value;
            }
        }

        public bool PressedElementDown
        {
            get
            {
                return pressedElementDown;
            }

            set
            {
                pressedElementDown = value;
            }
        }

        public bool PressedWeaponSwitchLeft
        {
            get
            {
                return pressedWeaponSwitchLeft;
            }

            set
            {
                pressedWeaponSwitchLeft = value;
            }
        }

        public bool PressedWeaponSwitchRight
        {
            get
            {
                return pressedWeaponSwitchRight;
            }

            set
            {
                pressedWeaponSwitchRight = value;
            }
        }
        #endregion

        #region Variables
        private float moveAxis_Vertical;
        private float moveAxis_Horizontal;
        private float lookAxis_Vertical;
        private float lookAxis_Horizontal;

        private float altAxis_Horizontal;
        private float altAxis_Vertical;

        private bool pressedConfirm;
        private bool pressedCancel;
        private bool pressedPause;
        private bool pressedAttack;
        private bool pressedAim;
        private bool pressedDash;

        private bool pressedElementUp;
        private bool pressedElementDown;
        private bool pressedWeaponSwitchLeft;
        private bool pressedWeaponSwitchRight;

        [Tooltip("If true, the active device is a controller. If false, the active device is the keyboard / mouse.")]
        public bool isController;

        public InputDevice Device { get; set; }

        public Controls controls;
        #endregion

        private void Awake()
        {
            //Detect if there are any controllers plugged in and if so use that, if not use the keyboard
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

            //Need this here so if something tried to use the controls in Update we dont get an NRE because of execution order
            Device = InputManager.ActiveDevice;
        }

        private void Update ()
        {
            Device = InputManager.ActiveDevice;

            MoveAxis_Horizontal = controls.move.X;
            MoveAxis_Vertical = controls.move.Y;
            LookAxis_Horizontal = controls.look.X;
            LookAxis_Vertical = controls.look.Y;

            PressedPause = controls.pause.WasPressed;
            PressedConfirm = controls.confirm.WasPressed;
            PressedCancel = controls.cancel.WasPressed;

            PressedAttack = controls.attack.WasPressed;
            PressedAim = controls.aim.WasPressed;
            PressedDash = controls.dash.WasPressed;

            PressedElementUp = controls.altAxis_Up.WasPressed;
            PressedElementDown = controls.altAxis_Down.WasPressed;
            PressedWeaponSwitchLeft = controls.altAxis_Left.WasPressed;
            PressedWeaponSwitchRight = controls.altAxis_Right.WasPressed;

            // This is for debugging purposes only, and should be deprecated
            if (controls.pause.WasReleased)
            {
                SetBinding();
            }
        }

        public void SetBinding()
        {
            //Checks to see what your current input method is and then switches to the opposite
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
