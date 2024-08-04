using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MVCProject.PL.Settings;
using System.Net;
//using System.Net.Mail;
using System.Threading.Tasks;

namespace MVCProject.PL.Services.EmailSender
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _configuration;
		private MailSettings _options;

		public EmailSender(IConfiguration configuration , IOptions<MailSettings> options)
		{
			_configuration = configuration;
			_options = options.Value;
		}

		#region SendEmail
		//public async Task SendEmailAsync(string from, string recipients, string subject, string bady)
		//{
		//	var SenderEmail = _configuration["EmailSettings:SenderEmail"];
		//	var SenderPassword = _configuration["EmailSettings:SenderPassword"];

		//	var emailMessage = new MailMessage();
		//	emailMessage.From = new MailAddress(from);
		//	emailMessage.To.Add(recipients);
		//	emailMessage.Subject = subject;
		//	emailMessage.Body = $"<html><bady>{bady}</bady></html>";
		//	emailMessage.IsBodyHtml = true;


		//	var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpClientServer"], int.Parse(_configuration["EmailSettings:SmtpClientPort"]))
		//	{
		//		Credentials = new NetworkCredential(SenderEmail, SenderPassword),
		//		EnableSsl = true
		//	};

		//	await smtpClient.SendMailAsync(emailMessage);
		//}
		#endregion


		public void SendMail(string from, string recipients, string subject, string bady)
		{
			var mail = new MimeMessage
			{
				Sender = MailboxAddress.Parse(from),
				Subject = subject
			};
			mail.To.Add(MailboxAddress.Parse(recipients));
			mail.From.Add(new MailboxAddress(_options.SenderName,_options.SenderEmail));

			var builder = new BodyBuilder();
			builder.TextBody = bady;
			mail.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();
			smtp.Connect(_options.SmtpClientServer, _options.SmtpClientPort, SecureSocketOptions.StartTls);
			smtp.Authenticate(_options.SenderEmail, _options.SenderPassword);
			smtp.Send(mail);

			smtp.Disconnect(true);
			
		}
	}
}
