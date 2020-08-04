using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace vorpcharacter_sv
{
    public class vorpcharacter_sv : BaseScript
    {
        public static string resourcePath = $"{API.GetResourcePath(API.GetCurrentResourceName())}";

        public vorpcharacter_sv()
        {
            EventHandlers["vorpcharacter:SaveSkinDB"] += new Action<Player, object, object, string>(SaveSkinDB);
            EventHandlers["vorpcharacter:getPlayerSkin"] += new Action<Player>(getPlayerSkin);
            /*CallBack*/
            EventHandlers["vorpcharacter:getPlayerComps"] += new Action<int, dynamic>(getPlayerComps);

            EventHandlers["vorpcharacter:setPlayerSkinChange"] += new Action<int, string>(setPlayerSkinChange);
            EventHandlers["vorpcharacter:setPlayerCompChange"] += new Action<int, string>(setPlayerCompChange);

            EventHandlers["vorpcharacter:CommandCreate"] += new Action<Player, int>(StartCreation);
        }

        private void StartCreation([FromSource] Player source, int target)
        {
            int _source = int.Parse(source.Handle);
            TriggerEvent("vorp:getCharacter", _source, new Action<dynamic>((user) =>
            {
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

        private void getPlayerComps(int source, dynamic cb)
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
                    Debug.WriteLine("WARNING: User not found");
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

        private void setPlayerCompChange(int source, string compsValue)
        {

            PlayerList pl = new PlayerList();
            Player p = pl[source];

            if (p == null)
            {
                return;
            }

            string sid = "steam:" + p.Identifiers["steam"];

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Debug.WriteLine("WARNING: User not found");
                }
                else
                {
                    string comp_string = result[0].compPlayer;
                    string s_skin = result[0].skinPlayer;
                    JObject comp = JObject.Parse(comp_string);

                    JObject newcomps = JObject.Parse(compsValue);

                    foreach (var nc in newcomps)
                    {
                        if (comp.ContainsKey(nc.Key))
                        {
                            comp[nc.Key] = ConvertValue(nc.Value.ToString());
                        }
                    }

                    Exports["ghmattimysql"].execute("UPDATE characters SET compPlayer=? WHERE identifier=?", new object[] { comp.ToString(), sid });

                    Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(comp.ToString());
                    Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_skin);

                    p.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);

                }

            }));
        }

        private void setPlayerSkinChange(int source, string compsValue)
        {

            PlayerList pl = new PlayerList();
            Player p = pl[source];

            if (p == null)
            {
                return;
            }

            string sid = "steam:" + p.Identifiers["steam"];

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Debug.WriteLine("WARNING: User not found");
                }
                else
                {
                    string skin_string = result[0].skinPlayer;
                    string s_body = result[0].compPlayer;
                    JObject skin = JObject.Parse(skin_string);

                    JObject newcomps = JObject.Parse(compsValue);

                    foreach (var nc in newcomps)
                    {
                        if (skin.ContainsKey(nc.Key))
                        {
                            skin[nc.Key] = ConvertValue(nc.Value.ToString());
                        }
                    }

                    Exports["ghmattimysql"].execute("UPDATE characters SET skinPlayer=? WHERE identifier=?", new object[] { skin.ToString(), sid });

                    Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skin.ToString());
                    Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_body);

                    p.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);

                }

            }));
        }

        private void getPlayerSkin([FromSource]Player source)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            Exports["ghmattimysql"].execute("SELECT * FROM characters WHERE identifier = ?", new[] { sid }, new Action<dynamic>((result) =>
            {
                if (result.Count == 0)
                {
                    Debug.WriteLine("Character: User not registered -> Starting Character Creation");
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
                    Exports["ghmattimysql"].execute("INSERT INTO characters (`identifier`, `group`, `firstname`, `lastname`, `skinPlayer`, `compPlayer`) VALUES (?, ?, ?, ?, ?, ?)", new object[] { sid, "user", firstname, lastname, skinPlayer, componentsPlayer });
                    SetStartMoney(int.Parse(source.Handle), source);
                    TriggerEvent("vorp:UpdateCharacter", sid, firstname, lastname);
                }
                else
                {
                    Exports["ghmattimysql"].execute("UPDATE characters SET firstname=?, lastname=?, skinPlayer=?, compPlayer=? WHERE identifier=?", new object[] { firstname, lastname, skinPlayer, componentsPlayer, sid });
                }

            }));

        }

        private async Task SetStartMoney(int source, Player player)
        {
            await Delay(5000);
            TriggerEvent("vorp:addMoney", source, 0, int.Parse(LoadConfig.Config["StartingMoney"].ToString()));
            await Delay(100);
            TriggerEvent("vorp:addMoney", source, 1, int.Parse(LoadConfig.Config["StartingGold"].ToString()));
            await Delay(100);
            TriggerEvent("vorp:addMoney", source, 2, int.Parse(LoadConfig.Config["StartingRol"].ToString()));
            await Delay(500);
            TriggerEvent("vorp:firstSpawn", source);
            player.TriggerEvent("vorp:firstSpawn");
        }

        public static uint ConvertValue(string s)
        {
            uint result;

            if (uint.TryParse(s, out result))
            {
                return result;
            }
            else
            {
                int interesante = int.Parse(s);
                result = (uint)interesante;
                return result;
            }
        }
    }
}
