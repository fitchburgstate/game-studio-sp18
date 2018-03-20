namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzMLGFightStickTEMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzMLGFightStickTEMacProfile()
		{
			Name = "Mad Catz MLG Fight Stick TE";
			Meta = "Mad Catz MLG Fight Stick TE on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf03e,
				},
			};
		}
	}
	// @endcond
}


