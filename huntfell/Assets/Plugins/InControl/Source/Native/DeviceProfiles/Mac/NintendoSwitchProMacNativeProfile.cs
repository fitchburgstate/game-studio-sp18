namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class NintendoSwitchProMacNativeProfile : NativeInputDeviceProfile
	{
		public NintendoSwitchProMacNativeProfile()
		{
			Name = "Nintendo Switch Pro Controller";
			Meta = "Nintendo Switch Pro Controller on Mac";
			// Link = "https://www.amazon.com/dp/B01NAWKYZ0";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.NintendoSwitch;

			UpperDeadZone = 0.7f;

			IncludePlatforms = new[] {
				"OS X"
			};

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x57e,
					ProductID = 0x2009,
					// VersionNumber = 0x1,
				},
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action1,
					Source = Button( 0 ),
				},
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action2,
					Source = Button( 1 ),
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action3,
					Source = Button( 2 ),
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action4,
					Source = Button( 3 ),
				},
				new InputControlMapping {
					Handle = "L",
					Target = InputControlType.LeftBumper,
					Source = Button( 4 ),
				},
				new InputControlMapping {
					Handle = "R",
					Target = InputControlType.RightBumper,
					Source = Button( 5 ),
				},
				new InputControlMapping {
					Handle = "ZL",
					Target = InputControlType.LeftTrigger,
					Source = Button( 6 ),
				},
				new InputControlMapping {
					Handle = "ZR",
					Target = InputControlType.RightTrigger,
					Source = Button( 7 ),
				},
				new InputControlMapping {
					Handle = "Minus",
					Target = InputControlType.Minus,
					Source = Button( 8 ),
				},
				new InputControlMapping {
					Handle = "Plus",
					Target = InputControlType.Plus,
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
				},
				new InputControlMapping {
					Handle = "Home",
					Target = InputControlType.Home,
					Source = Button( 12 ),
				},
				new InputControlMapping {
					Handle = "Capture",
					Target = InputControlType.Capture,
					Source = Button( 13 ),
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button( 16 ),
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button( 17 ),
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button( 18 ),
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button( 19 ),
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
					Handle = "Right Stick Left",
					Target = InputControlType.RightStickLeft,
					Source = Analog( 2 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Right",
					Target = InputControlType.RightStickRight,
					Source = Analog( 2 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Up",
					Target = InputControlType.RightStickUp,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Down",
					Target = InputControlType.RightStickDown,
					Source = Analog( 3 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
			};
		}
	}
	// @endcond
}

