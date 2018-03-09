namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPBattlefieldXBoxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public PDPBattlefieldXBoxOneControllerMacProfile()
		{
			Name = "PDP Battlefield XBox One Controller";
			Meta = "PDP Battlefield XBox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0164,
				},
			};
		}
	}
	// @endcond
}


