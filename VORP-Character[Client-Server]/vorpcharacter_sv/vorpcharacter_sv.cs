using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcharacter_sv
{
    public class vorpcharacter_sv : BaseScript
    {
        public static Dictionary<string, string> Config = new Dictionary<string, string>();
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();
        public static string resourcePath = $"{API.GetResourcePath(API.GetCurrentResourceName())}";

        public vorpcharacter_sv()
        {
            EventHandlers["vorpcharacter:SaveSkinDB"] += new Action<Player, object, object, string>(SaveSkinDB);
            EventHandlers["vorpcharacter:getPlayerSkin"] += new Action<Player>(getPlayerSkin);
            EventHandlers[$"{API.GetCurrentResourceName()}:getLanguage"] += new Action<Player>(getLanguage);

            LoadConfigAnfLang();

        }

        private void LoadConfigAnfLang()
        {

            if (File.Exists($"{resourcePath}/Config.json"))
            {
                string configstring = File.ReadAllText($"{resourcePath}/Config.json", Encoding.UTF8);
                Config = JsonConvert.DeserializeObject<Dictionary<string, string>>(configstring);
                if (File.Exists($"{resourcePath}/{Config["defaultlang"]}.json"))
                {
                    string langstring = File.ReadAllText($"{resourcePath}/{Config["defaultlang"]}.json", Encoding.UTF8);
                    Langs = JsonConvert.DeserializeObject<Dictionary<string, string>>(langstring);
                    Debug.WriteLine($"{API.GetCurrentResourceName()}: Language {Config["defaultlang"]}.json loaded!");
                }
                else
                {
                    Debug.WriteLine($"{API.GetCurrentResourceName()}: {Config["defaultlang"]}.json Not Found");
                }
            }
            else
            {
                Debug.WriteLine($"{API.GetCurrentResourceName()}: Config.json Not Found");
            }
        }

        private void getLanguage([FromSource]Player source)
        {
            source.TriggerEvent($"{API.GetCurrentResourceName()}:SendDefaultLang", Config, Langs);
        }

        private void getPlayerSkin([FromSource]Player source)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Debug.WriteLine("Character: Usuario no registrado");
                }
                else
                {

                    string s_skin = result[0].skinPlayer;
                    string s_body = result[0].compPlayer;

                    Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_skin);
                    Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_body);


                    source.TriggerEvent("vorp:loadPlayerSkin", sskin, scloth);
                }

            }));
        }

        private void SaveSkinDB([FromSource]Player source, dynamic skin, dynamic components, string name)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            string[] divider = name.Split(' ');

            string firstname = divider[0];
            string lastname = divider[1];
            string skinPlayer = JsonConvert.SerializeObject(skin);
            string componentsPlayer = JsonConvert.SerializeObject(components);

            Exports["ghmattimysql"].execute("INSERT INTO characters (`identifier`, `firstname`, `lastname`, `skinPlayer`, `compPlayer`) VALUES (?, ?, ?, ?, ?)", new object[] { sid, firstname, lastname, skinPlayer, componentsPlayer });



        }
    }
}
