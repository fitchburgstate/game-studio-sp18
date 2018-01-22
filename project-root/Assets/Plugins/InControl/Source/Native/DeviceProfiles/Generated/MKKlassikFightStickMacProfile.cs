namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MKKlassikFightStickMacProfile : Xbox360DriverMacProfile
	{
		public MKKlassikFightStickMacProfile()
		{
			Name = "MK Klassik Fight Stick";
			Meta = "MK Klassik Fight Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x12ab,
					ProductID = 0x0303,
				},
			};
		}
	}
	// @endcond
}


