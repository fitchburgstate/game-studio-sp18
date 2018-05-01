using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InControl;

namespace Hunter
{
    public class DeviceManager : MonoBehaviour
    {
        #region Variables
        private PlayerControls controls;
        private InputDevice currentDevice;

        public Vector2 Move { get; private set; } = new Vector2();
        public Vector2 Look { get; private set; } = new Vector2();

        public bool PressedConfirm { get; private set; }
        public bool PressedCancel { get; private set; }
        public bool PressedMenu { get; private set; }
        public bool PressedJournal { get; private set; }
        public bool PressedPageLeft { get; private set; }
        public bool PressedPageRight { get; private set; }

        public bool PressedElementUp { get; private set; }
        public bool PressedElementDown { get; private set; }
        public bool PressedWeaponUp { get; private set; }
        public bool PressedWeaponDown { get; private set; }
        public bool PressedSwitchRanged { get; private set; }
        public bool PressedSwitchMelee { get; private set; }

        public bool PressedAttack { get; private set; }
        public bool PressedDash { get; private set; }
        public bool PressedInteract { get; private set; }
        public bool PressedPotion { get; private set; }

        public bool overrideLayout;
        public ControlsLayout inspectorLayout;

        public bool uiInputEnabled = true;
        public bool gameInputEnabled = true;

        #endregion

        private void Awake ()
        {
            if (overrideLayout) { controls = new PlayerControls(inspectorLayout); }
            else { controls = new PlayerControls(); }

            InputManager.OnDeviceAttached += NewDeviceAttatched;
            InputManager.OnDeviceDetached += OldDeviceDetached;

            //For initalization if you start the game with a controller already attatched
            SeekNewDevice();
        }

        private void SeekNewDevice ()
        {
            foreach (var device in InputManager.Devices)
            {
                if (device.DeviceClass == InputDeviceClass.Controller)
                {
                    NewDeviceAttatched(device);
                    break;
                }
            }
        }

        private void NewDeviceAttatched (InputDevice device)
        {
            controls.SetDeviceAll(device);
            currentDevice = device;

            if (overrideLayout) { return; }

            if (device != null && device.DeviceClass == InputDeviceClass.Controller)
            {
                controls.SetBindingsAll(PlayerControls.DEFAULT_CONTROLLER_LAYOUT);
            }
            else
            {
                controls.SetBindingsAll(PlayerControls.DEFAULT_LAYOUT);
            }
        }

        public void SetDeviceVibration (float leftMotor, float rightMotor)
        {
            if (leftMotor == 0 && rightMotor == 0) { currentDevice.StopVibration(); }
            else { currentDevice.Vibrate(leftMotor, rightMotor); }
        }

        private void OldDeviceDetached (InputDevice device)
        {
            if (InputManager.Devices.Count == 0)
            {
                controls.SetDeviceAll(InputManager.ActiveDevice);
                currentDevice = InputManager.ActiveDevice;
                if (overrideLayout) { return; }
                controls.SetBindingsAll(PlayerControls.DEFAULT_LAYOUT);
            }
            else
            {
                SeekNewDevice();
            }
        }

        private void Update ()
        {
            //UI Inputs
            if (uiInputEnabled)
            {
                PressedMenu = controls.UI.MenuButton.WasPressed;
                PressedJournal = controls.UI.JournalsButton.WasPressed;

                PressedConfirm = controls.UI.ConfirmButton.WasPressed;
                PressedCancel = controls.UI.CancelButton.WasPressed;

                PressedPageLeft = controls.UI.PagesAxis.WasPressed && controls.UI.PagesAxis.Value < 0;
                PressedPageRight = controls.UI.PagesAxis.WasPressed && controls.UI.PagesAxis.Value > 0;
            }

            //Game Inputs
            if (gameInputEnabled)
            {
                Move = controls.Game.MoveAxes.Value;
                Look = controls.Game.LookAxes.Value;

                PressedElementDown = controls.Game.ElementsAxis.WasPressed && controls.Game.ElementsAxis.Value < 0;
                PressedElementUp = controls.Game.ElementsAxis.WasPressed && controls.Game.ElementsAxis.Value > 0;

                PressedWeaponDown = controls.Game.WeaponsAxis.WasPressed && controls.Game.WeaponsAxis.Value < 0;
                PressedWeaponUp = controls.Game.WeaponsAxis.WasPressed && controls.Game.WeaponsAxis.Value > 0;

                PressedSwitchRanged = controls.Game.WeaponTypeRangedButton.WasPressed;
                PressedSwitchMelee = controls.Game.WeaponTypeMeleeButton.WasPressed;

                PressedAttack = controls.Game.AttackButton.WasPressed;
                PressedDash = controls.Game.DashButton.WasPressed;
                PressedInteract = controls.Game.InteractButton.WasPressed;
                PressedPotion = controls.Game.PotionButton.WasPressed;
            }
        }
    }
}
