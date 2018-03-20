namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzFightStickTE2MacProfile : Xbox360DriverMacProfile
	{
		public MadCatzFightStickTE2MacProfile()
		{
			Name = "Mad Catz Fight Stick TE2";
			Meta = "Mad Catz Fight Stick TE2 on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf080,
				},
			};
		}
	}
	// @endcond
}


