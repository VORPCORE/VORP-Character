using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;
using VorpCharacter.Extensions;
using VorpCharacter.Script;

namespace VorpCharacter.Menus
{
    class FaceMenu
    {
        private static Menu faceMenu = new Menu(Common.GetTranslation("TitleFaceMenu"), Common.GetTranslation("SubTitleFaceMenu"));
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(faceMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            //Tipo de tamaños del 1 al 10
            List<string> sizeType = new List<string>();

            for (float i = 1; i < 21; i++) //find me
            {
                sizeType.Add(Common.GetTranslation("TypeValues") + i);
            }

            MenuListItem mListHead = new MenuListItem(Common.GetTranslation("FaceSize"), sizeType, 4, Common.GetTranslation("FaceSizeDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListHead); // Lo añadimos al menu

            //Altura cejas
            MenuListItem mListEyeBrowH = new MenuListItem(Common.GetTranslation("EyebrowHeight"), sizeType, 4, Common.GetTranslation("EyebrowHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowH); // Lo añadimos al menu
            //Anchura cejas
            MenuListItem mListEyeBrowW = new MenuListItem(Common.GetTranslation("EyebrowWidth"), sizeType, 4, Common.GetTranslation("EyebrowWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowW); // Lo añadimos al menu
            //Profundida cejas
            MenuListItem mListEyeBrowD = new MenuListItem(Common.GetTranslation("EyebrowDepth"), sizeType, 4, Common.GetTranslation("EyebrowDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowD); // Lo añadimos al menu

            //Altura Orejas
            MenuListItem mListEarsH = new MenuListItem(Common.GetTranslation("EarsHeight"), sizeType, 4, Common.GetTranslation("EarsHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsH); // Lo añadimos al menu
            //Anchura Orejas
            MenuListItem mListEarsW = new MenuListItem(Common.GetTranslation("EarsAngle"), sizeType, 4, Common.GetTranslation("EarsAngleDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsW); // Lo añadimos al menu
            //Tamaño Orejas
            MenuListItem mListEarsD = new MenuListItem(Common.GetTranslation("EarsSize"), sizeType, 4, Common.GetTranslation("EarsSizeDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsD); // Lo añadimos al menu
            //Tamaño Lobulo de la oreja
            MenuListItem mListEarsL = new MenuListItem(Common.GetTranslation("EarsLobeSize"), sizeType, 4, Common.GetTranslation("EarsLobeSizeDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsL); // Lo añadimos al menu

            //Altura de los parpados
            MenuListItem mListEyeLidH = new MenuListItem(Common.GetTranslation("EyelidsHeight"), sizeType, 4, Common.GetTranslation("EyelidsHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeLidH); // Lo añadimos al menu
            //Ancho de los parpados
            MenuListItem mListEyeLidW = new MenuListItem(Common.GetTranslation("EyelidsWidth"), sizeType, 4, Common.GetTranslation("EyelidsWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeLidW); // Lo añadimos al menu

            //Profundida de los ojos
            MenuListItem mListEyeD = new MenuListItem(Common.GetTranslation("EyeDepth"), sizeType, 4, Common.GetTranslation("EyeDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeD); // Lo añadimos al menu
            //Angulo de los ojos
            MenuListItem mListEyeAng = new MenuListItem(Common.GetTranslation("EyeAngle"), sizeType, 4, Common.GetTranslation("EyeAngleDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeAng); // Lo añadimos al menu
            //Distancia de los ojos
            MenuListItem mListEyeDis = new MenuListItem(Common.GetTranslation("EyeSeparation"), sizeType, 4, Common.GetTranslation("EyeSeparationDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeDis); // Lo añadimos al menu
            //Altura de los ojos
            MenuListItem mListEyeH = new MenuListItem(Common.GetTranslation("EyeHeight"), sizeType, 4, Common.GetTranslation("EyeHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeH); // Lo añadimos al menu


            MenuListItem mListNoseW = new MenuListItem(Common.GetTranslation("NoseWidth"), sizeType, 4, Common.GetTranslation("NoseWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseW); // Lo añadimos al menu

            MenuListItem mListNoseS = new MenuListItem(Common.GetTranslation("NoseSize"), sizeType, 4, Common.GetTranslation("NoseSizeDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseS); // Lo añadimos al menu

            MenuListItem mListNoseH = new MenuListItem(Common.GetTranslation("NoseHeight"), sizeType, 4, Common.GetTranslation("NoseHeigthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseH); // Lo añadimos al menu

            MenuListItem mListNoseAng = new MenuListItem(Common.GetTranslation("NoseAngle"), sizeType, 4, Common.GetTranslation("NoseAngleDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseAng); // Lo añadimos al menu

            MenuListItem mListNoseC = new MenuListItem(Common.GetTranslation("NoseCurvature"), sizeType, 4, Common.GetTranslation("NoseCurvatureDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseC); // Lo añadimos al menu

            MenuListItem mListNoseDis = new MenuListItem(Common.GetTranslation("NostrilsSeparation"), sizeType, 4, Common.GetTranslation("NostrilsSeparationDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseDis); // Lo añadimos al menu


            MenuListItem mListCheekBonesH = new MenuListItem(Common.GetTranslation("CheekbonesHeight"), sizeType, 4, Common.GetTranslation("CheekbonesHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesH); // Lo añadimos al menu

            MenuListItem mListCheekBonesW = new MenuListItem(Common.GetTranslation("CheekbonesWidth"), sizeType, 4, Common.GetTranslation("CheekbonesWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesW); // Lo añadimos al menu

            MenuListItem mListCheekBonesD = new MenuListItem(Common.GetTranslation("CheekbonesDepth"), sizeType, 4, Common.GetTranslation("CheekbonesDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesD); // Lo añadimos al menu


            MenuListItem mListMouthW = new MenuListItem(Common.GetTranslation("MouthWidth"), sizeType, 4, Common.GetTranslation("MouthWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthW); // Lo añadimos al menu

            MenuListItem mListMouthD = new MenuListItem(Common.GetTranslation("MouthDepth"), sizeType, 4, Common.GetTranslation("MouthDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthD); // Lo añadimos al menu

            MenuListItem mListMouthX = new MenuListItem(Common.GetTranslation("MouthDeviation"), sizeType, 4, Common.GetTranslation("MouthDeviationDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthX); // Lo añadimos al menu

            MenuListItem mListMouthY = new MenuListItem(Common.GetTranslation("MouthHeight"), sizeType, 4, Common.GetTranslation("MouthHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthY); // Lo añadimos al menu


            MenuListItem mListULiphH = new MenuListItem(Common.GetTranslation("UpperLipHeight"), sizeType, 4, Common.GetTranslation("UpperLipHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphH); // Lo añadimos al menu
            MenuListItem mListULiphW = new MenuListItem(Common.GetTranslation("UpperLipWidth"), sizeType, 4, Common.GetTranslation("UpperLipWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphW); // Lo añadimos al menu
            MenuListItem mListULiphD = new MenuListItem(Common.GetTranslation("UpperLipDepth"), sizeType, 4, Common.GetTranslation("UpperLipDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphD); // Lo añadimos al menu
            MenuListItem mListLLiphH = new MenuListItem(Common.GetTranslation("LowerLipHeight"), sizeType, 4, Common.GetTranslation("LowerLipHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphH); // Lo añadimos al menu
            MenuListItem mListLLiphW = new MenuListItem(Common.GetTranslation("LowerLipWidth"), sizeType, 4, Common.GetTranslation("LowerLipWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphW); // Lo añadimos al menu
            MenuListItem mListLLiphD = new MenuListItem(Common.GetTranslation("LowerLipDepth"), sizeType, 4, Common.GetTranslation("LowerLipDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphD); // Lo añadimos al menu


            MenuListItem mListJawH = new MenuListItem(Common.GetTranslation("MandibleHeight"), sizeType, 4, Common.GetTranslation("MandibleHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawH); // Lo añadimos al menu

            MenuListItem mListJawW = new MenuListItem(Common.GetTranslation("MandibleWidth"), sizeType, 4, Common.GetTranslation("MandibleWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawW); // Lo añadimos al menu

            MenuListItem mListJawD = new MenuListItem(Common.GetTranslation("MandibleDepth"), sizeType, 4, Common.GetTranslation("MandibleDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawD); // Lo añadimos al menu


            MenuListItem mListChinH = new MenuListItem(Common.GetTranslation("ChinHeight"), sizeType, 4, Common.GetTranslation("ChinHeightDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinH); // Lo añadimos al menu

            MenuListItem mListChinW = new MenuListItem(Common.GetTranslation("ChinWidth"), sizeType, 4, Common.GetTranslation("ChinWidthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinW); // Lo añadimos al menu

            MenuListItem mListChinD = new MenuListItem(Common.GetTranslation("ChinDepth"), sizeType, 4, Common.GetTranslation("ChinDepthDesc")); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinD); //Lo añadimos al menu



            faceMenu.OnMenuOpen += (_menu) =>
            {

            };

            faceMenu.OnMenuClose += (_menu) =>
            {

            };

            faceMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                CreateCharacter.SetPlayerFaceBlend(_itemIndex, _newIndex);
            };

        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return faceMenu;
        }
    }
}
