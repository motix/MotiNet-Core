using System.Collections.Generic;

namespace MotiNet
{
    public interface ITagProcessor
    {
        string NormalizeTag(string tag);
        string NormalizeTags(string tags);

        string ProcessTagsForSaving(IEnumerable<string> tags);
        IEnumerable<string> ProcessTagsForEditting(string tags);
        string ProcessTagsForDisplaying(string tags);

        string UrlEncodeTag(string tag);
        string UrlDecodeTag(string tag);
        string HtmlDecodeTag(string tag);
    }
}