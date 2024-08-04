using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace MVCProject.PL.Services.SmsSenderByTwilio
{
	public interface ISmsSender
	{
		MessageResource SendSms(string PhoneNumber, string bady);
	}
}
