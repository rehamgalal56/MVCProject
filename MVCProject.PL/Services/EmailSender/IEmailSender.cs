using System.Threading.Tasks;

namespace MVCProject.PL.Services.EmailSender
{
	public interface IEmailSender
	{
		//Task SendEmailAsync(string from, string recipients, string subject, string bady);
		void SendMail(string from, string recipients, string subject, string bady);
	}
}
