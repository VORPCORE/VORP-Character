using CitizenFX.Core;
using MenuAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vorpcharacter_cl.Utils;

namespace vorpcharacter_cl.Menus
{
    class FaceMenu
    {
        private static Menu faceMenu = new Menu(Language.Langs["TitleFaceMenu"], Language.Langs["SubTitleFaceMenu"]);
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(faceMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            //Weapons Buy Menu
            //MenuController.AddSubmenu(mainMenu, BuyMenu.GetMenu());

            //MenuItem subMenuBuyBtn = new MenuItem(GetConfig.Langs["MenuMainButtonBuyWeapons"], " ")
            //{
            //    RightIcon = MenuItem.Icon.ARROW_RIGHT
            //};

            //mainMenu.AddMenuItem(subMenuBuyBtn);
            //MenuController.BindMenuItem(mainMenu, BuyMenu.GetMenu(), subMenuBuyBtn);



            faceMenu.OnMenuOpen += (_menu) => {

            };

            faceMenu.OnMenuClose += (_menu) =>
            {

            };

            faceMenu.OnItemSelect += (_menu, _item, _index) =>
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
            return faceMenu;
        }
    }
}
