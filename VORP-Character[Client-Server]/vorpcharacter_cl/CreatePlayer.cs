using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        static Dictionary<string, object> skinPlayer = new Dictionary<string, object>() {
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
        };

        //Para guardar en DB
        static Dictionary<string, object> clothesPlayer = new Dictionary<string, object>() {
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

        [Tick]
        private async Task InstancePlayer()
        {
            if (isSelectSexActive || isInCharCreation)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (API.NetworkIsPlayerActive(i))
                    {
                        if (API.GetPlayerPed(i) != API.PlayerPedId())
                        {
                            API.SetEntityAlpha(API.GetPlayerPed(i), 0, false);
                            API.SetEntityNoCollisionEntity(API.PlayerPedId(), API.GetPlayerPed(i), false);
                            await Delay(1);
                        }

                    }

                }
            }

            await Delay(3000);
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
                }
            }
            await Delay(1);
        }

        public static int vehCreated = 0;
        public static int pedCreated = 0;

        private static async void StartAnim()
        {
            uint HashVeh = (uint)API.GetHashKey("hotAirBalloon01");
            Vector3 coords = new Vector3(GetConfig.Config["StartingCoords"][0].ToObject<float>(), GetConfig.Config["StartingCoords"][1].ToObject<float>(), 220.3232f);
            Miscellanea.LoadModel(HashVeh);
            vehCreated = API.CreateVehicle(HashVeh, coords.X + 1, coords.Y, coords.Z, 0, true, true, true, true);
            //Spawn
            Function.Call((Hash)0x283978A15512B2FE, vehCreated, true);
            //TaskWanderStandard
            Function.Call((Hash)0xBB9CE077274F6A1B, 10, 10);


            uint HashPed = (uint)API.GetHashKey("CS_balloonoperator");
            Miscellanea.LoadModel(HashPed);
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

            for (int i = 0; i < 255; i++)
            {
                if (API.NetworkIsPlayerActive(i))
                {
                    if (API.GetPlayerPed(i) != API.PlayerPedId())
                    {
                        API.SetEntityAlpha(API.GetPlayerPed(i), 255, false);
                        API.SetEntityNoCollisionEntity(API.PlayerPedId(), API.GetPlayerPed(i), true);
                    }

                }

            }

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
                if (result.Length < 3) {
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

            uint HashVeh = (uint)API.GetHashKey("hotAirBalloon01");
            await Miscellanea.LoadModel(HashVeh);

            uint HashPed = (uint)API.GetHashKey("CS_balloonoperator");
            await Miscellanea.LoadModel(HashPed);

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

            uint hash_f = (uint)API.GetHashKey(model_f);
            uint hash_m = (uint)API.GetHashKey(model_m);
            /*
             * Esperamos a que cargen los modelos en cache
             */
            await Miscellanea.LoadModel(hash_f);
            await Miscellanea.LoadModel(hash_m);
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

        }
        private async void CreationSexPed(string model, int camedit)
        {
            model_selected = model;
            skinPlayer["sex"] = model;
            Debug.WriteLine(model);
            await Delay(200);
            int pID = API.PlayerId();
            int pPedID = API.PlayerPedId();
            Menus.MainMenu.GetMenu();
            Miscellanea.TeleportToCoords(-558.3258f, -3781.111f, 237.60f, 93.2f);
            API.FreezeEntityPosition(pPedID, true);
            uint model_hash = (uint)API.GetHashKey(model);
            await Miscellanea.LoadModel(model_hash);
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
                    Debug.WriteLine("Hombre");
                    CreationSexPed(model_m, Camera_Male);
                    isSelectSexActive = false;
                }
                else if (API.IsCamActive(Camera_Female))
                {
                    Debug.WriteLine("Mujer");
                    CreationSexPed(model_f, Camera_Female);
                    isSelectSexActive = false;
                }
                else
                {
                }
                await Delay(100);
            }

            if (isSelectSexActive)
            {
                await DrawTxt(GetConfig.Langs["PressRightOrLeft"], 0.5f, 0.9f, 0.7f, 0.7f, 255, 255, 255, 255, true, true);
            }

            if (isInCharCreation) //Fix Run Ped
            {
                await DrawTxt(GetConfig.Langs["PressGuide"], 0.5f, 0.9f, 0.7f, 0.7f, 255, 255, 255, 255, true, true);
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

        public static async Task CloseSecureMenu()
        {
            await Delay(200);
            if (!MenuController.IsAnyMenuOpen())
            {
                Menus.MainMenu.GetMenu().OpenMenu();
            }
        }

        public async Task DrawTxt(string text, float x, float y, float fontscale, float fontsize, int r, int g, int b, int alpha, bool textcentred, bool shadow)
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
