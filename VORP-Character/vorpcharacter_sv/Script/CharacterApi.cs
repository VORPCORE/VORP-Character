using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using VORP.Character.Server.Extensions;

namespace VORP.Character.Server.Script
{
    public class CharacterApi : BaseScript
    {
        public static CharacterApi Instance { get; private set; }

        public void Init()
        {
            Instance = this;

            //Event for create new character 
            EventHandlers["vorp_CreateNewCharacter"] += new Action<int>(CreateNewCharacter);
            //Event for select characters
            EventHandlers["vorp_GoToSelectionMenu"] += new Action<int>(GoToSelectionMenu);
            //Event for spawn unique character
            EventHandlers["vorp_SpawnUniqueCharacter"] += new Action<int>(SpawnUniqueCharacter);
            //Event for save the new character
            EventHandlers["vorp_SaveNewCharacter"] += new Action<Player, dynamic, dynamic, string>(SaveNewCharacter);
            EventHandlers["vorp_updateexisting"] += new Action<Player, dynamic, dynamic, string, dynamic, dynamic>(SaveNewCharacter2);
            //Event for delete the character
            EventHandlers["vorp_DeleteCharacter"] += new Action<Player, int>(DeleteCharacter);

            EventHandlers["vorp_CharSelectedCharacter"] += new Action<Player, int>(SelectCharacter);

            EventHandlers["vorpcharacter:getPlayerComps"] += new Action<int, dynamic>(getPlayerComps);
            EventHandlers["vorpcharacter:setPlayerSkinChange"] += new Action<int, string>(setPlayerSkinChange);
            EventHandlers["vorpcharacter:setPlayerCompChange"] += new Action<int, string>(setPlayerCompChange);
            EventHandlers["vorpcharacter:getPlayerSkin"] += new Action<Player>(getPlayerSkin);

            // maybe worth protecting this with ACE Permissions?
            API.RegisterCommand("createcharacter", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                if (source > 0) // it's a player.
                {
                    Player player = Common.GetPlayer(source);
                    if (player == null)
                    {
                        Logger.Error($"command createcharacter: Player '{source}' doesn't exist.");
                        return;
                    }

                    dynamic coreUser = player.GetCoreUser();
                    if (coreUser.getGroup == "admin")
                    {
                        CreateNewCharacter(source);
                    }
                }
                else
                {
                    Debug.WriteLine("This command was executed by the server console, Please use in game.");
                }
            }), false);

            Logger.Info($"INIT VORP Character CharacterApi");
        }

        private void SpawnUniqueCharacter(int source)
        {

            Player player = Common.GetPlayer(source);
            if (player == null)
            {
                Logger.Error($"SpawnUniqueCharacter: Player '{source}' doesn't exist.");
                return;
            }

            dynamic coreUserCharacters = player.GetCoreUser().getUserCharacters;

            Dictionary<string, dynamic> auxcharacter;
            List<Dictionary<string, dynamic>> UserCharacters = new List<Dictionary<string, dynamic>>();
            foreach (dynamic character in coreUserCharacters)
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
            player.TriggerEvent("vorpcharacter:spawnUniqueCharacter", UserCharacters);
        }

        private void SelectCharacter([FromSource] Player player, int charid = 0)
        {
            dynamic coreUser = player.GetCoreUser();
            if (coreUser == null)
            {
                Logger.Error($"SelectCharacter: Core User '{player.Handle}' doesn't exist.");
                return;
            }

            if (charid == 0)
            {
                Logger.Error($"SelectCharacter: CharID of '0' is not allowed.");
                return;
            }

            coreUser.setUsedCharacter(charid);
        }

        private void DeleteCharacter([FromSource] Player player, int charid)
        {
            dynamic CoreUser = player.GetCoreUser();
            if (CoreUser == null) return;
            CoreUser.removeCharacter(charid);
            PluginManager.Instance.DiscordClient.SendDiscordEmbededMessageAsync("vorp_character", $"VORP Character Sentry", $"Deleted Character", $"User: {player.Name}", Web.DiscordColor.Red);
        }

        private async void SaveNewCharacter([FromSource] Player player, dynamic skin, dynamic components, string name)
        {
            string sid = player.SteamHandle();

            string[] divider = name.Split(' ');

            string firstname = divider[0];
            string lastname = divider[1];
            string skinPlayer = JsonConvert.SerializeObject(skin);
            string compsPlayer = JsonConvert.SerializeObject(components);

            dynamic coreUser = player.GetCoreUser();
            if (coreUser == null)
            {
                Logger.Error($"SaveNewCharacter: Core User '{player.Handle}' doesn't exist.");
                return;
            }

            try
            {
                coreUser.addCharacter(firstname, lastname, skinPlayer, compsPlayer);
                Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(compsPlayer);
                Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skinPlayer);
                player.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
                await Delay(2000);
                TriggerEvent("vorp_NewCharacter", int.Parse(player.Handle));

                PluginManager.Instance.DiscordClient.SendDiscordEmbededMessageAsync("vorp_character", $"VORP Character Sentry", $"Created Character", $"User: {player.Name}", Web.DiscordColor.Green);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private async void SaveNewCharacter2([FromSource] Player player, dynamic skin, dynamic components, string name, dynamic charidx, dynamic charx)
        {
            string sid = player.SteamHandle();

            string[] divider = name.Split(' ');

            string firstname = divider[0];
            string lastname = divider[1];
            string skinPlayer = JsonConvert.SerializeObject(skin);
            string compsPlayer = JsonConvert.SerializeObject(components);
            dynamic UserCharacter = charidx;

            try
            {
                Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(compsPlayer);
                Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skinPlayer);
                Exports["ghmattimysql"].execute("UPDATE characters SET `firstname` = ? , `lastname` = ?, `skinPlayer` = ?, `compPlayer` = ? WHERE `identifier` = ? AND `charidentifier` = ? ", new object[] { firstname, lastname, skinPlayer, compsPlayer, sid, UserCharacter });
                await Delay(0);
                player.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void CreateNewCharacter(int source)
        {
            Player player = Common.GetPlayer(source);
            if (player == null)
            {
                Logger.Error($"CreateNewCharacter: Player '{source}' doesn't exist.");
                return;
            }
            player.TriggerEvent("vorpcharacter:createCharacter");
        }

        private void GoToSelectionMenu(int source)
        {
            try
            {
                Player player = Common.GetPlayer(source);
                if (player == null)
                {
                    Logger.Error($"GoToSelectionMenu: Player '{source}' doesn't exist.");
                    return;
                }

                Logger.Debug($"Loading '{player.Name}' characters");

                dynamic coreUserCharacters = player.GetCoreUserCharacters();
                Dictionary<string, dynamic> auxcharacter;
                List<Dictionary<string, dynamic>> UserCharacters = new List<Dictionary<string, dynamic>>();
                foreach (dynamic character in coreUserCharacters)
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

                Logger.Debug($"Found {UserCharacters.Count} characters for player '{player.Name}'");

                player.TriggerEvent("vorpcharacter:selectCharacter", UserCharacters);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"GoToSelectionMenu");
            }
        }

        private void setPlayerCompChange(int source, string compsValue)
        {

            Player player = Common.GetPlayer(source);
            if (player == null)
            {
                Logger.Error($"setPlayerCompChange: Player '{source}' doesn't exist.");
                return;
            }

            dynamic coreUserCharacter = player.GetCoreUserCharacter();

            if (coreUserCharacter == null)
            {
                Logger.Error($"setPlayerCompChange: Core User '{source}' doesn't exist.");
                return;
            }

            string comp_string = coreUserCharacter.comps;
            string s_skin = coreUserCharacter.skin;

            JObject comp = JObject.Parse(comp_string);

            JObject newcomps = JObject.Parse(compsValue);

            foreach (var nc in newcomps)
            {
                if (comp.ContainsKey(nc.Key))
                {
                    comp[nc.Key] = ConvertValue(nc.Value.ToString());
                }
            }

            coreUserCharacter.updateComps(compsValue.ToString());

            Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(comp.ToString());
            Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_skin);

            player.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
        }

        private void setPlayerSkinChange(int source, string compsValue)
        {

            Player player = Common.GetPlayer(source);
            if (player == null)
            {
                Logger.Error($"setPlayerSkinChange: Player '{source}' doesn't exist.");
                return;
            }

            dynamic coreUserCharacter = player.GetCoreUserCharacter();

            if (coreUserCharacter == null)
            {
                Logger.Error($"setPlayerSkinChange: Core User '{source}' doesn't exist.");
                return;
            }

            string sid = "steam:" + player.Identifiers["steam"];

            string skin_string = coreUserCharacter.skin;
            string s_body = coreUserCharacter.comps;

            JObject skin = JObject.Parse(skin_string);

            JObject newcomps = JObject.Parse(compsValue);

            foreach (var nc in newcomps)
            {
                if (skin.ContainsKey(nc.Key))
                {
                    skin[nc.Key] = ConvertValue(nc.Value.ToString());
                }
            }

            coreUserCharacter.updateSkin(skin.ToString());

            Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skin.ToString());
            Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_body);

            player.TriggerEvent("vorpcharacter:reloadPlayerComps", sskin, scloth);
        }

        private void getPlayerComps(int source, dynamic cb)
        {

            Player player = Common.GetPlayer(source);
            if (player == null)
            {
                Logger.Error($"getPlayerComps: Player '{source}' doesn't exist.");
                return;
            }

            dynamic coreUserCharacter = player.GetCoreUserCharacter();

            if (coreUserCharacter == null)
            {
                Logger.Error($"getPlayerComps: Core User '{source}' doesn't exist.");
                return;
            }

            Dictionary<string, string> comp = new Dictionary<string, string>();

            comp.Add("cloths", coreUserCharacter.comps);
            comp.Add("skins", coreUserCharacter.skin);

            cb.Invoke(comp);
        }

        private void getPlayerSkin([FromSource] Player player)
        {
            dynamic coreUserCharacter = player.GetCoreUserCharacter();

            if (coreUserCharacter == null)
            {
                Logger.Error($"getPlayerSkin: Core User '{player.Handle}' doesn't exist.");
                return;
            }

            string s_skin = coreUserCharacter.skin;
            string s_body = coreUserCharacter.comps;

            Dictionary<string, string> sskin = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_skin);
            Dictionary<string, string> scloth = JsonConvert.DeserializeObject<Dictionary<string, string>>(s_body);


            player.TriggerEvent("vorpcharacter:loadPlayerSkin", sskin, scloth);
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
