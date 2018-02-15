using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace Hunter.Character
{
    public class Controls : PlayerActionSet
    {
        public PlayerAction confirm;
        public PlayerAction cancel;
        public PlayerAction pause;

        public PlayerAction left;
        public PlayerAction right;
        public PlayerAction up;
        public PlayerAction down;

        public PlayerAction lookLeft;
        public PlayerAction lookRight;
        public PlayerAction lookDown;
        public PlayerAction lookUp;

        public PlayerTwoAxisAction move;
        public PlayerTwoAxisAction look;

        public Controls()
        {
            confirm = CreatePlayerAction("Confirm");
            cancel = CreatePlayerAction("Cancel");
            pause = CreatePlayerAction("Pause");

            left = CreatePlayerAction("Move Left");
            right = CreatePlayerAction("Move Right");
            up = CreatePlayerAction("Move Up");
            down = CreatePlayerAction("Move Down");

            lookLeft = CreatePlayerAction("Look Left");
            lookRight = CreatePlayerAction("Look Right");
            lookDown = CreatePlayerAction("Look Down");
            lookUp = CreatePlayerAction("Look Up");

            move = CreateTwoAxisPlayerAction(left, right, down, up);
            look = CreateTwoAxisPlayerAction(lookLeft, lookRight, lookDown, lookUp);
        }

        public static Controls ControllerBindings()
        {
            var controls = new Controls();
            
            controls.confirm.AddDefaultBinding(InputControlType.Action1);
            
            controls.cancel.AddDefaultBinding(InputControlType.Action2);

            controls.pause.AddDefaultBinding(Key.Escape);
            controls.pause.AddDefaultBinding(InputControlType.Command);
            
            controls.up.AddDefaultBinding(InputControlType.LeftStickUp);
            controls.up.AddDefaultBinding(InputControlType.DPadUp);
            
            controls.down.AddDefaultBinding(InputControlType.LeftStickDown);
            controls.down.AddDefaultBinding(InputControlType.DPadDown);
            
            controls.left.AddDefaultBinding(InputControlType.LeftStickLeft);
            controls.left.AddDefaultBinding(InputControlType.DPadLeft);
            
            controls.right.AddDefaultBinding(InputControlType.LeftStickRight);
            controls.right.AddDefaultBinding(InputControlType.DPadRight);
            
            controls.lookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
            
            controls.lookRight.AddDefaultBinding(InputControlType.RightStickRight);
            
            controls.lookDown.AddDefaultBinding(InputControlType.RightStickDown);
            
            controls.lookUp.AddDefaultBinding(InputControlType.RightStickUp);

            return controls;
        }
        
        public static Controls KeyboardBindings()
        {
            var controls = new Controls();

            controls.confirm.AddDefaultBinding(Key.Return);

            controls.cancel.AddDefaultBinding(Key.Delete);

            controls.pause.AddDefaultBinding(Key.Escape);

            controls.up.AddDefaultBinding(Key.W);

            controls.down.AddDefaultBinding(Key.S);

            controls.left.AddDefaultBinding(Key.A);

            controls.right.AddDefaultBinding(Key.D);

            controls.lookLeft.AddDefaultBinding(Mouse.NegativeX);

            controls.lookRight.AddDefaultBinding(Mouse.PositiveX);

            controls.lookDown.AddDefaultBinding(Mouse.NegativeY);

            controls.lookUp.AddDefaultBinding(Mouse.PositiveY);

            return controls;
        }
    }
}
