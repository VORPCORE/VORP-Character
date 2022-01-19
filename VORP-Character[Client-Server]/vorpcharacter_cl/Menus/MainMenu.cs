using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;
using VorpCharacter.Script;
using VorpCharacter.Utils;

namespace VorpCharacter.Menus
{
    class MainMenu
    {
        private static Menu mainMenu = new Menu(PluginManager.Langs["TitleMainMenu"], PluginManager.Langs["SubTitleMainMenu"]);
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(mainMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            //SkinMenu
            MenuController.AddSubmenu(mainMenu, SkinMenu.GetMenu());

            MenuItem subMenuSkinBtn = new MenuItem(PluginManager.Langs["TitleSkinMenu"], PluginManager.Langs["SubTitleSkinMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuSkinBtn);
            MenuController.BindMenuItem(mainMenu, SkinMenu.GetMenu(), subMenuSkinBtn);

            //FaceMenu
            MenuController.AddSubmenu(mainMenu, FaceMenu.GetMenu());

            MenuItem subMenuFaceBtn = new MenuItem(PluginManager.Langs["TitleFaceMenu"], PluginManager.Langs["SubTitleFaceMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuFaceBtn);
            MenuController.BindMenuItem(mainMenu, FaceMenu.GetMenu(), subMenuFaceBtn);

            //ClothesMenu
            MenuController.AddSubmenu(mainMenu, ClothesMenu.GetMenu());

            MenuItem subMenuClothesBtn = new MenuItem(PluginManager.Langs["TitleClothesMenu"], PluginManager.Langs["SubTitleClothesMenu"])
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuClothesBtn);
            MenuController.BindMenuItem(mainMenu, ClothesMenu.GetMenu(), subMenuClothesBtn);

            List<string> scaleValues = new List<string>();

            foreach (float sc in Utils.SkinsUtils.SCALE_LIST)
            {
                scaleValues.Add(PluginManager.Langs["Scale"] + sc.ToString());
            }

            MenuListItem ScaleBtn = new MenuListItem(PluginManager.Langs["ScaleList"], scaleValues, 4, PluginManager.Langs["ScaleDesc"])
            {
                RightIcon = MenuItem.Icon.STAR
            };
            mainMenu.AddMenuItem(ScaleBtn);

            //Finish Button
            MenuItem FinishBtn = new MenuItem(PluginManager.Langs["FinishBtnMainMenu"], PluginManager.Langs["SubFinishBtnMainMenu"])
            {
                RightIcon = MenuItem.Icon.TICK
            };
            mainMenu.AddMenuItem(FinishBtn);

            //Events
            mainMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                if (_itemIndex == 3)
                {
                    CreateCharacter.changeScale(SkinsUtils.SCALE_LIST[_newIndex]);
                }
            };

            mainMenu.OnMenuClose += (_menu) =>
            {
                if (CreateCharacter.isInCharCreation)
                {
                    CreateCharacter.CloseSecureMenu();
                }
            };

            mainMenu.OnItemSelect += (_menu, _item, _index) =>
            {
                // Code in here would get executed whenever an item is pressed.
                if (_index == 4)
                {
                    CreateCharacter.isInCharCreation = false;
                    CreateCharacter.SaveChanges();
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
