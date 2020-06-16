using CitizenFX.Core;
using MenuAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vorpcharacter_cl.Utils;
using vorpcore_cl;

namespace vorpcharacter_cl.Menus
{
    class MainMenu
    {
        private static Menu mainMenu = new Menu(Language.Langs["TitleMainMenu"], Language.Langs["SubTitleMainMenu"]);
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

            MenuItem subMenuImportBtn = new MenuItem(Language.Langs["TitleImportMenu"], Language.Langs["SubTitleImportMenu"])
            {
                RightIcon = MenuItem.Icon.STAR
            };

            mainMenu.AddMenuItem(subMenuImportBtn);
            MenuController.BindMenuItem(mainMenu, ImportMenu.GetMenu(), subMenuImportBtn);

            //SkinMenu
            MenuController.AddSubmenu(mainMenu, SkinMenu.GetMenu());

            MenuItem subMenuSkinBtn = new MenuItem(Language.Langs["TitleSkinMenu"], Language.Langs["SubTitleSkinMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuSkinBtn);
            MenuController.BindMenuItem(mainMenu, SkinMenu.GetMenu(), subMenuSkinBtn);

            //FaceMenu
            MenuController.AddSubmenu(mainMenu, FaceMenu.GetMenu());

            MenuItem subMenuFaceBtn = new MenuItem(Language.Langs["TitleFaceMenu"], Language.Langs["SubTitleFaceMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuFaceBtn);
            MenuController.BindMenuItem(mainMenu, FaceMenu.GetMenu(), subMenuFaceBtn);

            //ClothesMenu
            MenuController.AddSubmenu(mainMenu, ClothesMenu.GetMenu());

            MenuItem subMenuClothesBtn = new MenuItem(Language.Langs["TitleClothesMenu"], Language.Langs["SubTitleClothesMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuClothesBtn);
            MenuController.BindMenuItem(mainMenu, ClothesMenu.GetMenu(), subMenuClothesBtn);


            mainMenu.OnMenuOpen += (_menu) => {
                
            };

            mainMenu.OnMenuClose += (_menu) =>
            {

            };

            mainMenu.OnItemSelect += (_menu, _item, _index) =>
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
            return mainMenu;
        }

    }
}
