namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public XboxOneControllerMacProfile()
		{
			Name = "Xbox One Controller";
			Meta = "Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x561a,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x551a,
				},
			};
		}
	}
	// @endcond
}


