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
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(skinMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            if (vorpcore_cl.CreatePlayer.model_selected == "mp_male") // Male
            {
                //Body Colors
                List<string> bodyValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["BodyColorValues"] + i);
                }

                MenuListItem btnSelectorBody = new MenuListItem(GetConfig.Langs["BodyColor"], bodyValues, 0, GetConfig.Langs["BodyColorDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu

                //Faces
                List<string> faceValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"][0]["Faces"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["FaceValues"] + i);
                }

                MenuListItem btnSelectorFace = new MenuListItem(GetConfig.Langs["FaceType"], faceValues, 0, GetConfig.Langs["FaceTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu
            }
            else //Female
            {
                //Body Colors
                List<string> bodyValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["BodyColorValues"] + i);
                }

                MenuListItem btnSelectorBody = new MenuListItem(GetConfig.Langs["BodyColor"], bodyValues, 0, GetConfig.Langs["BodyColorDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu

                //Faces
                List<string> faceValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"][0]["Faces"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["FaceValues"] + i);
                }

                MenuListItem btnSelectorFace = new MenuListItem(GetConfig.Langs["FaceType"], faceValues, 0, GetConfig.Langs["FaceTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu
            }

            

            skinMenu.OnMenuOpen += (_menu) => {

            };

            skinMenu.OnMenuClose += (_menu) =>
            {

            };

            skinMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                // Code in here would get executed whenever an item is pressed.
                if (_index == 3)
                {
                    //isDressUpPed = false;
                    //SaveChanges();
                    //mdu.CloseMenu();
                }
            };

        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return skinMenu;
        }
    }
}
