using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace html
{
    public class HtmlSerializer
    {
       public static async Task<int> FindElementsAsync(string link,string query)
        {
            var html = await Load(link);
            var cleanhtml = new Regex(@"[\t\n\r\f\v]").Replace(html, "");
            var rawTags = new Regex("(<.*?>)").Split(cleanhtml).Where(s => s.Trim().Length > 0).ToList();
            var tree = BuildHtmlTree(rawTags.Skip(1).ToList());
            Selector s = Selector.ConvertToSelector(query);
            HashSet<HtmlElement> matchTags = tree.FindTags(s);
            return matchTags.Count();

        }


        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

       public static HtmlElement BuildHtmlTree(List<string> rawTags)
        {
            HtmlHelper help = HtmlHelper.Instance;
            var root = new HtmlElement { Name = "html" };
            var currentTag = root;
            string tagName;
            foreach (var rawTag in rawTags)
            {
                if (rawTag.StartsWith("</"))
                {
                    if (currentTag != null) currentTag = currentTag.Parent;
                }
                else if (rawTag.StartsWith("<"))
                {
                    HtmlElement newTag;
                    string[] parts = rawTag.Split(" ", 2);
                    if (parts.Length == 2)
                    {
                        tagName = parts[0].Substring(1).Trim();
                        if (isExist(tagName))
                        {
                            newTag = new HtmlElement(tagName, parts[1] ?? " ");
                            currentTag.Children.Add(newTag);
                            newTag.Parent = currentTag;
                            currentTag = newTag;
                        }
                    }
                    else
                    {
                        if (rawTag.EndsWith("/>"))
                        {
                            tagName = rawTag.Substring(1, rawTag.Length - 3);
                            if (isExist(tagName))
                            {
                                newTag = new HtmlElement() { Name = tagName };
                                currentTag.Children.Add(newTag);
                                newTag.Parent = currentTag;
                            }
                        }
                        else
                        {
                            tagName = rawTag.Substring(1, rawTag.Length - 2);
                            if (isExist(tagName))
                            {
                                newTag = new HtmlElement() { Name = tagName };
                                currentTag.Children.Add(newTag);
                                newTag.Parent = currentTag;
                                currentTag = newTag;
                            }
                        }
                    }

                }
                else
                    if (currentTag != null)
                    currentTag.InnerHtml = rawTag;
            }
            return root;
        }

       public static bool isExist(string tag)
        {
            return (HtmlHelper.Instance.HtmlVoidTags.Contains(tag) || HtmlHelper.Instance.HtmlTags.Contains(tag));
        }


    }

}
