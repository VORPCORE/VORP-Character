using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace vorpcharacter_cl.Utils
{
    public class Language : BaseScript
    {
        public static Dictionary<string, string> Config = new Dictionary<string, string>();
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();

        public Language()
        {
            EventHandlers[$"{API.GetCurrentResourceName()}:SendDefaultLang"] += new Action<ExpandoObject, ExpandoObject>(setDefaultLang);

            TriggerServerEvent($"{API.GetCurrentResourceName()}:getLanguage");
        }

        private void setDefaultLang(ExpandoObject dc, ExpandoObject dl)
        {

            foreach (var c in dc)
            {
                Config[c.Key] = c.Value.ToString();
            }

            foreach (var l in dl)
            {
                Langs[l.Key] = l.Value.ToString();
            }

        }
    }
}
