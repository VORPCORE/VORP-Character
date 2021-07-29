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
    class ClothesMenu
    {
        private static Menu clothesMenu = new Menu(GetConfig.Langs["TitleClothesMenu"], GetConfig.Langs["SubTitleClothesMenu"]);
        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(clothesMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            List<string> hatType = new List<string>();
            hatType.Add(GetConfig.Langs["NoHatsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.HATS_MALE.Count + 1; i++)
                {
                    hatType.Add(GetConfig.Langs["HatsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.HATS_FEMALE.Count + 1; i++)
                {
                    hatType.Add(GetConfig.Langs["HatsValue"] + i);
                }
            }
            MenuListItem mListHats = new MenuListItem(GetConfig.Langs["Hats"], hatType, 0, GetConfig.Langs["HatsDesc"]);
            clothesMenu.AddMenuItem(mListHats);

            List<string> eyeWearType = new List<string>();
            eyeWearType.Add(GetConfig.Langs["NoGlassesValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.EYEWEAR_MALE.Count + 1; i++)
                {
                    eyeWearType.Add(GetConfig.Langs["GlassesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.EYEWEAR_FEMALE.Count + 1; i++)
                {
                    eyeWearType.Add(GetConfig.Langs["GlassesValue"] + i);
                }
            }
            MenuListItem mListEyeWear = new MenuListItem(GetConfig.Langs["Glasses"], eyeWearType, 0, GetConfig.Langs["GlassesDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListEyeWear); // Lo añadimos al menu

            List<string> maskType = new List<string>();
            maskType.Add(GetConfig.Langs["NoMaskValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.MASK_MALE.Count + 1; i++)
                {
                    maskType.Add(GetConfig.Langs["MaskValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.MASK_FEMALE.Count + 1; i++)
                {
                    maskType.Add(GetConfig.Langs["MaskValue"] + i);
                }
            }
            MenuListItem mListMask = new MenuListItem(GetConfig.Langs["Masks"], maskType, 0, GetConfig.Langs["MasksDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListMask); // Lo añadimos al menu

            List<string> neckWearType = new List<string>();
            neckWearType.Add(GetConfig.Langs["NoNeckwearValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.NECKWEAR_MALE.Count + 1; i++)
                {
                    neckWearType.Add(GetConfig.Langs["NeckwearValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.NECKWEAR_FEMALE.Count + 1; i++)
                {
                    neckWearType.Add(GetConfig.Langs["NeckwearValue"] + i);
                }
            }
            MenuListItem mListNeckWear = new MenuListItem(GetConfig.Langs["Neckwear"], neckWearType, 0, GetConfig.Langs["NeckwearDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListNeckWear); // Lo añadimos al menu

            List<string> neckTiesType = new List<string>();
            neckTiesType.Add(GetConfig.Langs["NoTiesValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.NECKTIES_MALE.Count + 1; i++)
                {
                    neckTiesType.Add(GetConfig.Langs["TiesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.NECKTIES_FEMALE.Count + 1; i++)
                {
                    neckTiesType.Add(GetConfig.Langs["TiesValue"] + i);
                }
            }
            MenuListItem mListNeckTies = new MenuListItem(GetConfig.Langs["Ties"], neckTiesType, 0, GetConfig.Langs["TiesDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListNeckTies); // Lo añadimos al menu

            List<string> shirtsType = new List<string>();
            shirtsType.Add(GetConfig.Langs["NoShirtsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.SHIRTS_MALE.Count + 1; i++)
                {
                    shirtsType.Add(GetConfig.Langs["ShirtsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SHIRTS_FEMALE.Count + 1; i++)
                {
                    shirtsType.Add(GetConfig.Langs["ShirtsValue"] + i);
                }
            }
            MenuListItem mListShirts = new MenuListItem(GetConfig.Langs["Shirts"], shirtsType, 0, GetConfig.Langs["ShirtsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListShirts); // Lo añadimos al menu

            List<string> suspendersType = new List<string>();
            suspendersType.Add(GetConfig.Langs["NoSuspendersValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.SUSPENDERS_MALE.Count + 1; i++)
                {
                    suspendersType.Add(GetConfig.Langs["SuspendersValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SUSPENDERS_FEMALE.Count + 1; i++)
                {
                    suspendersType.Add(GetConfig.Langs["SuspendersValue"] + i);
                }
            }
            MenuListItem mListSuspenders = new MenuListItem(GetConfig.Langs["Suspenders"], suspendersType, 0, GetConfig.Langs["SuspendersDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListSuspenders); // Lo añadimos al menu


            List<string> vestType = new List<string>();
            vestType.Add(GetConfig.Langs["NoVestsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.VEST_MALE.Count + 1; i++)
                {
                    vestType.Add(GetConfig.Langs["VestsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.VEST_FEMALE.Count + 1; i++)
                {
                    vestType.Add(GetConfig.Langs["VestsValue"] + i);
                }
            }
            MenuListItem mListVest = new MenuListItem(GetConfig.Langs["Vests"], vestType, 0, GetConfig.Langs["VestsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListVest); // Lo añadimos al menu

            List<string> coatsType = new List<string>();
            coatsType.Add(GetConfig.Langs["NoCoatsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.COATS_MALE.Count + 1; i++)
                {
                    coatsType.Add(GetConfig.Langs["CoatsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.COATS_FEMALE.Count + 1; i++)
                {
                    coatsType.Add(GetConfig.Langs["CoatsValue"] + i);
                }
            }
            MenuListItem mListCoats = new MenuListItem(GetConfig.Langs["Coats"], coatsType, 0, GetConfig.Langs["CoatsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListCoats); // Lo añadimos al menu

            List<string> coatsClosedType = new List<string>();
            coatsClosedType.Add(GetConfig.Langs["NoCoatsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.COATS_CLOSED_MALE.Count + 1; i++)
                {
                    coatsClosedType.Add(GetConfig.Langs["CoatsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.COATS_CLOSED_FEMALE.Count + 1; i++)
                {
                    coatsClosedType.Add(GetConfig.Langs["CoatsValue"] + i);
                }
            }
            MenuListItem mListCoatsClosed = new MenuListItem(GetConfig.Langs["CoatsClosed"], coatsClosedType, 0, GetConfig.Langs["CoatsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListCoatsClosed); // Lo añadimos al menu

            List<string> ponchosType = new List<string>();
            ponchosType.Add(GetConfig.Langs["NoPonchosValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.PONCHOS_MALE.Count + 1; i++)
                {
                    ponchosType.Add(GetConfig.Langs["PonchosValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.PONCHOS_FEMALE.Count + 1; i++)
                {
                    ponchosType.Add(GetConfig.Langs["PonchosValue"] + i);
                }
            }
            MenuListItem mListPonchos = new MenuListItem(GetConfig.Langs["Ponchos"], ponchosType, 0, GetConfig.Langs["PonchosDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListPonchos); // Lo añadimos al menu

            List<string> cloakType = new List<string>();
            cloakType.Add(GetConfig.Langs["NoCloaksValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.CLOAK_MALE.Count + 1; i++)
                {
                    cloakType.Add(GetConfig.Langs["CloaksValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.CLOAK_FEMALE.Count + 1; i++)
                {
                    cloakType.Add(GetConfig.Langs["CloaksValue"] + i);
                }
            }
            MenuListItem mListCloak = new MenuListItem(GetConfig.Langs["Cloaks"], cloakType, 0, GetConfig.Langs["CloaksDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListCloak); // Lo añadimos al menu

            List<string> glovesType = new List<string>();
            glovesType.Add(GetConfig.Langs["NoGlovesValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.GLOVES_MALE.Count + 1; i++)
                {
                    glovesType.Add(GetConfig.Langs["GlovesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.GLOVES_FEMALE.Count + 1; i++)
                {
                    glovesType.Add(GetConfig.Langs["GlovesValue"] + i);
                }
            }
            MenuListItem mListGloves = new MenuListItem(GetConfig.Langs["Gloves"], glovesType, 0, GetConfig.Langs["GlovesDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListGloves); // Lo añadimos al menu

            List<string> ringsRhType = new List<string>();
            ringsRhType.Add(GetConfig.Langs["NoRingsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.RINGS_RH_MALE.Count + 1; i++)
                {
                    ringsRhType.Add(GetConfig.Langs["RingsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.RINGS_RH_FEMALE.Count + 1; i++)
                {
                    ringsRhType.Add(GetConfig.Langs["RingsValue"] + i);
                }
            }
            MenuListItem mListRingsRhType = new MenuListItem(GetConfig.Langs["RightRings"], ringsRhType, 0, GetConfig.Langs["RightRingsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListRingsRhType); // Lo añadimos al menu

            List<string> ringsLhType = new List<string>();
            ringsLhType.Add(GetConfig.Langs["NoRingsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.RINGS_LH_MALE.Count + 1; i++)
                {
                    ringsLhType.Add(GetConfig.Langs["RingsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.RINGS_LH_FEMALE.Count + 1; i++)
                {
                    ringsLhType.Add(GetConfig.Langs["RingsValue"] + i);
                }
            }
            MenuListItem mListRingsLh = new MenuListItem(GetConfig.Langs["LeftRings"], ringsLhType, 0, GetConfig.Langs["LeftRingsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListRingsLh); // Lo añadimos al menu

            List<string> braceletsType = new List<string>();
            braceletsType.Add(GetConfig.Langs["NoBraceletsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BRACELETS_MALE.Count + 1; i++)
                {
                    braceletsType.Add(GetConfig.Langs["BraceletsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BRACELETS_FEMALE.Count + 1; i++)
                {
                    braceletsType.Add(GetConfig.Langs["BraceletsValue"] + i);
                }
            }
            MenuListItem mListbracelets = new MenuListItem(GetConfig.Langs["Bracelets"], braceletsType, 0, GetConfig.Langs["BraceletsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListbracelets); // Lo añadimos al menu

            List<string> gunbeltType = new List<string>();
            gunbeltType.Add(GetConfig.Langs["NoHolstersValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.GUNBELT_MALE.Count + 1; i++)
                {
                    gunbeltType.Add(GetConfig.Langs["HolstersValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.GUNBELT_FEMALE.Count + 1; i++)
                {
                    gunbeltType.Add(GetConfig.Langs["HolstersValue"] + i);
                }
            }
            MenuListItem mListGunbelt = new MenuListItem(GetConfig.Langs["PrimaryHolsters"], gunbeltType, 0, GetConfig.Langs["PrimaryHolstersDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListGunbelt); // Lo añadimos al menu


            List<string> beltType = new List<string>();
            beltType.Add(GetConfig.Langs["NoBeltsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BELT_MALE.Count + 1; i++)
                {
                    beltType.Add(GetConfig.Langs["BeltsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BELT_FEMALE.Count + 1; i++)
                {
                    beltType.Add(GetConfig.Langs["BeltsValue"] + i);
                }
            }
            MenuListItem mListBelt = new MenuListItem(GetConfig.Langs["Belts"], beltType, 0, GetConfig.Langs["BeltsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListBelt); // Lo añadimos al menu

            List<string> buckleType = new List<string>();
            buckleType.Add(GetConfig.Langs["NoBucklesValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BUCKLE_MALE.Count + 1; i++)
                {
                    buckleType.Add(GetConfig.Langs["BucklesValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BUCKLE_FEMALE.Count + 1; i++)
                {
                    buckleType.Add(GetConfig.Langs["BucklesValue"] + i);
                }
            }
            MenuListItem mListBuckle = new MenuListItem(GetConfig.Langs["Buckles"], buckleType, 0, GetConfig.Langs["BucklesDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListBuckle); // Lo añadimos al menu

            List<string> holstersSType = new List<string>();
            holstersSType.Add(GetConfig.Langs["NoHolstersValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.HOLSTERS_S_MALE.Count + 1; i++)
                {
                    holstersSType.Add(GetConfig.Langs["HolstersValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.HOLSTERS_S_FEMALE.Count + 1; i++)
                {
                    holstersSType.Add(GetConfig.Langs["HolstersValue"] + i);
                }
            }
            MenuListItem mListSHolsters = new MenuListItem(GetConfig.Langs["SecondaryHolsters"], holstersSType, 0, GetConfig.Langs["SecondaryHolstersDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListSHolsters); // Lo añadimos al menu

            List<string> pantsType = new List<string>();
            pantsType.Add(GetConfig.Langs["NoPantsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.PANTS_MALE.Count + 1; i++)
                {
                    pantsType.Add(GetConfig.Langs["PantsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.PANTS_FEMALE.Count + 1; i++)
                {
                    pantsType.Add(GetConfig.Langs["PantsValue"] + i);
                }
            }
            MenuListItem mListPants = new MenuListItem(GetConfig.Langs["Pants"], pantsType, 0, GetConfig.Langs["PantsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListPants); // Lo añadimos al menu

            List<string> skirtsType = new List<string>();
            skirtsType.Add(GetConfig.Langs["NoSkirtsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SKIRTS_FEMALE.Count + 1; i++)
                {
                    skirtsType.Add(GetConfig.Langs["SkirtsValue"] + i);
                }
            }
            MenuListItem mListSkirts = new MenuListItem(GetConfig.Langs["Skirts"], skirtsType, 0, GetConfig.Langs["SkirtsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListSkirts); // Lo añadimos al menu

            List<string> chapsType = new List<string>();
            chapsType.Add(GetConfig.Langs["NoChapsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.CHAPS_MALE.Count + 1; i++)
                {
                    chapsType.Add(GetConfig.Langs["ChapsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.CHAPS_FEMALE.Count + 1; i++)
                {
                    chapsType.Add(GetConfig.Langs["ChapsValue"] + i);
                }
            }
            MenuListItem mListChaps = new MenuListItem(GetConfig.Langs["Chaps"], chapsType, 0, GetConfig.Langs["ChapsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListChaps); // Lo añadimos al menu

            List<string> bootsType = new List<string>();
            bootsType.Add(GetConfig.Langs["NoBootsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.BOOTS_MALE.Count + 1; i++)
                {
                    bootsType.Add(GetConfig.Langs["BootsValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.BOOTS_FEMALE.Count + 1; i++)
                {
                    bootsType.Add(GetConfig.Langs["BootsValue"] + i);
                }
            }
            MenuListItem mListBoots = new MenuListItem(GetConfig.Langs["Boots"], bootsType, 0, GetConfig.Langs["BootsDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListBoots); // Lo añadimos al menu

            List<string> spursType = new List<string>();
            spursType.Add(GetConfig.Langs["NoSpursValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                //Cabellos de Hombre
                for (float i = 1; i < SkinsUtils.SPURS_MALE.Count + 1; i++)
                {
                    spursType.Add(GetConfig.Langs["SpursValue"] + i);
                }
            }
            else
            {
                //Cabellos de Mujer
                for (float i = 1; i < SkinsUtils.SPURS_FEMALE.Count + 1; i++)
                {
                    spursType.Add(GetConfig.Langs["SpursValue"] + i);
                }
            }
            MenuListItem mListSpurs = new MenuListItem(GetConfig.Langs["Spurs"], spursType, 0, GetConfig.Langs["SpursDesc"]); // Añadimos la lista al boton
            clothesMenu.AddMenuItem(mListSpurs); // Lo añadimos al menu


            List<string> spatsType = new List<string>();
            spatsType.Add(GetConfig.Langs["NoSpatsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                for (float i = 1; i < SkinsUtils.SPATS_MALE.Count + 1; i++)
                {
                    spatsType.Add(GetConfig.Langs["SpatsValue"] + i);
                }
            }
            else
            {
                for (float i = 1; i < SkinsUtils.SPATS_FEMALE.Count + 1; i++)
                {
                    spatsType.Add(GetConfig.Langs["SpatsValue"] + i);
                }
            }
            MenuListItem mListSpats = new MenuListItem(GetConfig.Langs["Spats"], spatsType, 0, GetConfig.Langs["SpatsDesc"]);
            clothesMenu.AddMenuItem(mListSpats);

            List<string> gunbeltaccsType = new List<string>();
            gunbeltaccsType.Add(GetConfig.Langs["NoGunbeltAccsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                for (float i = 1; i < SkinsUtils.GUNBELTACCS_MALE.Count + 1; i++)
                {
                    gunbeltaccsType.Add(GetConfig.Langs["GunbeltAccsValue"] + i);
                }
            }
            else
            {
                for (float i = 1; i < SkinsUtils.GUNBELTACCS_FEMALE.Count + 1; i++)
                {
                    gunbeltaccsType.Add(GetConfig.Langs["GunbeltAccsValue"] + i);
                }
            }
            MenuListItem mListGunbeltAccs = new MenuListItem(GetConfig.Langs["GunbeltAccs"], gunbeltaccsType, 0, GetConfig.Langs["GunbeltAccsDesc"]);
            clothesMenu.AddMenuItem(mListGunbeltAccs);

            List<string> gauntletsType = new List<string>();
            gauntletsType.Add(GetConfig.Langs["NoGauntletsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                for (float i = 1; i < SkinsUtils.GAUNTLETS_MALE.Count + 1; i++)
                {
                    gauntletsType.Add(GetConfig.Langs["GauntletsValue"] + i);
                }
            }
            else
            {
                for (float i = 1; i < SkinsUtils.GAUNTLETS_FEMALE.Count + 1; i++)
                {
                    gauntletsType.Add(GetConfig.Langs["GauntletsValue"] + i);
                }
            }
            MenuListItem mListGauntlets = new MenuListItem(GetConfig.Langs["Gauntlets"], gauntletsType, 0, GetConfig.Langs["GauntletsDesc"]);
            clothesMenu.AddMenuItem(mListGauntlets);

            List<string> loadoutsType = new List<string>();
            loadoutsType.Add(GetConfig.Langs["NoLoadoutsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                for (float i = 1; i < SkinsUtils.LOADOUTS_MALE.Count + 1; i++)
                {
                    loadoutsType.Add(GetConfig.Langs["LoadoutsValue"] + i);
                }
            }
            else
            {
                for (float i = 1; i < SkinsUtils.LOADOUTS_FEMALE.Count + 1; i++)
                {
                    loadoutsType.Add(GetConfig.Langs["LoadoutsValue"] + i);
                }
            }
            MenuListItem mListLoadouts = new MenuListItem(GetConfig.Langs["Loadouts"], loadoutsType, 0, GetConfig.Langs["LoadoutsDesc"]);
            clothesMenu.AddMenuItem(mListLoadouts);

            List<string> accessoriesType = new List<string>();
            accessoriesType.Add(GetConfig.Langs["NoAccessoriesValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                for (float i = 1; i < SkinsUtils.ACCESSORIES_MALE.Count + 1; i++)
                {
                    accessoriesType.Add(GetConfig.Langs["AccessoriesValue"] + i);
                }
            }
            else
            {
                for (float i = 1; i < SkinsUtils.ACCESSORIES_FEMALE.Count + 1; i++)
                {
                    accessoriesType.Add(GetConfig.Langs["AccessoriesValue"] + i);
                }
            }
            MenuListItem mListAccessories = new MenuListItem(GetConfig.Langs["Accessories"], accessoriesType, 0, GetConfig.Langs["AccessoriesDesc"]);
            clothesMenu.AddMenuItem(mListAccessories);

            List<string> satchelsType = new List<string>();
            satchelsType.Add(GetConfig.Langs["NoSatchelsValue"]);

            if (CreateCharacter.model_selected == "mp_male")
            {
                for (float i = 1; i < SkinsUtils.SATCHELS_MALE.Count + 1; i++)
                {
                    satchelsType.Add(GetConfig.Langs["SatchelsValue"] + i);
                }
            }
            else
            {
                for (float i = 1; i < SkinsUtils.SATCHELS_FEMALE.Count + 1; i++)
                {
                    satchelsType.Add(GetConfig.Langs["SatchelsValue"] + i);
                }
            }
            MenuListItem mListSatchels = new MenuListItem(GetConfig.Langs["Satchels"], satchelsType, 0, GetConfig.Langs["SatchelsDesc"]);
            clothesMenu.AddMenuItem(mListSatchels);

            clothesMenu.OnMenuOpen += (_menu) => {

            };

            clothesMenu.OnMenuClose += (_menu) =>
            {

            };

            clothesMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                switch (_itemIndex)
                {
                    case 0:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x9925C067, "Hat", SkinsUtils.HATS_MALE, SkinsUtils.HATS_FEMALE);
                        break;
                    case 1:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x5E47CA6, "EyeWear", SkinsUtils.EYEWEAR_MALE, SkinsUtils.EYEWEAR_FEMALE);
                        break;
                    case 2:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x7505EF42, "Mask", SkinsUtils.MASK_MALE, SkinsUtils.MASK_FEMALE);
                        break;
                    case 3:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x5FC29285, "NeckWear", SkinsUtils.NECKWEAR_MALE, SkinsUtils.NECKWEAR_FEMALE);
                        break;
                    case 4:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x7A96FACA, "NeckTies", SkinsUtils.NECKTIES_MALE, SkinsUtils.NECKTIES_FEMALE);
                        break;
                    case 5:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x2026C46D, "Shirt", SkinsUtils.SHIRTS_MALE, SkinsUtils.SHIRTS_FEMALE);
                        break;
                    case 6:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x877A2CF7, "Suspender", SkinsUtils.SUSPENDERS_MALE, SkinsUtils.SUSPENDERS_FEMALE);
                        break;
                    case 7:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x485EE834, "Vest", SkinsUtils.VEST_MALE, SkinsUtils.VEST_FEMALE);
                        break;
                    case 8:
                        CreateCharacter.SetPlayerComponent(0, 0x0662AC34, "CoatClosed", SkinsUtils.COATS_CLOSED_MALE, SkinsUtils.COATS_CLOSED_FEMALE);
                        mListCoatsClosed.ListIndex = 0;
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xE06D30CE, "Coat", SkinsUtils.COATS_MALE, SkinsUtils.COATS_FEMALE);
                        break;
                    case 9:
                        CreateCharacter.SetPlayerComponent(0, 0xE06D30CE, "Coat", SkinsUtils.COATS_MALE, SkinsUtils.COATS_FEMALE);
                        mListCoats.ListIndex = 0;
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x0662AC34, "CoatClosed", SkinsUtils.COATS_CLOSED_MALE, SkinsUtils.COATS_CLOSED_FEMALE);
                        break;
                    case 10:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xAF14310B, "Poncho", SkinsUtils.PONCHOS_MALE, SkinsUtils.PONCHOS_FEMALE);
                        break;
                    case 11:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x3C1A74CD, "Cloak", SkinsUtils.CLOAK_MALE, SkinsUtils.CLOAK_FEMALE);
                        break;
                    case 12:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xEABE0032, "Glove", SkinsUtils.GLOVES_MALE, SkinsUtils.GLOVES_FEMALE);
                        break;
                    case 13:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x7A6BBD0B, "RingRh", SkinsUtils.RINGS_RH_MALE, SkinsUtils.RINGS_RH_FEMALE);
                        break;
                    case 14:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xF16A1D23, "RingLh", SkinsUtils.RINGS_LH_MALE, SkinsUtils.RINGS_LH_FEMALE);
                        break;
                    case 15:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x7BC10759, "Bracelet", SkinsUtils.BRACELETS_MALE, SkinsUtils.BRACELETS_FEMALE);
                        break;
                    case 16:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x9B2C8B89, "Gunbelt", SkinsUtils.GUNBELT_MALE, SkinsUtils.GUNBELT_FEMALE);
                        break;
                    case 17:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xA6D134C6, "Belt", SkinsUtils.BELT_MALE, SkinsUtils.BELT_FEMALE);
                        break;
                    case 18:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xFAE9107F, "Buckle", SkinsUtils.BUCKLE_MALE, SkinsUtils.BUCKLE_FEMALE);
                        break;
                    case 19:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xB6B6122D, "Holster", SkinsUtils.HOLSTERS_S_MALE, SkinsUtils.HOLSTERS_S_FEMALE);
                        break;
                    case 20:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x1D4C528A, "Pant", SkinsUtils.PANTS_MALE, SkinsUtils.PANTS_FEMALE);
                        break;
                    case 21:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xA0E3AB7F, "Skirt", SkinsUtils.SKIRTS_FEMALE, SkinsUtils.SKIRTS_FEMALE);
                        break;
                    case 22:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x3107499B, "Chap", SkinsUtils.CHAPS_MALE, SkinsUtils.CHAPS_FEMALE);
                        break;
                    case 23:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x777EC6EF, "Boots", SkinsUtils.BOOTS_MALE, SkinsUtils.BOOTS_FEMALE);
                        break;
                   case 24:
                        CreateCharacter.SetPlayerComponent(0, 0x514ADCEA, "Spats", SkinsUtils.SPATS_MALE, SkinsUtils.SPATS_FEMALE);
                        mListSpats.ListIndex = 0;
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x18729F39, "Spurs", SkinsUtils.SPURS_MALE, SkinsUtils.SPURS_FEMALE);
                        break;
                    case 25:
                        CreateCharacter.SetPlayerComponent(0, 0x18729F39, "Spurs", SkinsUtils.SPURS_MALE, SkinsUtils.SPURS_FEMALE);
                        mListSpurs.ListIndex = 0;
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x514ADCEA, "Spats", SkinsUtils.SPATS_MALE, SkinsUtils.SPATS_FEMALE);
                        break;
                    case 26:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x91CE9B20, "Gauntlets", SkinsUtils.GAUNTLETS_MALE, SkinsUtils.GAUNTLETS_FEMALE);
                        break;
                    case 27:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x83887E88, "Loadouts", SkinsUtils.LOADOUTS_MALE, SkinsUtils.LOADOUTS_FEMALE);
                        break;
                    case 28:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x79D7DF96, "Accessories", SkinsUtils.ACCESSORIES_MALE, SkinsUtils.ACCESSORIES_FEMALE);
                        break;
                    case 29:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0x94504D26, "Satchels", SkinsUtils.SATCHELS_MALE, SkinsUtils.SATCHELS_FEMALE);
                        break;
                    case 30:
                        CreateCharacter.SetPlayerComponent(_newIndex, 0xF1542D11, "GunbeltAccs", SkinsUtils.SATCHELS_MALE, SkinsUtils.SATCHELS_FEMALE);
                    break;
                }
            };

        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return clothesMenu;
        }
    }
}
