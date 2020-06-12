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
            /*CallBack*/
            EventHandlers["vorpcharacter:getPlayerClothes"] += new Action<int, dynamic>(getPlayerClothes);

            LoadConfigAnfLang();

            EventHandlers["vorpcharacter:CommandCreate"] += new Action<Player, int>(StartCreation);
        }

        private void StartCreation([FromSource] Player source, int target)
        {
            int _source = int.Parse(source.Handle);
            TriggerEvent("vorp:getCharacter", _source, new Action<dynamic>((user) =>
            {
                Debug.WriteLine(user.group);
                if(user.group == "admin")
                {
                    if (target == -1)
                    {
                        source.TriggerEvent("vorpcharacter:createPlayer");
                    }
                    else
                    {
                        PlayerList pl = new PlayerList();
                        Player p = pl[target];
                        p.TriggerEvent("vorpcharacter:createPlayer");
                    }
                }
                else
                {
                    source.TriggerEvent("vorp:Tip", "You don't have enough permissions", 5000);
                }
            }));
        }

        private void getPlayerClothes(int source, dynamic cb)
        {

            PlayerList pl = new PlayerList();
            Player p = pl[source];

            if (p == null)
            {
                cb.Invoke("Not Conected - Can't Find SteamID");
                return;
            }

            string sid = "steam:" + p.Identifiers["steam"];

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Debug.WriteLine("ERROR: User not found");
                }
                else
                {
                    Dictionary<string, string> comp = new Dictionary<string, string>();

                    /* Seteamos todos los paraametros que nos puedan servir para comprobaciones*/
                    comp.Add("cloths", result[0].compPlayer);
                    comp.Add("skins", result[0].skinPlayer);

                    cb.Invoke(comp); //Enviamos los datos de vuelta
                }

            }));
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


                    source.TriggerEvent("vorpcharacter:loadPlayerSkin", sskin, scloth);
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

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Exports["ghmattimysql"].execute("INSERT INTO characters (`identifier`, `firstname`, `lastname`, `skinPlayer`, `compPlayer`) VALUES (?, ?, ?, ?, ?)", new object[] { sid, firstname, lastname, skinPlayer, componentsPlayer });
                    SetStartMoney(int.Parse(source.Handle));
                }
                else
                {
                    Exports["ghmattimysql"].execute("UPDATE characters SET firstname=?, lastname=?, skinPlayer=?, compPlayer=? WHERE identifier=?", new object[] { firstname, lastname, skinPlayer, componentsPlayer, sid });
                }
            }));
        }

        private async Task SetStartMoney(int source)
        {
            await Delay(5000);
            TriggerEvent("vorp:addMoney", source, 0, int.Parse(Config["StartingMoney"].ToString()));
            await Delay(100);
            TriggerEvent("vorp:addMoney", source, 1, int.Parse(Config["StartingGold"].ToString()));
            await Delay(100);
            TriggerEvent("vorp:addMoney", source, 2, int.Parse(Config["StartingRol"].ToString()));
            await Delay(500);
            TriggerEvent("vorp:firstSpawn", source);
        }
    }
}
