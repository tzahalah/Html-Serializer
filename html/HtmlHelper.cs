using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace html
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper  _instance = new HtmlHelper();
        string a;
        public List<string> HtmlTags { get; set; }
        public List<string> HtmlVoidTags { get; set; }
        private HtmlHelper() 
        {
            var tags = File.ReadAllText("Seed/HtmlTags.json");
            var voidTags = File.ReadAllText("Seed/HtmlVoidTags.json");
            

            HtmlTags = JsonConvert.DeserializeObject<string[]>(tags).ToList();
            HtmlVoidTags = JsonConvert.DeserializeObject<string[]>(voidTags).ToList();

        }

        public static HtmlHelper Instance => _instance;
    }
}
