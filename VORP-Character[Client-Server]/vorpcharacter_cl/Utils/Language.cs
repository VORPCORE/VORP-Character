using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace vorpcharacter_cl.Utils
{
    public class Language : BaseScript
    {

        public static Dictionary<string, string> DefaultLang = new Dictionary<string, string>(); 

        public Language()
        {
            //XmlDocument LanguageDoc = new XmlDocument();
            //LanguageDoc.Load(@"Languages.xml");
            //string defaultLang = LanguageDoc.SelectSingleNode("default").InnerText;

            //XDocument.Load(@"Languages.xml").Descendants(defaultLang).ToDictionary(elementSelector => elementSelector.Name, elementSelector => elementSelector.Value);

            //Debug.WriteLine($"Loaded language {defaultLang}");

            //Debug.WriteLine($"Lang Nodes {DefaultLang.ToString()}");

        }





    }
}
