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
        public static dynamic CORE;
        public static int MaxCharacters;

        public vorpcharacter_sv()
        {
            //Event for create new character 
            EventHandlers["vorp_CreateNewCharacter"] += new Action<int>(CreateNewCharacter);
            //Event for select characters
            EventHandlers["vorp_GoToSelectionMenu"] += new Action<int>(GoToSelectionMenu);
            //Event for spawn unique character
            EventHandlers["vorp_SpawnUniqueCharacter"] += new Action<int>(SpawnUniqueCharacter);
            //Event for save the new character
            EventHandlers["vorp_SaveNewCharacter"] += new Action<Player, dynamic, dynamic, string>(SaveNewCharacter);
            EventHandlers["vorp_updateexisting"] += new Action<Player, dynamic, dynamic, string,dynamic,dynamic>(SaveNewCharacter2);
            //Event for delete the character
            EventHandlers["vorp_DeleteCharacter"] += new Action<Player,int>(DeleteCharacter);

            EventHandlers["vorp_CharSelectedCharacter"] += new Action<Player, int>(SelectCharacter);

            EventHandlers["vorpcharacter:getPlayerComps"] += new Action<int, dynamic>(getPlayerComps);
            EventHandlers["vorpcharacter:setPlayerSkinChange"] += new Action<int, string>(setPlayerSkinChange);
            EventHandlers["vorpcharacter:setPlayerCompChange"] += new Action<int, string>(setPlayerCompChange);
            EventHandlers["vorpcharacter:getPlayerSkin"] += new Action<Player>(getPlayerSkin);

            //GetCore Event
            TriggerEvent("getCore",new Action<dynamic>((dic) =>
            {
                CORE = dic;
                MaxCharacters = (int) CORE.maxCharacters;
                CORE.addRpcCallback("vorp_characters:getMaxCharacters", new Action<int, CallbackDelegate, dynamic>((source,cb,args) => {
                    cb(MaxCharacters);
                }));
            }));

            API.RegisterCommand("createcharacter", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (source > 0) // it's a player.
                {
                    dynamic User = CORE.getUser(source);
                    if (User.getGroup == "admin")
                    {
                        CreateNewCharacter(source);
                    }
                }
                else
                {
                    Debug.WriteLine("This command was executed by the server console, Please use in game.");
                }
            }), false);

        }

        private void SpawnUniqueCharacter(int source)
        {
            dynamic CorePlayerCharacters = CORE.getUser(source).getUserCharacters;
            PlayerList pl = new PlayerList();
            Player p = pl[source];
            Dictionary<string, dynamic> auxcharacter;
            List<Dictionary<string, dynamic>> UserCharacters = new List<Dictionary<string, dynamic>>();
            foreach (dynamic character in CorePlayerCharacters)
            {
                auxcharacter = new Dictionary<string, dynamic>
                {
                    ["charIdentifier"] = character.charIdentifier,
                    ["money"] = character.money,
                    ["gold"] = character.gold,
                    ["firstname"] = character.firstname,
                    ["lastname"] = character.lastname,
                    ["skin"] = character.skin,
                    ["components"] = character.comps,
                    ["coords"] = character.coords,
                    ["isDead"] = character.isdead
                };
                UserCharacters.Add(auxcharacter);
            }
            p.TriggerEvent("vorpcharacter:spawnUniqueCharacter", UserCharacters);
        }

        private void SelectCharacter([FromSource]Player source, int charid)
        {
            dynamic CoreUser = CORE.getUser(int.Parse(source.Handle));
            if(charid != null)
            {
                CoreUser.setUsedCharacter(charid);
            }
        }

        private void DeleteCharacter([FromSource]Player source,int charid)
        {
            dynamic CoreUser = CORE.getUser(int.Parse(source.Handle));
            CoreUser.removeCharacter(charid);
        }
        
        private async void SaveNewCharacter([FromSource] Player source, dynamic skin, dynamic components, string name)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            string[] divider = name.Split(' ');

            string firstname = divider[0];
            string lastname = divider[1];
            string skinPlayer = JsonConvert.SerializeObject(skin);
            string compsPlayer = JsonConvert.SerializeObject(components);

            dynamic CorePlayer = CORE.getUser(int.Parse(source.Handle));
            try
            {
                CorePlayer.addCharacter(firstname, lastname, skinPlayer, compsPlayer);
                Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(compsPlayer);
                Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skinPlayer);
                source.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
                await Delay(2000);
                source.TriggerEvent("vorp_NewCharacter");
                TriggerEvent("vorp_NewCharacter", int.Parse(source.Handle));
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async void SaveNewCharacter2([FromSource] Player source, dynamic skin, dynamic components, string name, dynamic charidx, dynamic charx)
        {
            string sid = ("steam:" + source.Identifiers["steam"]);

            string[] divider = name.Split(' ');

            string firstname = divider[0];
            string lastname = divider[1];
            string skinPlayer = JsonConvert.SerializeObject(skin);
            string compsPlayer = JsonConvert.SerializeObject(components);

            dynamic CorePlayer = CORE.getUser(int.Parse(source.Handle));
            dynamic UserCharacter = charidx;
            
            try
            {
                Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(compsPlayer);
                Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skinPlayer);
                //charx.setFirstname(firstname.ToString());
                //charx.setLastname(lastname.ToString());
                Exports["ghmattimysql"].execute("UPDATE characters SET `firstname` = ? , `lastname` = ?, `skinPlayer` = ?, `compPlayer` = ? WHERE `identifier` = ? AND `charidentifier` = ? ", new object[] { firstname,lastname,skinPlayer,compsPlayer, sid,UserCharacter });
                source.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
                await Delay(2000);
                //source.TriggerEvent("vorp_NewCharacter");
                //TriggerEvent("vorp_NewCharacter", int.Parse(source.Handle));
            
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void CreateNewCharacter(int source)
        {
            PlayerList pl = new PlayerList();
            Player p = pl[source];
            p.TriggerEvent("vorpcharacter:createCharacter");
        }

        private void GoToSelectionMenu(int source)
        {
            dynamic CorePlayerCharacters = CORE.getUser(source).getUserCharacters;
            Dictionary<string, dynamic> auxcharacter;
            List<Dictionary<string, dynamic>> UserCharacters = new List<Dictionary<string, dynamic>>();
            foreach (dynamic character in CorePlayerCharacters)
            {
                auxcharacter = new Dictionary<string, dynamic>
                {
                    ["charIdentifier"] = character.charIdentifier,
                    ["money"] = character.money,
                    ["gold"] = character.gold,
                    ["firstname"] = character.firstname,
                    ["lastname"] = character.lastname,
                    ["skin"] = character.skin,
                    ["components"] = character.comps,
                    ["coords"] = character.coords,
                    ["isDead"] = character.isdead
                };
                UserCharacters.Add(auxcharacter);
            }
            PlayerList pl = new PlayerList();
            Player p = pl[source];
            p.TriggerEvent("vorpcharacter:selectCharacter", UserCharacters);
        }

        private void setPlayerCompChange(int source, string compsValue)
        {

            PlayerList pl = new PlayerList();
            Player p = pl[source];

            if (p == null)
            {
                return;
            }

            dynamic UserCharacter = CORE.getUser(source).getUsedCharacter;

            string sid = "steam:" + p.Identifiers["steam"];
        
            string comp_string = UserCharacter.comps;
            string s_skin = UserCharacter.skin;

            JObject comp = JObject.Parse(comp_string);

            JObject newcomps = JObject.Parse(compsValue);

            foreach (var nc in newcomps)
            {
                if (comp.ContainsKey(nc.Key))
                {
                    comp[nc.Key] = ConvertValue(nc.Value.ToString());
                }
            }

            UserCharacter.updateComps(compsValue.ToString());

            Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(comp.ToString());
            Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_skin);

            p.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
        }

        private void setPlayerSkinChange(int source, string compsValue)
        {

            PlayerList pl = new PlayerList();
            Player p = pl[source];

            if (p == null)
            {
                return;
            }

            dynamic UserCharacter = CORE.getUser(source).getUsedCharacter;

            string sid = "steam:" + p.Identifiers["steam"];
        
            string skin_string = UserCharacter.skin;
            string s_body = UserCharacter.comps;

            JObject skin = JObject.Parse(skin_string);

            JObject newcomps = JObject.Parse(compsValue);

            foreach (var nc in newcomps)
            {
                if (skin.ContainsKey(nc.Key))
                {
                    skin[nc.Key] = ConvertValue(nc.Value.ToString());
                }
            }

            UserCharacter.updateSkin(skin.ToString());

            Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skin.ToString());
            Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_body);

            p.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
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

            dynamic UserCharacter = CORE.getUser(source).getUsedCharacter;
            string sid = "steam:" + p.Identifiers["steam"];
           
            Dictionary<string, string> comp = new Dictionary<string, string>();

            comp.Add("cloths", UserCharacter.comps);
            comp.Add("skins", UserCharacter.skin);

            cb.Invoke(comp);
        }

        private void getPlayerSkin([FromSource] Player source)
        {
            dynamic UserCharacter = CORE.getUser(int.Parse(source.Handle)).getUsedCharacter;

            string s_skin = UserCharacter.skin;
            string s_body = UserCharacter.comps;

            Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_skin);
            Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_body);


            source.TriggerEvent("vorpcharacter:loadPlayerSkin", sskin, scloth);
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
