using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace Hunter { 

    public class Controls : PlayerActionSet
    {
        #region Variables
        // UI Input Actions
        public PlayerAction confirm;
        public PlayerAction cancel;
        public PlayerAction pause;

        // Left Axis Input Actions
        public PlayerAction leftAxis_Left;
        public PlayerAction leftAxis_Right;
        public PlayerAction leftAxis_Up;
        public PlayerAction leftAxis_Down;

        public PlayerTwoAxisAction move;

        // Right Axis Input Actions
        public PlayerAction rightAxis_Left;
        public PlayerAction rightAxis_Right;
        public PlayerAction rightAxis_Down;
        public PlayerAction rightAxis_Up;

        public PlayerTwoAxisAction look;

        // Alt Axis Input Actions
        public PlayerAction altAxis_Left;
        public PlayerAction altAxis_Right;
        public PlayerAction altAxis_Down;
        public PlayerAction altAxis_Up;

        public PlayerOneAxisAction switchWeapon;
        public PlayerOneAxisAction switchElement;

        // Main Game Input Actions
        public PlayerAction dash;
        public PlayerAction attack;
        public PlayerAction interact;
        public PlayerAction aim;
        #endregion

        public Controls ()
        {
            // UI
            confirm = CreatePlayerAction("Confirm");
            cancel = CreatePlayerAction("Cancel");
            pause = CreatePlayerAction("Pause");

            // Left Axis
            leftAxis_Left = CreatePlayerAction("Move Left");
            leftAxis_Right = CreatePlayerAction("Move Right");
            leftAxis_Up = CreatePlayerAction("Move Up");
            leftAxis_Down = CreatePlayerAction("Move Down");

            move = CreateTwoAxisPlayerAction(leftAxis_Left, leftAxis_Right, leftAxis_Down, leftAxis_Up);

            // Right Axis
            rightAxis_Left = CreatePlayerAction("Look Left");
            rightAxis_Right = CreatePlayerAction("Look Right");
            rightAxis_Down = CreatePlayerAction("Look Down");
            rightAxis_Up = CreatePlayerAction("Look Up");

            look = CreateTwoAxisPlayerAction(rightAxis_Left, rightAxis_Right, rightAxis_Down, rightAxis_Up);

            // Alt Axis
            altAxis_Up = CreatePlayerAction("Cycle Element Up");
            altAxis_Down = CreatePlayerAction("Cycle Element Down");
            altAxis_Left = CreatePlayerAction("Cycle Ranged Weapon");
            altAxis_Right = CreatePlayerAction("Cycle Melee Weapon");

            switchElement = CreateOneAxisPlayerAction(altAxis_Down, altAxis_Up);
            switchWeapon = CreateOneAxisPlayerAction(altAxis_Left, altAxis_Right);

            // Main Game
            dash = CreatePlayerAction("Dash");
            attack = CreatePlayerAction("Attack");
            interact = CreatePlayerAction("Interact");
            aim = CreatePlayerAction("Aim");
        }

        public static Controls ControllerBindings()
        {
            var controls = new Controls();
            
            // UI Input Bindings
            controls.confirm.AddDefaultBinding(InputControlType.Action1);

            controls.cancel.AddDefaultBinding(InputControlType.Action2);
            // This is in controller bindings anyways just for debugging and shit
            controls.pause.AddDefaultBinding(Key.Escape);
            controls.pause.AddDefaultBinding(InputControlType.Command);
            
            // Left Axis Input Bindings
            controls.leftAxis_Up.AddDefaultBinding(InputControlType.LeftStickUp);

            controls.leftAxis_Down.AddDefaultBinding(InputControlType.LeftStickDown);
            
            controls.leftAxis_Left.AddDefaultBinding(InputControlType.LeftStickLeft);
            
            controls.leftAxis_Right.AddDefaultBinding(InputControlType.LeftStickRight);

            // Right Axis Input Bindings
            controls.rightAxis_Left.AddDefaultBinding(InputControlType.RightStickLeft);
            
            controls.rightAxis_Right.AddDefaultBinding(InputControlType.RightStickRight);
            
            controls.rightAxis_Down.AddDefaultBinding(InputControlType.RightStickDown);
            
            controls.rightAxis_Up.AddDefaultBinding(InputControlType.RightStickUp);

            // Alt Axis Input Bindings
            controls.altAxis_Up.AddDefaultBinding(InputControlType.DPadUp);

            controls.altAxis_Down.AddDefaultBinding(InputControlType.DPadDown);

            controls.altAxis_Left.AddDefaultBinding(InputControlType.DPadLeft);

            controls.altAxis_Right.AddDefaultBinding(InputControlType.DPadRight);

            // Main Game Input Bindings
            controls.dash.AddDefaultBinding(InputControlType.Action1);

            controls.attack.AddDefaultBinding(InputControlType.RightTrigger);

            controls.interact.AddDefaultBinding(InputControlType.Action4);

            controls.aim.AddDefaultBinding(InputControlType.LeftTrigger);

            return controls;
        }
        
        public static Controls KeyboardBindings()
        {
            var controls = new Controls();

            // UI Input Bindings
            controls.confirm.AddDefaultBinding(Key.Return);

            controls.cancel.AddDefaultBinding(Key.Escape);

            controls.pause.AddDefaultBinding(Key.Escape);

            // Left Axis Input Bindings
            controls.leftAxis_Up.AddDefaultBinding(Key.W);

            controls.leftAxis_Down.AddDefaultBinding(Key.S);

            controls.leftAxis_Left.AddDefaultBinding(Key.A);

            controls.leftAxis_Right.AddDefaultBinding(Key.D);

            // Right Axis Input Bindings
            controls.rightAxis_Left.AddDefaultBinding(Mouse.NegativeX);

            controls.rightAxis_Right.AddDefaultBinding(Mouse.PositiveX);

            controls.rightAxis_Down.AddDefaultBinding(Mouse.NegativeY);

            controls.rightAxis_Up.AddDefaultBinding(Mouse.PositiveY);

            // Alt Axis Input Bindings
            controls.altAxis_Up.AddDefaultBinding(Mouse.PositiveScrollWheel);

            controls.altAxis_Down.AddDefaultBinding(Mouse.NegativeScrollWheel);

            controls.altAxis_Right.AddDefaultBinding(Key.Key1);

            controls.altAxis_Left.AddDefaultBinding(Key.Key2);

            // Main Game Input Bindings
            controls.dash.AddDefaultBinding(Key.Space);

            controls.attack.AddDefaultBinding(Mouse.LeftButton);

            controls.interact.AddDefaultBinding(Key.E);
            controls.interact.AddDefaultBinding(Key.F);

            controls.aim.AddDefaultBinding(Mouse.RightButton);

            return controls;
        }
    }
}
