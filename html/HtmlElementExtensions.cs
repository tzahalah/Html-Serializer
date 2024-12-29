using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace html
{
    public  static class HtmlElementExtensions
    {
        public static HashSet<HtmlElement> FindTags(this HtmlElement element, Selector selector)
        {
           return FindMatchesTags(selector ,new HashSet<HtmlElement>() { element });
        }
        public static HashSet<HtmlElement> FindMatchesTags (Selector selector, HashSet<HtmlElement> proccess)
        {
            if (selector == null)
                return proccess;
            if (proccess == null)
                return new HashSet<HtmlElement>();
            HashSet<HtmlElement> res =new HashSet<HtmlElement>();
            foreach (var tag in proccess) 
                foreach (var item2 in tag.Descendants())
                {
                    if(selector.isMatch(item2))
                        res.Add(item2);
                }
          return FindMatchesTags(selector.Child, res);
            
           
        }
    }
}
