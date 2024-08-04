using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using MVCProject.PL.Settings;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MVCProject.PL.Services.SmsSenderByTwilio
{
	public class SmsSender : ISmsSender
	{
		private TwilioSettings _Options;

		public SmsSender(IOptions<TwilioSettings> options )
		{ 
			_Options =options.Value;
		}

		public MessageResource SendSms(string PhoneNumber, string bady)
		{
			TwilioClient.Init(_Options.AccountSid, _Options.AuthToken);

			var result =MessageResource.Create(
				body: bady,
				from: new Twilio.Types.PhoneNumber(_Options.PhoneNumber),
				to: PhoneNumber
				);

			return result;
		}
	}
}
