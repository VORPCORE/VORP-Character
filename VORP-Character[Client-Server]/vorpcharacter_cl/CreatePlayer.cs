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

namespace vorpcore_cl.Scripts
{
    public class CreatePlayer : BaseScript
    {
        bool isSelectSexActive = false;
        bool isDressUpPed = false;
        bool isInCharCreation = false;
        string model_f = "mp_female";
        string model_m = "mp_male";
        int PedFemale;
        int PedMale;
        int Camera;
        int Camera_Male;
        int Camera_Female;
        int Camera_Editor;
        int Camera_DressUp;
        float DressHeading = 359.9842f;
        bool onKeyBoard = false;

        //Para guardar en DB
        static Dictionary<string, object> skinPlayer = new Dictionary<string, object>() {
            { "sex", "mp_male" },

            { "Head", 0.0f },

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

        public CreatePlayer()
        {
            EventHandlers["vorpcharacter:createPlayer"] += new Action(StartCreation);

            Tick += OnTick;
            Tick += OnTickAnimm;

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

        private void CheckCreation()
        {
            TriggerServerEvent("vorpcharacter:CommandCreate");
        }

        [Tick]
        private async Task InstancePlayer()
        {
            if (isSelectSexActive || isDressUpPed || isInCharCreation)
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

        private async void StartAnim()
        {
            uint HashVeh = (uint)API.GetHashKey("hotAirBalloon01");
            Vector3 coords = new Vector3(-366.431f, 727.4282f, 220.3232f);
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

            TriggerEvent("vorp:Tip", Language.Langs["TipFinal"], 15000);

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

        private async void StopCreation()
        {
            isSelectSexActive = false;
            isDressUpPed = false;
            isInCharCreation = false;
            await DeleteAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void MenuCreateCharacter(string model)
        {
            MenuController.Menus.Clear();

            skinPlayer["sex"] = model;

            //Definimos el nombre y subtitlo del menu con un constructor
            Menu mcc = new Menu(Language.Langs["TitleMenuBody"], Language.Langs["SubTitleMenuBody"]);
            MenuController.AddMenu(mcc);
            MenuController.MenuToggleKey = (Control)0;

            //Tipo de tamaños del 1 al 10
            List<string> sizeType = new List<string>();

            for (float i = 1; i < 11; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
            {
                sizeType.Add(Language.Langs["TypeValues"] + i);
            }

            MenuListItem mListHead = new MenuListItem(Language.Langs["FaceSize"], sizeType, 0, Language.Langs["FaceSizeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListHead); // Lo añadimos al menu

            //Altura cejas
            MenuListItem mListEyeBrowH = new MenuListItem(Language.Langs["EyebrowHeight"], sizeType, 0, Language.Langs["EyebrowHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeBrowH); // Lo añadimos al menu
            //Anchura cejas
            MenuListItem mListEyeBrowW = new MenuListItem(Language.Langs["EyebrowWidth"], sizeType, 0, Language.Langs["EyebrowWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeBrowW); // Lo añadimos al menu
            //Profundida cejas
            MenuListItem mListEyeBrowD = new MenuListItem(Language.Langs["EyebrowDepth"], sizeType, 0, Language.Langs["EyebrowDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeBrowD); // Lo añadimos al menu

            //Altura Orejas
            MenuListItem mListEarsH = new MenuListItem(Language.Langs["EarsHeight"], sizeType, 0, Language.Langs["EarsHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEarsH); // Lo añadimos al menu
            //Anchura Orejas
            MenuListItem mListEarsW = new MenuListItem(Language.Langs["EarsAngle"], sizeType, 0, Language.Langs["EarsAngleDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEarsW); // Lo añadimos al menu
            //Tamaño Orejas
            MenuListItem mListEarsD = new MenuListItem(Language.Langs["EarsSize"], sizeType, 0, Language.Langs["EarsSizeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEarsD); // Lo añadimos al menu
            //Tamaño Lobulo de la oreja
            MenuListItem mListEarsL = new MenuListItem(Language.Langs["EarsLobeSize"], sizeType, 0, Language.Langs["EarsLobeSizeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEarsL); // Lo añadimos al menu

            //Altura de los parpados
            MenuListItem mListEyeLidH = new MenuListItem(Language.Langs["EyelidsHeight"], sizeType, 0, Language.Langs["EyelidsHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeLidH); // Lo añadimos al menu
            //Ancho de los parpados
            MenuListItem mListEyeLidW = new MenuListItem(Language.Langs["EyelidsWidth"], sizeType, 0, Language.Langs["EyelidsWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeLidW); // Lo añadimos al menu

            //Profundida de los ojos
            MenuListItem mListEyeD = new MenuListItem(Language.Langs["EyeDepth"], sizeType, 0, Language.Langs["EyeDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeD); // Lo añadimos al menu
            //Angulo de los ojos
            MenuListItem mListEyeAng = new MenuListItem(Language.Langs["EyeAngle"], sizeType, 0, Language.Langs["EyeAngleDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeAng); // Lo añadimos al menu
            //Distancia de los ojos
            MenuListItem mListEyeDis = new MenuListItem(Language.Langs["EyeSeparation"], sizeType, 0, Language.Langs["EyeSeparationDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeDis); // Lo añadimos al menu
            //Altura de los ojos
            MenuListItem mListEyeH = new MenuListItem(Language.Langs["EyeHeight"], sizeType, 0, Language.Langs["EyeHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListEyeH); // Lo añadimos al menu


            MenuListItem mListNoseW = new MenuListItem(Language.Langs["NoseWidth"], sizeType, 0, Language.Langs["NoseWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListNoseW); // Lo añadimos al menu

            MenuListItem mListNoseS = new MenuListItem(Language.Langs["NoseSize"], sizeType, 0, Language.Langs["NoseSizeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListNoseS); // Lo añadimos al menu

            MenuListItem mListNoseH = new MenuListItem(Language.Langs["NoseHeight"], sizeType, 0, Language.Langs["NoseHeigthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListNoseH); // Lo añadimos al menu

            MenuListItem mListNoseAng = new MenuListItem(Language.Langs["NoseAngle"], sizeType, 0, Language.Langs["NoseAngleDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListNoseAng); // Lo añadimos al menu

            MenuListItem mListNoseC = new MenuListItem(Language.Langs["NoseCurvature"], sizeType, 0, Language.Langs["NoseCurvatureDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListNoseC); // Lo añadimos al menu

            MenuListItem mListNoseDis = new MenuListItem(Language.Langs["NostrilsSeparation"], sizeType, 0, Language.Langs["NostrilsSeparationDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListNoseDis); // Lo añadimos al menu


            MenuListItem mListCheekBonesH = new MenuListItem(Language.Langs["CheekbonesHeight"], sizeType, 0, Language.Langs["CheekbonesHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListCheekBonesH); // Lo añadimos al menu

            MenuListItem mListCheekBonesW = new MenuListItem(Language.Langs["CheekbonesWidth"], sizeType, 0, Language.Langs["CheekbonesWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListCheekBonesW); // Lo añadimos al menu

            MenuListItem mListCheekBonesD = new MenuListItem(Language.Langs["CheekbonesDepth"], sizeType, 0, Language.Langs["CheekbonesDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListCheekBonesD); // Lo añadimos al menu


            MenuListItem mListMouthW = new MenuListItem(Language.Langs["MouthWidth"], sizeType, 0, Language.Langs["MouthWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListMouthW); // Lo añadimos al menu

            MenuListItem mListMouthD = new MenuListItem(Language.Langs["MouthDepth"], sizeType, 0, Language.Langs["MouthDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListMouthD); // Lo añadimos al menu

            MenuListItem mListMouthX = new MenuListItem(Language.Langs["MouthDeviation"], sizeType, 0, Language.Langs["MouthDeviationDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListMouthX); // Lo añadimos al menu

            MenuListItem mListMouthY = new MenuListItem(Language.Langs["MouthHeight"], sizeType, 0, Language.Langs["MouthHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListMouthY); // Lo añadimos al menu


            MenuListItem mListULiphH = new MenuListItem("UpperLipHeight", sizeType, 0, Language.Langs["UpperLipHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListULiphH); // Lo añadimos al menu
            MenuListItem mListULiphW = new MenuListItem(Language.Langs["UpperLipWidth"], sizeType, 0, Language.Langs["UpperLipWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListULiphW); // Lo añadimos al menu
            MenuListItem mListULiphD = new MenuListItem(Language.Langs["UpperLipDepth"], sizeType, 0, Language.Langs["UpperLipDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListULiphD); // Lo añadimos al menu
            MenuListItem mListLLiphH = new MenuListItem(Language.Langs["LowerLipHeight"], sizeType, 0, Language.Langs["LowerLipHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListLLiphH); // Lo añadimos al menu
            MenuListItem mListLLiphW = new MenuListItem(Language.Langs["LowerLipWidth"], sizeType, 0, Language.Langs["LowerLipWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListLLiphW); // Lo añadimos al menu
            MenuListItem mListLLiphD = new MenuListItem(Language.Langs["LowerLipDepth"], sizeType, 0, Language.Langs["LowerLipDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListLLiphD); // Lo añadimos al menu


            MenuListItem mListJawH = new MenuListItem(Language.Langs["MandibleHeight"], sizeType, 0, Language.Langs["MandibleHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListJawH); // Lo añadimos al menu

            MenuListItem mListJawW = new MenuListItem(Language.Langs["MandibleWidth"], sizeType, 0, Language.Langs["MandibleWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListJawW); // Lo añadimos al menu

            MenuListItem mListJawD = new MenuListItem(Language.Langs["MandibleDepth"], sizeType, 0, Language.Langs["MandibleDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListJawD); // Lo añadimos al menu


            MenuListItem mListChinH = new MenuListItem(Language.Langs["ChinHeight"], sizeType, 0, Language.Langs["ChinHeightDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListChinH); // Lo añadimos al menu

            MenuListItem mListChinW = new MenuListItem(Language.Langs["ChinWidth"], sizeType, 0, Language.Langs["ChinWidthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListChinW); // Lo añadimos al menu

            MenuListItem mListChinD = new MenuListItem(Language.Langs["ChinDepth"], sizeType, 0, Language.Langs["ChinDepthDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListChinD); //Lo añadimos al menu
            
            List<string> beardType = new List<string>();

            if (model == "mp_male")
            {
                //Barbas de Hombre
                for (float i = 1; i < SkinsUtils.BEARD_MALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    beardType.Add(Language.Langs["BeardValue"] + i);
                }
            }
            else
            {
                    beardType.Add(Language.Langs["NoExistValue"]);
            }

            MenuListItem mListBeard = new MenuListItem(Language.Langs["BeardType"], beardType, 0, Language.Langs["BeardTypeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListBeard); // Lo añadimos al menu


            //List<string> mustacheType = new List<string>();

            //if (model == "mp_male")
            //{
                //Dientes de Hombre
            //    for (float i = 1; i < SkinsUtils.MUSTACHE_MALE.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
            //    {
            //        mustacheType.Add("Bigote #" + i);
            //    }
            //}
            //else
            //{
            //    mustacheType.Add("No Hay");
           // }

            //MenuListItem mListMustache = new MenuListItem("Tipo de Bigote", mustacheType, 0, "Elije el bigote de tu personaje"); // Añadimos la lista al boton
            //mcc.AddMenuItem(mListMustache); // Lo añadimos al menu

            List<string> hairType = new List<string>();

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.HAIR_MALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    hairType.Add(Language.Langs["HairValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.HAIR_FEMALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    hairType.Add(Language.Langs["HairValue"] + i);
                }
            }

            MenuListItem mListHair = new MenuListItem(Language.Langs["HairType"], hairType, 0, Language.Langs["HairTypeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListHair); // Lo añadimos al menu

            //Tipo de cuerpo
            List<string> bodyType = new List<string>();

            for(int i = 1; i < SkinsUtils.BODY_TYPES.Count +1 ; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
            {
                bodyType.Add(Language.Langs["BodySizeValue"] + i);
            }

            MenuListItem mListBody = new MenuListItem(Language.Langs["BodyType"], bodyType, 0, Language.Langs["BodyTypeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListBody); // Lo añadimos al menu

            //Tipo de cuerpo
            List<string> waistType = new List<string>();

            for (int i = 1; i < SkinsUtils.WAIST_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
            {
                waistType.Add(Language.Langs["WaistValue"] + i);
            }

            MenuListItem mListWaist = new MenuListItem(Language.Langs["WaistType"], waistType, 0, Language.Langs["WaistTypeDesc"]); // Añadimos la lista al boton
            mcc.AddMenuItem(mListWaist); // Lo añadimos al menu

            //Terminamos y confirmamos
            mcc.AddMenuItem(new MenuItem(Language.Langs["Confirm"], Language.Langs["ConfirmDesc"])
            {
                Enabled = true,
                LeftIcon = MenuItem.Icon.TICK
            });

            mcc.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                // Code in here would get executed whenever the selected value of a list item changes (when left/right key is pressed).
                Debug.WriteLine($"Cambios del menu: [{_menu}, {_listItem}, {_oldIndex}, {_newIndex}, {_itemIndex}]");
                int pPID = API.PlayerPedId();
                float _sizeValue = (float)_newIndex;
                _sizeValue = _sizeValue / 10.0f;

                switch (_itemIndex)
                {
                    case 0:
                        Function.Call((Hash)0x5653AB26C82938CF, pPID, 0x84D6, _sizeValue);
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                        skinPlayer["Head"] = _sizeValue;
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
                    case 39:
                        if (model == "mp_male")
                        {
                            if (_newIndex == 0)
                            {
                                Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x15D3C7F2, 0);
                                Function.Call((Hash)0xD710A5007C2AC539, pPID, 0xB6B63737, 0);
                                Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                                skinPlayer["Beard"] = -1;
                            }
                            else
                            {
                                Function.Call((Hash)0xD3A7B003ED343FD9, pPID, SkinsUtils.BEARD_MALE[_newIndex - 1], true, false, true);
                                skinPlayer["Beard"] = SkinsUtils.BEARD_MALE[_newIndex - 1];
                            }
                        }
                        else
                        {
                            //Function.Call((Hash)0xD3A7B003ED343FD9, pPID, SkinsUtils.TEETH_FEMALE[_newIndex], true, true, true);
                        }
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                        break;
                    /**case 40:
                        if (model == "mp_male")
                        {
                          
                            //Function.Call((Hash)0xD3A7B003ED343FD9, pPID, SkinsUtils.MUSTACHE_MALE[_newIndex], true, true, true);
                        }
                        else
                        {
                            //Function.Call((Hash)0xD3A7B003ED343FD9, pPID, SkinsUtils.TEETH_FEMALE[_newIndex], true, true, true);
                        }
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                        break;*/
                    case 40:
                        if (model == "mp_male")
                        {
                            if (_newIndex == 0)
                            {
                                Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x864B03AE, 0);
                                Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                                skinPlayer["Hair"] = -1;
                            }
                            else
                            {
                                Function.Call((Hash)0xD3A7B003ED343FD9, pPID, SkinsUtils.HAIR_MALE[_newIndex - 1], true, true, true);
                                skinPlayer["Hair"] = SkinsUtils.HAIR_MALE[_newIndex - 1];
                            }

                        }
                        else
                        {
                            if (_newIndex == 0)
                            {
                                Function.Call((Hash)0xD710A5007C2AC539, pPID, 0x864B03AE, 0);
                                Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, 0);
                                skinPlayer["Hair"] = -1;
                            }
                            else
                            {
                                Function.Call((Hash)0xD3A7B003ED343FD9, pPID, SkinsUtils.HAIR_FEMALE[_newIndex - 1], true, true, true);
                                skinPlayer["Hair"] = SkinsUtils.HAIR_FEMALE[_newIndex - 1];
                            }
                        }
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                        break;
                    case 41:
                        Function.Call((Hash)0x1902C4CFCC5BE57C, pPID, SkinsUtils.BODY_TYPES[_newIndex]);
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                        skinPlayer["Body"] = SkinsUtils.BODY_TYPES[_newIndex];
                        break;
                    case 42:
                        Function.Call((Hash)0x1902C4CFCC5BE57C, pPID, SkinsUtils.WAIST_TYPES[_newIndex]);
                        Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
                        skinPlayer["Waist"] = SkinsUtils.WAIST_TYPES[_newIndex];
                        break;
                }
            };

            mcc.OnItemSelect += (_menu, _item, _index) =>
            {
                // Code in here would get executed whenever an item is pressed.
                if (_index == 43)
                {
                    isInCharCreation = false;
                    DressUp(model);
                    mcc.CloseMenu();
                }
            };


            mcc.OnMenuClose += (_menu) =>
            {
                // Code in here gets triggered whenever the menu is closed.
                if (isInCharCreation)
                {
                    mcc.OpenMenu();
                }

            };

            mcc.OpenMenu();
        }
        /// <summary>
        /// Menu Vestidor
        /// </summary>
        /// <param name="model"></param>
        public void MenuDressUpCharacter(string model, bool isCommand)
        {
            MenuController.Menus.Clear();

            //Definimos el nombre y subtitlo del menu con un constructor
            Menu mdu = new Menu(Language.Langs["TitleMenuClothes"], Language.Langs["SubTitleMenuClothes"]);
            MenuController.AddMenu(mdu);
            MenuController.MenuToggleKey = (Control)0;

            List<string> hatType = new List<string>();
            hatType.Add(Language.Langs["NoHatsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.HATS_MALE.Count + 1; i++)
                {
                    hatType.Add(Language.Langs["HatsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.HATS_FEMALE.Count + 1; i++)
                {
                    hatType.Add(Language.Langs["HatsValue"] + i);
                }
            }
            MenuListItem mListHats = new MenuListItem(Language.Langs["Hats"], hatType, 0, Language.Langs["HatsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListHats); // Lo añadimos al menu

            List<string> eyeWearType = new List<string>();
            eyeWearType.Add(Language.Langs["NoGlassesValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.EYEWEAR_MALE.Count + 1; i++) 
                {
                    eyeWearType.Add(Language.Langs["GlassesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.EYEWEAR_FEMALE.Count + 1; i++) 
                {
                    eyeWearType.Add(Language.Langs["GlassesValue"] + i);
                }
            }
            MenuListItem mListEyeWear = new MenuListItem(Language.Langs["Glasses"], eyeWearType, 0, Language.Langs["GlassesDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListEyeWear); // Lo añadimos al menu

            List<string> neckWearType = new List<string>();
            neckWearType.Add(Language.Langs["NoNeckwearValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.NECKWEAR_MALE.Count + 1; i++)
                {
                    neckWearType.Add(Language.Langs["NeckwearValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.NECKWEAR_FEMALE.Count + 1; i++)
                {
                    neckWearType.Add(Language.Langs["NeckwearValue"] + i);
                }
            }
            MenuListItem mListNeckWear = new MenuListItem(Language.Langs["Neckwear"], neckWearType, 0, Language.Langs["NeckwearDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListNeckWear); // Lo añadimos al menu

            List<string> neckTiesType = new List<string>();
            neckTiesType.Add(Language.Langs["NoTiesValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.NECKTIES_MALE.Count + 1; i++)
                {
                    neckTiesType.Add(Language.Langs["TiesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.NECKTIES_FEMALE.Count + 1; i++)
                {
                    neckTiesType.Add(Language.Langs["TiesValue"] + i);
                }
            }
            MenuListItem mListNeckTies = new MenuListItem(Language.Langs["Ties"], neckTiesType, 0, Language.Langs["TiesDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListNeckTies); // Lo añadimos al menu

            List<string> shirtsType = new List<string>();
            shirtsType.Add(Language.Langs["NoShirtsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.SHIRTS_MALE.Count + 1; i++)
                {
                    shirtsType.Add(Language.Langs["ShirtsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SHIRTS_FEMALE.Count + 1; i++)
                {
                    shirtsType.Add(Language.Langs["ShirtsValue"] + i);
                }
            }
            MenuListItem mListShirts = new MenuListItem(Language.Langs["Shirts"], shirtsType, 0, Language.Langs["ShirtsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListShirts); // Lo añadimos al menu

            List<string> suspendersType = new List<string>();
            suspendersType.Add(Language.Langs["NoSuspendersValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.SUSPENDERS_MALE.Count + 1; i++)
                {
                    suspendersType.Add(Language.Langs["SuspendersValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SUSPENDERS_FEMALE.Count + 1; i++)
                {
                    suspendersType.Add(Language.Langs["SuspendersValue"] + i);
                }
            }
            MenuListItem mListSuspenders = new MenuListItem(Language.Langs["Suspenders"], suspendersType, 0, Language.Langs["SuspendersDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListSuspenders); // Lo añadimos al menu


            List<string> vestType = new List<string>();
            vestType.Add(Language.Langs["NoVestsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.VEST_MALE.Count + 1; i++)
                {
                    vestType.Add(Language.Langs["VestsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.VEST_FEMALE.Count + 1; i++)
                {
                    vestType.Add(Language.Langs["VestsValue"] + i);
                }
            }
            MenuListItem mListVest = new MenuListItem(Language.Langs["Vests"], vestType, 0, Language.Langs["VestsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListVest); // Lo añadimos al menu

            List<string> coatsType = new List<string>();
            coatsType.Add(Language.Langs["NoCoatsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.COATS_MALE.Count + 1; i++)
                {
                    coatsType.Add(Language.Langs["CoatsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.COATS_FEMALE.Count + 1; i++)
                {
                    coatsType.Add(Language.Langs["CoatsValue"] + i);
                }
            }
            MenuListItem mListCoats = new MenuListItem(Language.Langs["Coats"], coatsType, 0, Language.Langs["CoatsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListCoats); // Lo añadimos al menu

            List<string> ponchosType = new List<string>();
            ponchosType.Add(Language.Langs["NoPonchosValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.PONCHOS_MALE.Count + 1; i++)
                {
                    ponchosType.Add(Language.Langs["PonchosValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.PONCHOS_FEMALE.Count + 1; i++)
                {
                    ponchosType.Add(Language.Langs["PonchosValue"] + i);
                }
            }
            MenuListItem mListPonchos = new MenuListItem(Language.Langs["Ponchos"], ponchosType, 0, Language.Langs["PonchosDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListPonchos); // Lo añadimos al menu

            List<string> cloakType = new List<string>();
            cloakType.Add(Language.Langs["NoCloaksValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.CLOAK_MALE.Count + 1; i++)
                {
                    cloakType.Add(Language.Langs["CloaksValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.CLOAK_FEMALE.Count + 1; i++)
                {
                    cloakType.Add(Language.Langs["CloaksValue"] + i);
                }
            }
            MenuListItem mListCloak = new MenuListItem(Language.Langs["Cloaks"], cloakType, 0, Language.Langs["CloaksDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListCloak); // Lo añadimos al menu

            List<string> glovesType = new List<string>();
            glovesType.Add(Language.Langs["NoGlovesValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.GLOVES_MALE.Count + 1; i++)
                {
                    glovesType.Add(Language.Langs["GlovesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.GLOVES_FEMALE.Count + 1; i++)
                {
                    glovesType.Add(Language.Langs["GlovesValue"] + i);
                }
            }
            MenuListItem mListGloves = new MenuListItem(Language.Langs["Gloves"], glovesType, 0, Language.Langs["GlovesDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListGloves); // Lo añadimos al menu

            List<string> ringsRhType = new List<string>();
            ringsRhType.Add(Language.Langs["NoRingsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.RINGS_RH_MALE.Count + 1; i++)
                {
                    ringsRhType.Add(Language.Langs["RingsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.RINGS_RH_FEMALE.Count + 1; i++)
                {
                    ringsRhType.Add(Language.Langs["RingsValue"] + i);
                }
            }
            MenuListItem mListRingsRhType = new MenuListItem(Language.Langs["RightRings"], ringsRhType, 0, Language.Langs["RightRingsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListRingsRhType); // Lo añadimos al menu

            List<string> ringsLhType = new List<string>();
            ringsLhType.Add(Language.Langs["NoRingsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.RINGS_LH_MALE.Count + 1; i++)
                {
                    ringsLhType.Add(Language.Langs["RingsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.RINGS_LH_FEMALE.Count + 1; i++)
                {
                    ringsLhType.Add(Language.Langs["RingsValue"] + i);
                }
            }
            MenuListItem mListRingsLh = new MenuListItem(Language.Langs["LeftRings"], ringsLhType, 0, Language.Langs["LeftRingsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListRingsLh); // Lo añadimos al menu

            List<string> braceletsType = new List<string>();
            braceletsType.Add(Language.Langs["NoBraceletsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BRACELETS_MALE.Count + 1; i++)
                {
                    braceletsType.Add(Language.Langs["BraceletsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BRACELETS_FEMALE.Count + 1; i++)
                {
                    braceletsType.Add(Language.Langs["BraceletsValue"] + i);
                }
            }
            MenuListItem mListbracelets = new MenuListItem(Language.Langs["Bracelets"], braceletsType, 0, Language.Langs["BraceletsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListbracelets); // Lo añadimos al menu

            List<string> gunbeltType = new List<string>();
            gunbeltType.Add(Language.Langs["NoHolstersValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.GUNBELT_MALE.Count + 1; i++)
                {
                    gunbeltType.Add(Language.Langs["HolstersValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.GUNBELT_FEMALE.Count + 1; i++)
                {
                    gunbeltType.Add(Language.Langs["HolstersValue"] + i);
                }
            }
            MenuListItem mListGunbelt = new MenuListItem(Language.Langs["PrimaryHolsters"], gunbeltType, 0, Language.Langs["PrimaryHolstersDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListGunbelt); // Lo añadimos al menu


            List<string> beltType = new List<string>();
            beltType.Add(Language.Langs["NoBeltsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BELT_MALE.Count + 1; i++)
                {
                    beltType.Add(Language.Langs["BeltsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BELT_FEMALE.Count + 1; i++)
                {
                    beltType.Add(Language.Langs["BeltsValue"] + i);
                }
            }
            MenuListItem mListBelt = new MenuListItem(Language.Langs["Belts"], beltType, 0, Language.Langs["BeltsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListBelt); // Lo añadimos al menu

            List<string> buckleType = new List<string>();
            buckleType.Add(Language.Langs["NoBucklesValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BUCKLE_MALE.Count + 1; i++)
                {
                    buckleType.Add(Language.Langs["BucklesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BUCKLE_FEMALE.Count + 1; i++)
                {
                    buckleType.Add(Language.Langs["BucklesValue"] + i);
                }
            }
            MenuListItem mListBuckle = new MenuListItem(Language.Langs["Buckles"], buckleType, 0, Language.Langs["BucklesDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListBuckle); // Lo añadimos al menu

            List<string> holstersSType = new List<string>();
            holstersSType.Add(Language.Langs["NoHolstersValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.HOLSTERS_S_MALE.Count + 1; i++)
                {
                    holstersSType.Add(Language.Langs["HolstersValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.HOLSTERS_S_FEMALE.Count + 1; i++)
                {
                    holstersSType.Add(Language.Langs["HolstersValue"] + i);
                }
            }
            MenuListItem mListSHolsters = new MenuListItem(Language.Langs["SecondaryHolsters"], holstersSType, 0, Language.Langs["SecondaryHolstersDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListSHolsters); // Lo añadimos al menu

            List<string> pantsType = new List<string>();
            pantsType.Add(Language.Langs["NoPantsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.PANTS_MALE.Count + 1; i++)
                {
                    pantsType.Add(Language.Langs["PantsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.PANTS_FEMALE.Count + 1; i++)
                {
                    pantsType.Add(Language.Langs["PantsValue"] + i);
                }
            }
            MenuListItem mListPants = new MenuListItem(Language.Langs["Pants"], pantsType, 0, Language.Langs["PantsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListPants); // Lo añadimos al menu

            List<string> skirtsType = new List<string>();
            skirtsType.Add(Language.Langs["NoSkirtsValue"]);

            if (model == "mp_male")
            {
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SKIRTS_FEMALE.Count + 1; i++)
                {
                    skirtsType.Add(Language.Langs["SkirtsValue"] + i);
                }
            }
            MenuListItem mListSkirts = new MenuListItem(Language.Langs["Skirts"], skirtsType, 0, Language.Langs["SkirtsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListSkirts); // Lo añadimos al menu

            List<string> chapsType = new List<string>();
            chapsType.Add(Language.Langs["NoChapsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.CHAPS_MALE.Count + 1; i++)
                {
                    chapsType.Add(Language.Langs["ChapsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.CHAPS_FEMALE.Count + 1; i++)
                {
                    chapsType.Add(Language.Langs["ChapsValue"] + i);
                }
            }
            MenuListItem mListChaps = new MenuListItem(Language.Langs["Chaps"], chapsType, 0, Language.Langs["ChapsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListChaps); // Lo añadimos al menu

            List<string> bootsType = new List<string>();
            bootsType.Add(Language.Langs["NoBootsValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BOOTS_MALE.Count + 1; i++)
                {
                    bootsType.Add(Language.Langs["BootsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BOOTS_FEMALE.Count + 1; i++)
                {
                    bootsType.Add(Language.Langs["BootsValue"] + i);
                }
            }
            MenuListItem mListBoots = new MenuListItem(Language.Langs["Boots"], bootsType, 0, Language.Langs["BootsDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListBoots); // Lo añadimos al menu

            List<string> spursType = new List<string>();
            spursType.Add(Language.Langs["NoSpursValue"]);

            if (model == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.SPURS_MALE.Count + 1; i++)
                {
                    spursType.Add(Language.Langs["SpursValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SPURS_FEMALE.Count + 1; i++)
                {
                    spursType.Add(Language.Langs["SpursValue"] + i);
                }
            }
            MenuListItem mListSpurs = new MenuListItem(Language.Langs["Spurs"], spursType, 0, Language.Langs["SpursDesc"]); // Añadimos la lista al boton
            mdu.AddMenuItem(mListSpurs); // Lo añadimos al menu

            //Terminamos y confirmamos
            mdu.AddMenuItem(new MenuItem(Language.Langs["Finish"], Language.Langs["FinishDesc"])
            {
                Enabled = true,
                LeftIcon = MenuItem.Icon.TICK
            });

            mdu.OnIndexChange += (_menu, _oldItem, _newItem, _oldIndex, _newIndex) =>
            {
                if (_newIndex == 21 || _newIndex == 22) // botas spurs
                {
                    API.SetCamCoord(Camera_DressUp, -561.0254f, -3775.601f, 238.4716f);
                    API.SetCamRot(Camera_DressUp, -42.500001f, 0.0f, -179.7251f, 0);
                }else if((_newIndex > 13 && _newIndex < 21) || _newIndex == 10 ) // cintura y pantalones
                {
                    API.SetCamCoord(Camera_DressUp, -561.0295f, -3775.138f, 237.8114f); 
                    API.SetCamRot(Camera_DressUp, 20.1059f, 0.0f, -177.3419f, 0);
                }
                else if (_newIndex == 12 || _newIndex == 13) // cintura y pantalones
                {
                    API.SetCamCoord(Camera_DressUp, -561.769f, -3776.043f, 238.42f);
                    API.SetCamRot(Camera_DressUp, 4.749468f, 0.0f, -121.8509f, 0);
                }
                else if (_newIndex == 11) // cintura y pantalones
                {
                    API.SetCamCoord(Camera_DressUp, -560.1163f, -3776.413f, 238.2835f);
                    API.SetCamRot(Camera_DressUp, 12.45644f, 0.0f, 95.26049f, 0);
                }
                else if (_newIndex == 23) // cintura y pantalones
                {
                    API.SetCamCoord(Camera_DressUp, -560.9759f, -3774.125f, 239.6413f);
                    API.SetCamRot(Camera_DressUp, -22.19865f, 0.0f, 179.3026f, 0);
                }
                else
                {
                    API.SetCamCoord(Camera_DressUp, -560.7456f, -3775.021f, 239.3405f);
                    API.SetCamRot(Camera_DressUp, -5.915717f, 0.0f, 179.2227f, 0);
                }
            };

            mdu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                // Code in here would get executed whenever the selected value of a list item changes (when left/right key is pressed).
                int pPID = API.PlayerPedId();
                
                switch (_itemIndex)
                {
                    // New System more simplificated
                    case 0:
                        SetPlayerComponent(model, _newIndex, 0x9925C067, "Hat", SkinsUtils.HATS_MALE, SkinsUtils.HATS_FEMALE);
                        break;
                    case 1:
                        SetPlayerComponent(model, _newIndex, 0x5E47CA6, "EyeWear", SkinsUtils.EYEWEAR_MALE, SkinsUtils.EYEWEAR_FEMALE);                        
                        break;
                    case 2:
                        SetPlayerComponent(model, _newIndex, 0x5FC29285, "NeckWear", SkinsUtils.NECKWEAR_MALE, SkinsUtils.NECKWEAR_FEMALE);
                        break;
                    case 3:
                        SetPlayerComponent(model, _newIndex, 0x7A96FACA, "NeckTies", SkinsUtils.NECKTIES_MALE, SkinsUtils.NECKTIES_FEMALE);                      
                        break;
                    case 4:
                        SetPlayerComponent(model, _newIndex, 0x2026C46D, "Shirt", SkinsUtils.SHIRTS_MALE, SkinsUtils.SHIRTS_FEMALE);
                        break;
                    case 5:
                        SetPlayerComponent(model, _newIndex, 0x877A2CF7, "Suspender", SkinsUtils.SUSPENDERS_MALE, SkinsUtils.SUSPENDERS_FEMALE);
                        break;
                    case 6:
                        SetPlayerComponent(model, _newIndex, 0x485EE834, "Vest", SkinsUtils.VEST_MALE, SkinsUtils.VEST_FEMALE);
                        break;
                    case 7:
                        SetPlayerComponent(model, _newIndex, 0xE06D30CE, "Coat", SkinsUtils.COATS_MALE, SkinsUtils.COATS_FEMALE);
                        break;
                    case 8:
                        SetPlayerComponent(model, _newIndex, 0xAF14310B, "Poncho", SkinsUtils.PONCHOS_MALE, SkinsUtils.PONCHOS_FEMALE);
                        break;
                    case 9:
                        SetPlayerComponent(model, _newIndex, 0x3C1A74CD, "Cloak", SkinsUtils.CLOAK_MALE, SkinsUtils.CLOAK_FEMALE);
                        break;
                    case 10:
                        SetPlayerComponent(model, _newIndex, 0xEABE0032, "Glove", SkinsUtils.GLOVES_MALE, SkinsUtils.GLOVES_FEMALE);
                        break;
                    case 11:
                        SetPlayerComponent(model, _newIndex, 0x7A6BBD0B, "RingRh", SkinsUtils.RINGS_RH_MALE, SkinsUtils.RINGS_RH_FEMALE);
                        break;
                    case 12:
                        SetPlayerComponent(model, _newIndex, 0xF16A1D23, "RingLh", SkinsUtils.RINGS_LH_MALE, SkinsUtils.RINGS_LH_FEMALE);
                        break;
                    case 13:
                        SetPlayerComponent(model, _newIndex, 0x7BC10759, "Bracelet", SkinsUtils.BRACELETS_MALE, SkinsUtils.BRACELETS_FEMALE);
                        break;
                    case 14:
                        SetPlayerComponent(model, _newIndex, 0x9B2C8B89, "Gunbelt", SkinsUtils.GUNBELT_MALE, SkinsUtils.GUNBELT_FEMALE);
                        break;
                    case 15:
                        SetPlayerComponent(model, _newIndex, 0xA6D134C6, "Belt", SkinsUtils.BELT_MALE, SkinsUtils.BELT_FEMALE);
                        break;
                    case 16:
                        SetPlayerComponent(model, _newIndex, 0xFAE9107F, "Buckle", SkinsUtils.BUCKLE_MALE, SkinsUtils.BUCKLE_FEMALE);
                        break;
                    case 17:
                        SetPlayerComponent(model, _newIndex, 0xB6B6122D, "Holster", SkinsUtils.HOLSTERS_S_MALE, SkinsUtils.HOLSTERS_S_FEMALE);
                        break;
                    case 18:
                        SetPlayerComponent(model, _newIndex, 0x1D4C528A, "Pant", SkinsUtils.PANTS_MALE, SkinsUtils.PANTS_FEMALE);
                        break;
                    case 19:
                        SetPlayerComponent(model, _newIndex, 0xA0E3AB7F, "Skirt", SkinsUtils.SKIRTS_FEMALE, SkinsUtils.SKIRTS_FEMALE);
                        break;
                    case 20:
                        SetPlayerComponent(model, _newIndex, 0x3107499B, "Chap", SkinsUtils.CHAPS_MALE, SkinsUtils.CHAPS_FEMALE);
                        break;
                    case 21:
                        SetPlayerComponent(model, _newIndex, 0x777EC6EF, "Boots", SkinsUtils.BOOTS_MALE, SkinsUtils.BOOTS_FEMALE);
                        break;
                    case 22:
                        SetPlayerComponent(model, _newIndex, 0x18729F39, "Spurs", SkinsUtils.SPURS_MALE, SkinsUtils.SPURS_FEMALE);
                        break;
                }
            };

            mdu.OnItemSelect += (_menu, _item, _index) =>
            {
                // Code in here would get executed whenever an item is pressed.
                if (_index == 23)
                {
                    isDressUpPed = false;
                    SaveChanges();
                    mdu.CloseMenu();
                }
            };

            mdu.OnMenuClose += (_menu) =>
            {
                // Code in here gets triggered whenever the menu is closed.
                if (isDressUpPed)
                {
                    mdu.OpenMenu();
                }
                
            };


            mdu.OpenMenu();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="_newIndex"></param>
        /// <param name="category"></param>
        /// <param name="idlist"></param>
        /// <param name="male_components"></param>
        /// <param name="female_components"></param>
        private void SetPlayerComponent(string model, int _newIndex, uint category, string idlist, List<uint> male_components, List<uint> female_components)
        {
            int pPID = API.PlayerPedId();
            if (model == "mp_male")
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
            Function.Call((Hash)0xCC8CA3E88256E58F, pPID, 0, 1, 1, 1, false);
        }

        private async Task SaveChanges()
        {
            TriggerEvent("vorpinputs:getInput", Language.Langs["ButtonInputName"], Language.Langs["PlaceHolderInputName"], new Action<dynamic>(async (cb) =>
            {
                string result = cb;
                await Delay(1000);
                if (result.Length < 3) {
                    TriggerEvent("vorp:Tip", Language.Langs["PlaceHolderInputName"], 3000); // from client side
                    SaveChanges();
                }
                else
                {
                    TriggerServerEvent("vorpcharacter:SaveSkinDB", skinPlayer, clothesPlayer, result);
                    StopCreation();
                    StartAnim();
                }
            }));
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

            Camera_DressUp = API.CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", -560.7456f, -3775.021f, 239.3405f, -5.915717f, 0.0f, 179.2227f, 45.00f, false, 0);

            uint HashVeh = (uint)API.GetHashKey("hotAirBalloon01");
            await Miscellanea.LoadModel(HashVeh);

            uint HashPed = (uint)API.GetHashKey("CS_balloonoperator");
            await Miscellanea.LoadModel(HashPed);

        }

        private async Task DeleteAll()
        {
            API.SetCamActive(Camera, false);
            API.DestroyCam(Camera, true);
            API.SetCamActive(Camera_Male, false);
            API.DestroyCam(Camera_Male, true);
            API.SetCamActive(Camera_Female, false);
            API.DestroyCam(Camera_Female, true);
            API.SetCamActive(Camera_Editor, false);
            API.DestroyCam(Camera_Editor, true);
            API.SetCamActive(Camera_DressUp, false);
            API.DestroyCam(Camera_DressUp, true);
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
            int pID = API.PlayerId();
            int pPedID = API.PlayerPedId();
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
            MenuCreateCharacter(model);
        }

        private async void DressUp(string model)
        {
            int pPedID = API.PlayerPedId();
            API.FreezeEntityPosition(pPedID, false);
            Miscellanea.TeleportToCoords(-560.9953f, -3776.425f, 237.60f, DressHeading);
            //API.FreezeEntityPosition(pPedID, true);
            API.RenderScriptCams(false, true, 2000, true, true, 0);
            await Delay(1000);
            API.SetCamActive(Camera_DressUp, true);
            API.RenderScriptCams(true, true, 1000, true, true, 0);
            isDressUpPed = true;
            MenuDressUpCharacter(model, false);
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
                await DrawTxt(Language.Langs["PressRightOrLeft"], 0.5f, 0.9f, 0.7f, 0.7f, 255, 255, 255, 255, true, true);
            }

            if (isInCharCreation) //Fix Run Ped
            {
                API.FreezeEntityPosition(API.PlayerPedId(), true);
                API.ClearPedTasks(API.PlayerPedId(), 1, 1);
            }


            if (isDressUpPed) //Feature Rotate Cam
            {
                API.FreezeEntityPosition(API.PlayerPedId(), true);
                API.ClearPedTasks(API.PlayerPedId(), 1, 1);

                if (API.IsControlPressed(2, (uint)Controls.Keys.MoveLeftOnly))
                {
                    DressHeading += 1.0f;
                    API.SetEntityHeading(API.PlayerPedId(), DressHeading);
                    Debug.WriteLine(DressHeading.ToString());
                }
                else if (API.IsControlPressed(2, (uint)Controls.Keys.MoveRightOnly))
                {
                    DressHeading -= 1.0f;
                    API.SetEntityHeading(API.PlayerPedId(), DressHeading);
                    Debug.WriteLine(DressHeading.ToString());
                }
              
                await Delay(0);
            }
            else
            {
                API.DrawLightWithRange(-560.1646f, -3782.066f, 238.5975f, 255, 255, 255, 7.0f, 150.0f);
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
