/**
 * @file RegexUtil.cs
 * @author Zhan WANG <wangzhan.1985@gmail.com>
 * @Date 2013
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace YNote.Common
{
    public class RegexUtil
    {
        public static string RemoveTags(string htmlText)
        {
            // TODO:
            //Regex regex = new Regex(@"<(/|!)?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.IgnoreCase);
            Regex regex = new Regex(@"<(/|!)?.*?/?>", RegexOptions.IgnoreCase);
            string result = regex.Replace(htmlText, "");
            return result;
        }

        public static List<string> GetThumbnailsUrl(string htmlText)
        {
            return GetTagAttr("img", "src", htmlText);
        }

        public static List<string> GetTagAttr(string tagName, string attrKey, string htmlText)
        {
            List<string> urls = new List<string>();
            if (string.IsNullOrEmpty(tagName))
            {
                tagName = @"\w*";
            }
            string pattern = @"<(/|!)?" + tagName + @"(.*?)(\s+" + attrKey + 
                            @"\s*=\s*(?<value>("".*?"")|('.*?')))[^>]*/?>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(htmlText);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                string value = groups["value"].Value;
                TrimQuto(ref value);
                urls.Add(value);
            }
            return urls;
        }

        public static string ReplaceThumbnailsUrl(string htmlText, string basePath)
        {
            string result = string.Empty;
            List<string> urls = new List<string>();
            string pattern = @"<(/|!)?" + "img" + @"(.*?)(\s+" + "src" +
                @"\s*=\s*(?<value>("".*?"")|('.*?')))[^>]*/?>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            result = regex.Replace(htmlText, delegate(Match match)
            {
                GroupCollection groups = match.Groups;
                string url = groups["value"].Value;
                TrimQuto(ref url);
                int index = url.LastIndexOf('/');
                ++index;
                string id = url.Substring(index, url.Length - index);
                string path = string.Format("{0}\\{1}", basePath, id);
                return groups[0].Value.Replace(url, path);
            });
            return result;
        }

        private static void TrimQuto(ref string value)
        {
            value = value.Trim('\'');
            value = value.Trim('\"');
        }
    }
}
