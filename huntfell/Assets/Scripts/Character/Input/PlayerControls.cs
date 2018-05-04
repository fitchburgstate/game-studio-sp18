using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace Hunter {

    public enum ControlsLayout
    {
        Controller_Face,
        Controller_Trigger,
        Keyboard_and_Mouse
    }

    public class PlayerControls
    {
        #region Variables
        public const ControlsLayout DEFAULT_LAYOUT = ControlsLayout.Keyboard_and_Mouse;
        public const ControlsLayout DEFAULT_CONTROLLER_LAYOUT = ControlsLayout.Controller_Face;

        public UIInput UI { get; private set; }
        public GameInput Game { get; private set; }
        #endregion

        public abstract class InputActions : PlayerActionSet
        {
            public abstract void SetBindings (ControlsLayout layout);
            public abstract void SetDevice (InputDevice device);
        }

        public class UIInput : InputActions
        {
            public PlayerAction ConfirmButton { get; private set; }
            public PlayerAction CancelButton { get; private set; }
            public PlayerAction MenuButton { get; private set; }
            public PlayerAction JournalsButton { get; private set; }

            private PlayerAction cyclePage_Negative;
            private PlayerAction cyclePage_Positive;

            public PlayerOneAxisAction PagesAxis { get; private set; }

            public UIInput ()
            {
                ConfirmButton = CreatePlayerAction("Confirm");
                CancelButton = CreatePlayerAction("Cancel");
                MenuButton = CreatePlayerAction("Pause");
                JournalsButton = CreatePlayerAction("Journals");

                cyclePage_Negative = CreatePlayerAction("Page Left");
                cyclePage_Positive = CreatePlayerAction("Page Right");

                PagesAxis = CreateOneAxisPlayerAction(cyclePage_Negative, cyclePage_Positive);
            }

            public override void SetBindings (ControlsLayout layout)
            {
                if (layout == ControlsLayout.Controller_Face || layout == ControlsLayout.Controller_Trigger)
                {
                    // UI Input Bindings
                    ConfirmButton.AddDefaultBinding(InputControlType.Action1);

                    CancelButton.AddDefaultBinding(InputControlType.Action2);

                    // This is in controller bindings anyways just for debugging and shit
                    MenuButton.AddDefaultBinding(Key.Escape);
                    MenuButton.AddDefaultBinding(InputControlType.Start);

                    JournalsButton.AddDefaultBinding(InputControlType.Select);

                    cyclePage_Negative.AddDefaultBinding(InputControlType.LeftBumper);

                    cyclePage_Positive.AddDefaultBinding(InputControlType.RightBumper);
                }
                else if (layout == ControlsLayout.Keyboard_and_Mouse)
                {
                    ConfirmButton.AddDefaultBinding(Key.Return);
                    ConfirmButton.AddDefaultBinding(Mouse.LeftButton);

                    CancelButton.AddDefaultBinding(Key.Escape);

                    MenuButton.AddDefaultBinding(Key.Escape);

                    JournalsButton.AddDefaultBinding(Key.Tab);

                    cyclePage_Negative.AddDefaultBinding(Key.A);

                    cyclePage_Positive.AddDefaultBinding(Key.D);
                }
                else
                {
                    Debug.LogWarning("Could not set UI bindings with the layout provided, falling back to the default layout.");
                    SetBindings(DEFAULT_LAYOUT);
                }
            }

            public override void SetDevice (InputDevice device)
            {
                Device = device;
            }
        }

        public class GameInput : InputActions
        {
            // Left Axis Input Actions
            private PlayerAction moveAxis_NegativeX;
            private PlayerAction moveAxis_PositiveX;
            private PlayerAction moveAxis_NegativeY;
            private PlayerAction moveAxis_PositiveY;

            public PlayerTwoAxisAction MoveAxes { get; private set; }

            // Right Axis Input Actions
            private PlayerAction lookAxis_NegativeX;
            private PlayerAction lookAxis_PositiveX;
            private PlayerAction lookAxis_NegativeY;
            private PlayerAction lookAxis_PositiveY;

            public PlayerTwoAxisAction LookAxes { get; private set; }

            // Weapon Input Actions
            public PlayerAction WeaponTypeRangedButton { get; private set; }
            public PlayerAction WeaponTypeMeleeButton { get; private set; }

            private PlayerAction cycleElement_Negative;
            private PlayerAction cycleElement_Positive;

            public PlayerOneAxisAction ElementsAxis { get; private set; }

            private PlayerAction cycleWeapon_Negative;
            private PlayerAction cycleWeapon_Positive;

            public PlayerOneAxisAction WeaponsAxis { get; private set; }

            // Main Game Input Actions
            public PlayerAction AttackButton { get; private set; }
            public PlayerAction DashButton { get; private set; }
            public PlayerAction InteractButton { get; private set; }
            public PlayerAction PotionButton { get; private set; }
            //public PlayerAction aim;

            public GameInput ()
            {
                // Move Axis
                moveAxis_NegativeX = CreatePlayerAction("Move Left");
                moveAxis_PositiveX = CreatePlayerAction("Move Right");
                moveAxis_NegativeY = CreatePlayerAction("Move Down");
                moveAxis_PositiveY = CreatePlayerAction("Move Up");

                MoveAxes = CreateTwoAxisPlayerAction(moveAxis_NegativeX, moveAxis_PositiveX, moveAxis_NegativeY, moveAxis_PositiveY);

                // Look Axis
                lookAxis_NegativeX = CreatePlayerAction("Look Left");
                lookAxis_PositiveX = CreatePlayerAction("Look Right");
                lookAxis_NegativeY = CreatePlayerAction("Look Down");
                lookAxis_PositiveY = CreatePlayerAction("Look Up");

                LookAxes = CreateTwoAxisPlayerAction(lookAxis_NegativeX, lookAxis_PositiveX, lookAxis_NegativeY, lookAxis_PositiveX);

                // Weapons
                WeaponTypeRangedButton = CreatePlayerAction("Switch to Ranged");
                WeaponTypeMeleeButton = CreatePlayerAction("Switch to Melee");

                cycleElement_Negative = CreatePlayerAction("Cycle Element Down");
                cycleElement_Positive = CreatePlayerAction("Cycle Element Up");

                ElementsAxis = CreateOneAxisPlayerAction(cycleElement_Negative, cycleElement_Positive);

                cycleWeapon_Negative = CreatePlayerAction("Cycle Weapon Down");
                cycleWeapon_Positive = CreatePlayerAction("Cycle Weapon Up");

                WeaponsAxis = CreateOneAxisPlayerAction(cycleWeapon_Negative, cycleWeapon_Positive);

                // Main Game
                DashButton = CreatePlayerAction("Dash");
                AttackButton = CreatePlayerAction("Attack");
                InteractButton = CreatePlayerAction("Interact");
                PotionButton = CreatePlayerAction("Potion");
                //aim = CreatePlayerAction("Aim");
            }

            public override void SetBindings (ControlsLayout layout)
            {
                if (layout == ControlsLayout.Controller_Face || layout == ControlsLayout.Controller_Trigger)
                {
                    // Movement Bindings
                    moveAxis_NegativeX.AddDefaultBinding(InputControlType.LeftStickLeft);
                    moveAxis_PositiveX.AddDefaultBinding(InputControlType.LeftStickRight);
                    moveAxis_NegativeY.AddDefaultBinding(InputControlType.LeftStickDown);
                    moveAxis_PositiveY.AddDefaultBinding(InputControlType.LeftStickUp);

                    // Look Bindings
                    lookAxis_NegativeX.AddDefaultBinding(InputControlType.RightStickLeft);
                    lookAxis_PositiveX.AddDefaultBinding(InputControlType.RightStickRight);
                    lookAxis_NegativeY.AddDefaultBinding(InputControlType.RightStickDown);
                    lookAxis_PositiveY.AddDefaultBinding(InputControlType.RightStickUp);
                    
                    // Weapons Management Bindings
                    cycleElement_Negative.AddDefaultBinding(InputControlType.DPadDown);
                    cycleElement_Positive.AddDefaultBinding(InputControlType.DPadUp);

                    cycleWeapon_Negative.AddDefaultBinding(InputControlType.DPadRight);
                    cycleWeapon_Positive.AddDefaultBinding(InputControlType.DPadLeft);

                    //WeaponTypeRangedButton.AddDefaultBinding(InputControlType.DPadLeft);
                    //WeaponTypeMeleeButton.AddDefaultBinding(InputControlType.DPadRight);

                    // Main Bindings
                    if (layout == ControlsLayout.Controller_Face)
                    {
                        AttackButton.AddDefaultBinding(InputControlType.Action3);

                        DashButton.AddDefaultBinding(InputControlType.Action2);
                    }
                    else
                    {
                        AttackButton.AddDefaultBinding(InputControlType.RightTrigger);

                        DashButton.AddDefaultBinding(InputControlType.Action1);
                    }

                    InteractButton.AddDefaultBinding(InputControlType.Action1);

                    PotionButton.AddDefaultBinding(InputControlType.Action4);
                }
                else if (layout == ControlsLayout.Keyboard_and_Mouse)
                {
                    // Movement Bindings
                    moveAxis_NegativeY.AddDefaultBinding(Key.S);
                    moveAxis_PositiveY.AddDefaultBinding(Key.W);
                    moveAxis_NegativeX.AddDefaultBinding(Key.A);
                    moveAxis_PositiveX.AddDefaultBinding(Key.D);

                    // Look Bindings
                    lookAxis_NegativeX.AddDefaultBinding(Mouse.NegativeX);
                    lookAxis_PositiveX.AddDefaultBinding(Mouse.PositiveX);
                    lookAxis_NegativeY.AddDefaultBinding(Mouse.NegativeY);
                    lookAxis_PositiveY.AddDefaultBinding(Mouse.PositiveY);

                    // Weapon Management Bindings
                    cycleElement_Positive.AddDefaultBinding(Key.LeftShift);
                    cycleElement_Negative.AddDefaultBinding(Key.LeftControl);

                    cycleWeapon_Positive.AddDefaultBinding(Mouse.PositiveScrollWheel);
                    cycleWeapon_Negative.AddDefaultBinding(Mouse.NegativeScrollWheel);

                    WeaponTypeMeleeButton.AddDefaultBinding(Key.Key1);
                    WeaponTypeRangedButton.AddDefaultBinding(Key.Key2);

                    // Main Bindings
                    AttackButton.AddDefaultBinding(Mouse.LeftButton);

                    DashButton.AddDefaultBinding(Key.Space);

                    InteractButton.AddDefaultBinding(Key.E);

                    PotionButton.AddDefaultBinding(Key.Q);
                }
                else
                {
                    Debug.LogWarning("Could not set the Game bindings with the layout provided, falling back to the default layout.");
                    SetBindings(DEFAULT_LAYOUT);
                }
            }

            public override void SetDevice (InputDevice device)
            {
                Device = device;
            }
        }

        public PlayerControls ()
        {
            SetBindingsAll(DEFAULT_LAYOUT);
        }

        public PlayerControls (ControlsLayout layout)
        {
            SetBindingsAll(layout);
        }

        private void RefreshBindings ()
        {
            //Debug.Log("Erasing current bindings...");
            UI?.Destroy();
            Game?.Destroy();
            UI = new UIInput();
            Game = new GameInput();
        }

        public void SetBindingsAll (ControlsLayout layout)
        {
            RefreshBindings();
            Debug.Log("Setting the bindings to use the layout " + layout);
            UI?.SetBindings(layout);
            Game?.SetBindings(layout);
        }

        public void SetDeviceAll(InputDevice device)
        {
            if(device != null) { Debug.Log("Attaching new device: " + device.Name); }
            else { Debug.LogWarning("No valid devices were detected, falling back to Active Input."); }
            UI?.SetDevice(device);
            Game?.SetDevice(device);
        }
    }
}
