using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Net.Mime;
using System.Web;

namespace Invoisys.Infrastructure.CrossCutting.Identity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var msg = new MailMessage()
            {
                From = new MailAddress(ConfigurationManager.AppSettings["ContaEmail"], "Invoisys Sistemas")
            };
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            var text = HttpUtility.HtmlDecode(message.Body);
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Html));
            var smtpClient = new SmtpClient(ConfigurationManager.AppSettings["ServidorEmail"], int.Parse(ConfigurationManager.AppSettings["PortaEmail"]));
            var credentials = new NetworkCredential(ConfigurationManager.AppSettings["LoginEmail"], ConfigurationManager.AppSettings["SenhaEmail"]);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
            return Task.FromResult(0);
        }
    }
}