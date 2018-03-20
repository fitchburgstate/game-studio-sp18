namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzNeoFightStickMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzNeoFightStickMacProfile()
		{
			Name = "Mad Catz Neo Fight Stick";
			Meta = "Mad Catz Neo Fight Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf03a,
				},
			};
		}
	}
	// @endcond
}


