namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzSoulCaliberFightStickMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzSoulCaliberFightStickMacProfile()
		{
			Name = "Mad Catz Soul Caliber Fight Stick";
			Meta = "Mad Catz Soul Caliber Fight Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf03f,
				},
			};
		}
	}
	// @endcond
}


