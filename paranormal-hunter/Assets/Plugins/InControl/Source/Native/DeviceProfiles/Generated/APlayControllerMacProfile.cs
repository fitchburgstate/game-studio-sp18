namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class APlayControllerMacProfile : Xbox360DriverMacProfile
	{
		public APlayControllerMacProfile()
		{
			Name = "A Play Controller";
			Meta = "A Play Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0xfafb,
				},
			};
		}
	}
	// @endcond
}


