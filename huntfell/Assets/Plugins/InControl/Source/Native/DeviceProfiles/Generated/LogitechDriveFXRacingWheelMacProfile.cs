namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class LogitechDriveFXRacingWheelMacProfile : Xbox360DriverMacProfile
	{
		public LogitechDriveFXRacingWheelMacProfile()
		{
			Name = "Logitech DriveFX Racing Wheel";
			Meta = "Logitech DriveFX Racing Wheel on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x046d,
					ProductID = 0xcaa3,
				},
			};
		}
	}
	// @endcond
}


