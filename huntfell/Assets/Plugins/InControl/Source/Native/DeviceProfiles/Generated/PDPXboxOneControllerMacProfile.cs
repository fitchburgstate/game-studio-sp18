namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public PDPXboxOneControllerMacProfile()
		{
			Name = "PDP Xbox One Controller";
			Meta = "PDP Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x013a,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0162,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x561a,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0161,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0163,
				},
			};
		}
	}
	// @endcond
}


