using System;
using System.Globalization;
using System.IO;

namespace Motix.Extensions.MessageSenders
{
    public class MultilingualHtmlMessageTemplateResolver<TMarker> : MultilingualHtmlMessageTemplateResolver, IMessageTemplateResolver<TMarker> { }

    public class MultilingualHtmlMessageTemplateResolver : IMessageTemplateResolver
    {
        protected string Language => CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        public string ResolveTemplate(string location, string templateName)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentNullException(nameof(location));
            }
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName));
            }

            var path = $"{location}/{Language}/{templateName}.html";
            if (!File.Exists(path))
            {
                path = $"{location}/{templateName}.html";
            }

            try
            {
                return File.ReadAllText(path);
            }
            catch
            {
                throw new IOException(string.Format(Resources.TemplateReadingError, templateName, location));
            }
        }
    }
}
