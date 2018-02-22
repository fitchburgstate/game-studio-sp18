namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPXbox360ControllerMacProfile()
		{
			Name = "PDP Xbox 360 Controller";
			Meta = "PDP Xbox 360 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0501,
				},
			};
		}
	}
	// @endcond
}


