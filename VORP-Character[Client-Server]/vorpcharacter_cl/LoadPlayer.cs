using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcharacter_cl
{
    public class LoadPlayer : BaseScript
    {
        public LoadPlayer()
        {
            EventHandlers["vorpcharacter:loadPlayerSkin"] += new Action<string, string>(loadPlayerSkin);
            EventHandlers["vorpcharacter:refreshPlayerSkin"] += new Action(RefreshPlayerSkin);
            EventHandlers["vorpcharacter:getPlayerComps"] += new Action<CallbackDelegate>(getPlayerComps);
            EventHandlers["vorpcharacter:reloadPlayerComps"] += new Action<ExpandoObject, ExpandoObject>(reloadPlayerComps);
            Tick += SetScale;

            API.RegisterCommand("rc", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (!API.IsPlayerDead(API.PlayerId())) // Fixed Revive
                {
                    LoadAllComps(cache_skin, cache_cloths);
                } 
            }), false);

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

        public static Dictionary<string, string> cache_skin = new Dictionary<string, string>();
        public static Dictionary<string, uint> cache_cloths = new Dictionary<string, uint>();

        public void RefreshPlayerSkin()
        {
            LoadAllComps(cache_skin, cache_cloths);
        }

        public void getPlayerComps(CallbackDelegate cb)
        {
            cb.Invoke(cache_skin, cache_cloths);
        }

        private async void loadPlayerSkin(string s_skin, string s_cloths)
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

            await Delay(2000);

            cache_skin = skin;
            cache_cloths = cloths;

            await LoadAllComps(skin, cloths);

        }

        [Tick]
        public async Task SetScale()
        {
            await Delay(5000);
            if (!cache_skin.ContainsKey("Scale"))
            {
                return;
            }
            else if (float.Parse(cache_skin["Scale"]) == 1.0f)
            {
                return;
            }

            await CreateCharacter.changeScale(float.Parse(cache_skin["Scale"]));
        }

        public async Task LoadAllComps(Dictionary<string, string> skin, Dictionary<string, uint> cloths)
        {
            int pHealth = Function.Call<int>((Hash)0x36731AC041289BB1, API.PlayerPedId(), 0);
            int pID = API.PlayerId();

            uint model_hash = (uint)API.GetHashKey(skin["sex"]);
            await Utils.Miscellanea.LoadModel(model_hash);
            await Delay(250);
            Function.Call((Hash)0xED40380076A31506, pID, model_hash, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
            await Delay(2000);
            int pPedID = API.PlayerPedId();
            TriggerServerEvent("syn_walkanim:getwalk");
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
            CreateCharacter.ApplyDefaultSkinCanaryEdition(API.PlayerPedId());
            //LoadSkin
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), ConvertValue(skin["HeadType"]), true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), ConvertValue(skin["BodyType"]), true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), ConvertValue(skin["LegsType"]), true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x84D6, float.Parse(skin["HeadSize"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3303, float.Parse(skin["EyeBrowH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x2FF9, float.Parse(skin["EyeBrowW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x4AD1, float.Parse(skin["EyeBrowD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC04F, float.Parse(skin["EarsH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xB6CE, float.Parse(skin["EarsW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x2844, float.Parse(skin["EarsD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xED30, float.Parse(skin["EarsL"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x8B2B, float.Parse(skin["EyeLidH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1B6B, float.Parse(skin["EyeLidW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xEE44, float.Parse(skin["EyeD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xD266, float.Parse(skin["EyeAng"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xA54E, float.Parse(skin["EyeDis"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xDDFB, float.Parse(skin["EyeH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x6E7F, float.Parse(skin["NoseW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3471, float.Parse(skin["NoseS"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x03F5, float.Parse(skin["NoseH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x34B1, float.Parse(skin["NoseAng"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xF156, float.Parse(skin["NoseC"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x561E, float.Parse(skin["NoseDis"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x6A0B, float.Parse(skin["CheekBonesH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xABCF, float.Parse(skin["CheekBonesW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x358D, float.Parse(skin["CheekBonesD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xF065, float.Parse(skin["MouthW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xAA69, float.Parse(skin["MouthD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x7AC3, float.Parse(skin["MouthX"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x410D, float.Parse(skin["MouthY"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1A00, float.Parse(skin["ULiphH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x91C1, float.Parse(skin["ULiphW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC375, float.Parse(skin["ULiphD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xBB4D, float.Parse(skin["LLiphH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xB0B0, float.Parse(skin["LLiphW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x5D16, float.Parse(skin["LLiphD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x8D0A, float.Parse(skin["JawH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xEBAE, float.Parse(skin["JawW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1DF6, float.Parse(skin["JawD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3C0F, float.Parse(skin["ChinH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC3B2, float.Parse(skin["ChinW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xE323, float.Parse(skin["ChinD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);

            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, ConvertValue(skin["Eyes"]), true, true, true);
            await Delay(100);

            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, ConvertValue(skin["Hair"]), true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            
                        
            Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, ConvertValue(skin["Body"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(100);
            

            Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, ConvertValue(skin["Waist"]));

            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(500);
            
            
            SetPlayerComponent(skin["sex"], 0x9925C067, "Hat", cloths);
            SetPlayerComponent(skin["sex"], 0x5E47CA6, "EyeWear", cloths);
            SetPlayerComponent(skin["sex"], 0x7505EF42, "Mask", cloths);
            SetPlayerComponent(skin["sex"], 0x5FC29285, "NeckWear", cloths);
            SetPlayerComponent(skin["sex"], 0x7A96FACA, "NeckTies", cloths);
            SetPlayerComponent(skin["sex"], 0x2026C46D, "Shirt", cloths);
            SetPlayerComponent(skin["sex"], 0x877A2CF7, "Suspender", cloths);
            SetPlayerComponent(skin["sex"], 0x485EE834, "Vest", cloths);
            SetPlayerComponent(skin["sex"], 0xE06D30CE, "Coat", cloths);
            SetPlayerComponent(skin["sex"], 0x0662AC34, "CoatClosed", cloths);
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

            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);

            //Load Face Texture
            await Delay(500);
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
            
            await Delay(4000);
            Function.Call((Hash)0x59BD177A1A48600A, pPedID, 0xF8016BCA);
            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, ConvertValue(skin["Beard"]), true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);

            Function.Call((Hash)0xC6258F41D86676E0, pPedID, 0, pHealth);

            IsLoaded();
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
                result = (uint) interesante;
                return result;
            }
        }

        public static void SetPlayerComponent(string model, uint category, string component, Dictionary<string, uint> cloths)
        {
            int pPID = API.PlayerPedId();
            if (model == "mp_male")
            {
                if(cloths[component] != -1)
                {
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, cloths[component], true, false, false);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, cloths[component], true, true, false);
                }
            }
            else
            {
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, cloths[component], true, false, true);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, cloths[component], true, true, true);
            }

            //Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
        }

        private async Task IsLoaded()
        {
            await Delay(1500);
            bool loaded = Function.Call<bool>((Hash)0xA0BC8FAED8CFEB3C, API.PlayerPedId());
            if (!loaded)
            {
                LoadAllComps(cache_skin, cache_cloths);
            }
        }

    }
}
