using CitizenFX.Core;
using CitizenFX.Core.Native;
using MenuAPI;
using System.Collections.Generic;
using System.Linq;
using VorpCharacter.Extensions;
using VorpCharacter.Script;
using VorpCharacter.Utils;

namespace VorpCharacter.Menus
{
    /*
     * Note:
     * Add opacity sliders
     * Add additional components
     * Load random settings first time around for both peds
     * Figure out colour issue with Grime
     * */

    class SkinMenu
    {
        private static Menu skinMenu;

        private static int _bodyIndex = 0;

        private static MenuListItem miLstBodyType; // bodyIndex
        private static MenuListItem miLstBodyColor; // body
        private static MenuListItem miLstFaceSelection; // headtype
        private static MenuListItem miLstTorso; // bodytype
        private static MenuListItem miLstBodyWaist; // waist
        private static MenuListItem miLstBodyLegs; // legstype
        private static MenuListItem miLstHairType; // 6
        private static MenuListItem miLstEyes; // 7
        private static MenuListItem miLstBeards; // 8
        private static MenuListItem miLstEyeBrows; // 9
        private static MenuListItem miLstScars; // 10
        private static MenuListItem miLstSpots; // 11
        private static MenuListItem miLstDisc; // 12
        private static MenuListItem miLstComplexion; // 13
        private static MenuListItem miLstAcne; // 14
        private static MenuListItem miLstAging; // 15
        private static MenuListItem miLstMoles; // 16
        private static MenuListItem miLstFreckles; // 17
        private static MenuListItem miLstGrime; // 18
        private static MenuListItem miLstLipstick; // 19
        private static MenuListItem miLstLipstickColor; // 20
        private static MenuListItem miLstLipstickColorTwo; // 21
        private static MenuListItem miLstShadows; // 22
        private static MenuListItem miLstShadowColor; // 23
        private static MenuListItem miLstShadowColorTwo; // 24

        public static Menu SetupMenu()
        {
            if (skinMenu is not null) return skinMenu;
            skinMenu = new Menu(Common.GetTranslation("TitleSkinMenu"), Common.GetTranslation("SubTitleSkinMenu"));
            MenuController.AddMenu(skinMenu);

            skinMenu.OnMenuClose += SkinMenu_OnMenuClose;

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            List<string> lstBodyType = SkinsUtils.BODY_TYPES.Select((x, i) => Common.GetTranslation("BodySizeValue") + i).ToList();
            miLstBodyType = new MenuListItem(Common.GetTranslation("BodyType"), lstBodyType, 0, Common.GetTranslation("BodyTypeDesc"));

            List<string> lstBodyWaist = SkinsUtils.WAIST_TYPES.Select((x, i) => Common.GetTranslation("WaistValue") + i).ToList();
            miLstBodyWaist = new MenuListItem(Common.GetTranslation("WaistType"), lstBodyWaist, 0, Common.GetTranslation("WaistTypeDesc"));

            List<string> lstBody = PluginManager.Config.Male.Select((x, i) => Common.GetTranslation("BodyColorValues") + i).ToList();
            List<string> lstFaces = PluginManager.Config.Male[_bodyIndex].Heads.Select((x, i) => Common.GetTranslation("FaceValues") + i).ToList();
            List<string> lstTorso = PluginManager.Config.Male[_bodyIndex].Body.Select((x, i) => Common.GetTranslation("TorsoValues") + i).ToList();
            List<string> lstLegs = PluginManager.Config.Male[_bodyIndex].Legs.Select((x, i) => Common.GetTranslation("LegsValues") + i).ToList();
            List<string> lstHair = SkinsUtils.HAIR_MALE.Select((x, i) => Common.GetTranslation("HairValue") + i).ToList();
            List<string> lstEyes = SkinsUtils.EYES_MALE.Select((x, i) => Common.GetTranslation("Eyes") + i).ToList();
            List<string> lstBeards = SkinsUtils.BEARD_MALE.Select((x, i) => Common.GetTranslation("Eyes") + i).ToList();

            List<string> lstEyeBrows = SkinsUtils.overlays_info["eyebrows"].Select((x, i) => Common.GetTranslation("EyeBrows") + i).ToList();
            List<string> lstScars = SkinsUtils.overlays_info["scars"].Select((x, i) => Common.GetTranslation("Scars") + i).ToList();
            List<string> lstSpots = SkinsUtils.overlays_info["spots"].Select((x, i) => Common.GetTranslation("Spots") + i).ToList();
            List<string> lstDiscValues = SkinsUtils.overlays_info["disc"].Select((x, i) => Common.GetTranslation("Disc") + i).ToList();
            List<string> lstComplexion = SkinsUtils.overlays_info["complex"].Select((x, i) => Common.GetTranslation("Complex") + i).ToList();
            List<string> lstAcne = SkinsUtils.overlays_info["acne"].Select((x, i) => Common.GetTranslation("Acne") + i).ToList();
            List<string> lstAging = SkinsUtils.overlays_info["ageing"].Select((x, i) => Common.GetTranslation("Ageing") + i).ToList();
            List<string> lstMoles = SkinsUtils.overlays_info["moles"].Select((x, i) => Common.GetTranslation("Moles") + i).ToList();
            List<string> lstFreckles = SkinsUtils.overlays_info["freckles"].Select((x, i) => Common.GetTranslation("Freckles") + i).ToList();
            List<string> lstGrime = SkinsUtils.overlays_info["grime"].Select((x, i) => Common.GetTranslation("Grimes") + i).ToList();
            List<string> lstLipStick = SkinsUtils.overlays_info["lipsticks"].Select((x, i) => Common.GetTranslation("Lipsticks") + i).ToList();
            List<string> lstLipStickColor = SkinsUtils.COLOR_PALETTES.Select((x, i) => Common.GetTranslation("LipsticksColors") + i).ToList();
            List<string> lstLipStickColorTwo = new string[255].Select((x, i) => Common.GetTranslation("LipsticksPColors") + i).ToList();
            List<string> lstShadows = SkinsUtils.overlays_info["shadows"].Select((x, i) => Common.GetTranslation("Shadows") + i).ToList();
            List<string> lstShadowColor = SkinsUtils.COLOR_PALETTES.Select((x, i) => Common.GetTranslation("ShadowsColors") + i).ToList();
            List<string> lstShadowColorTwo = new string[255].Select((x, i) => Common.GetTranslation("ShadowsPColors") + i).ToList();

            if (!CreateCharacter.isMale)
            {
                lstBody = PluginManager.Config.Female.Select((x, i) => Common.GetTranslation("BodyColorValues") + i).ToList();
                lstFaces = PluginManager.Config.Female[_bodyIndex].Heads.Select((x, i) => Common.GetTranslation("FaceValues") + i).ToList();
                lstTorso = PluginManager.Config.Female[_bodyIndex].Body.Select((x, i) => Common.GetTranslation("TorsoValues") + i).ToList();
                lstLegs = PluginManager.Config.Female[_bodyIndex].Legs.Select((x, i) => Common.GetTranslation("LegsValues") + i).ToList();
                lstHair = SkinsUtils.HAIR_FEMALE.Select((x, i) => Common.GetTranslation("HairValue") + i).ToList();
                lstEyes = SkinsUtils.EYES_FEMALE.Select((x, i) => Common.GetTranslation("Eyes") + i).ToList();
            }

            miLstBodyColor = new MenuListItem(Common.GetTranslation("BodyColor"), lstBody, 0, Common.GetTranslation("BodyColorDesc"));
            skinMenu.AddMenuItem(miLstBodyColor);

            miLstFaceSelection = new MenuListItem(Common.GetTranslation("FaceType"), lstFaces, 0, Common.GetTranslation("FaceTypeDesc"));
            skinMenu.AddMenuItem(miLstFaceSelection);

            miLstTorso = new MenuListItem(Common.GetTranslation("TorsoType"), lstTorso, 0, Common.GetTranslation("TorsoTypeDesc"));
            skinMenu.AddMenuItem(miLstTorso);

            skinMenu.AddMenuItem(miLstBodyType);
            skinMenu.AddMenuItem(miLstBodyWaist);

            miLstBodyLegs = new MenuListItem(Common.GetTranslation("LegsType"), lstLegs, 0, Common.GetTranslation("LegsTypeDesc"));
            skinMenu.AddMenuItem(miLstBodyLegs);

            miLstHairType = new MenuListItem(Common.GetTranslation("HairType"), lstHair, 0, Common.GetTranslation("HairTypeDesc"));
            skinMenu.AddMenuItem(miLstHairType);

            miLstEyes = new MenuListItem(Common.GetTranslation("EyesList"), lstEyes, 0, Common.GetTranslation("EyesDesc"));
            skinMenu.AddMenuItem(miLstEyes);

            if (CreateCharacter.isMale)
            {
                miLstBeards = new MenuListItem(Common.GetTranslation("BeardType"), lstBeards, 0, Common.GetTranslation("BeardTypeDesc"));
                skinMenu.AddMenuItem(miLstBeards);
            }

            miLstEyeBrows = new MenuListItem(Common.GetTranslation("EyeBrowsList"), lstEyeBrows, 0, Common.GetTranslation("EyeBrowsDesc"));
            skinMenu.AddMenuItem(miLstEyeBrows);

            miLstScars = new MenuListItem(Common.GetTranslation("ScarsList"), lstScars, 0, Common.GetTranslation("ScarsDesc"));
            skinMenu.AddMenuItem(miLstScars);

            miLstSpots = new MenuListItem(Common.GetTranslation("SpotsList"), lstSpots, 0, Common.GetTranslation("SpotsDesc"));
            skinMenu.AddMenuItem(miLstSpots);

            miLstDisc = new MenuListItem(Common.GetTranslation("DiscList"), lstDiscValues, 0, Common.GetTranslation("DiscDesc"));
            skinMenu.AddMenuItem(miLstDisc);

            miLstComplexion = new MenuListItem(Common.GetTranslation("ComplexList"), lstComplexion, 0, Common.GetTranslation("ComplexDesc"));
            skinMenu.AddMenuItem(miLstComplexion);

            miLstAcne = new MenuListItem(Common.GetTranslation("AcneList"), lstAcne, 0, Common.GetTranslation("AcneDesc"));
            skinMenu.AddMenuItem(miLstAcne);

            miLstAging = new MenuListItem(Common.GetTranslation("AgeingList"), lstAging, 0, Common.GetTranslation("AgeingDesc"));
            skinMenu.AddMenuItem(miLstAging);

            miLstMoles = new MenuListItem(Common.GetTranslation("MolesList"), lstMoles, 0, Common.GetTranslation("MolesDesc"));
            skinMenu.AddMenuItem(miLstMoles);

            miLstFreckles = new MenuListItem(Common.GetTranslation("FrecklesList"), lstFreckles, 0, Common.GetTranslation("FrecklesDesc"));
            skinMenu.AddMenuItem(miLstFreckles);

            miLstGrime = new MenuListItem(Common.GetTranslation("GrimesList"), lstGrime, 0, Common.GetTranslation("GrimesDesc"));
            skinMenu.AddMenuItem(miLstGrime);

            miLstLipstick = new MenuListItem(Common.GetTranslation("LipsticksList"), lstLipStick, 0, Common.GetTranslation("LipsticksDesc"));
            skinMenu.AddMenuItem(miLstLipstick);

            miLstLipstickColor = new MenuListItem(Common.GetTranslation("LipsticksColorsList"), lstLipStickColor, 0, Common.GetTranslation("LipsticksColorsDesc"));
            skinMenu.AddMenuItem(miLstLipstickColor);

            miLstLipstickColorTwo = new MenuListItem(Common.GetTranslation("LipsticksPColorsList"), lstLipStickColorTwo, 0, Common.GetTranslation("LipsticksPColorsDesc"));
            skinMenu.AddMenuItem(miLstLipstickColorTwo);

            miLstShadows = new MenuListItem(Common.GetTranslation("ShadowsList"), lstShadows, 0, Common.GetTranslation("ShadowsDesc"));
            skinMenu.AddMenuItem(miLstShadows);

            miLstShadowColor = new MenuListItem(Common.GetTranslation("ShadowsColorsList"), lstShadowColor, 0, Common.GetTranslation("ShadowsColorsDesc"));
            skinMenu.AddMenuItem(miLstShadowColor);

            miLstShadowColorTwo = new MenuListItem(Common.GetTranslation("ShadowsPColorsList"), lstShadowColorTwo, 0, Common.GetTranslation("ShadowsPColorsDesc"));
            skinMenu.AddMenuItem(miLstShadowColorTwo);

            skinMenu.OnListIndexChange += SkinMenu_OnListIndexChange;

            return skinMenu;
        }

        private static void SkinMenu_OnListIndexChange(Menu menu, MenuListItem listItem, int oldSelectionIndex, int newSelectionIndex, int itemIndex)
        {
            if (listItem == miLstBodyColor)
            {
                UpdateLists(newSelectionIndex);
            }
            else if (listItem == miLstTorso)
            {
                dynamic lst = CreateCharacter.isMale ? PluginManager.Config.Male[_bodyIndex].Body : PluginManager.Config.Female[_bodyIndex].Body;
                CreateCharacter.SetPlayerModelComponent(lst[newSelectionIndex], "BodyType");
            }
            else if (listItem == miLstFaceSelection)
            {
                dynamic lst = CreateCharacter.isMale ? PluginManager.Config.Male[_bodyIndex].Heads : PluginManager.Config.Female[_bodyIndex].Heads;
                CreateCharacter.SetPlayerModelComponent(lst[newSelectionIndex], "HeadType");
            }
            else if (listItem == miLstBodyType)
            {
                CreateCharacter.SetPlayerBodyComponent((uint)SkinsUtils.BODY_TYPES.ElementAt(newSelectionIndex), "Body");
            }
            else if (listItem == miLstBodyWaist)
            {
                CreateCharacter.SetPlayerBodyComponent((uint)SkinsUtils.WAIST_TYPES.ElementAt(newSelectionIndex), "Waist");
            }
            else if (listItem == miLstBodyLegs)
            {
                dynamic lst = CreateCharacter.isMale ? PluginManager.Config.Male[_bodyIndex].Legs : PluginManager.Config.Female[_bodyIndex].Legs;
                CreateCharacter.SetPlayerModelComponent(lst[newSelectionIndex], "LegsType");
            }
            else if (listItem == miLstHairType)
            {
                List<uint> lst = SkinsUtils.HAIR_FEMALE;
                if (CreateCharacter.isMale)
                    lst = SkinsUtils.HAIR_MALE;

                CreateCharacter.SetPlayerModelListComps("Hair", lst[newSelectionIndex], 0x864B03AE);
            }
            else if (listItem == miLstEyes)
            {
                List<uint> lst = SkinsUtils.EYES_FEMALE;
                if (CreateCharacter.isMale)
                    lst = SkinsUtils.EYES_MALE;

                CreateCharacter.SetPlayerModelListComps("Eyes", lst[newSelectionIndex], 0x864B03AE);
            }
            else if (listItem == miLstBeards)
            {
                if (newSelectionIndex == 0 || !CreateCharacter.isMale)
                    CreateCharacter.SetPlayerModelListComps("Beard", 0, 0xF8016BCA);

                if (CreateCharacter.isMale)
                    CreateCharacter.SetPlayerModelListComps("Beard", SkinsUtils.BEARD_MALE.ElementAt(newSelectionIndex), 0xF8016BCA);
            }
            else if (listItem == miLstEyeBrows)
            {
                CreateCharacter.ToggleOverlayChange("eyebrows", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex);
            }
            else if (listItem == miLstScars)
            {
                CreateCharacter.ToggleOverlayChange("scars", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstSpots)
            {
                CreateCharacter.ToggleOverlayChange("spots", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstDisc)
            {
                CreateCharacter.ToggleOverlayChange("disc", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstComplexion)
            {
                CreateCharacter.ToggleOverlayChange("complex", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstAcne)
            {
                CreateCharacter.ToggleOverlayChange("acne", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstAging)
            {
                CreateCharacter.ToggleOverlayChange("ageing", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstMoles)
            {
                CreateCharacter.ToggleOverlayChange("moles", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstFreckles)
            {
                CreateCharacter.ToggleOverlayChange("freckles", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstGrime)
            {
                CreateCharacter.ToggleOverlayChange("grime", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, tx_color_type: 1);
            }
            else if (listItem == miLstLipstick)
            {
                CreateCharacter.ToggleOverlayChange("lipsticks", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, palette_id: miLstLipstickColor.ListIndex, palette_color_primary: miLstLipstickColorTwo.ListIndex);
            }
            else if (listItem == miLstLipstickColor)
            {
                CreateCharacter.ToggleOverlayChange("lipsticks", newSelectionIndex == 0 ? 0 : 1, miLstLipstick.ListIndex, palette_id: newSelectionIndex, palette_color_primary: miLstLipstickColorTwo.ListIndex);
            }
            else if (listItem == miLstLipstickColorTwo)
            {
                CreateCharacter.ToggleOverlayChange("lipsticks", newSelectionIndex == 0 ? 0 : 1, miLstLipstick.ListIndex, palette_id: miLstLipstickColor.ListIndex, palette_color_primary: newSelectionIndex);
            }
            else if (listItem == miLstShadows)
            {
                CreateCharacter.ToggleOverlayChange("shadows", newSelectionIndex == 0 ? 0 : 1, newSelectionIndex, palette_id: miLstShadowColor.ListIndex, palette_color_primary: miLstShadowColorTwo.ListIndex);
            }
            else if (listItem == miLstShadowColor)
            {
                CreateCharacter.ToggleOverlayChange("shadows", newSelectionIndex == 0 ? 0 : 1, miLstShadows.ListIndex, palette_id: newSelectionIndex, palette_color_primary: miLstShadowColorTwo.ListIndex);
            }
            else if (listItem == miLstShadowColorTwo)
            {
                CreateCharacter.ToggleOverlayChange("shadows", newSelectionIndex == 0 ? 0 : 1, miLstShadows.ListIndex, palette_id: miLstShadowColor.ListIndex, palette_color_primary: newSelectionIndex);
            }
        }

        private static void SkinMenu_OnMenuClose(Menu menu)
        {

        }

        private static void ReloadTextures()
        {
            CreateCharacter.ToggleOverlayChange("eyebrows", miLstEyeBrows.ListIndex == 0 ? 0 : 1, miLstEyeBrows.ListIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("scars", miLstScars.ListIndex == 0 ? 0 : 1, miLstScars.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("spots", miLstSpots.ListIndex == 0 ? 0 : 1, miLstSpots.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("disc", miLstDisc.ListIndex == 0 ? 0 : 1, miLstDisc.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("complex", miLstComplexion.ListIndex == 0 ? 0 : 1, miLstComplexion.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("acne", miLstAcne.ListIndex == 0 ? 0 : 1, miLstAcne.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("ageing", miLstAging.ListIndex == 0 ? 0 : 1, miLstAging.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("moles", miLstMoles.ListIndex == 0 ? 0 : 1, miLstMoles.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("freckles", miLstFreckles.ListIndex == 0 ? 0 : 1, miLstFreckles.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("grime", miLstGrime.ListIndex == 0 ? 0 : 1, miLstGrime.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("lipsticks", miLstLipstick.ListIndex == 0 ? 0 : 1, miLstLipstick.ListIndex, 0, 0, 0, 1.0f, 0, miLstLipstickColor.ListIndex, miLstLipstickColorTwo.ListIndex, 0, 0, 0, 1.0f);
            CreateCharacter.ToggleOverlayChange("shadows", miLstShadows.ListIndex == 0 ? 0 : 1, miLstLipstick.ListIndex, 0, 0, 0, 1.0f, 0, miLstShadowColor.ListIndex, miLstLipstickColorTwo.ListIndex, 0, 0, 0, 1.0f);
        }

        private static void UpdateLists(int bodyIndex)
        {
            miLstFaceSelection.ListItems.Clear();
            miLstTorso.ListItems.Clear();
            miLstBodyLegs.ListItems.Clear();

            _bodyIndex = bodyIndex;

            dynamic dynList = PluginManager.Config.Male;
            if (!CreateCharacter.isMale) dynList = PluginManager.Config.Female;

            List<string> lstFaces = PluginManager.Config.Male[bodyIndex].Heads.Select((x, i) => Common.GetTranslation("FaceValues") + i).ToList();
            List<string> lstTorso = PluginManager.Config.Male[bodyIndex].Body.Select((x, i) => Common.GetTranslation("TorsoValues") + i).ToList();
            List<string> lstLegs = PluginManager.Config.Male[bodyIndex].Legs.Select((x, i) => Common.GetTranslation("LegsValues") + i).ToList();

            if (!CreateCharacter.isMale)
            {
                lstFaces = PluginManager.Config.Female[bodyIndex].Heads.Select((x, i) => Common.GetTranslation("FaceValues") + i).ToList();
                lstTorso = PluginManager.Config.Female[bodyIndex].Body.Select((x, i) => Common.GetTranslation("TorsoValues") + i).ToList();
                lstLegs = PluginManager.Config.Female[bodyIndex].Legs.Select((x, i) => Common.GetTranslation("LegsValues") + i).ToList();
            }

            //Faces
            miLstFaceSelection.ListItems = lstFaces;
            miLstFaceSelection.ListIndex = 0;

            //Torso
            miLstTorso.ListItems = lstTorso;
            miLstTorso.ListIndex = 0;

            //Legs
            miLstBodyLegs.ListItems = lstLegs;
            miLstBodyLegs.ListIndex = 0;

            CreateCharacter.SetPlayerModelComponent(lstFaces[0].ToString(), "HeadType");
            CreateCharacter.SetPlayerModelComponent(lstTorso[0].ToString(), "BodyType");
            CreateCharacter.SetPlayerModelComponent(lstLegs[0].ToString(), "LegsType");

            CreateCharacter.texture_types["albedo"] = API.GetHashKey(dynList[bodyIndex].HeadTexture);
            CreateCharacter.skinPlayer["albedo"] = API.GetHashKey(dynList[bodyIndex].HeadTexture);
            ReloadTextures();
        }
    }
}
