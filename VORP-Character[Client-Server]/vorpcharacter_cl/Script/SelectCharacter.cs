using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VorpCharacter.Extensions;
using VorpCharacter.Utils;
using VorpCharacter.Diagnostics;
using Newtonsoft.Json;

namespace VorpCharacter.Script
{
    public class SelectCharacter : BaseScript
    {
        private int CreatePrompt = -1;
        private int DeletePrompt = -1;

        private int selectedChar = 0;

        private static int playerPedId = 0;
        private static int mainCamera = -1;
        dynamic myChars = null;
        private static bool isInCharacterSelector = false;
        private int tagId = 0;
        private static bool swappingChar = true;

        internal SelectCharacter()
        {
            EventHandlers["vorpcharacter:selectCharacter"] += new Action<dynamic>(LoadCharacters);
            EventHandlers["vorpcharacter:spawnUniqueCharacter"] += new Action<dynamic>(SpawnCharacter);
        }

        private async void SpawnCharacter(dynamic myChar)
        {
            try
            {
                Logger.Debug($"SpawnCharacter Called");

                int charIdentifier = int.Parse(myChar[0].charIdentifier.ToString());
                TriggerServerEvent("vorp_CharSelectedCharacter", charIdentifier);

                string json_skin = myChar[0].skin;
                string json_components = myChar[0].components;
                string json_coords = myChar[0].coords;
                JObject jPos = JObject.Parse(json_coords);

                // TriggerEvent("vorpcharacter:loadPlayerSkin", json_skin, json_components); // WHY?! just call the class method
                await PluginManager._loadPlayer.LoadPlayerSkin(json_skin, json_components);

                API.DoScreenFadeOut(1000);
                await Delay(800);
                Vector3 playerCoords = new Vector3(jPos["x"].ToObject<float>(), jPos["y"].ToObject<float>(), jPos["z"].ToObject<float>());
                bool isDead = false;
                try
                {
                    isDead = (bool)myChar[0].isDead;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                float heading = jPos["heading"].ToObject<float>();
                TriggerEvent("vorp:initCharacter", playerCoords, heading, isDead);
                await Delay(1000);
                API.DoScreenFadeIn(1000);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                API.DoScreenFadeIn(1000);
            }
        }

        public async Task StartAnim()
        {
            uint hashmodel = (uint)API.GetHashKey("mp_male");
            await Utilities.RequestModel(hashmodel);
            int character_1 = API.CreatePed(hashmodel, 1701.316f, 1512.134f, 146.87f, 116.70f, false, false, true, true);
            Function.Call((Hash)0x283978A15512B2FE, character_1, true);
            await Delay(1000);
            API.TaskGoToCoordAnyMeans(character_1, 1696.17f, 1508.474f, 147.85f, 0.5f, 0, false, 524419, -1f);
            await Delay(8000);
            API.TaskGoToCoordAnyMeans(character_1, 1697.74f, 1510.202f, 147.87f, 0.5f, 0, false, 524419, -1f);
            await Delay(5000);
            API.DeletePed(ref character_1);
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

            API.TaskGoToCoordAnyMeans(playerPedId, 1697.74f, 1510.202f, 147.87f, 0.8f, 0, false, 524419, -1f);
            await Delay(2000);
            Function.Call((Hash)0xA0D7CE5F83259663, tagId, "");
            Function.Call((Hash)0x839BFD7D7E49FE09, tagId);
            API.DeletePed(ref playerPedId);
            await LoadNpcComps(json_skin, json_components);
            tagId = Function.Call<int>((Hash)0x53CB4B502E1C57EA, playerPedId, $"{Common.GetTranslation("MoneyTag")}: ~COLOR_WHITE~$" + "~COLOR_REPLAY_GREEN~" + myChars[selectedChar].money, false, false, "", 0);
            Function.Call((Hash)0xA0D7CE5F83259663, tagId, myChars[selectedChar].firstname + " " + myChars[selectedChar].lastname);
            Function.Call((Hash)0x5F57522BC1EB9D9D, tagId, 0);
            await Delay(500);
            API.TaskGoToCoordAnyMeans(playerPedId, 1696.17f, 1508.474f, 147.85f, 0.8f, 0, false, 524419, -1f);

            API.PromptSetEnabled(DeletePrompt, 1);
            API.PromptSetVisible(DeletePrompt, 1);
            API.PromptSetEnabled(CreatePrompt, 1);
            API.PromptSetVisible(CreatePrompt, 1);
            await Delay(500);
            swappingChar = false;
        }


        public async void LoadCharacters(dynamic myCharacters)
        {

            RegisterPrompts();

            isInCharacterSelector = true;
            Controller();
            DrawInformation();
            Function.Call(Hash.SET_CLOCK_TIME, 12, 00, 0);
            API.SetClockTime(12, 00, 00);

            API.SetEntityCoords(API.PlayerPedId(), 1687.03f, 1507.06f, 145.60f, false, false, false, false);

            myChars = myCharacters;

            mainCamera = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", 1693.301f, 1507.959f, 148.84f, -13.82f, 0f, -84.67f, 50.00f, false, 0);

            API.SetCamActive(mainCamera, true);

            API.RenderScriptCams(true, true, 1000, true, true, 0);

            await Delay(5000);

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

                Logger.Debug($"CHARACTER DUMP!");
                Logger.Debug($"{JsonConvert.SerializeObject(myChars)}");
                Logger.Debug($"CHARACTER DUMP!");

                int charIdentifier = int.Parse(myChars[selectedChar].charIdentifier.ToString());
                TriggerServerEvent("vorp_CharSelectedCharacter", charIdentifier);

                API.PromptSetEnabled(DeletePrompt, 0);
                API.PromptSetVisible(DeletePrompt, 0);
                API.PromptSetEnabled(CreatePrompt, 0);
                API.PromptSetVisible(CreatePrompt, 0);
                API.DeletePed(ref playerPedId);

                string json_skin = myChars[selectedChar].skin; // make this a class
                string json_components = myChars[selectedChar].components;
                string json_coords = myChars[selectedChar].coords;
                JObject jPos = JObject.Parse(json_coords);



                Logger.Debug($"CHARACTER DUMP!");
                Logger.Debug($"{JsonConvert.SerializeObject(json_skin)}");
                Logger.Debug($"CHARACTER DUMP!");

                // TriggerEvent("vorpcharacter:loadPlayerSkin", json_skin, json_components); // WHY?! just call the class method
                await PluginManager._loadPlayer.LoadPlayerSkin(json_skin, json_components);
                
                API.DoScreenFadeOut(1000);
                await Delay(800);
                API.SetCamActive(mainCamera, false);
                API.DestroyCam(mainCamera, true);
                API.RenderScriptCams(true, true, 1000, true, true, 0);
                Vector3 playerCoords = new Vector3(jPos["x"].ToObject<float>(), jPos["y"].ToObject<float>(), jPos["z"].ToObject<float>());
                bool isDead = false;
                try
                {
                    isDead = (bool)myChars[selectedChar].isDead;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                float heading = jPos["heading"].ToObject<float>();
                TriggerEvent("vorp:initCharacter", playerCoords, heading, isDead);

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
                if (API.IsControlJustPressed(0, 0xDEB34313) && !swappingChar)
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

                if (API.IsControlJustPressed(0, 0xA65EBAB4) && !swappingChar)
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

                if (API.IsControlJustPressed(0, 0xC7B5340A) && !swappingChar)
                {
                    CharSelect();
                    isInCharacterSelector = false;
                    await Delay(200);
                }

                if (API.PromptHasHoldModeCompleted(CreatePrompt) && !swappingChar)
                {
                    if (myChars.Count < PluginManager.MAX_ALLOWED_CHARACTERS)
                    {
                        PluginManager._createCharacter.StartCreationOfCharacter();
                        API.PromptSetEnabled(DeletePrompt, 0);
                        API.PromptSetVisible(DeletePrompt, 0);
                        API.PromptSetEnabled(CreatePrompt, 0);
                        API.PromptSetVisible(CreatePrompt, 0);
                        API.DeletePed(ref playerPedId);
                        isInCharacterSelector = false;
                        return;
                    }
                    else
                    {
                        PluginManager.CORE.displayRightTip(Common.GetTranslation("CharactersFull"), 4000);
                        await Delay(1000);
                    }
                }

                if (API.PromptHasHoldModeCompleted(DeletePrompt) && !swappingChar)
                {
                    TriggerServerEvent("vorp_DeleteCharacter", (int)myChars[selectedChar].charIdentifier);
                    if (myChars.Count <= 1)
                    {
                        PluginManager._createCharacter.StartCreationOfCharacter();
                        API.PromptSetEnabled(DeletePrompt, 0);
                        API.PromptSetVisible(DeletePrompt, 0);
                        API.PromptSetEnabled(CreatePrompt, 0);
                        API.PromptSetVisible(CreatePrompt, 0);
                        API.DeletePed(ref playerPedId);
                        isInCharacterSelector = false;
                        return;
                    }
                    else
                    {
                        myChars.RemoveAt(selectedChar);

                        Function.Call((Hash)0x7D6F58F69DA92530, 1696.17f, 1508.474f, 147.85f, 26, 50.0f, true, false, true);

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
                }

                await Delay(0);
            }

        }

        public async Task LoadNpcComps(string skin_json, string cloths_json)
        {
            JObject jskin = JObject.Parse(skin_json);
            JObject jcomp = JObject.Parse(cloths_json);

            Dictionary<string, string> skin = new Dictionary<string, string>();

            foreach (var s in jskin)
            {
                skin[s.Key] = s.Value.ToString();
            }

            Dictionary<string, uint> cloths = new Dictionary<string, uint>();

            foreach (var s in jcomp)
            {
                cloths[s.Key] = LoadPlayer.ConvertValue(s.Value.ToString());
            }

            uint model_hash = (uint)API.GetHashKey(skin["sex"]);
            await Utilities.RequestModel(model_hash);
            playerPedId = API.CreatePed(model_hash, 1701.316f, 1512.134f, 146.87f, 116.70f, false, false, true, true);
            CreateCharacter.ApplyDefaultSkinSettings(playerPedId);
            await Delay(200);

            //PreLoad TextureFace
            if (skin["sex"].ToString().Equals("mp_male"))
            {
                CreateCharacter.texture_types["albedo"] = int.Parse(skin["albedo"]);
                CreateCharacter.texture_types["normal"] = API.GetHashKey("mp_head_mr1_000_nm");
                CreateCharacter.texture_types["material"] = 0x7FC5B1E1;
                CreateCharacter.texture_types["color_type"] = 1;
                CreateCharacter.texture_types["texture_opacity"] = 1.0f;
                CreateCharacter.texture_types["unk_arg"] = 0;
            }
            else
            {
                CreateCharacter.texture_types["albedo"] = int.Parse(skin["albedo"]);
                CreateCharacter.texture_types["normal"] = API.GetHashKey("head_fr1_mp_002_nm");
                CreateCharacter.texture_types["material"] = 0x7FC5B1E1;
                CreateCharacter.texture_types["color_type"] = 1;
                CreateCharacter.texture_types["texture_opacity"] = 1.0f;
                CreateCharacter.texture_types["unk_arg"] = 0;
            }
            //End

            //LoadSkin
            await Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(skin["HeadType"]));
            await Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(skin["BodyType"]));
            await Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(skin["LegsType"]));

            await Utilities.SetPedFaceFeature(playerPedId, 0x84D6, float.Parse(skin["HeadSize"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x3303, float.Parse(skin["EyeBrowH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x2FF9, float.Parse(skin["EyeBrowW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x4AD1, float.Parse(skin["EyeBrowD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xC04F, float.Parse(skin["EarsH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xB6CE, float.Parse(skin["EarsW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x2844, float.Parse(skin["EarsD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xED30, float.Parse(skin["EarsL"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x8B2B, float.Parse(skin["EyeLidH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x1B6B, float.Parse(skin["EyeLidW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xEE44, float.Parse(skin["EyeD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xD266, float.Parse(skin["EyeAng"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xA54E, float.Parse(skin["EyeDis"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xDDFB, float.Parse(skin["EyeH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x6E7F, float.Parse(skin["NoseW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x3471, float.Parse(skin["NoseS"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x03F5, float.Parse(skin["NoseH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x34B1, float.Parse(skin["NoseAng"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xF156, float.Parse(skin["NoseC"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x561E, float.Parse(skin["NoseDis"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x6A0B, float.Parse(skin["CheekBonesH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xABCF, float.Parse(skin["CheekBonesW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x358D, float.Parse(skin["CheekBonesD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xF065, float.Parse(skin["MouthW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xAA69, float.Parse(skin["MouthD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x7AC3, float.Parse(skin["MouthX"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x410D, float.Parse(skin["MouthY"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x1A00, float.Parse(skin["ULiphH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x91C1, float.Parse(skin["ULiphW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xC375, float.Parse(skin["ULiphD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xBB4D, float.Parse(skin["LLiphH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xB0B0, float.Parse(skin["LLiphW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x5D16, float.Parse(skin["LLiphD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x8D0A, float.Parse(skin["JawH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xEBAE, float.Parse(skin["JawW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x1DF6, float.Parse(skin["JawD"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0x3C0F, float.Parse(skin["ChinH"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xC3B2, float.Parse(skin["ChinW"]));
            await Utilities.SetPedFaceFeature(playerPedId, 0xE323, float.Parse(skin["ChinD"]));
            await Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(skin["Eyes"]), true, true, true);
            await Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(skin["Hair"]), true, true, true);
            Utilities.UpdatePedVariation(playerPedId);
            await Delay(100);
            Utilities.SetPedBodyComponent(playerPedId, LoadPlayer.ConvertValue(skin["Body"]));
            await Delay(100);
            Utilities.SetPedBodyComponent(playerPedId, LoadPlayer.ConvertValue(skin["Waist"]));
            Utilities.UpdatePedVariation(playerPedId);
            await Delay(500);


            SetPlayerComponent(skin["sex"], 0x9925C067, "Hat", cloths);
            SetPlayerComponent(skin["sex"], 0x5E47CA6, "EyeWear", cloths);
            SetPlayerComponent(skin["sex"], 0x7505EF42, "Mask", cloths);
            SetPlayerComponent(skin["sex"], 0x5FC29285, "NeckWear", cloths);
            SetPlayerComponent(skin["sex"], 0x877A2CF7, "Suspender", cloths);
            SetPlayerComponent(skin["sex"], 0x485EE834, "Vest", cloths);
            SetPlayerComponent(skin["sex"], 0xE06D30CE, "Coat", cloths);
            SetPlayerComponent(skin["sex"], 0x2026C46D, "Shirt", cloths);
            SetPlayerComponent(skin["sex"], 0x7A96FACA, "NeckTies", cloths);
            SetPlayerComponent(skin["sex"], 0xAF14310B, "Poncho", cloths);
            SetPlayerComponent(skin["sex"], 0x3C1A74CD, "Cloak", cloths);
            SetPlayerComponent(skin["sex"], 0xEABE0032, "Glove", cloths);
            SetPlayerComponent(skin["sex"], 0x7A6BBD0B, "RingRh", cloths);
            SetPlayerComponent(skin["sex"], 0xF16A1D23, "RingLh", cloths);
            SetPlayerComponent(skin["sex"], 0x7BC10759, "Bracelet", cloths);
            SetPlayerComponent(skin["sex"], 0x9B2C8B89, "Gunbelt", cloths);
            SetPlayerComponent(skin["sex"], 0xA6D134C6, "Belt", cloths);
            SetPlayerComponent(skin["sex"], 0xFAE9107F, "Buckle", cloths);
            SetPlayerComponent(skin["sex"], 0xB6B6122D, "Holster", cloths);
            if (cloths["Skirt"] != -1) // Prevents both Pant & Skirt in female ped.
            {
                SetPlayerComponent(skin["sex"], 0x1D4C528A, "Pant", cloths);
            }
            SetPlayerComponent(skin["sex"], 0xA0E3AB7F, "Skirt", cloths);
            SetPlayerComponent(skin["sex"], 0x3107499B, "Chap", cloths);
            SetPlayerComponent(skin["sex"], 0x777EC6EF, "Boots", cloths);
            SetPlayerComponent(skin["sex"], 0x18729F39, "Spurs", cloths);
            SetPlayerComponent(skin["sex"], 0x514ADCEA, "Spats", cloths);
            SetPlayerComponent(skin["sex"], 0x91CE9B20, "Gauntlets", cloths);
            SetPlayerComponent(skin["sex"], 0x83887E88, "Loadouts", cloths);
            SetPlayerComponent(skin["sex"], 0x79D7DF96, "Accessories", cloths);
            SetPlayerComponent(skin["sex"], 0x94504D26, "Satchels", cloths);
            SetPlayerComponent(skin["sex"], 0xF1542D11, "GunbeltAccs", cloths);


            Function.Call((Hash)0xCC8CA3E88256E58F, playerPedId, 0, 1, 1, 1, false);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false); // this fix Hair not appears

            //Load Face Texture
            //CreateCharacter.toggleOverlayChange("eyebrows", int.Parse(skin["eyebrows_visibility"]), int.Parse(skin["eyebrows_tx_id"]), 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("scars", int.Parse(skin["scars_visibility"]), int.Parse(skin["scars_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("spots", int.Parse(skin["spots_visibility"]), int.Parse(skin["spots_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("disc", int.Parse(skin["disc_visibility"]), int.Parse(skin["disc_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("complex", int.Parse(skin["complex_visibility"]), int.Parse(skin["complex_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("acne", int.Parse(skin["acne_visibility"]), int.Parse(skin["acne_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("ageing", int.Parse(skin["ageing_visibility"]), int.Parse(skin["ageing_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("freckles", int.Parse(skin["freckles_visibility"]), int.Parse(skin["freckles_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("moles", int.Parse(skin["moles_visibility"]), int.Parse(skin["moles_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("grime", int.Parse(skin["grime_visibility"]), int.Parse(skin["grime_tx_id"]), 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("lipsticks", int.Parse(skin["lipsticks_visibility"]), int.Parse(skin["lipsticks_tx_id"]), 0, 0, 0, 1.0f, 0, int.Parse(skin["lipsticks_palette_id"]), int.Parse(skin["lipsticks_palette_color_primary"]), 0, 0, 0, 1.0f);
            //CreateCharacter.toggleOverlayChange("shadows", int.Parse(skin["shadows_visibility"]), int.Parse(skin["shadows_tx_id"]), 0, 0, 0, 1.0f, 0, int.Parse(skin["shadows_palette_id"]), int.Parse(skin["shadows_palette_color_primary"]), 0, 0, 0, 1.0f);

            await Delay(500);
            Function.Call((Hash)0x59BD177A1A48600A, playerPedId, 0xF8016BCA);
            Function.Call((Hash)0xD3A7B003ED343FD9, playerPedId, LoadPlayer.ConvertValue(skin["Beard"]), true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, playerPedId, 0, 1, 1, 1, false);

        }

        public static void SetPlayerComponent(string model, uint category, string component, Dictionary<string, uint> cloths)
        {
            if (model == "mp_male")
            {
                if (cloths[component] != -1)
                {
                    Function.Call((Hash)0x59BD177A1A48600A, playerPedId, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, playerPedId, cloths[component], true, true, false);
                }
            }
            else
            {
                Function.Call((Hash)0x59BD177A1A48600A, playerPedId, category);
                Function.Call((Hash)0xD3A7B003ED343FD9, playerPedId, cloths[component], true, true, true);
            }

            //Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
        }

        public static async Task DrawTxt3D(float x, float y, float z, string text)
        {
            float _x = 0.0F;
            float _y = 0.0F;
            //Debug.WriteLine(position.X.ToString());
            API.GetScreenCoordFromWorldCoord(x, y, z, ref _x, ref _y);
            API.SetTextScale(0.35F, 0.35F);
            API.SetTextFontForCurrentCommand(1);
            API.SetTextColor(255, 255, 255, 215);
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call((Hash)0xBE5261939FBECB8C, 1);
            Function.Call((Hash)0xD79334A4BB99BAD1, str, _x, _y);
            //float factor = text.Length / 150.0F;
            //Function.Call((Hash)0xC9884ECADE94CB34, "generic_textures", "hud_menu_4a", _x, _y + 0.0125F, 0.015F + factor, 0.03F, 0.1F, 100, 1, 1, 190, 0);
        }

    }
}
