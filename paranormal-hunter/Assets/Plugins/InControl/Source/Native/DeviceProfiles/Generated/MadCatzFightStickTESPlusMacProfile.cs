namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzFightStickTESPlusMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzFightStickTESPlusMacProfile()
		{
			Name = "Mad Catz Fight Stick TES Plus";
			Meta = "Mad Catz Fight Stick TES Plus on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf042,
				},
			};
		}
	}
	// @endcond
}


