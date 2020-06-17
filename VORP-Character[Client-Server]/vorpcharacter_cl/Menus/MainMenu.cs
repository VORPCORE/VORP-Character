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
    class MainMenu
    {
        private static Menu mainMenu = new Menu(GetConfig.Langs["TitleMainMenu"], GetConfig.Langs["SubTitleMainMenu"]);
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(mainMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            //ImportMenu
            MenuController.AddSubmenu(mainMenu, ImportMenu.GetMenu());

            MenuItem subMenuImportBtn = new MenuItem(GetConfig.Langs["TitleImportMenu"], GetConfig.Langs["SubTitleImportMenu"])
            {
                RightIcon = MenuItem.Icon.STAR
            };

            mainMenu.AddMenuItem(subMenuImportBtn);
            MenuController.BindMenuItem(mainMenu, ImportMenu.GetMenu(), subMenuImportBtn);

            //SkinMenu
            MenuController.AddSubmenu(mainMenu, SkinMenu.GetMenu());

            MenuItem subMenuSkinBtn = new MenuItem(GetConfig.Langs["TitleSkinMenu"], GetConfig.Langs["SubTitleSkinMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuSkinBtn);
            MenuController.BindMenuItem(mainMenu, SkinMenu.GetMenu(), subMenuSkinBtn);

            //FaceMenu
            MenuController.AddSubmenu(mainMenu, FaceMenu.GetMenu());

            MenuItem subMenuFaceBtn = new MenuItem(GetConfig.Langs["TitleFaceMenu"], GetConfig.Langs["SubTitleFaceMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuFaceBtn);
            MenuController.BindMenuItem(mainMenu, FaceMenu.GetMenu(), subMenuFaceBtn);

            //ClothesMenu
            MenuController.AddSubmenu(mainMenu, ClothesMenu.GetMenu());

            MenuItem subMenuClothesBtn = new MenuItem(GetConfig.Langs["TitleClothesMenu"], GetConfig.Langs["SubTitleClothesMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuClothesBtn);
            MenuController.BindMenuItem(mainMenu, ClothesMenu.GetMenu(), subMenuClothesBtn);

            //Finish Button
            MenuItem FinishBtn = new MenuItem(GetConfig.Langs["FinishBtnMainMenu"], GetConfig.Langs["SubFinishBtnMainMenu"])
            {
                RightIcon = MenuItem.Icon.TICK
            };
            mainMenu.AddMenuItem(FinishBtn);

            //Events
            mainMenu.OnMenuOpen += (_menu) => {
                
            };

            mainMenu.OnMenuClose += (_menu) =>
            {
                if (CreatePlayer.isInCharCreation)
                {
                    CreatePlayer.CloseSecureMenu();
                }
            };

            mainMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                // Code in here would get executed whenever an item is pressed.
                if (_index == 4)
                {
                    CreatePlayer.isInCharCreation = false;
                    CreatePlayer.SaveChanges();
                    mainMenu.CloseMenu();
                }
            };

        }



        public static Menu GetMenu()
        {
            SetupMenu();
            return mainMenu;
        }

    }
}
