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
        public bool PressedTabLeft { get; private set; }
        public bool PressedTabRight { get; private set; }

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

        private bool pauseInputEnabled = false;
        private bool gameInputEnabled = false;

        public bool PauseInputEnabled
        {
            get
            {
                return pauseInputEnabled;
            }

            set
            {
                pauseInputEnabled = value;
            }
        }

        public bool GameInputEnabled
        {
            get
            {
                return gameInputEnabled;
            }

            set
            {
                gameInputEnabled = value;
            }
        }

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
            //Inputs used for UI outside of Pause Menu
            PressedMenu = controls.Pause.MenuButton.WasPressed;
            PressedJournal = controls.Pause.JournalsButton.WasPressed;

            PressedConfirm = controls.Pause.ConfirmButton.WasPressed;
            PressedCancel = controls.Pause.CancelButton.WasPressed;

            //Inputs Unique to Pause Menu
            if (PauseInputEnabled)
            {
                PressedPageLeft = controls.Pause.PagesAxis.WasPressed && controls.Pause.PagesAxis.Value < 0;
                PressedPageRight = controls.Pause.PagesAxis.WasPressed && controls.Pause.PagesAxis.Value > 0;

                PressedTabLeft = controls.Pause.TabsAxis.WasPressed && controls.Pause.TabsAxis.Value < 0;
                PressedTabRight = controls.Pause.TabsAxis.WasPressed && controls.Pause.TabsAxis.Value > 0;
            }

            //Inputs used to control game actions
            if (GameInputEnabled)
            {
                //Doing this so if your joystick gets caught at a close to but non-zero value, the character doesnt creep forward which prevents you from attacking
                var tempMove = controls.Game.MoveAxes.Value;
                if(Mathf.Abs(tempMove.x) < 0.1f) { tempMove.x = 0; }
                if(Mathf.Abs(tempMove.y) < 0.1f) { tempMove.y = 0; }
                Move = tempMove;
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
