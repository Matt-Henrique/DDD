using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Net.Mime;
using System.Web;

namespace Invoisys.Infrastructure.CrossCutting.Identity.Configuration
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Account"]))
                return Task.FromResult(0);
            var msg = new MailMessage()
            {
                From = new MailAddress(ConfigurationManager.AppSettings["Account"], "Invoisys Sistemas")
            };
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            var text = HttpUtility.HtmlDecode(message.Body);
            if (text != null) msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));
            var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["MailServer"], int.Parse(ConfigurationManager.AppSettings["ServerPort"]));
            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailLogin"], ConfigurationManager.AppSettings["EmailPassword"]);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
            return Task.FromResult(0);
        }
    }
}