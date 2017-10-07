using System;
using System.Collections.Generic;
using System.Linq;

namespace MotiNet
{
    public class TagProcessor : ITagProcessor
    {
        public string NormalizeTag(string tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag));
            }

            while (tag.Contains("--"))
            {
                tag = tag.Replace("--", "-");
            }

            while (tag.Contains("  "))
            {
                tag = tag.Replace("  ", " ");
            }

            return tag.Replace("-", "--").Trim();
        }

        public string NormalizeTags(string tags)
        {
            var tagsAsArray = ProcessTagsForEditting(tags).ToArray();
            for (var i = 0; i < tagsAsArray.Length; i++)
            {
                tagsAsArray[i] = NormalizeTag(tagsAsArray[i]);
            }
            return ProcessTagsForSaving(tagsAsArray.Distinct());
        }

        public string ProcessTagsForSaving(IEnumerable<string> tags)
        {
            return tags == null || tags.Select(x => x).Count() == 0 ? null : $"\n{string.Join("\n", tags.Select(x => x))}\n";
        }

        public IEnumerable<string> ProcessTagsForEditting(string tags)
        {
            return (string.IsNullOrWhiteSpace(tags) ? string.Empty : tags).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string ProcessTagsForDisplaying(string tags)
        {
            return string.Join(", ", ProcessTagsForEditting(tags));
        }

        public string UrlEncodeTag(string tag)
        {
            return tag?.Replace(" ", "-");
        }

        public string UrlDecodeTag(string tag)
        {
            return tag?.Replace("-", " ");
        }

        public string HtmlDecodeTag(string tag)
        {
            return tag?.Replace("_", "-");
        }
    }
}
