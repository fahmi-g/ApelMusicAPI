namespace ApelMusicAPI.Email
{
    public class EmailModel
    {
        //Receiver
        public List<string> To { get; }
        public List<string>? BCC { get; }
        public List<string>? CC { get; }

        //COntent
        public string Subject { get; }
        public string? Body { get; }

        public EmailModel( List <String> to, string subject, string? body = null, List<String>? bcc = null, List<String>? cc = null)
        {
            To = to;
            BCC = bcc;
            CC = cc;

            Subject = subject;
            Body = body;
        }
    }
}
