using System.Configuration;
using System.Net.Mail;
using HandlebarsDotNet;

namespace Common
{
    public class Email
    {
        public virtual string To { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual string Template { get; set; }
        public virtual object Data { get; set; }

        public void Send()
        {
            if (string.IsNullOrWhiteSpace(To))
                To = ConfigurationManager.AppSettings["Email.To"];

            var email = new MailMessage();
            email.To.Add(To);
            email.Subject = Subject;

            email.Body = !string.IsNullOrWhiteSpace(Template)
                    ? Handlebars.Compile(Template)(Data)
                    : Body;

            var smtp = new SmtpClient();
            smtp.Send(email);
        }
    };
}