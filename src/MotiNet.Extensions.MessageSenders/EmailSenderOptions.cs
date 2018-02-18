namespace MotiNet.Extensions.MessageSenders
{
    public class EmailSenderOptions<TMarker> : EmailSenderOptions { }

    public class EmailSenderOptions
    {
        public class SmtpOptions
        {
            public string Host { get; set; }

            public int Port { get; set; }

            public bool UseSsl { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }
        }

        public string TemplatesLocation { get; set; }

        public SmtpOptions Smtp { get; set; }
    }
}
