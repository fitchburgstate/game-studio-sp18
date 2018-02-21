namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriPadUltimateMacProfile : Xbox360DriverMacProfile
	{
		public HoriPadUltimateMacProfile()
		{
			Name = "HoriPad Ultimate";
			Meta = "HoriPad Ultimate on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x0090,
				},
			};
		}
	}
	// @endcond
}


