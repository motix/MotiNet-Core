using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MotiNet
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string source)
        {
            if (source == null)
            {
                return null;
            }

            if (source.Length == 1)
            {
                return source.ToUpper();
            }

            var words = source.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
                              .Select(x => $"{x.Substring(0, 1).ToUpper()}{x.Substring(1)}")
                              .ToArray();

            return string.Join(string.Empty, words);
        }

        public static string ToCamelCase(this string source)
        {
            if (source == null)
            {
                return null;
            }

            if (source.Length == 1)
            {
                return source.ToLower();
            }

            var words = source.Split(new char[0], StringSplitOptions.RemoveEmptyEntries)
                              .Select(x => $"{x.Substring(0, 1).ToUpper()}{x.Substring(1)}")
                              .ToArray();

            if (words.Length > 0)
            {
                words[0] = $"{words[0].Substring(0, 1).ToLower()}{words[0].Substring(1)}";
            }

            return string.Join(string.Empty, words);
        }

        public static string WordWiseSubstring(this string source, int maxLength)
        {
            if (source == null)
            {
                return null;
            }

            var result = source = source.Trim();
            if (source.Length > maxLength)
            {
                result = result.Substring(0, maxLength).Trim();
                if (source[result.Length] != ' ' && result.Contains(" "))
                {
                    result = result.Substring(0, result.LastIndexOf(' ')).Trim();
                }
                result = $"{result}...";
            }

            return result;
        }

        public static string RemoveHtmlTags(this string source)
        {
            if (source == null)
            {
                return null;
            }

            // Faster than using regular expression
            source = source.Replace("&nbsp;", " ")
                           .Replace("<br>", "\n")
                           .Replace("<br />", "\n")
                           .Replace("<p", "\n<p")
                           .Replace("<div", "\n<div");
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }

            return new string(array, 0, arrayIndex);
        }

        public static string ToAscii(this string source)
        {
            if (source == null)
            {
                return null;
            }

            string unicode = "áàảãạăắằẳẵặâấầẩẫậéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠĂẮẰẲẴẶÂẤẦẨẪẬÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴđĐ",
                     ascii = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYdD";

            for (var i = 0; i < unicode.Length; i++)
            {
                source = source.Replace(unicode[i], ascii[i]);
            }

            return source;
        }

        public static string ToUrlFriendly(this string source)
        {
            if (source == null)
            {
                return null;
            }

            source = ToAscii(source).Trim().ToLower();
            source = source.Replace(' ', '-');
            source = source.Replace("&nbsp;", "-");
            source = new Regex("[^0-9a-z-]").Replace(source, string.Empty);

            while (source.IndexOf("--") > -1)
            {
                source = source.Replace("--", "-");
            }

            return source;
        }
    }
}
