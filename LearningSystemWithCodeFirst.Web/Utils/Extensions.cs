using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace LearningSystemWithCodeFirst.Web.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// String extension method for render text with fixed length
        /// </summary>
        /// <param name="newLength">Length of the text parameter that will be return</param>
        /// <param name="suffix">Add suffix if text is more than passed length</param>
        public static string StripText(this string text, int newLength, string suffix)
        {
            // set length of the text
            if (text != null)
            {
                if (text.Length > newLength)
                {
                    text = text.Substring(0, newLength - suffix.Length - 1) + suffix;
                }
            }

            return text;
        }

        /// <summary>
        /// String extension method for render text. If text is null render default symbol, else render the whole text
        /// </summary>
        public static string RenderText(this string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                // trim text
                string trimedText = text.Trim();
                if (!string.IsNullOrEmpty(trimedText))
                {
                    return trimedText;
                }
            }

            return "-";
        }

        /// <summary>
        /// Truncates a string containing HTML to a number of text characters, keeping whole words.
        /// The result contains HTML and any tags left open are closed.</summary>
        /// <param name="maxCharacters">Int MaxCharacters</param>
        public static string TruncateHtml(this string html, int maxCharacters)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            // find the spot to truncate
            // count the text characters and ignore tags
            var textCount = 0;
            var charCount = 0;
            var ignore = false;
            foreach (char c in html)
            {
                charCount++;
                if (c == '<')
                {
                    ignore = true;
                }
                else if (!ignore)
                {
                    textCount++;
                }

                if (c == '>')
                {
                    ignore = false;
                }

                // stop once we hit the limit
                if (textCount >= maxCharacters)
                {
                    break;
                }
            }

            // Truncate the html and keep whole words only
            var trunc = new StringBuilder(html.TruncateWords(charCount));

            // keep track of open tags and close any tags left open
            var tags = new Stack<string>();
            var matches = Regex.Matches(trunc.ToString(),
                @"<((?<tag>[^\s/>]+)|/(?<closeTag>[^\s>]+)).*?(?<selfClose>/)?\s*>",
                RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var tag = match.Groups["tag"].Value;
                    var closeTag = match.Groups["closeTag"].Value;

                    // push to stack if open tag and ignore it if it is self-closing, i.e. <br />
                    if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                    {
                        tags.Push(tag);
                    }

                    // pop from stack if close tag
                    else if (!string.IsNullOrEmpty(closeTag))
                    {
                        // pop the tag to close it.. find the matching opening tag
                        // ignore any unclosed tags
                        while (tags.Pop() != closeTag && tags.Count > 0)
                        { }
                    }
                }
            }

            // pop the rest off the stack to close remainder of tags
            while (tags.Count > 0)
            {
                trunc.Append("</");
                trunc.Append(tags.Pop());
                trunc.Append('>');
            }

            return trunc.ToString();
        }

        /// <summary>
        /// Truncates text to a number of characters
        /// </summary>
        /// <param name="text">Int maxCharacters</param>
        public static string Truncate(this string text, int maxCharacters)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxCharacters);
            }
        }

        /// <summary>
        /// Truncates text and discars any partial words left at the end
        /// </summary>
        /// <param name="maxCharacters">Int maxCharacters</param>
        public static string TruncateWords(this string text, int maxCharacters)
        {
            if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
            {
                return text;
            }

            // truncate the text, then remove the partial word at the end
            return Regex.Replace(text.Truncate(maxCharacters), @"\s+[^\s]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static List<List<T>> DivideListToThree<T>(List<T> sourceList) where T: class
        {
            List<T> list1 = new List<T>();
            List<T> list2 = new List<T>();
            List<T> list3 = new List<T>();

            for (int i = 0; i < sourceList.Count; i++)
            {
                int number = i % 3;

                if (number == 0)
                {
                    list1.Add(sourceList[i]);
                }
                else if (number == 1)
                {
                    list2.Add(sourceList[i]);
                }
                else
                {
                    list3.Add(sourceList[i]);
                }
            }

            List<List<T>> resultList = new List<List<T>>();
            resultList.Add(list1);
            resultList.Add(list2);
            resultList.Add(list3);

            return resultList;
        }
    }
}