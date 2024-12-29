using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace html
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; } = " ";
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Child { get; set; }
        public Selector Parent { get; set; }


        public Selector() { }
       
        public static Selector ConvertToSelector(string query)
        {
            Selector root = new Selector();
            Selector currentSelector = root;
            Selector newSelector = new Selector();
            List<string>querys= new List<string>(query.Split(" ").ToList());
            foreach (string s in querys)
            {
                var name = new Regex("([^\\s#.]*)").Matches(s);
                var id = new Regex("#([^\\s.]*)").Matches(s);
                var clas = new Regex("\\.([^\\.#]*)").Matches(s);
                if(name != null && name.Count > 0 && name[0].Groups.Count > 1)
                    newSelector.TagName = name[0].Groups[1].Value;
                if(id != null && id.Count > 0 && id[0].Groups.Count > 1)
                     newSelector.Id = id[0].Groups[1].Value;
                if(clas!=null && clas.Count > 0 && clas[0].Groups.Count > 1)
                    newSelector.Classes =  clas[0].Groups[1].Value.Split(" ").ToList() ;

                newSelector.Parent = currentSelector;
                currentSelector.Child = newSelector;
                currentSelector = newSelector;
                newSelector = new Selector();


            }
            
            return root.Child;//convert query string to selector
        }

        public bool isMatch(HtmlElement tag)
        {
            bool id=true, name=true, clas=true;
            if (this.Id != " ")
                id = this.Id.Equals(tag.Id);
            if ( this.TagName!=null)
                name = this.TagName.Equals(tag.Name);
            if (this.Classes != null && tag.Classes != null)
                    clas= this.Classes.All(c => tag.Classes.Contains(c));
            return id && name &&clas;

        }


    }


   }
