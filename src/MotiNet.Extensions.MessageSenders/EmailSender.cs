using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.RegularExpressions;
using MimeKit.Utils;

namespace MotiNet.MessageSenders
{
    public class EmailSender<TMarker> : EmailSender, IEmailSender<TMarker>
    {
        public EmailSender(
            IMessageTemplateResolver<TMarker> messageTemplateResolver,
            IOptions<EmailSenderOptions<TMarker>> emailSenderOptions)
            : base(messageTemplateResolver, emailSenderOptions)
        { }
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(
            IMessageTemplateResolver messageTemplateResolver,
            IOptions<EmailSenderOptions> emailSenderOptions)
        {
            if (emailSenderOptions == null)
            {
                throw new ArgumentNullException(nameof(emailSenderOptions));
            }

            TemplateResolver = messageTemplateResolver ?? throw new ArgumentNullException(nameof(messageTemplateResolver));
            TemplatesLocation = emailSenderOptions.Value.TemplatesLocation;
            Smtp = emailSenderOptions.Value.Smtp;
        }

        protected IMessageTemplateResolver TemplateResolver { get; }

        protected string TemplatesLocation { get; }

        protected EmailSenderOptions.SmtpOptions Smtp { get; }

        public Task SendEmailAsync(
            string senderName, string senderEmail, string receiverName, string receiverEmail,
            string subject, string templateName, IDictionary<string, string> replacements)
        {
            var template = TemplateResolver.ResolveTemplate(TemplatesLocation, templateName);
            var body = template;
            foreach (var key in replacements.Keys)
            {
                body = body.Replace($"{{{key}}}", replacements[key]);
            }

            return SendEmailAsync(senderName, senderEmail, receiverName, receiverEmail, subject, body);
        }

        public async Task SendEmailAsync(
            string senderName, string senderEmail, string receiverName, string receiverEmail,
            string subject, string body)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            mailMessage.To.Add(new MailboxAddress(receiverName, receiverEmail));
            mailMessage.Subject = subject;

            List<(string Alpha, string Beta)> l;
            (string Alpha, string Beta) namedLetters = ("a", "b");
            namedLetters.Alpha = "x";

            var inlineReplacements = new List<(string OldValue, string NewValue)>();
            var builder = new BodyBuilder();
            Regex regex = new Regex(@"([""'])cid:(?:(?=(\\?))\2.)*?\1");
            var matches = regex.Matches(body);
            foreach (Match match in matches)
            {
                var path = match.Value.Substring(5, match.Value.Length - 6);
                var inline = builder.LinkedResources.Add(path);
                inline.ContentId = MimeUtils.GenerateMessageId();
                inlineReplacements.Add((match.Value, $"\"cid:{inline.ContentId}\""));
            }

            foreach (var replacement in inlineReplacements)
            {
                body = body.Replace(replacement.OldValue, replacement.NewValue);
            }

            builder.HtmlBody = body;

            mailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(Smtp.Host, Smtp.Port, Smtp.UseSsl);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(Smtp.Username, Smtp.Password);

                await client.SendAsync(mailMessage);
                client.Disconnect(true);
            }
        }
    }
}
