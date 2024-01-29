using MimeKit;
using MailKit.Net.Smtp;
using RazorEngineCore;

namespace ApelMusicAPI.Email
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _fromDisplayName;
        private readonly string _from;
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly string _port;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _fromDisplayName = _configuration.GetSection("EmailSettings:FromDisplayName").Value;
            _from = _configuration.GetSection("EmailSettings:From").Value;
            _host = _configuration.GetSection("EmailSettings:Host").Value;
            _username = _configuration.GetSection("EmailSettings:UserName").Value;
            _password = _configuration.GetSection("EmailSettings:Password").Value;
            _port = _configuration.GetSection("EmailSettings:Port").Value;
        }

        public async Task<bool> SendEmailAsync(EmailModel emailModel, CancellationToken cancellationToken = default)
        {
            try
            {
                // Initialize a new instance of the MimeKit.MimeMessage class
                var mail = new MimeMessage();

                #region Sender / Receiver
                // Sender
                mail.From.Add(new MailboxAddress(_fromDisplayName, _from));
                mail.Sender = new MailboxAddress(_fromDisplayName, _from);

                // Receiver
                foreach (string mailAddress in emailModel.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                // Check if a BCC was supplied in the request
                if (emailModel.BCC != null)
                {
                    // Get only addresses where value is not null or with whitespace. x = value of address
                    foreach (string mailAddress in emailModel.BCC.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }

                // Check if a CC address was supplied in the request
                if (emailModel.CC != null)
                {
                    foreach (string mailAddress in emailModel.CC.Where(x => !string.IsNullOrWhiteSpace(x)))
                        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
                }
                #endregion

                #region Content

                // Add Content to Mime Message
                var body = new BodyBuilder();
                mail.Subject = emailModel.Subject;
                body.HtmlBody = emailModel.Body;
                mail.Body = body.ToMessageBody();

                #endregion

                #region Send Mail

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(_host, Convert.ToInt32(_port), true, cancellationToken);

                await smtp.AuthenticateAsync(_username, _password, cancellationToken);
                await smtp.SendAsync(mail, cancellationToken);
                await smtp.DisconnectAsync(true, cancellationToken);

                #endregion

                return true;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmailTemplate<T>(T emailTemplateModel)
        {
            string mailTemplate = MailConstant.EmailTemplate;

            IRazorEngine razorEngine = new RazorEngine();
            IRazorEngineCompiledTemplate modifiedMailTemplate = razorEngine.Compile(mailTemplate);

            return modifiedMailTemplate.Run(emailTemplateModel);
        }

    }
}
