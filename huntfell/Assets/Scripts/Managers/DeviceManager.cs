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
                return move_Vertical;
            }
        }

        public float MoveAxis_Horizontal
        {
            get
            {
                return move_Horizontal;
            }
        }

        public float LookAxis_Vertical
        {
            get
            {
                return look_Vertical;
            }
        }

        public float LookAxis_Horizontal
        {
            get
            {
                return look_Horizontal;
            }
        }

        public float AltAxis_Horizontal
        {
            get
            {
                return altAxis_Horizontal;
            }
        }

        public float AltAxis_Vertical
        {
            get
            {
                return altAxis_Vertical;
            }
        }

        public bool PressedConfirm
        {
            get
            {
                return pressedConfirm;
            }
        }

        public bool PressedCancel
        {
            get
            {
                return pressedCancel;
            }
        }

        public bool PressedPause
        {
            get
            {
                return pressedPause;
            }
        }

        public bool PressedAttack
        {
            get
            {
                return pressedAttack;
            }
        }

        public bool PressedAim
        {
            get
            {
                return pressedAim;
            }
        }

        public bool PressedDash
        {
            get
            {
                return pressedDash;
            }
        }

        public bool PressedInteract
        {
            get
            {
                return pressedInteract;
            }
        }

        public bool PressedElementUp
        {
            get
            {
                return pressedElementUp;
            }

        }

        public bool PressedElementDown
        {
            get
            {
                return pressedElementDown;
            }
        }

        public bool PressedWeaponSwitchLeft
        {
            get
            {
                return pressedSwitchRanged;
            }
        }

        public bool PressedWeaponSwitchRight
        {
            get
            {
                return pressedSwitchMelee;
            }
        }

        public bool PressedPotion
        {
            get
            {
                return pressedPotion;
            }
        }
        #endregion

        #region Variables
        private float move_Vertical;
        private float move_Horizontal;
        private float look_Vertical;
        private float look_Horizontal;

        private float altAxis_Horizontal;
        private float altAxis_Vertical;

        private bool pressedConfirm;
        private bool pressedCancel;
        private bool pressedPause;
        private bool pressedAttack;
        private bool pressedAim;
        private bool pressedDash;
        private bool pressedInteract;
        private bool pressedPotion;

        private bool pressedElementUp;
        private bool pressedElementDown;
        private bool pressedSwitchRanged;
        private bool pressedSwitchMelee;

        [Tooltip("If true, the active device is a controller. If false, the active device is the keyboard / mouse.")]
        public bool isController;

        public InputDevice Device { get; private set; }

        public Controls controls;

        public static DeviceManager instance;
        #endregion

        private void Awake ()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            NewDeviceAttatched(InputManager.ActiveDevice);
            InputManager.OnDeviceAttached += NewDeviceAttatched;
            InputManager.OnDeviceDetached += OldDeviceDetached;
        }

        private void NewDeviceAttatched(InputDevice device)
        {
            if (InputManager.Devices.Count == 0)
            {
                controls = Controls.KeyboardDefaultLayout();
                isController = false;
            }
            else if (InputManager.Devices.Count > 0)
            {
                controls = Controls.ControllerFaceLayout();
                isController = true;
            }

            Device = device;
        }

        private void OldDeviceDetached (InputDevice device)
        {
            if (InputManager.Devices.Count == 0)
            {
                controls = Controls.KeyboardDefaultLayout();
                isController = false;
            }
            else if (InputManager.Devices.Count > 0)
            {
                controls = Controls.ControllerFaceLayout();
                isController = true;
            }

            Device = InputManager.ActiveDevice;
        }

        private void Update ()
        {
            move_Horizontal = controls.moveAxes.X;
            move_Vertical = controls.moveAxes.Y;
            look_Horizontal = controls.lookAxes.X;
            look_Vertical = controls.lookAxes.Y;

            pressedPause = controls.openMenuButton.WasPressed;
            pressedConfirm = controls.confirmButton.WasPressed;
            pressedCancel = controls.cancelButton.WasPressed;

            pressedAttack = controls.attackButton.WasPressed;
            pressedAim = controls.aim.WasPressed;
            pressedDash = controls.dashButton.WasPressed;
            pressedInteract = controls.interactButton.WasPressed;
            pressedPotion = controls.potionButton.WasPressed;

            pressedElementUp = controls.switchElement.Value < 0;
            pressedElementDown = controls.switchElement.Value > 0;
            pressedSwitchRanged = controls.switchWeaponTypeRangedButton.WasPressed;
            pressedSwitchMelee = controls.switchWeaponTypeMeleeButton.WasPressed;
        }
    }
}
