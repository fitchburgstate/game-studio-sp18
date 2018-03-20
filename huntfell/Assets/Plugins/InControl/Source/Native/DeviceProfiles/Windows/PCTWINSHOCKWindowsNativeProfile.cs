namespace InControl.NativeProfile
{
	// @cond nodoc
	[AutoDiscover]
	public class PCTWINSHOCKWindowsNativeProfile : NativeInputDeviceProfile
	{
		public PCTWINSHOCKWindowsNativeProfile()
		{
			Name = "PC TWIN SHOCK";
			Meta = "PC TWIN SHOCK on Windows";
			// Link = "https://www.amazon.com/Playstation-2-Twin-Shock-Pad-Controller/dp/B000DZEUTY";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.PlayStation2;

			IncludePlatforms = new[] {
				"Windows"
			};

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x79,
					ProductID = 0x6,
				}
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "4",
					Target = InputControlType.Action3,
					Source = Button( 3 ),
				},
				new InputControlMapping {
					Handle = "3",
					Target = InputControlType.Action1,
					Source = Button( 2 ),
				},
				new InputControlMapping {
					Handle = "2",
					Target = InputControlType.Action2,
					Source = Button( 1 ),
				},
				new InputControlMapping {
					Handle = "1",
					Target = InputControlType.Action4,
					Source = Button( 0 ),
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button( 6 ),
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button( 7 ),
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button( 4 ),
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button( 5 ),
				},
				new InputControlMapping {
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button( 8 ),
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button( 9 ),
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button( 10 ),
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button( 11 ),
				}
			};

			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "Right Stick Up",
					Target = InputControlType.RightStickUp,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Down",
					Target = InputControlType.RightStickDown,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Left",
					Target = InputControlType.RightStickLeft,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Right",
					Target = InputControlType.RightStickRight,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Up",
					Target = InputControlType.LeftStickUp,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Down",
					Target = InputControlType.LeftStickDown,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Left",
					Target = InputControlType.LeftStickLeft,
					Source = Analog( 4 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Right",
					Target = InputControlType.LeftStickRight,
					Source = Analog( 4 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog( 5 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog( 5 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog( 6 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog( 6 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
			};
		}
	}
	// @endcond
}
