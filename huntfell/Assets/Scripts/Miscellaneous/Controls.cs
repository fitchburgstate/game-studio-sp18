using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace Hunter {

    public class Controls : PlayerActionSet
    {
        #region Variables
        // UI Input Actions
        public PlayerAction confirmButton;
        public PlayerAction cancelButton;
        public PlayerAction openMenuButton;
        public PlayerAction openJournalsButton;

        private PlayerAction cyclePage_Negative;
        private PlayerAction cyclePage_Positive;

        public PlayerOneAxisAction cyclePagesAxis;

        // Left Axis Input Actions
        private PlayerAction moveAxis_XNegative;
        private PlayerAction moveAxis_XPositive;
        private PlayerAction moveAxis_YNegative;
        private PlayerAction moveAxis_YPositive;

        public PlayerTwoAxisAction moveAxes;

        // Right Axis Input Actions
        private PlayerAction lookAxis_XNegative;
        private PlayerAction lookAxis_XPositive;
        private PlayerAction lookAxis_YNegative;
        private PlayerAction lookAxis_YPositive;

        public PlayerTwoAxisAction lookAxes;

        // Weapon Input Actions
        public PlayerAction switchWeaponTypeRangedButton;
        public PlayerAction switchWeaponTypeMeleeButton;

        private PlayerAction cycleElement_Negative;
        private PlayerAction cycleElement_Positive;

        public PlayerOneAxisAction cycleElementsAxis;

        private PlayerAction cycleWeapon_Negative;
        private PlayerAction cycleWeapon_Positive;

        public PlayerOneAxisAction cycleWeaponsAxis;

        // Main Game Input Actions
        public PlayerAction attackButton;
        public PlayerAction dashButton;
        public PlayerAction interactButton;
        public PlayerAction potionButton;
        //public PlayerAction aim;
        #endregion

        public Controls ()
        {
            // UI
            confirmButton = CreatePlayerAction("Confirm");
            cancelButton = CreatePlayerAction("Cancel");
            openMenuButton = CreatePlayerAction("Pause");
            openJournalsButton = CreatePlayerAction("Journals");

            cyclePage_Negative = CreatePlayerAction("Page Left");
            cyclePage_Positive = CreatePlayerAction("Page Right");

            cyclePagesAxis = CreateOneAxisPlayerAction(cyclePage_Negative, cyclePage_Positive);

            // Move Axis
            moveAxis_XNegative = CreatePlayerAction("Move Left");
            moveAxis_XPositive = CreatePlayerAction("Move Right");
            moveAxis_YPositive = CreatePlayerAction("Move Up");
            moveAxis_YNegative = CreatePlayerAction("Move Down");

            moveAxes = CreateTwoAxisPlayerAction(moveAxis_XNegative, moveAxis_XPositive, moveAxis_YNegative, moveAxis_YPositive);

            // Look Axis
            lookAxis_XNegative = CreatePlayerAction("Look Left");
            lookAxis_XPositive = CreatePlayerAction("Look Right");
            lookAxis_YNegative = CreatePlayerAction("Look Down");
            lookAxis_XPositive = CreatePlayerAction("Look Up");

            lookAxes = CreateTwoAxisPlayerAction(lookAxis_XNegative, lookAxis_XPositive, lookAxis_YNegative, lookAxis_XPositive);

            // Weapons
            switchWeaponTypeRangedButton = CreatePlayerAction("Switch to Ranged");
            switchWeaponTypeMeleeButton = CreatePlayerAction("Switch to Melee");

            cycleElement_Negative = CreatePlayerAction("Cycle Element Down");
            cycleElement_Positive = CreatePlayerAction("Cycle Element Up");

            cycleElementsAxis = CreateOneAxisPlayerAction(cycleElement_Negative, cycleElement_Positive);

            cycleWeapon_Negative = CreatePlayerAction("Cycle Weapon Down");
            cycleWeapon_Positive = CreatePlayerAction("Cycle Weapon Up");

            cycleWeaponsAxis = CreateOneAxisPlayerAction(cycleWeapon_Negative, cycleWeapon_Positive);

            // Main Game
            dashButton = CreatePlayerAction("Dash");
            attackButton = CreatePlayerAction("Attack");
            interactButton = CreatePlayerAction("Interact");
            potionButton = CreatePlayerAction("Potion");
            //aim = CreatePlayerAction("Aim");
        }

        public static Controls ControllerFaceLayout ()
        {
            var controls = new Controls();

            // UI Input Bindings
            controls.confirmButton.AddDefaultBinding(InputControlType.Action1);

            controls.cancelButton.AddDefaultBinding(InputControlType.Action2);

            // This is in controller bindings anyways just for debugging and shit
            controls.openMenuButton.AddDefaultBinding(Key.Escape);
            controls.openMenuButton.AddDefaultBinding(InputControlType.Start);

            controls.openJournalsButton.AddDefaultBinding(InputControlType.Select);

            controls.cyclePage_Negative.AddDefaultBinding(InputControlType.LeftBumper);

            controls.cyclePage_Positive.AddDefaultBinding(InputControlType.RightBumper);

            // Left Axis Input Bindings
            controls.moveAxis_YPositive.AddDefaultBinding(InputControlType.LeftStickUp);

            controls.moveAxis_YNegative.AddDefaultBinding(InputControlType.LeftStickDown);

            controls.moveAxis_XNegative.AddDefaultBinding(InputControlType.LeftStickLeft);

            controls.moveAxis_XPositive.AddDefaultBinding(InputControlType.LeftStickRight);

            // Right Axis Input Bindings
            controls.lookAxis_XNegative.AddDefaultBinding(InputControlType.RightStickLeft);

            controls.lookAxis_XPositive.AddDefaultBinding(InputControlType.RightStickRight);

            controls.lookAxis_YNegative.AddDefaultBinding(InputControlType.RightStickDown);

            controls.lookAxis_XPositive.AddDefaultBinding(InputControlType.RightStickUp);

            // Alt Axis Input Bindings
            controls.cycleElement_Positive.AddDefaultBinding(InputControlType.DPadUp);

            controls.cycleElement_Negative.AddDefaultBinding(InputControlType.DPadDown);

            controls.switchWeaponTypeRangedButton.AddDefaultBinding(InputControlType.DPadLeft);

            controls.switchWeaponTypeMeleeButton.AddDefaultBinding(InputControlType.DPadRight);

            controls.cc

            // Main Game Input Bindings
            controls.attackButton.AddDefaultBinding(InputControlType.Action1);

            controls.dashButton.AddDefaultBinding(InputControlType.Action2);

            controls.interactButton.AddDefaultBinding(InputControlType.Action3);

            controls.potionButton.AddDefaultBinding(InputControlType.Action4);

            //controls.aim.AddDefaultBinding(InputControlType.LeftTrigger);

            return controls;
        }

        public static Controls ControllerTriggerLayout ()
        {
            var controls = new Controls();

            // UI Input Bindings
            controls.confirmButton.AddDefaultBinding(InputControlType.Action1);

            controls.cancelButton.AddDefaultBinding(InputControlType.Action2);

            // This is in controller bindings anyways just for debugging and shit
            controls.openMenuButton.AddDefaultBinding(Key.Escape);
            controls.openMenuButton.AddDefaultBinding(InputControlType.Start);

            controls.openJournalsButton.AddDefaultBinding(InputControlType.Select);

            // Left Axis Input Bindings
            controls.moveAxis_YPositive.AddDefaultBinding(InputControlType.LeftStickUp);

            controls.moveAxis_YNegative.AddDefaultBinding(InputControlType.LeftStickDown);

            controls.moveAxis_XNegative.AddDefaultBinding(InputControlType.LeftStickLeft);

            controls.moveAxis_XPositive.AddDefaultBinding(InputControlType.LeftStickRight);

            // Right Axis Input Bindings
            controls.lookAxis_XNegative.AddDefaultBinding(InputControlType.RightStickLeft);

            controls.lookAxis_XPositive.AddDefaultBinding(InputControlType.RightStickRight);

            controls.lookAxis_YNegative.AddDefaultBinding(InputControlType.RightStickDown);

            controls.lookAxis_XPositive.AddDefaultBinding(InputControlType.RightStickUp);

            // Alt Axis Input Bindings
            controls.cycleElement_Positive.AddDefaultBinding(InputControlType.DPadUp);

            controls.cycleElement_Negative.AddDefaultBinding(InputControlType.DPadDown);

            controls.switchWeaponTypeRangedButton.AddDefaultBinding(InputControlType.DPadLeft);

            controls.switchWeaponTypeMeleeButton.AddDefaultBinding(InputControlType.DPadRight);

            // Main Game Input Bindings
            controls.attackButton.AddDefaultBinding(InputControlType.RightTrigger);

            controls.dashButton.AddDefaultBinding(InputControlType.Action1);

            controls.interactButton.AddDefaultBinding(InputControlType.Action3);

            controls.potionButton.AddDefaultBinding(InputControlType.Action4);

            //controls.aim.AddDefaultBinding(InputControlType.LeftTrigger);

            return controls;
        }
        
        public static Controls KeyboardDefaultLayout()
        {
            var controls = new Controls();

            // UI Input Bindings
            controls.confirmButton.AddDefaultBinding(Key.Return);

            controls.cancelButton.AddDefaultBinding(Key.Escape);

            controls.openMenuButton.AddDefaultBinding(Key.Escape);

            // Left Axis Input Bindings
            controls.moveAxis_YPositive.AddDefaultBinding(Key.W);

            controls.moveAxis_YNegative.AddDefaultBinding(Key.S);

            controls.moveAxis_XNegative.AddDefaultBinding(Key.A);

            controls.moveAxis_XPositive.AddDefaultBinding(Key.D);

            // Right Axis Input Bindings
            controls.lookAxis_XNegative.AddDefaultBinding(Mouse.NegativeX);

            controls.lookAxis_XPositive.AddDefaultBinding(Mouse.PositiveX);

            controls.lookAxis_YNegative.AddDefaultBinding(Mouse.NegativeY);

            controls.lookAxis_XPositive.AddDefaultBinding(Mouse.PositiveY);

            // Alt Axis Input Bindings
            controls.cycleElement_Positive.AddDefaultBinding(Mouse.PositiveScrollWheel);

            controls.cycleElement_Negative.AddDefaultBinding(Mouse.NegativeScrollWheel);

            controls.switchWeaponTypeMeleeButton.AddDefaultBinding(Key.Key1);

            controls.switchWeaponTypeRangedButton.AddDefaultBinding(Key.Key2);

            // Main Game Input Bindings
            controls.dashButton.AddDefaultBinding(Key.Space);

            controls.attackButton.AddDefaultBinding(Mouse.LeftButton);

            controls.interactButton.AddDefaultBinding(Key.E);
            controls.interactButton.AddDefaultBinding(Key.F);

            controls.aim.AddDefaultBinding(Mouse.RightButton);

            return controls;
        }
    }
}
