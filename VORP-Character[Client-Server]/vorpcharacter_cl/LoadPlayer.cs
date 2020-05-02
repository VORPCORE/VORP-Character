using CitizenFX.Core;
using CitizenFX.Core.Native;
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
            EventHandlers["playerSpawned"] += new Action<object>(callToSetOutFit);
            EventHandlers["vorpcharacter:loadPlayerSkin"] += new Action<ExpandoObject, ExpandoObject>(loadPlayerSkin);

            /*CallBack*/ //
            EventHandlers["vorpcharacter:loadPlayerSkin"] += new Action<ExpandoObject, ExpandoObject>(loadPlayerSkin);
        }

        private async void loadPlayerSkin(ExpandoObject sskin, ExpandoObject scloth)
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

            await Delay(2000);


            await LoadAllComps(skin, cloths);

        }


        public async Task LoadAllComps(Dictionary<string, string> skin, Dictionary<string, uint> cloths)
        {
            int pID = API.PlayerId();

            uint model_hash = (uint)API.GetHashKey(skin["sex"]);
            await Utils.Miscellanea.LoadModel(model_hash);
            
            Function.Call((Hash)0xED40380076A31506, pID, model_hash, true);
            await Delay(2000);
            int pPedID = API.PlayerPedId();

            //LoadSkin
            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x84D6, float.Parse(skin["Head"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3303, float.Parse(skin["EyeBrowH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x2FF9, float.Parse(skin["EyeBrowW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x4AD1, float.Parse(skin["EyeBrowD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC04F, float.Parse(skin["EarsH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xB6CE, float.Parse(skin["EarsW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x2844, float.Parse(skin["EarsD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xED30, float.Parse(skin["EarsL"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x8B2B, float.Parse(skin["EyeLidH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1B6B, float.Parse(skin["EyeLidW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xEE44, float.Parse(skin["EyeD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xD266, float.Parse(skin["EyeAng"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xA54E, float.Parse(skin["EyeDis"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xDDFB, float.Parse(skin["EyeH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x6E7F, float.Parse(skin["NoseW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3471, float.Parse(skin["NoseS"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x03F5, float.Parse(skin["NoseH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x34B1, float.Parse(skin["NoseAng"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xF156, float.Parse(skin["NoseC"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x561E, float.Parse(skin["NoseDis"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x6A0B, float.Parse(skin["CheekBonesH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xABCF, float.Parse(skin["CheekBonesW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x358D, float.Parse(skin["CheekBonesD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xF065, float.Parse(skin["MouthW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xAA69, float.Parse(skin["MouthD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x7AC3, float.Parse(skin["MouthX"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x410D, float.Parse(skin["MouthY"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1A00, float.Parse(skin["ULiphH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x91C1, float.Parse(skin["ULiphW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC375, float.Parse(skin["ULiphD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xBB4D, float.Parse(skin["LLiphH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xB0B0, float.Parse(skin["LLiphW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x5D16, float.Parse(skin["LLiphD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x8D0A, float.Parse(skin["JawH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xEBAE, float.Parse(skin["JawW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x1DF6, float.Parse(skin["JawD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0x3C0F, float.Parse(skin["ChinH"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xC3B2, float.Parse(skin["ChinW"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x5653AB26C82938CF, pPedID, 0xE323, float.Parse(skin["ChinD"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);


            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, ConvertValue(skin["Hair"]), true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0xD3A7B003ED343FD9, pPedID, ConvertValue(skin["Beard"]), true, false, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
                        
            Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, ConvertValue(skin["Body"]));
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(50);
            

            Function.Call((Hash)0x1902C4CFCC5BE57C, pPedID, ConvertValue(skin["Waist"]));

            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            await Delay(1000);
            
            
            SetPlayerComponent(skin["sex"], 0x9925C067, "Hat", cloths);
            SetPlayerComponent(skin["sex"], 0x5E47CA6, "EyeWear", cloths);
            SetPlayerComponent(skin["sex"], 0x5FC29285, "NeckWear", cloths);
            SetPlayerComponent(skin["sex"], 0x7A96FACA, "NeckTies", cloths);
            SetPlayerComponent(skin["sex"], 0x2026C46D, "Shirt", cloths);
            SetPlayerComponent(skin["sex"], 0x877A2CF7, "Suspender", cloths);
            SetPlayerComponent(skin["sex"], 0x485EE834, "Vest", cloths);
            SetPlayerComponent(skin["sex"], 0xE06D30CE, "Coat", cloths);
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
            SetPlayerComponent(skin["sex"], 0x1D4C528A, "Pant", cloths);
            SetPlayerComponent(skin["sex"], 0xA0E3AB7F, "Skirt", cloths);
            SetPlayerComponent(skin["sex"], 0x3107499B, "Chap", cloths);
            SetPlayerComponent(skin["sex"], 0x777EC6EF, "Boots", cloths);
            SetPlayerComponent(skin["sex"], 0x18729F39, "Spurs", cloths);

            Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false);
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPedID, 0, 1, 1, 1, false); // this fix Hair not appears
            

        }

        public uint ConvertValue(string s)
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

        private void SetPlayerComponent(string model, uint category, string component, Dictionary<string, uint> cloths)
        {
            int pPID = API.PlayerPedId();
            if (model == "mp_male")
            {
                if(cloths[component] != -1)
                {
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, cloths[component], true, true, false);
                }
                    
                
            }
            else
            {
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, cloths[component], true, true, true);                
            }
            //Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
        }

        private void callToSetOutFit(object spawnInfo)
        {
            TriggerServerEvent("vorpcharacter:getPlayerSkin");
        }



    }
}
