using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vorpcharacter_cl.Utils;

namespace vorpcharacter_cl
{
    public class CreatePlayer : BaseScript
    {
        bool isSelectSexActive = false;
        public static bool isInCharCreation = false;
        public static string model_selected;
        string model_f = "mp_female";
        string model_m = "mp_male";
        int PedFemale;
        int PedMale;
        static int Camera;
        static int Camera_Male;
        static int Camera_Female;
        static int Camera_Editor;
        static int Camera_FaceEditor;
        static int Camera_Waist;
        static int Camera_Legs;
        static int Camera_Body;
        static int Camera_Back;
        float DressHeading = 93.2f;

        //Para guardar en DB
        public static Dictionary<string, object> skinPlayer = new Dictionary<string, object>() {
            { "sex", "none" },

            { "HeadType", 0 }, 
            { "BodyType", 0 },
            { "LegsType", 0 },

            { "HeadSize", 0.0f },

            { "EyeBrowH", 0.0f },
            { "EyeBrowW", 0.0f },
            { "EyeBrowD", 0.0f },

            { "EarsH", 0.0f },
            { "EarsW", 0.0f },
            { "EarsD", 0.0f },
            { "EarsL", 0.0f },

            { "EyeLidH", 0.0f },
            { "EyeLidW", 0.0f },

            { "EyeD", 0.0f },
            { "EyeAng", 0.0f },
            { "EyeDis", 0.0f },
            { "EyeH", 0.0f },

            { "NoseW", 0.0f },
            { "NoseS", 0.0f },
            { "NoseH", 0.0f },
            { "NoseAng", 0.0f },
            { "NoseC", 0.0f },
            { "NoseDis", 0.0f },

            { "CheekBonesH", 0.0f },
            { "CheekBonesW", 0.0f },
            { "CheekBonesD", 0.0f },

            { "MouthW", 0.0f },
            { "MouthD", 0.0f },
            { "MouthX", 0.0f },
            { "MouthY", 0.0f },

            { "ULiphH", 0.0f },
            { "ULiphW", 0.0f },
            { "ULiphD", 0.0f },

            { "LLiphH", 0.0f },
            { "LLiphW", 0.0f },
            { "LLiphD", 0.0f },

            { "JawH", 0.0f },
            { "JawW", 0.0f },
            { "JawD", 0.0f },

            { "ChinH", 0.0f },
            { "ChinW", 0.0f },
            { "ChinD", 0.0f },

            { "Beard", 0 },

            { "Hair", 0 },

            { "Body", 0 },

            { "Waist", 0 },

            { "Eyes", 0 },

            { "Scale", 1.0f },

            //New DB
            { "eyebrows_visibility", 0 },
            { "eyebrows_tx_id", 0 },

            { "scars_visibility", 0 },
            { "scars_tx_id", 0 },

            { "spots_visibility", 0 },
            { "spots_tx_id", 0 },

            { "disc_visibility", 0 },
            { "disc_tx_id", 0 },

            { "complex_visibility", 0 },
            { "complex_tx_id", 0 },

            { "acne_visibility", 0 },
            { "acne_tx_id", 0 },

            { "ageing_visibility", 0 },
            { "ageing_tx_id", 0 },

            { "freckles_visibility", 0 },
            { "freckles_tx_id", 0 },

            { "moles_visibility", 0 },
            { "moles_tx_id", 0 },

            { "grime_visibility", 0 },
            { "grime_tx_id", 0 },

            { "lipsticks_visibility", 0 },
            { "lipsticks_tx_id", 0 },
            { "lipsticks_palette_id", 0 },
            { "lipsticks_palette_color_primary", 0 },

            { "shadows_visibility", 0 },
            { "shadows_tx_id", 0 },
            { "shadows_palette_id", 0 },
            { "shadows_palette_color_primary", 0 },
        };

        //Para guardar en DB
        public static Dictionary<string, object> clothesPlayer = new Dictionary<string, object>() {
            { "Hat", -1 },
            { "EyeWear", -1 },
            { "Mask", -1},
            { "NeckWear", -1 },
            { "NeckTies", -1 },
            { "Shirt", -1 },
            { "Suspender", -1 },
            { "Vest", -1 },
            { "Coat", -1 },
            { "Poncho", -1 },
            { "Cloak", -1 },
            { "Glove", -1 },
            { "RingRh", -1 },
            { "RingLh", -1 },
            { "Bracelet", -1 },
            { "Gunbelt", -1 },
            { "Belt", -1 },
            { "Buckle", -1 },
            { "Holster", -1 },
            { "Pant", -1 },
            { "Skirt", -1 },
            { "Chap", -1 },
            { "Boots", -1 },
            { "Spurs", -1 },
        };

        public static int indexCamera = 1;

        public CreatePlayer()
        {
            EventHandlers["vorpcharacter:createPlayer"] += new Action(StartCreation);

            Tick += OnTick;
            Tick += OnTickAnimm;
            Tick += OnTickCameras;

            API.RegisterCommand("createchar", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (args[0] == null)
                {
                    TriggerServerEvent("vorpcharacter:CommandCreate", -1);
                }
                else
                {
                    int target;
                    if (int.TryParse(args[0].ToString(), out target))
                    {
                        TriggerServerEvent("vorpcharacter:CommandCreate", target);
                    }
                    else
                    {
                        TriggerEvent("vorp:Tip", "Please use sintax: [/createchar id] id is a number.", 5000);
                    }

                }

            }), false);
        }


        public static int textureId = -1;
        public static float overlay_opacity = 1.0f;
        public static bool is_overlay_change_active = false;

        public static Dictionary<string, dynamic> texture_types = new Dictionary<string, dynamic>();

        public static void toggleOverlayChange(string name, int visibility, int tx_id, int tx_normal, int tx_material, int tx_color_type, float tx_opacity, int tx_unk, int palette_id, int palette_color_primary, int palette_color_secondary, int palette_color_tertiary, int var, float opacity)
        {
            for (int i = 0; i < SkinsUtils.overlay_all_layers.Count(); i++)
            {
                if (SkinsUtils.overlay_all_layers[i]["name"].ToString().Equals(name))
                {

                    skinPlayer[$"{name}_visibility"] = visibility;
                    skinPlayer[$"{name}_tx_id"] = tx_id;

                    if (name.Contains("shadows") || name.Contains("lipsticks"))
                    {
                        skinPlayer[$"{name}_palette_id"] = palette_id;
                        skinPlayer[$"{name}_palette_color_primary"] = palette_color_primary;
                    }

                    SkinsUtils.overlay_all_layers[i]["visibility"] = visibility;
                    if (visibility != 0)
                    {
                        SkinsUtils.overlay_all_layers[i]["tx_normal"] = tx_normal;
                        SkinsUtils.overlay_all_layers[i]["tx_material"] = tx_material;
                        SkinsUtils.overlay_all_layers[i]["tx_color_type"] = tx_color_type;
                        SkinsUtils.overlay_all_layers[i]["tx_opacity"] = tx_opacity;
                        SkinsUtils.overlay_all_layers[i]["tx_unk"] = tx_unk;
                        if (tx_color_type == 0)
                        {
                            SkinsUtils.overlay_all_layers[i]["palette"] = SkinsUtils.COLOR_PALETTES[palette_id];
                            SkinsUtils.overlay_all_layers[i]["palette_color_primary"] = palette_color_primary;
                            SkinsUtils.overlay_all_layers[i]["palette_color_secondary"] = palette_color_secondary;
                            SkinsUtils.overlay_all_layers[i]["palette_color_tertiary"] = palette_color_tertiary;
                        }
                        if (name.Equals("shadows") || name.Equals("eyeliners") || name.Equals("lipsticks"))
                        {
                            SkinsUtils.overlay_all_layers[i]["var"] = var;
                            SkinsUtils.overlay_all_layers[i]["tx_id"] = (int)SkinsUtils.overlays_info[name][0]["id"];
                        }
                        else
                        {
                            SkinsUtils.overlay_all_layers[i]["var"] = 0;
                            SkinsUtils.overlay_all_layers[i]["tx_id"] = (int)SkinsUtils.overlays_info[name][tx_id]["id"];
                        }
                        SkinsUtils.overlay_all_layers[i]["opacity"] = opacity;
                    }
                }
            }
            changeOverlays();
        }

        public static async Task changeOverlays()
        {

            int ped = API.PlayerPedId();
            if (textureId != -1)
            {
                Function.Call((Hash)0xB63B9178D0F58D82, textureId);
                Function.Call((Hash)0x6BEFAA907B076859, textureId);
            }

            textureId = Function.Call<int>((Hash)0xC5E7204F322E49EB, texture_types["albedo"], texture_types["normal"], texture_types["material"]);
            

            foreach (Dictionary<string, dynamic> layer in SkinsUtils.overlay_all_layers)
            {
                if (layer["visibility"] != 0)
                {
                    int overlay_id = Function.Call<int>((Hash)0x86BB5FF45F193A02, textureId, layer["tx_id"], layer["tx_normal"], layer["tx_material"], layer["tx_color_type"], layer["tx_opacity"], layer["tx_unk"]);
                    if (layer["tx_color_type"] == 0)
                    {
                        Function.Call((Hash)0x1ED8588524AC9BE1, textureId, overlay_id, layer["palette"]);
                        Function.Call((Hash)0x2DF59FFE6FFD6044, textureId, overlay_id, layer["palette_color_primary"], layer["palette_color_secondary"], layer["palette_color_tertiary"]);
                    }
                    Function.Call((Hash)0x3329AAE2882FC8E4, textureId, overlay_id, layer["var"]);
                    Function.Call((Hash)0x6C76BC24F8BB709A, textureId, overlay_id, layer["opacity"]);
                }
            }

            while(!Function.Call<bool>((Hash)0x31DC8D3F216D8509, textureId))
            {
                await Delay(0);
            }
            
            Function.Call<bool>((Hash)0x0B46E25761519058, ped, API.GetHashKey("heads"), textureId);
            Function.Call<bool>((Hash)0x92DAABA2C1C10B0E, textureId);
            Function.Call<bool>((Hash)0xCC8CA3E88256E58F, ped, 0, 1, 1, 1, false);
        }

        public static async Task changeScale(float scale)
        {
            skinPlayer["Scale"] = scale;
            Function.Call((Hash)0x25ACFC650B65C538, API.PlayerPedId(), scale);
        }

        public static uint FromHex(string value)
        {
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }
            return (uint)Int32.Parse(value, NumberStyles.HexNumber);
        }

        [Tick]
        private async Task OnTickAnimm()
        {
            if (pedCreated != 0)
            {
                //SetPedIntoVehicle 0x0D3B5BAEA08F63E9 
                Function.Call((Hash)0xF75B0D629E1C063D, pedCreated, vehCreated, -1, false);

            }

            if (pedCreated != 0)
            {
                float above = Function.Call<float>((Hash)0x0D3B5BAEA08F63E9, API.PlayerPedId());

                if (above < 1.0f)
                {
                    API.FreezeEntityPosition(pedCreated, false);
                    API.DeletePed(ref pedCreated);
                    API.DeleteVehicle(ref vehCreated);
                    pedCreated = 0;
                    TriggerEvent("vorp:setInstancePlayer", false);
                }
            }
            await Delay(1);
        }

        public static int vehCreated = 0;
        public static int pedCreated = 0;

        private static async void StartAnim()
        {
            uint HashVeh = await Miscellanea.GetHash("hotAirBalloon01");
            Vector3 coords = new Vector3(GetConfig.Config["StartingCoords"][0].ToObject<float>(), GetConfig.Config["StartingCoords"][1].ToObject<float>(), 220.3232f);
           
            vehCreated = API.CreateVehicle(HashVeh, coords.X + 1, coords.Y, coords.Z, 0, false, true, true, true);
            //Spawn
            Function.Call((Hash)0x283978A15512B2FE, vehCreated, true);
            //TaskWanderStandard
            Function.Call((Hash)0xBB9CE077274F6A1B, 10, 10);


            uint HashPed = await Miscellanea.GetHash("CS_balloonoperator");
            pedCreated = API.CreatePed(HashPed, coords.X + 1, coords.Y, coords.Z, 0.0f, false, true, true, true);
            //Spawn
            Function.Call((Hash)0x283978A15512B2FE, pedCreated, true);

            Function.Call((Hash)0xF75B0D629E1C063D, API.PlayerPedId(), vehCreated, -1, false);


            API.TaskLeaveVehicle(API.PlayerPedId(), vehCreated, 0, 0);


            //API.SetEntityCoords(API.PlayerPedId(), coords.X, coords.Y, coords.Z, true, true, true, false);
            //API.SetEntityHeading(API.PlayerPedId(), 0);

            await Delay(1000);
            //SetPedIntoVehicle
            Function.Call((Hash)0xF75B0D629E1C063D, pedCreated, vehCreated, -1, false);

            API.SetEntityAsMissionEntity(pedCreated, true, true);
            API.SetEntityAsMissionEntity(pedCreated, true, true);

            API.FreezeEntityPosition(pedCreated, true);

            API.SetRelationshipBetweenGroups(1, HashPed, (uint)API.GetHashKey("PLAYER"));

            TriggerEvent("vorp:Tip", GetConfig.Langs["TipFinal"], 15000);

        }

        private static async void StopCreation()
        {
            isInCharCreation = false;
            await DeleteAll();
        }

        public static void SetPlayerComponent(int _newIndex, uint category, string idlist, List<uint> male_components, List<uint> female_components)
        {
            int pPID = API.PlayerPedId();
            if (model_selected == "mp_male")
            {
                if (_newIndex == 0)
                {
                    //Coats is a really shit
                    if (category == 0xE06D30CE)
                    {
                        Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x662AC34, 0);
                    }
                    //end
                    Function.Call((Hash)0xD710A5007C2AC539, pPID, category, 0);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                    clothesPlayer[idlist] = -1;
                }
                else
                {
                    //Coats is a really shit
                    if (category == 0xE06D30CE) {
                        Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x662AC34, 0);
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                    }
                    //end
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, male_components[_newIndex - 1], true, true, false);
                    clothesPlayer[idlist] = male_components[_newIndex - 1];
                }
            }
            else
            {
                if (_newIndex == 0)
                {
                    Function.Call((Hash)0xD710A5007C2AC539, pPID, category, 0);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                    clothesPlayer[idlist] = -1;
                }
                else
                {
                    //Miscellanea.LoadModel(male_components[_newIndex - 1]);
                    Function.Call((Hash)0x59BD177A1A48600A, pPID, category);
                    Function.Call((Hash)0xD3A7B003ED343FD9, pPID, female_components[_newIndex - 1], true, true, true);
                    clothesPlayer[idlist] = female_components[_newIndex - 1];
                }
            }
            //Fix Hair
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x864B03AE, 0);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), ConvertValue(skinPlayer["Hair"].ToString()), true, true, true);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), ConvertValue(skinPlayer["Beard"].ToString()), true, true, true);
            //end
            Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
        }
        public static void SetPlayerModelComponent(string hex, string skinP)
        {
            string comp = "0x" + hex;
            int compInt = Convert.ToInt32(comp, 16);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), compInt, true, true, true);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
            skinPlayer[skinP] = compInt;
        }
        public static void SetPlayerModelListComps(string skinP, uint comp, uint category)
        {
            if (comp == 0)
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), category, 0);
                skinPlayer[skinP] = -1;
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), comp, true, true, true);
                skinPlayer[skinP] = comp;
            }


            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
        }
        public static void SetPlayerBodyComponent(uint comp, string skinP)
        {
            Function.Call((Hash)0x1902C4CFCC5BE57C, API.PlayerPedId(), comp);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, false);
            skinPlayer[skinP] = comp;
        }

        public static void SetPlayerFaceBlend(int item, int index)
        {
            int pPID = API.PlayerPedId();
            float _sizeValue = (float)index;
            _sizeValue = _sizeValue / 10.0f;

            switch (item)
            {
                case 0:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x84D6, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["HeadSize"] = _sizeValue;
                    break;
                case 1:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x3303, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeBrowH"] = _sizeValue;
                    break;
                case 2:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x2FF9, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeBrowW"] = _sizeValue;
                    break;
                case 3:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x4AD1, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeBrowD"] = _sizeValue;
                    break;
                case 4:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xC04F, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsH"] = _sizeValue;
                    break;
                case 5:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xB6CE, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsW"] = _sizeValue;
                    break;
                case 6:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x2844, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsD"] = _sizeValue;
                    break;
                case 7:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xED30, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EarsL"] = _sizeValue;
                    break;
                case 8:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x8B2B, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeLidH"] = _sizeValue;
                    break;
                case 9:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x1B6B, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeLidW"] = _sizeValue;
                    break;
                case 10:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xEE44, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeD"] = _sizeValue;
                    break;
                case 11:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xD266, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeAng"] = _sizeValue;
                    break;
                case 12:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xA54E, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeDis"] = _sizeValue;
                    break;
                case 13:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xDDFB, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["EyeH"] = _sizeValue;
                    break;
                case 14:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x6E7F, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseW"] = _sizeValue;
                    break;
                case 15:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x3471, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseS"] = _sizeValue;
                    break;
                case 16:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x03F5, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseH"] = _sizeValue;
                    break;
                case 17:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x34B1, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseAng"] = _sizeValue;
                    break;
                case 18:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xF156, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseC"] = _sizeValue;
                    break;
                case 19:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x561E, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["NoseDis"] = _sizeValue;
                    break;
                case 20:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x6A0B, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["CheekBonesH"] = _sizeValue;
                    break;
                case 21:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xABCF, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["CheekBonesW"] = _sizeValue;
                    break;
                case 22:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x358D, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["CheekBonesD"] = _sizeValue;
                    break;
                case 23:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xF065, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthW"] = _sizeValue;
                    break;
                case 24:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xAA69, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthD"] = _sizeValue;
                    break;
                case 25:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x7AC3, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthX"] = _sizeValue;
                    break;
                case 26:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x410D, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["MouthY"] = _sizeValue;
                    break;
                case 27:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x1A00, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ULiphH"] = _sizeValue;
                    break;
                case 28:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x91C1, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ULiphW"] = _sizeValue;
                    break;
                case 29:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xC375, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ULiphD"] = _sizeValue;
                    break;
                case 30:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xBB4D, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["LLiphH"] = _sizeValue;
                    break;
                case 31:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xB0B0, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["LLiphW"] = _sizeValue;
                    break;
                case 32:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x5D16, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["LLiphD"] = _sizeValue;
                    break;
                case 33:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x8D0A, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["JawH"] = _sizeValue;
                    break;
                case 34:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xEBAE, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["JawW"] = _sizeValue;
                    break;
                case 35:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x1DF6, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["JawD"] = _sizeValue;
                    break;
                case 36:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x3C0F, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ChinH"] = _sizeValue;
                    break;
                case 37:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xC3B2, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ChinW"] = _sizeValue;
                    break;
                case 38:
                    Function.Call((Hash)0x5653AB26C82938CF, pPID, 0xE323, _sizeValue);
                    Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                    skinPlayer["ChinD"] = _sizeValue;
                    break;
            }
        }
        public static async Task SaveChanges()
        {
            TriggerEvent("vorpinputs:getInput", GetConfig.Langs["ButtonInputName"], GetConfig.Langs["PlaceHolderInputName"], new Action<dynamic>(async (cb) =>
            {
                string result = cb;
                await Delay(1000);

                string[] words = result.Trim().Split(' ');

                if (result.Length < 3 || words.Count() < 2) {
                    TriggerEvent("vorp:Tip", GetConfig.Langs["PlaceHolderInputName"], 3000); // from client side
                    SaveChanges();
                }
                else
                {
                    TriggerServerEvent("vorpcharacter:SaveSkinDB", skinPlayer, clothesPlayer, result);
                    SaveLocalChanges(result.ToLower());
                    StopCreation();
                    StartAnim();
                }
            }));
        }

        public async static Task SaveLocalChanges(string name)
        {

        }

        private async void StartCreation()
        {
            /*
             * Cargamos los modelos del mapa de creación del online de RDR2
             */
            Function.Call(Hash._REQUEST_IMAP, 183712523);
            Function.Call(Hash._REQUEST_IMAP, -1699673416);
            Function.Call(Hash._REQUEST_IMAP, 1679934574);
            /*
             * Cambiamos el tiempo para que se vea de mañana
             */
            Function.Call(Hash.SET_CLOCK_TIME, 12, 00, 0);
            API.SetClockTime(12, 00, 00);

            Miscellanea.TeleportToCoords(-563.1345f, -3775.811f, 237.60f);
            /*
             * Cargammos las Peds en el sitio
             */
            await CreationSelectPeds();
            /*
             * Creamos las camaras para movernos de sala con ella y la activamos
             */
            await CreateCams();
            /*
             * Esperamos un tiempo a que pueda cargar los modelos y el tiempo
             */

            API.SetCamActive(Camera, true);
            API.RenderScriptCams(true, true, 1000, true, true, 0);

            isSelectSexActive = true;

        }

        private async Task CreateCams()
        {
            Camera = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -560.83f, -3776.33f, 239.58f, -13.56231f, 0.00f, -91.93626f, 45.00f, false, 0);
            Camera_Male = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.6671f, -3775.44f, 239.4266f, -9.622695f, 0.0f, -86.08074f, 45.00f, false, 0);
            Camera_Female = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.8455f, -3776.596f, 239.4435f, -13.41718f, 0.0f, -88.04576f, 45.00f, false, 0);
            Camera_Editor = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -560.1333f, -3780.923f, 239.4437f, -11.32719f, 0.0f, -90.96693f, 45.00f, false, 0);
            Camera_FaceEditor = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -558.9781f, -3780.955f, 239.186f, 6.591177f, 0.0f, -92.76723f, 40.00f, false, 0);
            Camera_Waist = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.1779f, -3780.964f, 238.4654f, -0.6631846f, 0.0f, -91.76698f, 40.00f, false, 0);
            Camera_Legs = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -559.2103f, -3781.039f, 238.4678f, -42.50001f, 0.0f, -89.2997f, 40.00f, false, 0);
            Camera_Body = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -560.6195f, -3780.708f, 239.1954f, -15.75687f, 0.0f, -89.49976f, 40.00f, false, 0);
            //Camera_Back = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -563.0956f, -3780.669f, 238.465f, 0.906957f, 0.0f, -89.36639f, 40.00f, false, 0);

            uint HashVeh = await Miscellanea.GetHash("hotAirBalloon01");

            uint HashPed = await Miscellanea.GetHash("CS_balloonoperator");

        }

        private static async Task DeleteAll()
        {
            API.SetCamActive(Camera, false);
            API.DestroyCam(Camera, true);
            API.SetCamActive(Camera_Male, false);
            API.DestroyCam(Camera_Male, true);
            API.SetCamActive(Camera_Female, false);
            API.DestroyCam(Camera_Female, true);
            API.SetCamActive(Camera_Editor, false);
            API.DestroyCam(Camera_Editor, true);
            API.SetCamActive(Camera_FaceEditor, false);
            API.DestroyCam(Camera_FaceEditor, true);
            API.SetCamActive(Camera_Waist, false);
            API.DestroyCam(Camera_Waist, true);
            API.SetCamActive(Camera_Legs, false);
            API.DestroyCam(Camera_Legs, true);
            API.SetCamActive(Camera_Body, false);
            API.DestroyCam(Camera_Body, true);
            API.SetCamActive(Camera_Back, false);
            API.DestroyCam(Camera_Back, true);
            API.RenderScriptCams(false, true, 5000, true, true, 0);
            API.FreezeEntityPosition(API.PlayerPedId(), false);
        }

        private async Task CreationSelectPeds()
        {

            uint hash_f = await Miscellanea.GetHash(model_f);
            uint hash_m = await Miscellanea.GetHash(model_m);
            /*
             * Esperamos a que cargen los modelos en cache
             */
            /*
             * Creamos los modelos en el sitio de creacion
             */
            PedFemale = API.CreatePed((uint)hash_f, -558.43f, -3776.65f, 237.7f, 93.2f, false, true, true, true);
            PedMale = API.CreatePed((uint)hash_m, -558.52f, -3775.6f, 237.7f, 93.2f, false, true, true, true);
            /*
             * Necesitan un radom Outfit ya que no se por que no salen si no
             */
            Function.Call((Hash)0x283978A15512B2FE, PedFemale, true);
            Function.Call((Hash)0x283978A15512B2FE, PedMale, true);
            /*
             * Congelamos las Peds
             */
            API.FreezeEntityPosition(PedFemale, true);
            API.FreezeEntityPosition(PedMale, true);

            TriggerEvent("vorp:setInstancePlayer", true);

        }
        private async Task CreationSexPed(string model, int camedit)
        {
            model_selected = model;
            skinPlayer["sex"] = model;
            

            if (model_selected == model_m)
            {
                skinPlayer["albedo"] = API.GetHashKey("mp_head_mr1_sc08_c0_000_ab");
                texture_types["albedo"] = API.GetHashKey("mp_head_mr1_sc08_c0_000_ab");
                texture_types["normal"] = API.GetHashKey("mp_head_mr1_000_nm");
                texture_types["material"] = 0x7FC5B1E1;
                texture_types["color_type"] = 1;
                texture_types["texture_opacity"] = 1.0f;
                texture_types["unk_arg"] = 0;
            }
            else
            {
                skinPlayer["albedo"] = API.GetHashKey("mp_head_fr1_sc08_c0_000_ab");
                texture_types["albedo"] = API.GetHashKey("mp_head_fr1_sc08_c0_000_ab");
                texture_types["normal"] = API.GetHashKey("head_fr1_mp_002_nm");
                texture_types["material"] = 0x7FC5B1E1;
                texture_types["color_type"] = 1;
                texture_types["texture_opacity"] = 1.0f;
                texture_types["unk_arg"] = 0;
            }

            await Delay(200);
            int pID = API.PlayerId();
            int pPedID = API.PlayerPedId();
            Menus.MainMenu.GetMenu();
            Miscellanea.TeleportToCoords(-558.3258f, -3781.111f, 237.60f, 93.2f);
            API.FreezeEntityPosition(pPedID, true);
            uint model_hash = await Miscellanea.GetHash(model);
            Function.Call((Hash)0xED40380076A31506, pID, model_hash, true);
            Function.Call((Hash)0x283978A15512B2FE, pPedID, true);
            API.RenderScriptCams(false, true, 3000, true, true, 0);
            await Delay(2500);
            API.SetCamActive(Camera_Editor, true);
            API.RenderScriptCams(true, true, 1000, true, true, 0);
            API.DeletePed(ref PedFemale);
            API.DeletePed(ref PedMale);
            isInCharCreation = true;
            Menus.MainMenu.GetMenu().OpenMenu();

            //MenuCreateCharacter(model);
        }

        public void SwapCameras(int index)
        {
            switch (index)
            {
                case 0:
                    API.SetCamActive(Camera_Editor, true);
                    API.SetCamActive(Camera_Body, false);
                    API.SetCamActive(Camera_Back, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 1:
                    API.SetCamActive(Camera_Body, true);
                    API.SetCamActive(Camera_Editor, false);
                    API.SetCamActive(Camera_FaceEditor, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 2:
                    API.SetCamActive(Camera_FaceEditor, true);
                    API.SetCamActive(Camera_Body, false);
                    API.SetCamActive(Camera_Waist, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 3:
                    API.SetCamActive(Camera_Waist, true);
                    API.SetCamActive(Camera_FaceEditor, false);
                    API.SetCamActive(Camera_Legs, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 4:
                    API.SetCamActive(Camera_Legs, true);
                    API.SetCamActive(Camera_Waist, false);
                    API.SetCamActive(Camera_Back, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
                case 5:
                    API.SetCamActive(Camera_Back, true);
                    API.SetCamActive(Camera_Legs, false);
                    API.SetCamActive(Camera_Editor, false);
                    API.RenderScriptCams(true, true, 200, true, true, 0);
                    break;
            }
        }

        private async Task OnTickCameras()
        {
            if (isInCharCreation)
            {
                
                if (API.IsControlJustPressed(0, (uint)Controls.Keys.MoveUpOnly))
                {
                    indexCamera +=  1;
                    if (indexCamera > 4)
                    {
                        indexCamera = 0;
                    }

                    SwapCameras(indexCamera);
                }
                if (API.IsControlJustPressed(0, (uint)Controls.Keys.MoveDownOnly))
                {
                    indexCamera -= 1;
                    if (indexCamera < 0)
                    {
                        indexCamera = 4;
                    }

                    SwapCameras(indexCamera);
                }
                if (API.IsControlPressed(0, (uint)Controls.Keys.MoveLeftOnly))
                {
                    DressHeading += 1.0f;
                    API.SetEntityHeading(API.PlayerPedId(), DressHeading);
                    await Delay(0);
                }
                if (API.IsControlPressed(0, (uint)Controls.Keys.MoveRightOnly))
                {
                    DressHeading -= 1.0f;
                    API.SetEntityHeading(API.PlayerPedId(), DressHeading);
                    await Delay(0);
                }
            }
        }

        private async Task OnTick()
        {
            if (API.IsControlJustPressed(2, (uint)Controls.Keys.FrontendRight) && isSelectSexActive) {

                if (API.IsCamActive(Camera))
                {
                    API.SetCamActiveWithInterp(Camera_Female, Camera, 2000, 0, 0);
                    API.SetCamActive(Camera, false);
                }
                else if (API.IsCamActive(Camera_Male))
                {
                    API.SetCamActiveWithInterp(Camera_Female, Camera_Male, 2000, 0, 0);
                    API.SetCamActive(Camera_Male, false);
                }
                else
                {
                    API.SetCamActiveWithInterp(Camera, Camera_Female, 2000, 0, 0);
                    API.SetCamActive(Camera_Female, false);
                }
                await Delay(2000);
            }

            if (API.IsControlJustPressed(2, (uint)Controls.Keys.FrontendLeft) && isSelectSexActive)
            {

                if (API.IsCamActive(Camera))
                {
                    API.SetCamActiveWithInterp(Camera_Male, Camera, 2000, 0, 0);
                    API.SetCamActive(Camera, false);
                }
                else if (API.IsCamActive(Camera_Female))
                {
                    API.SetCamActiveWithInterp(Camera_Male, Camera_Female, 2000, 0, 0);
                    API.SetCamActive(Camera_Female, false);
                }
                else
                {
                    API.SetCamActiveWithInterp(Camera, Camera_Male, 2000, 0, 0);
                    API.SetCamActive(Camera_Male, false);
                }
                await Delay(2000);
            }

            if (API.IsControlJustPressed(2, (uint)Controls.Keys.FrontendAccept) && isSelectSexActive)
            {
                
                if (API.IsCamActive(Camera_Male))
                {
                    CreationSexPed(model_m, Camera_Male).Wait();
                    isSelectSexActive = false;
                }
                else if (API.IsCamActive(Camera_Female))
                {
                    CreationSexPed(model_f, Camera_Female).Wait();
                    isSelectSexActive = false;
                }
                else
                {
                }
                await Delay(100);
            }

            if (isSelectSexActive)
            {
                DrawTxt(GetConfig.Langs["PressRightOrLeft"], 0.5f, 0.9f, 0.7f, 0.7f, 255, 255, 255, 255, true, true);
            }

            if (isInCharCreation) //Fix Run Ped
            {
                DrawTxt(GetConfig.Langs["PressGuide"], 0.5f, 0.9f, 0.7f, 0.7f, 255, 255, 255, 255, true, true);
                API.FreezeEntityPosition(API.PlayerPedId(), true);
                API.ClearPedTasks(API.PlayerPedId(), 1, 1);
                API.DrawLightWithRange(-560.1646f, -3782.066f, 238.5975f, 255, 255, 255, 7.0f, 150.0f);
            }

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
                int eresante = int.Parse(s);
                result = (uint)eresante;
                return result;
            }
        }

        public static void  CloseSecureMenu()
        {
            if (!MenuController.IsAnyMenuOpen())
            {
                Menus.MainMenu.GetMenu().OpenMenu();
            }
        }

        public void DrawTxt(string text, float x, float y, float fontscale, float fontsize, int r, int g, int b, int alpha, bool textcentred, bool shadow)
        {
            long str = Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text);
            Function.Call(Hash.SET_TEXT_SCALE, fontscale, fontsize);
            Function.Call(Hash._SET_TEXT_COLOR, r, g, b, alpha);
            Function.Call(Hash.SET_TEXT_CENTRE, textcentred);
            if (shadow) { Function.Call(Hash.SET_TEXT_DROPSHADOW, 1, 0, 0, 255); }
            Function.Call(Hash.SET_TEXT_FONT_FOR_CURRENT_COMMAND, 1);
            Function.Call(Hash._DISPLAY_TEXT, str, x, y);
        }
    }
}
