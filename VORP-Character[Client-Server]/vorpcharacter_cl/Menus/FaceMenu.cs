using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;
using VorpCharacter.Script;

namespace VorpCharacter.Menus
{
    class FaceMenu
    {
        private static Menu faceMenu = new Menu(PluginManager.Langs["TitleFaceMenu"], PluginManager.Langs["SubTitleFaceMenu"]);
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
                sizeType.Add(PluginManager.Langs["TypeValues"] + i);
            }

            MenuListItem mListHead = new MenuListItem(PluginManager.Langs["FaceSize"], sizeType, 4, PluginManager.Langs["FaceSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListHead); // Lo añadimos al menu

            //Altura cejas
            MenuListItem mListEyeBrowH = new MenuListItem(PluginManager.Langs["EyebrowHeight"], sizeType, 4, PluginManager.Langs["EyebrowHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowH); // Lo añadimos al menu
            //Anchura cejas
            MenuListItem mListEyeBrowW = new MenuListItem(PluginManager.Langs["EyebrowWidth"], sizeType, 4, PluginManager.Langs["EyebrowWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowW); // Lo añadimos al menu
            //Profundida cejas
            MenuListItem mListEyeBrowD = new MenuListItem(PluginManager.Langs["EyebrowDepth"], sizeType, 4, PluginManager.Langs["EyebrowDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowD); // Lo añadimos al menu

            //Altura Orejas
            MenuListItem mListEarsH = new MenuListItem(PluginManager.Langs["EarsHeight"], sizeType, 4, PluginManager.Langs["EarsHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsH); // Lo añadimos al menu
            //Anchura Orejas
            MenuListItem mListEarsW = new MenuListItem(PluginManager.Langs["EarsAngle"], sizeType, 4, PluginManager.Langs["EarsAngleDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsW); // Lo añadimos al menu
            //Tamaño Orejas
            MenuListItem mListEarsD = new MenuListItem(PluginManager.Langs["EarsSize"], sizeType, 4, PluginManager.Langs["EarsSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsD); // Lo añadimos al menu
            //Tamaño Lobulo de la oreja
            MenuListItem mListEarsL = new MenuListItem(PluginManager.Langs["EarsLobeSize"], sizeType, 4, PluginManager.Langs["EarsLobeSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsL); // Lo añadimos al menu

            //Altura de los parpados
            MenuListItem mListEyeLidH = new MenuListItem(PluginManager.Langs["EyelidsHeight"], sizeType, 4, PluginManager.Langs["EyelidsHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeLidH); // Lo añadimos al menu
            //Ancho de los parpados
            MenuListItem mListEyeLidW = new MenuListItem(PluginManager.Langs["EyelidsWidth"], sizeType, 4, PluginManager.Langs["EyelidsWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeLidW); // Lo añadimos al menu

            //Profundida de los ojos
            MenuListItem mListEyeD = new MenuListItem(PluginManager.Langs["EyeDepth"], sizeType, 4, PluginManager.Langs["EyeDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeD); // Lo añadimos al menu
            //Angulo de los ojos
            MenuListItem mListEyeAng = new MenuListItem(PluginManager.Langs["EyeAngle"], sizeType, 4, PluginManager.Langs["EyeAngleDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeAng); // Lo añadimos al menu
            //Distancia de los ojos
            MenuListItem mListEyeDis = new MenuListItem(PluginManager.Langs["EyeSeparation"], sizeType, 4, PluginManager.Langs["EyeSeparationDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeDis); // Lo añadimos al menu
            //Altura de los ojos
            MenuListItem mListEyeH = new MenuListItem(PluginManager.Langs["EyeHeight"], sizeType, 4, PluginManager.Langs["EyeHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeH); // Lo añadimos al menu


            MenuListItem mListNoseW = new MenuListItem(PluginManager.Langs["NoseWidth"], sizeType, 4, PluginManager.Langs["NoseWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseW); // Lo añadimos al menu

            MenuListItem mListNoseS = new MenuListItem(PluginManager.Langs["NoseSize"], sizeType, 4, PluginManager.Langs["NoseSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseS); // Lo añadimos al menu

            MenuListItem mListNoseH = new MenuListItem(PluginManager.Langs["NoseHeight"], sizeType, 4, PluginManager.Langs["NoseHeigthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseH); // Lo añadimos al menu

            MenuListItem mListNoseAng = new MenuListItem(PluginManager.Langs["NoseAngle"], sizeType, 4, PluginManager.Langs["NoseAngleDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseAng); // Lo añadimos al menu

            MenuListItem mListNoseC = new MenuListItem(PluginManager.Langs["NoseCurvature"], sizeType, 4, PluginManager.Langs["NoseCurvatureDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseC); // Lo añadimos al menu

            MenuListItem mListNoseDis = new MenuListItem(PluginManager.Langs["NostrilsSeparation"], sizeType, 4, PluginManager.Langs["NostrilsSeparationDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseDis); // Lo añadimos al menu


            MenuListItem mListCheekBonesH = new MenuListItem(PluginManager.Langs["CheekbonesHeight"], sizeType, 4, PluginManager.Langs["CheekbonesHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesH); // Lo añadimos al menu

            MenuListItem mListCheekBonesW = new MenuListItem(PluginManager.Langs["CheekbonesWidth"], sizeType, 4, PluginManager.Langs["CheekbonesWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesW); // Lo añadimos al menu

            MenuListItem mListCheekBonesD = new MenuListItem(PluginManager.Langs["CheekbonesDepth"], sizeType, 4, PluginManager.Langs["CheekbonesDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesD); // Lo añadimos al menu


            MenuListItem mListMouthW = new MenuListItem(PluginManager.Langs["MouthWidth"], sizeType, 4, PluginManager.Langs["MouthWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthW); // Lo añadimos al menu

            MenuListItem mListMouthD = new MenuListItem(PluginManager.Langs["MouthDepth"], sizeType, 4, PluginManager.Langs["MouthDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthD); // Lo añadimos al menu

            MenuListItem mListMouthX = new MenuListItem(PluginManager.Langs["MouthDeviation"], sizeType, 4, PluginManager.Langs["MouthDeviationDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthX); // Lo añadimos al menu

            MenuListItem mListMouthY = new MenuListItem(PluginManager.Langs["MouthHeight"], sizeType, 4, PluginManager.Langs["MouthHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthY); // Lo añadimos al menu


            MenuListItem mListULiphH = new MenuListItem(PluginManager.Langs["UpperLipHeight"], sizeType, 4, PluginManager.Langs["UpperLipHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphH); // Lo añadimos al menu
            MenuListItem mListULiphW = new MenuListItem(PluginManager.Langs["UpperLipWidth"], sizeType, 4, PluginManager.Langs["UpperLipWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphW); // Lo añadimos al menu
            MenuListItem mListULiphD = new MenuListItem(PluginManager.Langs["UpperLipDepth"], sizeType, 4, PluginManager.Langs["UpperLipDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphD); // Lo añadimos al menu
            MenuListItem mListLLiphH = new MenuListItem(PluginManager.Langs["LowerLipHeight"], sizeType, 4, PluginManager.Langs["LowerLipHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphH); // Lo añadimos al menu
            MenuListItem mListLLiphW = new MenuListItem(PluginManager.Langs["LowerLipWidth"], sizeType, 4, PluginManager.Langs["LowerLipWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphW); // Lo añadimos al menu
            MenuListItem mListLLiphD = new MenuListItem(PluginManager.Langs["LowerLipDepth"], sizeType, 4, PluginManager.Langs["LowerLipDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphD); // Lo añadimos al menu


            MenuListItem mListJawH = new MenuListItem(PluginManager.Langs["MandibleHeight"], sizeType, 4, PluginManager.Langs["MandibleHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawH); // Lo añadimos al menu

            MenuListItem mListJawW = new MenuListItem(PluginManager.Langs["MandibleWidth"], sizeType, 4, PluginManager.Langs["MandibleWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawW); // Lo añadimos al menu

            MenuListItem mListJawD = new MenuListItem(PluginManager.Langs["MandibleDepth"], sizeType, 4, PluginManager.Langs["MandibleDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawD); // Lo añadimos al menu


            MenuListItem mListChinH = new MenuListItem(PluginManager.Langs["ChinHeight"], sizeType, 4, PluginManager.Langs["ChinHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinH); // Lo añadimos al menu

            MenuListItem mListChinW = new MenuListItem(PluginManager.Langs["ChinWidth"], sizeType, 4, PluginManager.Langs["ChinWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinW); // Lo añadimos al menu

            MenuListItem mListChinD = new MenuListItem(PluginManager.Langs["ChinDepth"], sizeType, 4, PluginManager.Langs["ChinDepthDesc"]); // Añadimos la lista al boton
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
