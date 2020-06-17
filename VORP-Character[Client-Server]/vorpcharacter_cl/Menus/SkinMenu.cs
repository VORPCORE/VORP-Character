using CitizenFX.Core;
using MenuAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vorpcharacter_cl.Utils;

namespace vorpcharacter_cl.Menus
{
    class SkinMenu
    {
        private static Menu skinMenu = new Menu(GetConfig.Langs["TitleSkinMenu"], GetConfig.Langs["SubTitleSkinMenu"]);

        private static MenuListItem btnSelectorBody;
        private static MenuListItem btnSelectorFace;
        private static MenuListItem btnSelectorTorso;
        private static MenuListItem btnSelectorLegs;
            

        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(skinMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            if (CreatePlayer.model_selected == "mp_male") // Male
            {
                //Body Colors
                List<string> bodyValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["BodyColorValues"] + i);
                }

                btnSelectorBody = new MenuListItem(GetConfig.Langs["BodyColor"], bodyValues, 0, GetConfig.Langs["BodyColorDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu

                //Faces
                List<string> faceValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"][0]["Heads"].Count(); i++)
                {
                    faceValues.Add(GetConfig.Langs["FaceValues"] + i);
                }

                btnSelectorFace = new MenuListItem(GetConfig.Langs["FaceType"], faceValues, 0, GetConfig.Langs["FaceTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorFace); // Lo añadimos al menu

                //Torso
                List<string> torsoValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"][0]["Body"].Count(); i++)
                {
                    torsoValues.Add(GetConfig.Langs["TorsoValues"] + i);
                }

                btnSelectorTorso = new MenuListItem(GetConfig.Langs["TorsoType"], torsoValues, 0, GetConfig.Langs["TorsoTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorTorso); // Lo añadimos al menu

                //Endoforma
                List<string> endoformValues = new List<string>();

                for (int i = 1; i < SkinsUtils.BODY_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    endoformValues.Add(GetConfig.Langs["BodySizeValue"] + i);
                }

                MenuListItem btnSelectorBodyForm = new MenuListItem(GetConfig.Langs["BodyType"], endoformValues, 0, GetConfig.Langs["BodyTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBodyForm); // Lo añadimos al menu

                //Waist
                List<string> waistValues = new List<string>();

                for (int i = 1; i < SkinsUtils.WAIST_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    waistValues.Add(GetConfig.Langs["WaistValue"] + i);
                }

                MenuListItem btnSelectorWaist = new MenuListItem(GetConfig.Langs["WaistType"], waistValues, 0, GetConfig.Langs["WaistTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorWaist); // Lo añadimos al menu

                //Legs
                List<string> legsValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"][0]["Legs"].Count(); i++)
                {
                    legsValues.Add(GetConfig.Langs["LegsValues"] + i);
                }

                btnSelectorLegs = new MenuListItem(GetConfig.Langs["LegsType"], legsValues, 0, GetConfig.Langs["LegsTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLegs); // Lo añadimos al menu
                
                //Hair
                List<string> hairValues = new List<string>();

                for (float i = 1; i < SkinsUtils.HAIR_MALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    hairValues.Add(GetConfig.Langs["HairValue"] + i);
                }

                MenuListItem btnSelectorHairs = new MenuListItem(GetConfig.Langs["HairType"], hairValues, 0, GetConfig.Langs["HairTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorHairs); // Lo añadimos al menu

                //Beard
                List<string> beardValues = new List<string>();

                for (float i = 1; i < SkinsUtils.BEARD_MALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    beardValues.Add(GetConfig.Langs["BeardValue"] + i);
                }
                //beardType.Add(GetConfig.Langs["NoExistValue"]);

                MenuListItem btnSelectorBeards = new MenuListItem(GetConfig.Langs["BeardType"], beardValues, 0, GetConfig.Langs["BeardTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBeards); // Lo añadimos al menu


            }
            else //Female
            {
                //Body Colors
                List<string> bodyValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["BodyColorValues"] + i);
                }

                btnSelectorBody = new MenuListItem(GetConfig.Langs["BodyColor"], bodyValues, 0, GetConfig.Langs["BodyColorDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu

                //Faces
                List<string> faceValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"][0]["Heads"].Count(); i++)
                {
                    faceValues.Add(GetConfig.Langs["FaceValues"] + i);
                }

                btnSelectorFace = new MenuListItem(GetConfig.Langs["FaceType"], faceValues, 0, GetConfig.Langs["FaceTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorFace); // Lo añadimos al menu

                //Torso
                List<string> torsoValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"][0]["Body"].Count(); i++)
                {
                    torsoValues.Add(GetConfig.Langs["TorsoValues"] + i);
                }

                btnSelectorTorso = new MenuListItem(GetConfig.Langs["TorsoType"], torsoValues, 0, GetConfig.Langs["TorsoTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorTorso); // Lo añadimos al menu

                //Endoforma
                List<string> endoformValues = new List<string>();

                for (int i = 1; i < SkinsUtils.BODY_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    endoformValues.Add(GetConfig.Langs["BodySizeValue"] + i);
                }

                MenuListItem btnSelectorBodyForm = new MenuListItem(GetConfig.Langs["BodyType"], endoformValues, 0, GetConfig.Langs["BodyTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBodyForm); // Lo añadimos al menu

                //Waist
                List<string> waistValues = new List<string>();

                for (int i = 1; i < SkinsUtils.WAIST_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    waistValues.Add(GetConfig.Langs["WaistValue"] + i);
                }

                MenuListItem btnSelectorWaist = new MenuListItem(GetConfig.Langs["WaistType"], waistValues, 0, GetConfig.Langs["WaistTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorWaist); // Lo añadimos al menu

                //Legs
                List<string> legsValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"][0]["Legs"].Count(); i++)
                {
                    legsValues.Add(GetConfig.Langs["LegsValues"] + i);
                }

                btnSelectorLegs = new MenuListItem(GetConfig.Langs["LegsType"], legsValues, 0, GetConfig.Langs["LegsTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLegs); // Lo añadimos al menu

                //Hair
                List<string> hairValues = new List<string>();

                for (float i = 1; i < SkinsUtils.HAIR_FEMALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    hairValues.Add(GetConfig.Langs["HairValue"] + i);
                }

                MenuListItem btnSelectorHairs = new MenuListItem(GetConfig.Langs["HairType"], hairValues, 0, GetConfig.Langs["HairTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorHairs); // Lo añadimos al menu
            }


            skinMenu.OnMenuOpen += (_menu) => {

            };

            skinMenu.OnMenuClose += (_menu) =>
            {

            };

            skinMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                switch (_itemIndex)
                {
                    case 0:
                        if (CreatePlayer.model_selected == "mp_male") // Male
                        {
                            UpdateLists(_newIndex, "Male");
                        }
                        else
                        {
                            UpdateLists(_newIndex, "Female");
                        }
                        break;
                    case 1:
                        if (CreatePlayer.model_selected == "mp_male") // Male
                        {
                            CreatePlayer.SetPlayerModelComponent(GetConfig.Config["Male"][btnSelectorBody.ListIndex]["Heads"][_newIndex].ToString(), "HeadType");
                        }
                        else
                        {
                            CreatePlayer.SetPlayerModelComponent(GetConfig.Config["Female"][btnSelectorBody.ListIndex]["Heads"][_newIndex].ToString(), "HeadType");
                        }
                        break;
                    case 2:
                        if (CreatePlayer.model_selected == "mp_male") // Male
                        {
                            CreatePlayer.SetPlayerModelComponent(GetConfig.Config["Male"][btnSelectorBody.ListIndex]["Body"][_newIndex].ToString(), "BodyType");
                        }
                        else
                        {
                            CreatePlayer.SetPlayerModelComponent(GetConfig.Config["Female"][btnSelectorBody.ListIndex]["Body"][_newIndex].ToString(), "BodyType");
                        }
                        break;
                    case 3:
                        CreatePlayer.SetPlayerBodyComponent((uint)SkinsUtils.BODY_TYPES.ElementAt(_newIndex), "Body");
                        break;
                    case 4:
                        CreatePlayer.SetPlayerBodyComponent((uint)SkinsUtils.WAIST_TYPES.ElementAt(_newIndex), "Waist");
                        break;
                    case 5:
                        if (CreatePlayer.model_selected == "mp_male") // Male 
                        {
                            CreatePlayer.SetPlayerModelComponent(GetConfig.Config["Male"][btnSelectorBody.ListIndex]["Legs"][_newIndex].ToString(), "LegsType");
                        }
                        else
                        {
                            CreatePlayer.SetPlayerModelComponent(GetConfig.Config["Female"][btnSelectorBody.ListIndex]["Legs"][_newIndex].ToString(), "LegsType");
                        }
                        break;
                    case 6:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.SetPlayerModelListComps("Hair", 0, 0x864B03AE);
                        }
                        else if (CreatePlayer.model_selected == "mp_male")
                        {
                            CreatePlayer.SetPlayerModelListComps("Hair", SkinsUtils.HAIR_MALE.ElementAt(_newIndex - 1), 0x864B03AE);
                        }
                        else
                        {
                            CreatePlayer.SetPlayerModelListComps("Hair", SkinsUtils.HAIR_FEMALE.ElementAt(_newIndex - 1), 0x864B03AE);
                        }
                        break;
                    case 7:
                        if (_newIndex == 0)
                        {
                            CreatePlayer.SetPlayerModelListComps("Beard", 0, 0xF8016BCA);
                        }
                        else if (CreatePlayer.model_selected == "mp_male")
                        {
                            CreatePlayer.SetPlayerModelListComps("Beard", SkinsUtils.BEARD_MALE.ElementAt(_newIndex - 1), 0xF8016BCA);
                        }
                        break;
                }
            };

        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return skinMenu;
        }

        private static void UpdateLists(int index, string sex)
        {
            btnSelectorFace.ListItems.Clear();
            btnSelectorTorso.ListItems.Clear();
            btnSelectorLegs.ListItems.Clear();

            //Faces
            List<string> faceValues = new List<string>();
            for (int i = 1; i <= GetConfig.Config[sex][index]["Heads"].Count(); i++)
            {
                btnSelectorFace.ListItems.Add(GetConfig.Langs["FaceValues"] + i);
            }
            btnSelectorFace.ListIndex = 0;

            //Torso
            List<string> torsoValues = new List<string>();
            for (int i = 1; i <= GetConfig.Config[sex][index]["Body"].Count(); i++)
            {
                btnSelectorTorso.ListItems.Add(GetConfig.Langs["TorsoValues"] + i);
            }
            btnSelectorTorso.ListIndex = 0;

            //Legs
            List<string> legsValues = new List<string>();
            for (int i = 1; i <= GetConfig.Config[sex][index]["Legs"].Count(); i++)
            {
                btnSelectorLegs.ListItems.Add(GetConfig.Langs["LegsValues"] + i);
            }
            btnSelectorLegs.ListIndex = 0;

            CreatePlayer.SetPlayerModelComponent(GetConfig.Config[sex][index]["Heads"][0].ToString(), "HeadType");
            CreatePlayer.SetPlayerModelComponent(GetConfig.Config[sex][index]["Body"][0].ToString(), "BodyType");
            CreatePlayer.SetPlayerModelComponent(GetConfig.Config[sex][index]["Legs"][0].ToString(), "LegsType");
        } 
    }
}
