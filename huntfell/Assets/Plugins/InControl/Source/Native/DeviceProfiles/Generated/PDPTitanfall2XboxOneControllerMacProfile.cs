namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPTitanfall2XboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public PDPTitanfall2XboxOneControllerMacProfile()
		{
			Name = "PDP Titanfall 2 Xbox One Controller";
			Meta = "PDP Titanfall 2 Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0165,
				},
			};
		}
	}
	// @endcond
}


