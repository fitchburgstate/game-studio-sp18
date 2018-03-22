namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class Xbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		public Xbox360ControllerMacProfile()
		{
			Name = "Xbox 360 Controller";
			Meta = "Xbox 360 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0xf501,
				},
			};
		}
	}
	// @endcond
}


