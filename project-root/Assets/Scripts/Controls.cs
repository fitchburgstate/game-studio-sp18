using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

namespace CharacterScripts
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
        }

        public static Controls DefaultBindings()
        {
            var controls = new Controls();

            controls.confirm.AddDefaultBinding(Key.Return);
            controls.confirm.AddDefaultBinding(InputControlType.Action1);

            controls.cancel.AddDefaultBinding(Key.Delete);
            controls.cancel.AddDefaultBinding(InputControlType.Action2);

            controls.pause.AddDefaultBinding(Key.Escape);
            controls.pause.AddDefaultBinding(InputControlType.Command);

            controls.up.AddDefaultBinding(Key.W);
            controls.up.AddDefaultBinding(InputControlType.LeftStickUp);
            controls.up.AddDefaultBinding(InputControlType.DPadUp);

            controls.down.AddDefaultBinding(Key.S);
            controls.down.AddDefaultBinding(InputControlType.LeftStickDown);
            controls.down.AddDefaultBinding(InputControlType.DPadDown);

            controls.left.AddDefaultBinding(Key.A);
            controls.left.AddDefaultBinding(InputControlType.LeftStickLeft);
            controls.left.AddDefaultBinding(InputControlType.DPadLeft);

            controls.right.AddDefaultBinding(Key.D);
            controls.right.AddDefaultBinding(InputControlType.LeftStickRight);
            controls.right.AddDefaultBinding(InputControlType.DPadRight);

            controls.lookLeft.AddDefaultBinding(Mouse.NegativeX);
            controls.lookLeft.AddDefaultBinding(InputControlType.RightStickLeft);

            controls.lookRight.AddDefaultBinding(Mouse.PositiveX);
            controls.lookRight.AddDefaultBinding(InputControlType.RightStickRight);

            controls.lookDown.AddDefaultBinding(Mouse.NegativeY);
            controls.lookDown.AddDefaultBinding(InputControlType.RightStickDown);

            controls.lookUp.AddDefaultBinding(Mouse.PositiveY);
            controls.lookUp.AddDefaultBinding(InputControlType.RightStickUp);


            return controls;
        }
    }
}

