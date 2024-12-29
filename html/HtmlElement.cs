using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace html
{
    public class HtmlElement
    {
        public string Id { get; set; } = " ";
        public string Name { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public HtmlElement() { }
        public HtmlElement(string name, string attributes = " ")
        {
            this.Name = name;
            string pattern = @"(\w+)(?:=['""]?([^'""]*)['""]?)?";

            
            MatchCollection matches = Regex.Matches(attributes, pattern);

            foreach (Match match in matches)
            {
                string key = match.Groups[1].Value; 
                string value = match.Groups[2].Value; 

                if (key == "class")
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        var classes = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        this.Classes.AddRange(classes);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.Attributes[key] = key;
                    }
                    else
                    {
                        this.Attributes[key] = value;
                    }
                }
            }

            if(this.Attributes.Keys.Contains("id"))
              {
                this.Id = this.Attributes["id"];
                this.Attributes.Remove("id");
               }
        }
        

    public string ToString()
        {
            return this.Name;
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            var q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while(q.Count > 0)
            {
                HtmlElement child = q.Dequeue();
                foreach (HtmlElement childElement in child.Children)
                  { 
                      q.Enqueue(childElement);
                      yield return childElement;
                  }

            }

        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement htmlElement = this;
            while (!htmlElement.Parent.Name.Equals("html"))
            {
                yield return htmlElement;
                htmlElement = htmlElement.Parent;
            }

        }

        

       
    }
}
