using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;
using System.Linq;
using VorpCharacter.Extensions;
using VorpCharacter.Script;
using VorpCharacter.Utils;

namespace VorpCharacter.Menus
{
    class MainMenu
    {
        private static Menu mainMenu;
        private static List<string> ScaleList = SkinsUtils.SCALE_LIST.Select(x => $"{x}").ToList();

        private static MenuListItem miScaleList;
        private static MenuItem miFinish;

        private static void AddSubMenu(Menu menu, Menu subMenu, string titleTranslationKey, string subtitleTranslationKey)
        {
            MenuController.AddSubmenu(menu, subMenu);

            MenuItem subMenuButton = new MenuItem(Common.GetTranslation(titleTranslationKey), Common.GetTranslation(subtitleTranslationKey))
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            menu.AddMenuItem(subMenuButton);
            MenuController.BindMenuItem(menu, subMenu, subMenuButton);
        }

        private static void SetupMenu()
        {
            if (mainMenu is not null) return;
            mainMenu = new Menu(Common.GetTranslation("TitleMainMenu"), Common.GetTranslation("SubTitleMainMenu"));
            MenuController.AddMenu(mainMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            AddSubMenu(mainMenu, SkinMenu.SetupMenu(), "TitleSkinMenu", "SubTitleSkinMenu");
            AddSubMenu(mainMenu, FaceMenu.GetMenu(), "TitleFaceMenu", "SubTitleFaceMenu");
            AddSubMenu(mainMenu, ClothesMenu.GetMenu(), "TitleClothesMenu", "SubTitleClothesMenu");

            miScaleList = new MenuListItem(Common.GetTranslation("ScaleList"), ScaleList, 4, Common.GetTranslation("ScaleDesc"))
            {
                RightIcon = MenuItem.Icon.STAR
            };
            mainMenu.AddMenuItem(miScaleList);

            //Finish Button
            miFinish = new MenuItem(Common.GetTranslation("FinishBtnMainMenu"), Common.GetTranslation("SubFinishBtnMainMenu"))
            {
                RightIcon = MenuItem.Icon.TICK
            };
            mainMenu.AddMenuItem(miFinish);

            //Events
            mainMenu.OnListIndexChange += async (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                if (_itemIndex == 3)
                {
                    await Utilities.SetPedScale(Cache.PlayerPedId, SkinsUtils.SCALE_LIST[_newIndex]);
                }
            };

            mainMenu.OnMenuClose += MainMenu_OnMenuClose;
            mainMenu.OnItemSelect += MainMenu_OnItemSelect;
        }

        private static void MainMenu_OnMenuClose(Menu menu)
        {
            if (CreateCharacter.isInCharCreation)
            {
                CreateCharacter.CloseSecureMenu();
            }
        }

        private static void MainMenu_OnItemSelect(Menu menu, MenuItem menuItem, int itemIndex)
        {
            if (menuItem == miFinish)
            {
                CreateCharacter.isInCharCreation = false;
                CreateCharacter.SaveChanges();
                mainMenu.CloseMenu();
            }
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return mainMenu;
        }

    }
}
