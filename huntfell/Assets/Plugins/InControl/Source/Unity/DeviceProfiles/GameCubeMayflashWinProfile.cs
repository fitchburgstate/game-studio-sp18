namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class GameCubeMayflashWinProfile : UnityInputDeviceProfile
	{
		public GameCubeMayflashWinProfile()
		{
			Name = "GameCube Controller";
			Meta = "GameCube Controller on Windows via MAYFLASH adapter";
			// Link = "https://www.amazon.com/gp/offer-listing/B0000E5U6I/";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.NintendoGameCube;

			UpperDeadZone = 0.7f;

			IncludePlatforms = new[] {
				"Windows"
			};

			JoystickNames = new[] {
				"MAYFLASH GameCube Controller Adapter"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button( 0 ),
				},
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button( 1 ),
				},
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button( 2 ),
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button( 3 ),
				},
				new InputControlMapping {
					Handle = "Left Trigger Button",
					Target = InputControlType.Action5,
					Source = Button( 4 ),
				},
				new InputControlMapping {
					Handle = "Right Trigger Button",
					Target = InputControlType.Action6,
					Source = Button( 5 ),
				},
				new InputControlMapping {
					Handle = "Z",
					Target = InputControlType.RightBumper,
					Source = Button( 7 ),
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button( 9 ),
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button( 12 ),
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button( 13 ),
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button( 14 ),
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button( 15 ),
				},
			};

			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "Left Stick Left",
					Target = InputControlType.LeftStickLeft,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Right",
					Target = InputControlType.LeftStickRight,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Up",
					Target = InputControlType.LeftStickUp,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Down",
					Target = InputControlType.LeftStickDown,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},

				new InputControlMapping {
					Handle = "C Up",
					Target = InputControlType.RightStickUp,
					Source = Analog( 2 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "C Down",
					Target = InputControlType.RightStickDown,
					Source = Analog( 2 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "C Left",
					Target = InputControlType.RightStickLeft,
					Source = Analog( 5 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "C Right",
					Target = InputControlType.RightStickRight,
					Source = Analog( 5 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},

				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog( 3 ),
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog( 4 ),
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
				},


				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog( 6 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog( 6 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog( 7 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog( 7 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
			};
		}
	}
	// @endcond
}

