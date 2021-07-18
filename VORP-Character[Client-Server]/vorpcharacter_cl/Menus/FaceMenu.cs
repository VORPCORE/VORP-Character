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
    class FaceMenu
    {
        private static Menu faceMenu = new Menu(GetConfig.Langs["TitleFaceMenu"], GetConfig.Langs["SubTitleFaceMenu"]);
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
                sizeType.Add(GetConfig.Langs["TypeValues"] + i);
            }

            MenuListItem mListHead = new MenuListItem(GetConfig.Langs["FaceSize"], sizeType, 4, GetConfig.Langs["FaceSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListHead); // Lo añadimos al menu

            //Altura cejas
            MenuListItem mListEyeBrowH = new MenuListItem(GetConfig.Langs["EyebrowHeight"], sizeType, 4, GetConfig.Langs["EyebrowHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowH); // Lo añadimos al menu
            //Anchura cejas
            MenuListItem mListEyeBrowW = new MenuListItem(GetConfig.Langs["EyebrowWidth"], sizeType, 4, GetConfig.Langs["EyebrowWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowW); // Lo añadimos al menu
            //Profundida cejas
            MenuListItem mListEyeBrowD = new MenuListItem(GetConfig.Langs["EyebrowDepth"], sizeType, 4, GetConfig.Langs["EyebrowDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeBrowD); // Lo añadimos al menu

            //Altura Orejas
            MenuListItem mListEarsH = new MenuListItem(GetConfig.Langs["EarsHeight"], sizeType, 4, GetConfig.Langs["EarsHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsH); // Lo añadimos al menu
            //Anchura Orejas
            MenuListItem mListEarsW = new MenuListItem(GetConfig.Langs["EarsAngle"], sizeType, 4, GetConfig.Langs["EarsAngleDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsW); // Lo añadimos al menu
            //Tamaño Orejas
            MenuListItem mListEarsD = new MenuListItem(GetConfig.Langs["EarsSize"], sizeType, 4, GetConfig.Langs["EarsSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsD); // Lo añadimos al menu
            //Tamaño Lobulo de la oreja
            MenuListItem mListEarsL = new MenuListItem(GetConfig.Langs["EarsLobeSize"], sizeType, 4, GetConfig.Langs["EarsLobeSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEarsL); // Lo añadimos al menu

            //Altura de los parpados
            MenuListItem mListEyeLidH = new MenuListItem(GetConfig.Langs["EyelidsHeight"], sizeType, 4, GetConfig.Langs["EyelidsHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeLidH); // Lo añadimos al menu
            //Ancho de los parpados
            MenuListItem mListEyeLidW = new MenuListItem(GetConfig.Langs["EyelidsWidth"], sizeType, 4, GetConfig.Langs["EyelidsWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeLidW); // Lo añadimos al menu

            //Profundida de los ojos
            MenuListItem mListEyeD = new MenuListItem(GetConfig.Langs["EyeDepth"], sizeType, 4, GetConfig.Langs["EyeDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeD); // Lo añadimos al menu
            //Angulo de los ojos
            MenuListItem mListEyeAng = new MenuListItem(GetConfig.Langs["EyeAngle"], sizeType, 4, GetConfig.Langs["EyeAngleDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeAng); // Lo añadimos al menu
            //Distancia de los ojos
            MenuListItem mListEyeDis = new MenuListItem(GetConfig.Langs["EyeSeparation"], sizeType, 4, GetConfig.Langs["EyeSeparationDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeDis); // Lo añadimos al menu
            //Altura de los ojos
            MenuListItem mListEyeH = new MenuListItem(GetConfig.Langs["EyeHeight"], sizeType, 4, GetConfig.Langs["EyeHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListEyeH); // Lo añadimos al menu


            MenuListItem mListNoseW = new MenuListItem(GetConfig.Langs["NoseWidth"], sizeType, 4, GetConfig.Langs["NoseWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseW); // Lo añadimos al menu

            MenuListItem mListNoseS = new MenuListItem(GetConfig.Langs["NoseSize"], sizeType, 4, GetConfig.Langs["NoseSizeDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseS); // Lo añadimos al menu

            MenuListItem mListNoseH = new MenuListItem(GetConfig.Langs["NoseHeight"], sizeType, 4, GetConfig.Langs["NoseHeigthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseH); // Lo añadimos al menu

            MenuListItem mListNoseAng = new MenuListItem(GetConfig.Langs["NoseAngle"], sizeType, 4, GetConfig.Langs["NoseAngleDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseAng); // Lo añadimos al menu

            MenuListItem mListNoseC = new MenuListItem(GetConfig.Langs["NoseCurvature"], sizeType, 4, GetConfig.Langs["NoseCurvatureDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseC); // Lo añadimos al menu

            MenuListItem mListNoseDis = new MenuListItem(GetConfig.Langs["NostrilsSeparation"], sizeType, 4, GetConfig.Langs["NostrilsSeparationDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListNoseDis); // Lo añadimos al menu


            MenuListItem mListCheekBonesH = new MenuListItem(GetConfig.Langs["CheekbonesHeight"], sizeType, 4, GetConfig.Langs["CheekbonesHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesH); // Lo añadimos al menu

            MenuListItem mListCheekBonesW = new MenuListItem(GetConfig.Langs["CheekbonesWidth"], sizeType, 4, GetConfig.Langs["CheekbonesWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesW); // Lo añadimos al menu

            MenuListItem mListCheekBonesD = new MenuListItem(GetConfig.Langs["CheekbonesDepth"], sizeType, 4, GetConfig.Langs["CheekbonesDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListCheekBonesD); // Lo añadimos al menu


            MenuListItem mListMouthW = new MenuListItem(GetConfig.Langs["MouthWidth"], sizeType, 4, GetConfig.Langs["MouthWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthW); // Lo añadimos al menu

            MenuListItem mListMouthD = new MenuListItem(GetConfig.Langs["MouthDepth"], sizeType, 4, GetConfig.Langs["MouthDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthD); // Lo añadimos al menu

            MenuListItem mListMouthX = new MenuListItem(GetConfig.Langs["MouthDeviation"], sizeType, 4, GetConfig.Langs["MouthDeviationDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthX); // Lo añadimos al menu

            MenuListItem mListMouthY = new MenuListItem(GetConfig.Langs["MouthHeight"], sizeType, 4, GetConfig.Langs["MouthHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListMouthY); // Lo añadimos al menu


            MenuListItem mListULiphH = new MenuListItem(GetConfig.Langs["UpperLipHeight"], sizeType, 4, GetConfig.Langs["UpperLipHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphH); // Lo añadimos al menu
            MenuListItem mListULiphW = new MenuListItem(GetConfig.Langs["UpperLipWidth"], sizeType, 4, GetConfig.Langs["UpperLipWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphW); // Lo añadimos al menu
            MenuListItem mListULiphD = new MenuListItem(GetConfig.Langs["UpperLipDepth"], sizeType, 4, GetConfig.Langs["UpperLipDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListULiphD); // Lo añadimos al menu
            MenuListItem mListLLiphH = new MenuListItem(GetConfig.Langs["LowerLipHeight"], sizeType, 4, GetConfig.Langs["LowerLipHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphH); // Lo añadimos al menu
            MenuListItem mListLLiphW = new MenuListItem(GetConfig.Langs["LowerLipWidth"], sizeType, 4, GetConfig.Langs["LowerLipWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphW); // Lo añadimos al menu
            MenuListItem mListLLiphD = new MenuListItem(GetConfig.Langs["LowerLipDepth"], sizeType, 4, GetConfig.Langs["LowerLipDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListLLiphD); // Lo añadimos al menu


            MenuListItem mListJawH = new MenuListItem(GetConfig.Langs["MandibleHeight"], sizeType, 4, GetConfig.Langs["MandibleHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawH); // Lo añadimos al menu

            MenuListItem mListJawW = new MenuListItem(GetConfig.Langs["MandibleWidth"], sizeType, 4, GetConfig.Langs["MandibleWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawW); // Lo añadimos al menu

            MenuListItem mListJawD = new MenuListItem(GetConfig.Langs["MandibleDepth"], sizeType, 4, GetConfig.Langs["MandibleDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListJawD); // Lo añadimos al menu


            MenuListItem mListChinH = new MenuListItem(GetConfig.Langs["ChinHeight"], sizeType, 4, GetConfig.Langs["ChinHeightDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinH); // Lo añadimos al menu

            MenuListItem mListChinW = new MenuListItem(GetConfig.Langs["ChinWidth"], sizeType, 4, GetConfig.Langs["ChinWidthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinW); // Lo añadimos al menu

            MenuListItem mListChinD = new MenuListItem(GetConfig.Langs["ChinDepth"], sizeType, 4, GetConfig.Langs["ChinDepthDesc"]); // Añadimos la lista al boton
            faceMenu.AddMenuItem(mListChinD); //Lo añadimos al menu



            faceMenu.OnMenuOpen += (_menu) => {

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
