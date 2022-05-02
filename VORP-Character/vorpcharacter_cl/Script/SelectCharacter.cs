﻿using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VORP.Character.Client.Diagnostics;
using VORP.Character.Client.Enums;
using VORP.Character.Client.Extensions;
using VORP.Character.Client.Model;
using VORP.Character.Client.Utils;

namespace VORP.Character.Client.Script
{
    public class SelectCharacter : BaseScript
    {
        private int CreatePrompt = -1;
        private int DeletePrompt = -1;

        private int selectedChar = 0;

        private static int pedHandle = 0;
        private static int mainCamera = -1;
        dynamic myChars = null;
        private static bool isInCharacterSelector = false;
        private int tagId = 0;
        private static bool swappingChar = true;

        public SelectCharacter()
        {
            EventHandlers["vorpcharacter:selectCharacter"] += new Action<dynamic>(LoadCharacters);
            EventHandlers["vorpcharacter:spawnUniqueCharacter"] += new Action<dynamic>(SpawnCharacter);
            EventHandlers["vorpcharacter:createCharacter"] += new Action(OnCreateCharacter);

            Logger.Info($"VORP Character SelectCharacter");
        }

        private void OnCreateCharacter()
        {
            new CreateCharacter().StartCreationOfCharacter();
        }

        private async void SpawnCharacter(dynamic myChar)
        {
            try
            {
#if DEVELOPMENT
                Logger.Debug($"SpawnCharacter Called");
#endif

                int charIdentifier = int.Parse(myChar[0].charIdentifier.ToString());
                TriggerServerEvent("vorp_CharSelectedCharacter", charIdentifier);

                string json_skin = myChar[0].skin;
                string json_components = myChar[0].components;
                string json_coords = myChar[0].coords;
                Position position = JsonConvert.DeserializeObject<Position>(json_coords);

                // TriggerEvent("vorpcharacter:loadPlayerSkin", json_skin, json_components); // WHY?! just call the class method
                await LoadPlayer.Instance.LoadPlayerSkin(json_skin, json_components);

                API.DoScreenFadeOut(1000);
                await Delay(800);
                Vector3 playerCoords = position.AsVector();
                bool isDead = false;
                try
                {
                    isDead = (bool)myChar[0].isDead;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                float heading = position.H;
                TriggerEvent("vorp:initCharacter", playerCoords, heading, isDead);
                await Delay(2000);
                int playerPedId = API.PlayerPedId();
                API.SetEntityCoords(playerPedId, position.X, position.Y, position.Z, false, false, false, true);
                API.SetEntityHeading(playerPedId, position.H);
                API.DoScreenFadeIn(1000);
                while(API.IsScreenFadingIn())
                {
                    await BaseScript.Delay(100);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                API.DoScreenFadeIn(1000);
            }
        }

        private void RegisterPrompts()
        {
            //Left
            CreatePrompt = API.PromptRegisterBegin();
            Function.Call((Hash)0xB5352B7494A08258, CreatePrompt, 0x9959A6F0);
            long str_previous = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", Common.GetTranslation("CreateNewChar"));
            Function.Call((Hash)0x5DD02A8318420DD7, CreatePrompt, str_previous);
            API.PromptSetEnabled(CreatePrompt, 0);
            API.PromptSetVisible(CreatePrompt, 0);
            API.PromptSetHoldMode(CreatePrompt, 1);
            API.PromptRegisterEnd(CreatePrompt);

            //Enter
            DeletePrompt = API.PromptRegisterBegin();
            Function.Call((Hash)0xB5352B7494A08258, DeletePrompt, 0x156F7119);
            long str_select = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", Common.GetTranslation("DeleteChar"));
            Function.Call((Hash)0x5DD02A8318420DD7, DeletePrompt, str_select);
            API.PromptSetEnabled(DeletePrompt, 0);
            API.PromptSetVisible(DeletePrompt, 0);
            API.PromptSetHoldMode(DeletePrompt, 1);
            API.PromptRegisterEnd(DeletePrompt);
        }

        public async Task StartSwapCharacter()
        {
            swappingChar = true;
            string json_skin = myChars[selectedChar].skin;
            string json_components = myChars[selectedChar].components;

            API.PromptSetEnabled(DeletePrompt, 0);
            API.PromptSetVisible(DeletePrompt, 0);
            API.PromptSetEnabled(CreatePrompt, 0);
            API.PromptSetVisible(CreatePrompt, 0);

            API.TaskGoToCoordAnyMeans(pedHandle, 1697.74f, 1510.202f, 147.87f, 0.8f, 0, false, 524419, -1f);
            await Delay(2000);
            Function.Call((Hash)0xA0D7CE5F83259663, tagId, "");
            Function.Call((Hash)0x839BFD7D7E49FE09, tagId);
            API.DeletePed(ref pedHandle);
            await Delay(1000);
            pedHandle = await LoadNpcComps(json_skin, json_components);
            Utilities.UpdatePedVariation(pedHandle, true);

            tagId = Function.Call<int>((Hash)0x53CB4B502E1C57EA, pedHandle, $"{Common.GetTranslation("MoneyTag")}: ~COLOR_WHITE~$" + "~COLOR_REPLAY_GREEN~" + myChars[selectedChar].money, false, false, "", 0);
            Function.Call((Hash)0xA0D7CE5F83259663, tagId, myChars[selectedChar].firstname + " " + myChars[selectedChar].lastname);
            Function.Call((Hash)0x5F57522BC1EB9D9D, tagId, 0);
            await Delay(500);

            API.TaskGoToCoordAnyMeans(pedHandle, 1696.17f, 1508.474f, 147.85f, 0.8f, 0, false, 524419, -1f);

            API.PromptSetEnabled(DeletePrompt, 1);
            API.PromptSetVisible(DeletePrompt, 1);
            API.PromptSetEnabled(CreatePrompt, 1);
            API.PromptSetVisible(CreatePrompt, 1);
            await Delay(500);
            swappingChar = false;
        }


        public async void LoadCharacters(dynamic myCharacters)
        {
            await Utilities.FadeOutScreen(0);

            Logger.Info($"Loading player characters");

            RegisterPrompts();

            isInCharacterSelector = true;
            Controller();
            DrawInformation();

            API.NetworkClockTimeOverride(20, 0, 0, 0, false);
            API.NetworkClockTimeOverride_2(20, 0, 0, 0, true, false);
            API.FreezeEntityPosition(Cache.PlayerPedId, true);

            API.SetEntityCoords(Cache.PlayerPedId, 1687.03f, 1507.06f, 145.60f, false, false, false, false);

#if DEVELOPMENT
            Logger.Debug($"Load Characters");
            Logger.Debug(JsonConvert.SerializeObject(myCharacters));
#endif

            myChars = myCharacters;

            mainCamera = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", 1693.301f, 1507.959f, 148.84f, -13.82f, 0f, -84.67f, 50.00f, false, 0);

            API.SetCamActive(mainCamera, true);

            API.RenderScriptCams(true, true, 1000, true, true, 0);

            await Delay(5000);

            await Utilities.FadeInScreen(1000);

            StartSwapCharacter();
        }

        public async Task DrawInformation()
        {
            while (isInCharacterSelector)
            {
                API.DrawLightWithRange(1696.17f, 1508.474f, 147.85f, 255, 255, 255, 8.0f, 250.0f);
                await Utils.Miscellanea.DrawTxt(Common.GetTranslation("PressSelectInfo"), 0.5f, 0.90f, 0.75f, 0.70f, 255, 255, 255, 255, true, false);
                await Delay(0);
            }
        }

        public async Task CharSelect()
        {
            try
            {
                Logger.Debug($"CharSelect Called");

                API.DoScreenFadeOut(1000);

                while (API.IsScreenFadingOut())
                {
                    await Delay(100);
                }

                int charIdentifier = int.Parse(myChars[selectedChar].charIdentifier.ToString());
                TriggerServerEvent("vorp_CharSelectedCharacter", charIdentifier);

                API.PromptSetEnabled(DeletePrompt, 0);
                API.PromptSetVisible(DeletePrompt, 0);
                API.PromptSetEnabled(CreatePrompt, 0);
                API.PromptSetVisible(CreatePrompt, 0);
                API.DeletePed(ref pedHandle);

                string json_skin = myChars[selectedChar].skin; // make this a class
                string json_components = myChars[selectedChar].components;
                string json_coords = myChars[selectedChar].coords;
                Position pos = JsonConvert.DeserializeObject<Position>(json_coords);

                await LoadPlayer.Instance.LoadPlayerSkin(json_skin, json_components);

                API.NetworkClearClockTimeOverride();
                API.FreezeEntityPosition(Cache.PlayerPedId, false);

                API.DoScreenFadeOut(1000);
                await Delay(800);
                API.SetCamActive(mainCamera, false);
                API.DestroyCam(mainCamera, true);
                API.RenderScriptCams(true, true, 1000, true, true, 0);
                Vector3 playerCoords = pos.AsVector();
                bool isDead = false;
                try
                {
                    isDead = (bool)myChars[selectedChar].isDead;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                float heading = pos.H;
                TriggerEvent("vorp:initCharacter", playerCoords, heading, isDead);

                int playerPedId = API.PlayerPedId();
                API.SetEntityCoords(playerPedId, pos.X, pos.Y, pos.Z, false, false, false, true);
                API.SetEntityHeading(playerPedId, pos.H);

                await Delay(1000);
                API.DoScreenFadeIn(1000);

                while (API.IsScreenFadingIn())
                {
                    await Delay(100);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                API.DoScreenFadeIn(0);
            }
        }

        public async Task Controller()
        {
            while (isInCharacterSelector)
            {
                if (API.IsControlJustPressed(0, (uint)eControl.FrontendRight) && !swappingChar)
                {
                    if (selectedChar == myChars.Count - 1)
                    {
                        selectedChar = 0;
                    }
                    else
                    {
                        selectedChar += 1;
                    }
                    await StartSwapCharacter();
                }

                if (API.IsControlJustPressed(0, (uint)eControl.FrontendLeft) && !swappingChar)
                {
                    if (selectedChar == 0)
                    {
                        selectedChar = myChars.Count - 1;
                    }
                    else
                    {
                        selectedChar -= 1;
                    }
                    await StartSwapCharacter();
                }

                if (API.IsControlJustPressed(0, (uint)eControl.FrontendAccept) && !swappingChar)
                {
                    CharSelect();
                    isInCharacterSelector = false;
                    await Delay(200);
                }

                if (API.PromptHasHoldModeCompleted(CreatePrompt) && !swappingChar)
                {
                    if (myChars.Count < PluginManager.MAX_ALLOWED_CHARACTERS)
                    {
                        new CreateCharacter().StartCreationOfCharacter();
                        API.PromptSetEnabled(DeletePrompt, 0);
                        API.PromptSetVisible(DeletePrompt, 0);
                        API.PromptSetEnabled(CreatePrompt, 0);
                        API.PromptSetVisible(CreatePrompt, 0);
                        API.DeletePed(ref pedHandle);
                        isInCharacterSelector = false;
                        return;
                    }
                    else
                    {
                        PluginManager.CORE.displayRightTip(Common.GetTranslation("CharactersFull"), 4000);
                        await Delay(1000);
                    }
                }

                if (API.PromptHasHoldModeCompleted(DeletePrompt))
                {
                    API.PromptRestartModes(DeletePrompt);

                    API.PromptSetEnabled(DeletePrompt, 0);
                    API.PromptSetEnabled(CreatePrompt, 0);
                    await Delay(500);

                    isInCharacterSelector = false;

                    TriggerEvent("vorpinputs:getInput", Common.GetTranslation("ButtonSupprName"), Common.GetTranslation("SUPPRConfirmMsg"), new Action<dynamic>(async (cb) =>
                    {
                        string result = cb;
                        await Delay(100);

                        isInCharacterSelector = true;
                        Controller();
                        DrawInformation();

                        await Delay(100);

                        if (!result.Equals("close"))
                        {
                            if (result.Equals(Common.GetTranslation("SUPPRCode")))
                            {
                                TriggerServerEvent("vorp_DeleteCharacter", (int)myChars[selectedChar].charIdentifier);

                                myChars.RemoveAt(selectedChar);

                                Function.Call(Hash.ADD_EXPLOSION, 1696.17f, 1508.474f, 147.85f, 26, 50.0f, true, false, true);

                                if (myChars.Count == 0)
                                {
                                    new CreateCharacter().StartCreationOfCharacter();
                                    API.PromptSetEnabled(DeletePrompt, 0);
                                    API.PromptSetVisible(DeletePrompt, 0);
                                    API.PromptSetEnabled(CreatePrompt, 0);
                                    API.PromptSetVisible(CreatePrompt, 0);
                                    API.DeletePed(ref pedHandle);
                                    isInCharacterSelector = false;
                                }
                                else
                                {
                                    if (selectedChar == 0)
                                    {
                                        selectedChar = myChars.Count - 1;
                                    }
                                    else
                                    {
                                        selectedChar -= 1;
                                        if (selectedChar < 0)
                                            selectedChar = 0;
                                    }

                                    await StartSwapCharacter();
                                }
                            }
                        }
                    }));
                }

                await Delay(0);
            }
        }


        public async Task<int> LoadNpcComps(string skin_json, string cloths_json)
        {
            JObject jskin = JObject.Parse(skin_json);
            JObject jcomp = JObject.Parse(cloths_json);

            Dictionary<string, string> skin = new Dictionary<string, string>();

            foreach (var s in jskin)
            {
                skin[s.Key] = s.Value.ToString();
            }

            Dictionary<string, long> clothes = new Dictionary<string, long>();

            foreach (var s in jcomp)
            {
                clothes[s.Key] = LoadPlayer.ConvertValue(s.Value.ToString());
            }
            int _pedHandle = await LoadPlayer.Instance.SetupCharacter(false, skin, clothes);
            LoadPlayer.SetPedComponents(clothes, _pedHandle);
            return _pedHandle;
        }
    }
}
