namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class BetopBTPGenericAndroidProfile : UnityInputDeviceProfile
	{
		// This seems to be a generic Xbox-styled controller from BETOP;
		// different model numbers but same JoystickName.
		// Confirmed with customers that this single profile works with:
		//
		// BETOP 卡洛蓝牙六轴版  BTP-2171TN
		// http://www.betop-cn.com/product/kaluo/452.html
		//
		// BETOP 潘多拉无线手柄  BTP-2282
		// http://www.betop-cn.com/m/product/detail.html?id=441
		//
		public BetopBTPGenericAndroidProfile()
		{
			Name = "BETOP BTP Controller";
			Meta = "BETOP BTP Controller on Android";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.Xbox360;

			IncludePlatforms = new[] {
				"Android"
			};

			JoystickNames = new[] {
				"BETOP BFM GAMEPAD"
			};

			LastResortRegex = "betop ";

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "Back",
					Target = InputControlType.Back,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "Home",
					Target = InputControlType.Home,
					Source = Button12
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button9
				},
			};

			AnalogMappings = new[] {
				LeftStickLeftMapping( Analog0 ),
				LeftStickRightMapping( Analog0 ),
				LeftStickUpMapping( Analog1 ),
				LeftStickDownMapping( Analog1 ),

				RightStickLeftMapping( Analog2 ),
				RightStickRightMapping( Analog2 ),
				RightStickUpMapping( Analog3 ),
				RightStickDownMapping( Analog3 ),

				DPadLeftMapping( Analog4 ),
				DPadRightMapping( Analog4 ),
				DPadUpMapping( Analog5 ),
				DPadDownMapping( Analog5 )
			};
		}
	}
	// @endcond
}

