namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class Xbox360MortalKombatFightStickMacProfile : Xbox360DriverMacProfile
	{
		public Xbox360MortalKombatFightStickMacProfile()
		{
			Name = "Xbox 360 Mortal Kombat Fight Stick";
			Meta = "Xbox 360 Mortal Kombat Fight Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf906,
				},
			};
		}
	}
	// @endcond
}


