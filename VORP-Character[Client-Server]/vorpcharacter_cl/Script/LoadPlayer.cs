using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using VorpCharacter.Diagnostics;
using VorpCharacter.Enums;
using VorpCharacter.Utils;
using static CitizenFX.Core.Native.API;

namespace VorpCharacter.Script
{
    public class LoadPlayer : BaseScript
    {

        public static Dictionary<string, string> cache_skin = new Dictionary<string, string>();
        public static Dictionary<string, uint> cache_cloths = new Dictionary<string, uint>();
        public static bool IsCurrentlyRunningSetup = false;

        internal LoadPlayer()
        {
            EventHandlers["vorpcharacter:loadPlayerSkin"] += new Action<string, string>(OnLoadPlayerSkin);

            EventHandlers["vorpcharacter:refreshPlayerSkin"] += new Action(ReloadCharacterSkin);
            EventHandlers["vorpcharacter:getPlayerComps"] += new Action<CallbackDelegate>(getPlayerComps);
            EventHandlers["vorpcharacter:reloadPlayerComps"] += new Action<ExpandoObject, ExpandoObject>(reloadPlayerComps);

            API.RegisterCommand("rc", new Action<int, List<object>, string>((source, args, raw) =>
            {
                bool isCuffed = Utilities.IsPedCuffed(Cache.PlayerPedId);
                bool isHogtied = Utilities.IsPedHogtied(Cache.PlayerPedId);
                
                if (isCuffed || isHogtied) return; // need notification

                ReloadCharacterSkin();
            }), false);

        }

        private async void ReloadCharacterSkin()
        {
            if (!API.IsPlayerDead(API.PlayerId())) // Fixed Revive
            {
                string skin = GetResourceKvpString2("skin");
                string clothes = GetResourceKvpString2("clothes");

                if (!string.IsNullOrEmpty(skin))
                {
                    cache_skin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skin);
                    Logger.Debug($"Loaded skin from resource store");
                }


                if (!string.IsNullOrEmpty(clothes))
                {
                    cache_cloths = JsonConvert.DeserializeObject<Dictionary<string, uint>>(clothes);
                    Logger.Debug($"Loaded clothes from resource store");
                }

                await SetupCharacter(true, cache_skin, cache_cloths, true);
            }
        }

        private void reloadPlayerComps(ExpandoObject sskin, ExpandoObject scloth)
        {
            Dictionary<string, string> skin = new Dictionary<string, string>();

            foreach (var s in sskin)
            {
                skin[s.Key] = s.Value.ToString();
            }

            Dictionary<string, uint> cloths = new Dictionary<string, uint>();

            foreach (var s in scloth)
            {
                cloths[s.Key] = ConvertValue(s.Value.ToString());
            }

            cache_skin = skin;
            cache_cloths = cloths;
        }

        public void getPlayerComps(CallbackDelegate cb)
        {
            cb.Invoke(cache_skin, cache_cloths);
        }

        public async void OnLoadPlayerSkin(string s_skin, string s_cloths) => await LoadPlayerSkin(s_skin, s_cloths);

        public async Task LoadPlayerSkin(string s_skin, string s_cloths)
        {
            JObject jSkin = JObject.Parse(s_skin);
            JObject jCloth = JObject.Parse(s_cloths);

            Dictionary<string, string> skin = new Dictionary<string, string>();

            foreach (var s in jSkin)
            {
                skin[s.Key] = s.Value.ToString();
            }

            Dictionary<string, uint> cloths = new Dictionary<string, uint>();

            foreach (var s in jCloth)
            {
                cloths[s.Key] = ConvertValue(s.Value.ToString());
            }

            cache_skin = skin;
            cache_cloths = cloths;

            await SetupCharacter(true, skin, cloths);
        }

        //[Tick]
        //public async Task SetScale()
        //{
        //    await Delay(5000);
        //    if (!cache_skin.ContainsKey("Scale"))
        //    {
        //        return;
        //    }
        //    else if (float.Parse(cache_skin["Scale"]) == 1.0f)
        //    {
        //        return;
        //    }

        //    await CreateCharacter.changeScale(float.Parse(cache_skin["Scale"]));
        //}

        public async Task<int> SetupCharacter(bool isPlayer, Dictionary<string, string> skin, Dictionary<string, uint> clothes, bool doFades = false, int delay = 0)
        {
            if (IsCurrentlyRunningSetup) return -1;
            IsCurrentlyRunningSetup = true;

            // handle a weird issue where things are sent through more than once
            if (skin.Count > 0)
                API.SetResourceKvp2("skin", JsonConvert.SerializeObject(skin));

            if (clothes.Count > 0)
                API.SetResourceKvp2("clothes", JsonConvert.SerializeObject(clothes));

            if (skin.Count == 0)
            {
                string skinKvp = GetResourceKvpString2("skin");
                if (!string.IsNullOrEmpty(skinKvp))
                {
                    cache_skin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skinKvp);
                    Logger.Debug($"Loaded skin from resource store");
                    skin = cache_skin;
                }
            }

            if (clothes.Count == 0)
            {
                string clothesKvp = GetResourceKvpString2("clothes");
                if (!string.IsNullOrEmpty(clothesKvp))
                {
                    cache_cloths = JsonConvert.DeserializeObject<Dictionary<string, uint>>(clothesKvp);
                    Logger.Debug($"Loaded clothes from resource store");
                    clothes = cache_cloths;
                }
            }

            if (doFades) await Utilities.FadeOutScreen(1000);

            Logger.Debug($"{JsonConvert.SerializeObject(skin)}");

            if (!skin.ContainsKey("sex"))
            {
                Logger.Error($"Information on selected character is null");
                return -1;
            }
            int pedHandle = -1;

            if (isPlayer) pedHandle = Cache.PlayerPedId;

            bool isMale = skin["sex"] == "mp_male";
            uint model_hash = (uint)eModel.mp_male;
            if (!isMale)
                model_hash = (uint)eModel.mp_female;

            await Utilities.RequestModel(model_hash);

            if (isPlayer)
            {
                pedHandle = await Utilities.SetPlayerModel(model_hash); // Model changes the players ped id
                TriggerServerEvent("syn_walkanim:getwalk");
            }

            if (!isPlayer)
            {
                pedHandle = API.CreatePed(model_hash, 1701.316f, 1512.134f, 146.87f, 116.70f, false, false, true, true);
            }

            Utilities.UpdatePedVariation(pedHandle);
            int pHealth = Utilities.GetAttributeCoreValue(pedHandle, eAttributeCore.Health);

            //PreLoad TextureFace
            CreateCharacter.texture_types["albedo"] = int.Parse(skin["albedo"]);
            CreateCharacter.texture_types["normal"] = isMale ? API.GetHashKey("mp_head_mr1_000_nm") : API.GetHashKey("head_fr1_mp_002_nm");
            CreateCharacter.texture_types["material"] = 0x7FC5B1E1;
            CreateCharacter.texture_types["color_type"] = 1;
            CreateCharacter.texture_types["texture_opacity"] = 1.0f;
            CreateCharacter.texture_types["unk_arg"] = 0;
            //End
            await Delay(0);
            CreateCharacter.ApplyDefaultSkinSettings(pedHandle);
            //LoadSkin
            await Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(skin["HeadType"]), delay: delay);
            await Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(skin["BodyType"]), delay: delay);
            await Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(skin["LegsType"]), delay: delay);

            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.FaceSize, float.Parse(skin["HeadSize"])); // FaceSize
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyebrowHeight, float.Parse(skin["EyeBrowH"])); // EyebrowHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyebrowWidth, float.Parse(skin["EyeBrowW"])); // EyebrowWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyebrowDepth, float.Parse(skin["EyeBrowD"])); // EyebrowDepth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsHeight, float.Parse(skin["EarsH"])); // EarsHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsAngle, float.Parse(skin["EarsW"])); // EarsAngle
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsSize, float.Parse(skin["EarsD"])); // EarsSize
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsLobeSize, float.Parse(skin["EarsL"])); // EarsLobeSize
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyelidsHeight, float.Parse(skin["EyeLidH"])); // EyelidsHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyelidsWidth, float.Parse(skin["EyeLidW"])); // EyelidsWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeDepth, float.Parse(skin["EyeD"])); // EyeDepth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeAngle, float.Parse(skin["EyeAng"])); // EyeAngle
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeSeparation, float.Parse(skin["EyeDis"])); // EyeSeparation
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeHeight, float.Parse(skin["EyeH"])); // EyeHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseWidth, float.Parse(skin["NoseW"])); // NoseWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseSize, float.Parse(skin["NoseS"])); // NoseSize
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseHeight, float.Parse(skin["NoseH"])); // NoseHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseAngle, float.Parse(skin["NoseAng"])); // NoseAngle
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseCurvature, float.Parse(skin["NoseC"])); // NoseCurvature
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NostrilsSeparation, float.Parse(skin["NoseDis"])); // NostrilsSeparation
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.CheekbonesHeight, float.Parse(skin["CheekBonesH"])); // CheekbonesHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.CheekbonesWidth, float.Parse(skin["CheekBonesW"])); // CheekbonesWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.CheekbonesDepth, float.Parse(skin["CheekBonesD"])); // CheekbonesDepth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthWidth, float.Parse(skin["MouthW"])); // MouthWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthDepth, float.Parse(skin["MouthD"])); // MouthDepth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthDeviation, float.Parse(skin["MouthX"])); // MouthDeviation
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthHeight, float.Parse(skin["MouthY"])); // MouthHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.UpperLipHeight, float.Parse(skin["ULiphH"])); // UpperLipHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.UpperLipWidth, float.Parse(skin["ULiphW"])); // UpperLipWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.UpperLipDepth, float.Parse(skin["ULiphD"])); // UpperLipDepth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.LowerLipHeight, float.Parse(skin["LLiphH"])); // LowerLipHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.LowerLipWidth, float.Parse(skin["LLiphW"])); // LowerLipWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.LowerLipDepth, float.Parse(skin["LLiphD"])); // LowerLipDepth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MandibleHeight, float.Parse(skin["JawH"])); // MandibleHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MandibleWidth, float.Parse(skin["JawW"])); // MandibleWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MandibleDepth, float.Parse(skin["JawD"])); // MandibleDepth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.ChinHeight, float.Parse(skin["ChinH"])); // ChinHeight
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.ChinWidth, float.Parse(skin["ChinW"])); // ChinWidth
            await Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.ChinDepth, float.Parse(skin["ChinD"]), true); // ChinDepth

            Utilities.SetPedBodyComponent(pedHandle, ConvertValue(skin["Body"]));
            Utilities.SetPedBodyComponent(pedHandle, ConvertValue(skin["Waist"]));

            Utilities.UpdatePedVariation(pedHandle);

            await BaseScript.Delay(100);

            //Load Face Texture
            CreateCharacter.toggleOverlayChange("eyebrows", int.Parse(skin["eyebrows_visibility"]), int.Parse(skin["eyebrows_tx_id"]), 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("scars", int.Parse(skin["scars_visibility"]), int.Parse(skin["scars_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("spots", int.Parse(skin["spots_visibility"]), int.Parse(skin["spots_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("disc", int.Parse(skin["disc_visibility"]), int.Parse(skin["disc_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("complex", int.Parse(skin["complex_visibility"]), int.Parse(skin["complex_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("acne", int.Parse(skin["acne_visibility"]), int.Parse(skin["acne_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("ageing", int.Parse(skin["ageing_visibility"]), int.Parse(skin["ageing_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("freckles", int.Parse(skin["freckles_visibility"]), int.Parse(skin["freckles_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("moles", int.Parse(skin["moles_visibility"]), int.Parse(skin["moles_tx_id"]), 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("grime", int.Parse(skin["grime_visibility"]), int.Parse(skin["grime_tx_id"]), 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("lipsticks", int.Parse(skin["lipsticks_visibility"]), int.Parse(skin["lipsticks_tx_id"]), 0, 0, 0, 1.0f, 0, int.Parse(skin["lipsticks_palette_id"]), int.Parse(skin["lipsticks_palette_color_primary"]), 0, 0, 0, 1.0f);
            CreateCharacter.toggleOverlayChange("shadows", int.Parse(skin["shadows_visibility"]), int.Parse(skin["shadows_tx_id"]), 0, 0, 0, 1.0f, 0, int.Parse(skin["shadows_palette_id"]), int.Parse(skin["shadows_palette_color_primary"]), 0, 0, 0, 1.0f);

            SetPlayerComponent(pedHandle, isMale, ePedComponent.Hat, "Hat", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.EyeWear, "EyeWear", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Mask, "Mask", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.NeckWear, "NeckWear", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Suspender, "Suspender", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Vest, "Vest", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Coat, "Coat", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.CoatClosed, "CoatClosed", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Shirt, "Shirt", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.NeckTies, "NeckTies", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Poncho, "Poncho", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Cloak, "Cloak", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Glove, "Glove", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.RingRh, "RingRh", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.RingLh, "RingLh", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Bracelet, "Bracelet", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Gunbelt, "Gunbelt", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Belt, "Belt", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Buckle, "Buckle", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Holster, "Holster", clothes);
            if (clothes["Skirt"] != -1) // Prevents both Pant & Skirt in female ped.
            {
                SetPlayerComponent(pedHandle, isMale, ePedComponent.Pant, "Pant", clothes);
            }
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Skirt, "Skirt", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Chap, "Chap", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Boots, "Boots", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Spurs, "Spurs", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Spats, "Spats", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Gauntlets, "Gauntlets", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Loadouts, "Loadouts", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Accessories, "Accessories", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.Satchels, "Satchels", clothes);
            SetPlayerComponent(pedHandle, isMale, ePedComponent.GunbeltAccs, "GunbeltAccs", clothes);

            Utilities.UpdatePedVariation(pedHandle);

            await BaseScript.Delay(100);

            Function.Call((Hash)0x59BD177A1A48600A, pedHandle, 0xF8016BCA);
            await Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(skin["Eyes"]));
            await Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(skin["Beard"]));
            await Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(skin["Hair"]));
            Utilities.UpdatePedVariation(pedHandle);


            Utilities.SetAttributeCoreValue(pedHandle, (int)eAttributeCore.Health, pHealth);

            ResetEntityAlpha(pedHandle);

            float pedScale = 1f;
            float.TryParse(skin["Scale"], out pedScale);
            await Utilities.SetPedScale(pedHandle, pedScale);

            if (doFades) await Utilities.FadeInScreen(1000);

            IsCurrentlyRunningSetup = false;

            await IsLoaded(skin, clothes);

            return pedHandle;
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

        // what does this do really?
        public static async void SetPlayerComponent(int pedHandle, bool isMale, ePedComponent pedComponent, string component, Dictionary<string, uint> cloths)
        {
            if (isMale)
            {
                if (cloths[component] != -1)
                {
                    Function.Call((Hash)0x59BD177A1A48600A, pedHandle, (uint)pedComponent);
                    await Utilities.ApplyShopItemToPed(pedHandle, cloths[component], true, true, false);
                }
            }
            else
            {
                Function.Call((Hash)0x59BD177A1A48600A, pedHandle, (uint)pedComponent);
                await Utilities.ApplyShopItemToPed(pedHandle, cloths[component], true, true, true);
            }
        }

        private async Task IsLoaded(Dictionary<string, string> skin, Dictionary<string, uint> clothes)
        {
            await Delay(1500);
            bool loaded = Utilities.IsPedReadyToRender(Cache.PlayerPedId);
            if (!loaded)
            {
                Logger.Debug($"Ped is not ready to render");
                // maybe done because of some other delay?
                await SetupCharacter(true, skin, clothes);
            }
        }

    }
}
