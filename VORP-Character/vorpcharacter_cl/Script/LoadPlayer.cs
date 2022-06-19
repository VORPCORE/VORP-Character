using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using VorpCharacter.Diagnostics;
using VorpCharacter.Enums;
using VorpCharacter.Utils;
using static CitizenFX.Core.Native.API;

namespace VorpCharacter.Script
{
    public class LoadPlayer : BaseScript
    {
        public static LoadPlayer Instance { get; private set; }
        public static Dictionary<string, string> cache_skin = new Dictionary<string, string>();
        public static Dictionary<string, uint> cache_cloths = new Dictionary<string, uint>();
        public static bool IsCurrentlyRunningSetup = false;

        public LoadPlayer()
        {
            Instance = this;

            EventHandlers["vorpcharacter:loadPlayerSkin"] += new Action<string, string>(OnLoadPlayerSkin);

            EventHandlers["vorpcharacter:refreshPlayerSkin"] += new Action<string>(ReloadCharacterSkin);
            EventHandlers["vorpcharacter:getPlayerComps"] += new Action<CallbackDelegate>(getPlayerComps);
            EventHandlers["vorpcharacter:reloadPlayerComps"] += new Action<ExpandoObject, ExpandoObject>(reloadPlayerComps);

#if DEVELOPMENT
            RegisterCommand("cc_unstuck", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsEntityPositionFrozen(Cache.PlayerPedId))
                    FreezeEntityPosition(Cache.PlayerPedId, false);

            }), false);
#endif

            API.RegisterCommand("rc", new Action<int, List<object>, string>((source, args, raw) =>
            {
                bool isDead = API.IsPlayerDead(API.PlayerId());

                int pHealth = Utilities.GetAttributeCoreValue(API.PlayerPedId(), eAttributeCore.Health);
                int pStamina = Utilities.GetAttributeCoreValue(API.PlayerPedId(), eAttributeCore.Stamina);

                if (isDead) return; // need notification

                bool isCuffed = Utilities.IsPedCuffed(Cache.PlayerPedId);
                bool isHogtied = Utilities.IsPedHogtied(Cache.PlayerPedId);

                if (isCuffed || isHogtied) return; // need notification

                if (args.Count == 0)
                {
                    ReloadCharacterSkin(string.Empty);
                
                    Utilities.SetAttributeCoreValue(API.PlayerPedId(), eAttributeCore.Health, pHealth);
                    Utilities.SetAttributeCoreValue(API.PlayerPedId(), eAttributeCore.Stamina, pStamina);
                    
                    return;
                }

                ReloadCharacterSkin($"{args[0]}");
                
                Utilities.SetAttributeCoreValue(API.PlayerPedId(), eAttributeCore.Health, pHealth);
                Utilities.SetAttributeCoreValue(API.PlayerPedId(), eAttributeCore.Stamina, pStamina);
                

            }), false);

            Logger.Info($"VORP Character LoadPlayer");

        }

        private async void ReloadCharacterSkin(string part = "")
        {
            if (!API.IsPlayerDead(API.PlayerId())) // Fixed Revive
            {
                string skin = GetResourceKvpString2("skin");
                string clothes = GetResourceKvpString2("clothes");
                bool isMale = false;

                if (!string.IsNullOrEmpty(skin))
                {
                    cache_skin = JsonConvert.DeserializeObject<Dictionary<string, string>>(skin);
                    if (cache_skin.ContainsKey("sex"))
                        isMale = cache_skin["sex"] == "mp_male";
#if DEVELOPMENT
                    Logger.Debug($"Loaded skin from resource store");
#endif
                }


                if (!string.IsNullOrEmpty(clothes))
                {
                    cache_cloths = JsonConvert.DeserializeObject<Dictionary<string, uint>>(clothes);
#if DEVELOPMENT
                    Logger.Debug($"Loaded clothes from resource store");
#endif
                }

                switch (part)
                {
                    case "clothes":
                        SetPedComponents(cache_cloths, Cache.PlayerPedId);
                        Utilities.UpdatePedVariation(Cache.PlayerPedId, true);
                        await BaseScript.Delay(0);
                        SetPedComponents(cache_cloths, Cache.PlayerPedId);
                        Utilities.UpdatePedVariation(Cache.PlayerPedId, true);
                        break;
                    default:
                        await SetupCharacter(true, cache_skin, cache_cloths);
                        break;
                }
            }
        }

        private async void reloadPlayerComps(ExpandoObject sskin, ExpandoObject scloth)
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

            await SetupCharacter(true, cache_skin, cache_cloths);
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

        public static async void ApplyDefaultSkinSettings(int pedHandle)
        {
            Utilities.SetPedOutfitPreset(pedHandle, 0);

            while (!Utilities.IsPedReadyToRender(pedHandle))
            {
                await Delay(0);
            }

            Function.Call((Hash)0x0BFA1BD465CDFEFD, pedHandle);

            uint compEyes = SkinsUtils.EYES_MALE.ElementAt(0);
            uint compBody = Convert.ToUInt32($"0x{PluginManager.Config.Male[0].Body[0]}", 16);
            uint compHead = Convert.ToUInt32($"0x{PluginManager.Config.Male[0].Heads[0]}", 16);
            uint compLegs = Convert.ToUInt32($"0x{PluginManager.Config.Male[0].Legs[0]}", 16);

            if (!IsPedMale(pedHandle))
            {
                compEyes = SkinsUtils.EYES_FEMALE.ElementAt(0);
                compBody = Convert.ToUInt32($"0x{PluginManager.Config.Female[0].Body[0]}", 16);
                compHead = Convert.ToUInt32($"0x{PluginManager.Config.Female[0].Heads[0]}", 16);
                compLegs = Convert.ToUInt32($"0x{PluginManager.Config.Female[0].Legs[0]}", 16);
            }

            Utilities.ApplyShopItemToPed(pedHandle, compHead);
            Utilities.ApplyShopItemToPed(pedHandle, compEyes);
            Utilities.ApplyShopItemToPed(pedHandle, compBody);
            Utilities.ApplyShopItemToPed(pedHandle, compLegs);

            Utilities.RemoveTagFromMetaPed(pedHandle, 0x3F1F01E5, 0);
            Utilities.RemoveTagFromMetaPed(pedHandle, 0x1D4C528A, 0);
            Utilities.RemoveTagFromMetaPed(pedHandle, 0xDA0E2C55, 0);
            Utilities.UpdatePedVariation(pedHandle);
        }

        public async Task<int> SetupCharacter(bool isPlayer, Dictionary<string, string> skin, Dictionary<string, uint> clothes)
        {
            try
            {
                if (IsCurrentlyRunningSetup) return -1;
                IsCurrentlyRunningSetup = true;

                if (skin.Count > 0)
                    API.SetResourceKvp2("skin", JsonConvert.SerializeObject(skin));

                if (clothes.Count > 0)
                    API.SetResourceKvp2("clothes", JsonConvert.SerializeObject(clothes));

                if (!skin.ContainsKey("sex"))
                {
                    Logger.Error($"Information on selected character is null");
                    return -1;
                }

                int pedHandle = -1;
                if (isPlayer) pedHandle = Cache.PlayerPedId;
                string sex = GetKeyValue(skin, "sex");
                bool isMale = sex == "mp_male";

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
                    pedHandle = API.CreatePed(model_hash, 1701.316f, 1512.134f, 146.87f, 116.70f, false, true, true, true);
                }

                while (!Function.Call<bool>((Hash)0xA0BC8FAED8CFEB3C, pedHandle)) // IsPedReadyToRender
                {
                    await BaseScript.Delay(0);
                }

                Function.Call((Hash)0x0BFA1BD465CDFEFD, pedHandle);
                Utilities.UpdatePedVariation(pedHandle, true, true);

                // loop this twice, why, cause the first time round it doesn't apply correctly
                for (var i = 0; i < 2; i++)
                {
                    PreloadPedTextures(skin, isMale);
                    await BaseScript.Delay(0);
                    ApplyDefaultSkinSettings(pedHandle);
                    await BaseScript.Delay(0);
                    SetupPedBodyTypes(pedHandle, skin);
                    await BaseScript.Delay(0);
                    Utilities.RemoveTagFromMetaPed(pedHandle, 0x1D4C528A, 0);
                    Utilities.RemoveTagFromMetaPed(pedHandle, 0x3F1F01E5, 0);
                    Utilities.RemoveTagFromMetaPed(pedHandle, 0xDA0E2C55, 0);
                    await BaseScript.Delay(0);
                    SetupPedFaceFeatures(pedHandle, skin);
                    await BaseScript.Delay(0);
                    SetPedBodyComponents(pedHandle, skin);
                    await BaseScript.Delay(0);
                    SetPedFaceTextures(pedHandle, skin);
                    await BaseScript.Delay(0);
                    SetupPedAdditionalFaceFeatures(pedHandle, skin);
                    await BaseScript.Delay(0);
                    SetPedComponents(clothes, pedHandle);
                    await BaseScript.Delay(100);
                    SetPedComponents(clothes, pedHandle);
                }

                ResetEntityAlpha(pedHandle);

                float pedScale = 1f;
                float.TryParse(skin["Scale"], out pedScale);
                await Utilities.SetPedScale(pedHandle, pedScale);

                IsCurrentlyRunningSetup = false;

                await IsLoaded(skin, clothes);

                return pedHandle;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"SetupCharacter");
                return -1;
            }
        }

        private static void PreloadPedTextures(Dictionary<string, string> skin, bool isMale)
        {
            // How this preloads, I don't know, need to look into it, combining LoadPlayer and CreateCharacter would be a good start.
            string albedo = GetKeyValue(skin, "albedo");
            int.TryParse(albedo, out int iAlbedo);
            CreateCharacter.texture_types["albedo"] = iAlbedo;
            CreateCharacter.texture_types["normal"] = isMale ? API.GetHashKey("mp_head_mr1_000_nm") : API.GetHashKey("head_fr1_mp_002_nm");
            CreateCharacter.texture_types["material"] = 0x7FC5B1E1;
            CreateCharacter.texture_types["color_type"] = 1;
            CreateCharacter.texture_types["texture_opacity"] = 1.0f;
            CreateCharacter.texture_types["unk_arg"] = 0;
        }

        private static void SetPedBodyComponents(int pedHandle, Dictionary<string, string> skin)
        {
            string bodyValue = GetKeyValue(skin, "Body");
            string waistValue = GetKeyValue(skin, "Waist");
            Utilities.SetPedBodyComponent(pedHandle, ConvertValue(bodyValue));
            Utilities.SetPedBodyComponent(pedHandle, ConvertValue(waistValue));
            Utilities.UpdatePedVariation(pedHandle);
        }

        private static void SetupPedAdditionalFaceFeatures(int pedHandle, Dictionary<string, string> skin)
        {
            string eyesValue = GetKeyValue(skin, "Eyes");
            string beardValue = GetKeyValue(skin, "Beard");
            string hairValue = GetKeyValue(skin, "Hair");

            Function.Call((Hash)0x59BD177A1A48600A, pedHandle, 0xF8016BCA);
            Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(eyesValue));
            Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(beardValue));
            Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(hairValue));
            Utilities.UpdatePedVariation(pedHandle);
        }

        private static void SetupPedBodyTypes(int pedHandle, Dictionary<string, string> skin)
        {
            string headType = GetKeyValue(skin, "HeadType");
            string bodyType = GetKeyValue(skin, "BodyType");
            string legsType = GetKeyValue(skin, "LegsType");

            Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(headType));
            Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(bodyType));
            Utilities.ApplyShopItemToPed(pedHandle, ConvertValue(legsType));
            Utilities.UpdatePedVariation(pedHandle);
        }

        private static void SetupPedFaceFeatures(int pedHandle, Dictionary<string, string> skin)
        {
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.FaceSize, skin, "HeadSize"); // FaceSize
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyebrowHeight, skin, "EyeBrowH"); // EyebrowHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyebrowWidth, skin, "EyeBrowW"); // EyebrowWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyebrowDepth, skin, "EyeBrowD"); // EyebrowDepth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsHeight, skin, "EarsH"); // EarsHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsAngle, skin, "EarsW"); // EarsAngle
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsSize, skin, "EarsD"); // EarsSize
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EarsLobeSize, skin, "EarsL"); // EarsLobeSize
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyelidsHeight, skin, "EyeLidH"); // EyelidsHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyelidsWidth, skin, "EyeLidW"); // EyelidsWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeDepth, skin, "EyeD"); // EyeDepth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeAngle, skin, "EyeAng"); // EyeAngle
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeSeparation, skin, "EyeDis"); // EyeSeparation
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.EyeHeight, skin, "EyeH"); // EyeHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseWidth, skin, "NoseW"); // NoseWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseSize, skin, "NoseS"); // NoseSize
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseHeight, skin, "NoseH"); // NoseHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseAngle, skin, "NoseAng"); // NoseAngle
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NoseCurvature, skin, "NoseC"); // NoseCurvature
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.NostrilsSeparation, skin, "NoseDis"); // NostrilsSeparation
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.CheekbonesHeight, skin, "CheekBonesH"); // CheekbonesHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.CheekbonesWidth, skin, "CheekBonesW"); // CheekbonesWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.CheekbonesDepth, skin, "CheekBonesD"); // CheekbonesDepth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthWidth, skin, "MouthW"); // MouthWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthDepth, skin, "MouthD"); // MouthDepth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthDeviation, skin, "MouthX"); // MouthDeviation
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MouthHeight, skin, "MouthY"); // MouthHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.UpperLipHeight, skin, "ULiphH"); // UpperLipHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.UpperLipWidth, skin, "ULiphW"); // UpperLipWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.UpperLipDepth, skin, "ULiphD"); // UpperLipDepth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.LowerLipHeight, skin, "LLiphH"); // LowerLipHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.LowerLipWidth, skin, "LLiphW"); // LowerLipWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.LowerLipDepth, skin, "LLiphD"); // LowerLipDepth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MandibleHeight, skin, "JawH"); // MandibleHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MandibleWidth, skin, "JawW"); // MandibleWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.MandibleDepth, skin, "JawD"); // MandibleDepth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.ChinHeight, skin, "ChinH"); // ChinHeight
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.ChinWidth, skin, "ChinW"); // ChinWidth
            Utilities.SetPedFaceFeature(pedHandle, ePedFaceFeature.ChinDepth, skin, "ChinD", true); // ChinDepth
            Utilities.UpdatePedVariation(pedHandle, true);
        }

        private static void SetPedFaceTextures(int pedHandle, Dictionary<string, string> skin)
        {
            // if you're looking at this, and saying "WTF is this..." welcome to to supporting a legacy model before refactoring to classes, yes, I don't like this either!
            // also can see that another resource has added this information into another column, so which one is the correct one?! what a mess.
            // Character system is better than it was, for what it was, but, for it to add the new data, the 3 columns need to become one class object.

            string strEyebrowVisibility = GetKeyValue(skin, "eyebrows_visibility");
            string strEyebrowTxId = GetKeyValue(skin, "eyebrows_tx_id");
            string strScarsVisibility = GetKeyValue(skin, "scars_visibility");
            string strScarsTxId = GetKeyValue(skin, "scars_tx_id");
            string strSpotsVisibility = GetKeyValue(skin, "spots_visibility");
            string strSpotsTxId = GetKeyValue(skin, "spots_tx_id");
            string strDiscVisibility = GetKeyValue(skin, "disc_visibility");
            string strDiscTxId = GetKeyValue(skin, "disc_tx_id");
            string strComplexionVisibility = GetKeyValue(skin, "complex_visibility");
            string strComplexionTxId = GetKeyValue(skin, "complex_tx_id");
            string strAcneVisibility = GetKeyValue(skin, "acne_visibility");
            string strAcneTxId = GetKeyValue(skin, "acne_tx_id");
            string strAgingVisibility = GetKeyValue(skin, "ageing_visibility");
            string strAgingTxId = GetKeyValue(skin, "ageing_tx_id");
            string strFrecklesVisibility = GetKeyValue(skin, "freckles_visibility");
            string strFrecklesTxId = GetKeyValue(skin, "freckles_tx_id");
            string strMolesVisibility = GetKeyValue(skin, "moles_visibility");
            string strMolesTxId = GetKeyValue(skin, "moles_tx_id");
            string strGrimeVisibility = GetKeyValue(skin, "grime_visibility");
            string strGrimeTxId = GetKeyValue(skin, "grime_tx_id");
            string strLipstickVisibility = GetKeyValue(skin, "lipsticks_visibility");
            string strLipstickTxId = GetKeyValue(skin, "lipsticks_tx_id");
            string strLipstickPaletteId = GetKeyValue(skin, "lipsticks_palette_id");
            string strLipstickColorPrimary = GetKeyValue(skin, "lipsticks_palette_color_primary");
            string strShadowsVisibility = GetKeyValue(skin, "shadows_visibility");
            string strShadowsTxId = GetKeyValue(skin, "shadows_tx_id");
            string strShadowsPaletteId = GetKeyValue(skin, "shadows_palette_id");
            string strShadowsColorPrimary = GetKeyValue(skin, "shadows_palette_color_primary");

            int iEyebrowVisibility = 0;
            int iEyebrowTxId = 0;
            int iScarsVisibility = 0;
            int iScarsTxId = 0;
            int iSpotsVisibility = 0;
            int iSpotsTxId = 0;
            int iDiscVisibility = 0;
            int iDiscTxId = 0;
            int iComplexionVisibility = 0;
            int iComplexionTxId = 0;
            int iAcneVisibility = 0;
            int iAcneTxId = 0;
            int iAgingVisibility = 0;
            int iAgingTxId = 0;
            int iFrecklesVisibility = 0;
            int iFrecklesTxId = 0;
            int iMolesVisibility = 0;
            int iMolesTxId = 0;
            int iGrimeVisibility = 0;
            int iGrimeTxId = 0;
            int iLipstickVisibility = 0;
            int iLipstickTxId = 0;
            int iLipstickPaletteId = 0;
            int iLipstickColorPrimary = 0;
            int iShadowsVisibility = 0;
            int iShadowsTxId = 0;
            int iShadowsPaletteId = 0;
            int iShadowsColorPrimary = 0;

            int.TryParse(strEyebrowVisibility, out iEyebrowVisibility);
            int.TryParse(strEyebrowTxId, out iEyebrowTxId);
            int.TryParse(strScarsVisibility, out iScarsVisibility);
            int.TryParse(strScarsTxId, out iScarsTxId);
            int.TryParse(strSpotsVisibility, out iSpotsVisibility);
            int.TryParse(strSpotsTxId, out iSpotsTxId);
            int.TryParse(strDiscVisibility, out iDiscVisibility);
            int.TryParse(strDiscTxId, out iDiscTxId);
            int.TryParse(strComplexionVisibility, out iComplexionVisibility);
            int.TryParse(strComplexionTxId, out iComplexionTxId);
            int.TryParse(strAcneVisibility, out iAcneVisibility);
            int.TryParse(strAcneTxId, out iAcneTxId);
            int.TryParse(strAgingVisibility, out iAgingVisibility);
            int.TryParse(strAgingTxId, out iAgingTxId);
            int.TryParse(strFrecklesVisibility, out iFrecklesVisibility);
            int.TryParse(strFrecklesTxId, out iFrecklesTxId);
            int.TryParse(strMolesVisibility, out iMolesVisibility);
            int.TryParse(strMolesTxId, out iMolesTxId);
            int.TryParse(strGrimeVisibility, out iGrimeVisibility);
            int.TryParse(strGrimeTxId, out iGrimeTxId);
            int.TryParse(strLipstickVisibility, out iLipstickVisibility);
            int.TryParse(strLipstickTxId, out iLipstickTxId);
            int.TryParse(strLipstickPaletteId, out iLipstickPaletteId);
            int.TryParse(strLipstickColorPrimary, out iLipstickColorPrimary);
            int.TryParse(strShadowsVisibility, out iShadowsVisibility);
            int.TryParse(strShadowsTxId, out iShadowsTxId);
            int.TryParse(strShadowsPaletteId, out iShadowsPaletteId);
            int.TryParse(strShadowsColorPrimary, out iShadowsColorPrimary);

            // should plan to add opacity sliders into the menu and character class, when its a class!
            CreateCharacter.ToggleOverlayChange("eyebrows", iEyebrowVisibility, iEyebrowTxId);
            CreateCharacter.ToggleOverlayChange("scars", iScarsVisibility, iScarsTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("spots", iSpotsVisibility, iSpotsTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("disc", iDiscVisibility, iDiscTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("complex", iComplexionVisibility, iComplexionTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("acne", iAcneVisibility, iAcneTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("ageing", iAgingVisibility, iAgingTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("freckles", iFrecklesVisibility, iFrecklesTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("moles", iMolesVisibility, iMolesTxId, tx_color_type: 1);
            CreateCharacter.ToggleOverlayChange("grime", iGrimeVisibility, iGrimeTxId);
            CreateCharacter.ToggleOverlayChange("lipsticks", iLipstickVisibility, iLipstickTxId, palette_id: iLipstickPaletteId, palette_color_primary: iLipstickColorPrimary);
            CreateCharacter.ToggleOverlayChange("shadows", iShadowsVisibility, iShadowsTxId, palette_id: iShadowsPaletteId, palette_color_primary: iShadowsColorPrimary);
            Utilities.UpdatePedVariation(pedHandle, true);
        }

        public static void SetPedComponents(Dictionary<string, uint> clothes, int pedHandle)
        {
            SetPlayerComponent(pedHandle, ePedComponent.Hats, "Hat", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.EyeWear, "EyeWear", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Masks, "Mask", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Neckwear, "NeckWear", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Suspenders, "Suspender", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Vests, "Vest", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Coats, "Coat", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.CoatsClosed, "CoatClosed", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.ShirtsFull, "Shirt", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Neckties, "NeckTies", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Ponchos, "Poncho", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Cloaks, "Cloak", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Gloves, "Glove", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.JewelryRingsRight, "RingRh", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.JewelryRingsLeft, "RingLh", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.JewelryBracelets, "Bracelet", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Gunbelts, "Gunbelt", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Belts, "Belt", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.BeltBuckles, "Buckle", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.HolstersLeft, "Holster", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Pants, "Pant", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Bow, "bow", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Armor, "armor", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Teeth, "teeth", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Chaps, "Chap", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Boots, "Boots", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Spurs, "Spurs", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Spats, "Spats", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Gauntlets, "Gauntlets", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Loadouts, "Loadouts", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Accessories, "Accessories", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.Satchels, "Satchels", clothes);
            SetPlayerComponent(pedHandle, ePedComponent.GunbeltAccessories, "GunbeltAccs", clothes);
            Utilities.UpdatePedVariation(pedHandle, true);
        }

        private static string GetKeyValue(Dictionary<string, string> skin, string key)
        {
            string keyValue = string.Empty;
            if (!skin.TryGetValue(key, out keyValue)) Logger.Error($"Skin: key '{key}' is missing.");
            return keyValue;
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
        public static void SetPlayerComponent(int pedHandle, ePedComponent pedComponent, string component, Dictionary<string, uint> clothes)
        {
            if (!clothes.ContainsKey(component)) return;

            // if they have Pant and Skirt set, only the Pant will apply, Skirt is to be ignored
            if (clothes.ContainsKey("Pant") && clothes.ContainsKey("Skirt"))
            {
                if (clothes["Skirt"] > 0 && clothes["Pant"] > 0 && component == "Skirt") return;
            }

            Utilities.SetComponent(pedHandle, pedComponent, clothes[component]);
        }

        private async Task IsLoaded(Dictionary<string, string> skin, Dictionary<string, uint> clothes)
        {
            await Delay(1500);
            bool loaded = Utilities.IsPedReadyToRender(Cache.PlayerPedId);
            if (!loaded)
            {
#if DEVELOPMENT
                Logger.Debug($"Ped is not ready to render");
#endif
                // maybe done because of some other delay?
                await SetupCharacter(true, skin, clothes);
            }
        }

    }
}
