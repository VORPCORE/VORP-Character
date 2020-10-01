using CitizenFX.Core;
using CitizenFX.Core.Native;
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
    class SkinMenu
    {
        private static Menu skinMenu = new Menu(GetConfig.Langs["TitleSkinMenu"], GetConfig.Langs["SubTitleSkinMenu"]);

        private static MenuListItem btnSelectorBody;
        private static MenuListItem btnSelectorFace;
        private static MenuListItem btnSelectorTorso;
        private static MenuListItem btnSelectorLegs;
        private static MenuListItem btnSelectoreyeBrows;
        private static MenuListItem btnSelectorScars;
        private static MenuListItem btnSelectorSpots;
        private static MenuListItem btnSelectorDisc;
        private static MenuListItem btnSelectorComplex;
        private static MenuListItem btnSelectorAcne;
        private static MenuListItem btnSelectorAgeing;
        private static MenuListItem btnSelectorMoles;
        private static MenuListItem btnSelectorFreckles;
        private static MenuListItem btnSelectorGrime;
        private static MenuListItem btnSelectorLipsticks;
        private static MenuListItem btnSelectorLipsticksColor;
        private static MenuListItem btnSelectorLipsticksPColor;
        private static MenuListItem btnSelectorShadows;
        private static MenuListItem btnSelectorShadowsColor;
        private static MenuListItem btnSelectorShadowsPColor;

        private static bool setupDone = false;
        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(skinMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            if (CreateCharacter.model_selected == "mp_male") // Male
            {
                //Body Colors
                List<string> bodyValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["BodyColorValues"] + i);
                }

                btnSelectorBody = new MenuListItem(GetConfig.Langs["BodyColor"], bodyValues, 0, GetConfig.Langs["BodyColorDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu

                //Faces
                List<string> faceValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"][0]["Heads"].Count(); i++)
                {
                    faceValues.Add(GetConfig.Langs["FaceValues"] + i);
                }

                btnSelectorFace = new MenuListItem(GetConfig.Langs["FaceType"], faceValues, 0, GetConfig.Langs["FaceTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorFace); // Lo añadimos al menu

                //Torso
                List<string> torsoValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"][0]["Body"].Count(); i++)
                {
                    torsoValues.Add(GetConfig.Langs["TorsoValues"] + i);
                }

                btnSelectorTorso = new MenuListItem(GetConfig.Langs["TorsoType"], torsoValues, 0, GetConfig.Langs["TorsoTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorTorso); // Lo añadimos al menu

                //Endoforma
                List<string> endoformValues = new List<string>();

                for (int i = 1; i < SkinsUtils.BODY_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    endoformValues.Add(GetConfig.Langs["BodySizeValue"] + i);
                }

                MenuListItem btnSelectorBodyForm = new MenuListItem(GetConfig.Langs["BodyType"], endoformValues, 0, GetConfig.Langs["BodyTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBodyForm); // Lo añadimos al menu

                //Waist
                List<string> waistValues = new List<string>();

                for (int i = 1; i < SkinsUtils.WAIST_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    waistValues.Add(GetConfig.Langs["WaistValue"] + i);
                }

                MenuListItem btnSelectorWaist = new MenuListItem(GetConfig.Langs["WaistType"], waistValues, 0, GetConfig.Langs["WaistTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorWaist); // Lo añadimos al menu

                //Legs
                List<string> legsValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Male"][0]["Legs"].Count(); i++)
                {
                    legsValues.Add(GetConfig.Langs["LegsValues"] + i);
                }

                btnSelectorLegs = new MenuListItem(GetConfig.Langs["LegsType"], legsValues, 0, GetConfig.Langs["LegsTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLegs); // Lo añadimos al menu
                
                //Hair
                List<string> hairValues = new List<string>();

                for (float i = 1; i < SkinsUtils.HAIR_MALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    hairValues.Add(GetConfig.Langs["HairValue"] + i);
                }

                MenuListItem btnSelectorHairs = new MenuListItem(GetConfig.Langs["HairType"], hairValues, 0, GetConfig.Langs["HairTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorHairs); // Lo añadimos al menu


                List<string> eyesValues = new List<string>();

                for (float i = 1; i < SkinsUtils.EYES_MALE.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    eyesValues.Add(GetConfig.Langs["Eyes"] + i);
                }

                MenuListItem btnSelectorEyes = new MenuListItem(GetConfig.Langs["EyesList"], eyesValues, 0, GetConfig.Langs["EyesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorEyes); // Lo añadimos al menu

                //Beard
                List<string> beardValues = new List<string>();

                for (float i = 1; i < SkinsUtils.BEARD_MALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    beardValues.Add(GetConfig.Langs["BeardValue"] + i);
                }
                //beardType.Add(GetConfig.Langs["NoExistValue"]);

                MenuListItem btnSelectorBeards = new MenuListItem(GetConfig.Langs["BeardType"], beardValues, 0, GetConfig.Langs["BeardTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBeards); // Lo añadimos al menu

                //eyeBrows
                List<string> eyeBrowsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["eyebrows"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    eyeBrowsValues.Add(GetConfig.Langs["EyeBrows"] + i);
                }

                btnSelectoreyeBrows = new MenuListItem(GetConfig.Langs["EyeBrowsList"], eyeBrowsValues, 0, GetConfig.Langs["EyeBrowsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectoreyeBrows); // Lo añadimos al menu

                //Scars
                List<string> scarsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["scars"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    scarsValues.Add(GetConfig.Langs["Scars"] + i);
                }

                btnSelectorScars = new MenuListItem(GetConfig.Langs["ScarsList"], scarsValues, 0, GetConfig.Langs["ScarsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorScars); // Lo añadimos al menu

                //Spots
                List<string> spotsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["spots"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    spotsValues.Add(GetConfig.Langs["Spots"] + i);
                }

                btnSelectorSpots = new MenuListItem(GetConfig.Langs["SpotsList"], spotsValues, 0, GetConfig.Langs["SpotsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorSpots); // Lo añadimos al menu

                //Disc
                List<string> discValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["disc"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    discValues.Add(GetConfig.Langs["Disc"] + i);
                }

                btnSelectorDisc = new MenuListItem(GetConfig.Langs["DiscList"], discValues, 0, GetConfig.Langs["DiscDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorDisc); // Lo añadimos al menu

                //Complex
                List<string> complexValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["complex"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    complexValues.Add(GetConfig.Langs["Complex"] + i);
                }

                btnSelectorComplex = new MenuListItem(GetConfig.Langs["ComplexList"], complexValues, 0, GetConfig.Langs["ComplexDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorComplex); // Lo añadimos al menu

                //Acne
                List<string> acneValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["acne"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    acneValues.Add(GetConfig.Langs["Acne"] + i);
                }

                btnSelectorAcne = new MenuListItem(GetConfig.Langs["AcneList"], acneValues, 0, GetConfig.Langs["AcneDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorAcne); // Lo añadimos al menu

                //Ageing
                List<string> ageingValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["ageing"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    ageingValues.Add(GetConfig.Langs["Ageing"] + i);
                }

                btnSelectorAgeing = new MenuListItem(GetConfig.Langs["AgeingList"], ageingValues, 0, GetConfig.Langs["AgeingDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorAgeing); // Lo añadimos al menu

                //Moles
                List<string> molesValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["moles"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    molesValues.Add(GetConfig.Langs["Moles"] + i);
                }

                btnSelectorMoles = new MenuListItem(GetConfig.Langs["MolesList"], molesValues, 0, GetConfig.Langs["MolesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorMoles); // Lo añadimos al menu

                //Freckles
                List<string> frecklesValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["freckles"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    frecklesValues.Add(GetConfig.Langs["Freckles"] + i);
                }

                btnSelectorFreckles = new MenuListItem(GetConfig.Langs["FrecklesList"], frecklesValues, 0, GetConfig.Langs["FrecklesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorFreckles); // Lo añadimos al menu

                //Freckles
                List<string> grimeValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["grime"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    grimeValues.Add(GetConfig.Langs["Grimes"] + i);
                }

                btnSelectorGrime = new MenuListItem(GetConfig.Langs["GrimesList"], grimeValues, 0, GetConfig.Langs["GrimesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorGrime); // Lo añadimos al menu

                //Lipsticks
                List<string> lipsticksValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["lipsticks"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    lipsticksValues.Add(GetConfig.Langs["Lipsticks"] + i);
                }

                btnSelectorLipsticks = new MenuListItem(GetConfig.Langs["LipsticksList"], lipsticksValues, 0, GetConfig.Langs["LipsticksDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLipsticks); // Lo añadimos al menu

                //Lipsticks
                List<string> lipsticksColorValues = new List<string>();

                for (float i = 1; i < SkinsUtils.COLOR_PALETTES.Count() + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    lipsticksColorValues.Add(GetConfig.Langs["LipsticksColors"] + i);
                }

                btnSelectorLipsticksColor = new MenuListItem(GetConfig.Langs["LipsticksColorsList"], lipsticksColorValues, 0, GetConfig.Langs["LipsticksColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLipsticksColor); // Lo añadimos al menu

                //Lipsticks
                List<string> lipsticksPColorValues = new List<string>();

                for (float i = 1; i < 255; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    lipsticksPColorValues.Add(GetConfig.Langs["LipsticksPColors"] + i);
                }

                btnSelectorLipsticksPColor = new MenuListItem(GetConfig.Langs["LipsticksPColorsList"], lipsticksPColorValues, 0, GetConfig.Langs["LipsticksPColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLipsticksPColor); // Lo añadimos al menu

                //Lipsticks
                List<string> shadowsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["shadows"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    shadowsValues.Add(GetConfig.Langs["Shadows"] + i);
                }

                btnSelectorShadows = new MenuListItem(GetConfig.Langs["ShadowsList"], shadowsValues, 0, GetConfig.Langs["ShadowsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorShadows); // Lo añadimos al menu

                //Lipsticks
                List<string> shadowsColorValues = new List<string>();

                for (float i = 1; i < SkinsUtils.COLOR_PALETTES.Count() + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    shadowsColorValues.Add(GetConfig.Langs["ShadowsColors"] + i);
                }

                btnSelectorShadowsColor = new MenuListItem(GetConfig.Langs["ShadowsColorsList"], shadowsColorValues, 0, GetConfig.Langs["ShadowsColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorShadowsColor); // Lo añadimos al menu

                //Lipsticks
                List<string> shadowsPColorValues = new List<string>();

                for (float i = 1; i < 255; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    shadowsPColorValues.Add(GetConfig.Langs["ShadowsPColors"] + i);
                }

                btnSelectorShadowsPColor = new MenuListItem(GetConfig.Langs["ShadowsPColorsList"], shadowsPColorValues, 0, GetConfig.Langs["ShadowsPColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorShadowsPColor); // Lo añadimos al menu
            }
            else //Female
            {
                //Body Colors
                List<string> bodyValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"].Count(); i++)
                {
                    bodyValues.Add(GetConfig.Langs["BodyColorValues"] + i);
                }

                btnSelectorBody = new MenuListItem(GetConfig.Langs["BodyColor"], bodyValues, 0, GetConfig.Langs["BodyColorDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBody); // Lo añadimos al menu

                //Faces
                List<string> faceValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"][0]["Heads"].Count(); i++)
                {
                    faceValues.Add(GetConfig.Langs["FaceValues"] + i);
                }

                btnSelectorFace = new MenuListItem(GetConfig.Langs["FaceType"], faceValues, 0, GetConfig.Langs["FaceTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorFace); // Lo añadimos al menu

                //Torso
                List<string> torsoValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"][0]["Body"].Count(); i++)
                {
                    torsoValues.Add(GetConfig.Langs["TorsoValues"] + i);
                }

                btnSelectorTorso = new MenuListItem(GetConfig.Langs["TorsoType"], torsoValues, 0, GetConfig.Langs["TorsoTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorTorso); // Lo añadimos al menu

                //Endoforma
                List<string> endoformValues = new List<string>();

                for (int i = 1; i < SkinsUtils.BODY_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    endoformValues.Add(GetConfig.Langs["BodySizeValue"] + i);
                }

                MenuListItem btnSelectorBodyForm = new MenuListItem(GetConfig.Langs["BodyType"], endoformValues, 0, GetConfig.Langs["BodyTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBodyForm); // Lo añadimos al menu

                //Waist
                List<string> waistValues = new List<string>();

                for (int i = 1; i < SkinsUtils.WAIST_TYPES.Count + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    waistValues.Add(GetConfig.Langs["WaistValue"] + i);
                }

                MenuListItem btnSelectorWaist = new MenuListItem(GetConfig.Langs["WaistType"], waistValues, 0, GetConfig.Langs["WaistTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorWaist); // Lo añadimos al menu

                //Legs
                List<string> legsValues = new List<string>();
                for (int i = 1; i <= GetConfig.Config["Female"][0]["Legs"].Count(); i++)
                {
                    legsValues.Add(GetConfig.Langs["LegsValues"] + i);
                }

                btnSelectorLegs = new MenuListItem(GetConfig.Langs["LegsType"], legsValues, 0, GetConfig.Langs["LegsTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLegs); // Lo añadimos al menu

                //Hair
                List<string> hairValues = new List<string>();

                for (float i = 1; i < SkinsUtils.HAIR_FEMALE.Count + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    hairValues.Add(GetConfig.Langs["HairValue"] + i);
                }

                MenuListItem btnSelectorHairs = new MenuListItem(GetConfig.Langs["HairType"], hairValues, 0, GetConfig.Langs["HairTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorHairs); // Lo añadimos al menu

                //barbas
                List<string> eyesValues = new List<string>();

                for (float i = 1; i < SkinsUtils.EYES_FEMALE.Count; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    eyesValues.Add(GetConfig.Langs["Eyes"] + i);
                }

                MenuListItem btnSelectorEyes = new MenuListItem(GetConfig.Langs["EyesList"], eyesValues, 0, GetConfig.Langs["EyesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorEyes); // Lo añadimos al menu

                //Beard
                List<string> beardValues = new List<string>();

                beardValues.Add(GetConfig.Langs["NoExistValue"]);

                MenuListItem btnSelectorBeards = new MenuListItem(GetConfig.Langs["BeardType"], beardValues, 0, GetConfig.Langs["BeardTypeDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorBeards); // Lo añadimos al menu

                //eyeBrows
                List<string> eyeBrowsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["eyebrows"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    eyeBrowsValues.Add(GetConfig.Langs["EyeBrows"] + i);
                }

                btnSelectoreyeBrows = new MenuListItem(GetConfig.Langs["EyeBrowsList"], eyeBrowsValues, 0, GetConfig.Langs["EyeBrowsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectoreyeBrows); // Lo añadimos al menu

                //Scars
                List<string> scarsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["scars"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    scarsValues.Add(GetConfig.Langs["Scars"] + i);
                }

                btnSelectorScars = new MenuListItem(GetConfig.Langs["ScarsList"], scarsValues, 0, GetConfig.Langs["ScarsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorScars); // Lo añadimos al menu

                //Spots
                List<string> spotsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["spots"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    spotsValues.Add(GetConfig.Langs["Spots"] + i);
                }

                btnSelectorSpots = new MenuListItem(GetConfig.Langs["SpotsList"], spotsValues, 0, GetConfig.Langs["SpotsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorSpots); // Lo añadimos al menu

                //Disc
                List<string> discValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["disc"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    discValues.Add(GetConfig.Langs["Disc"] + i);
                }

                btnSelectorDisc = new MenuListItem(GetConfig.Langs["DiscList"], discValues, 0, GetConfig.Langs["DiscDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorDisc); // Lo añadimos al menu

                //Complex
                List<string> complexValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["complex"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    complexValues.Add(GetConfig.Langs["Complex"] + i);
                }

                btnSelectorComplex = new MenuListItem(GetConfig.Langs["ComplexList"], complexValues, 0, GetConfig.Langs["ComplexDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorComplex); // Lo añadimos al menu

                //Acne
                List<string> acneValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["acne"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    acneValues.Add(GetConfig.Langs["Acne"] + i);
                }

                btnSelectorAcne = new MenuListItem(GetConfig.Langs["AcneList"], acneValues, 0, GetConfig.Langs["AcneDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorAcne); // Lo añadimos al menu

                //Ageing
                List<string> ageingValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["ageing"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    ageingValues.Add(GetConfig.Langs["Ageing"] + i);
                }

                btnSelectorAgeing = new MenuListItem(GetConfig.Langs["AgeingList"], ageingValues, 0, GetConfig.Langs["AgeingDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorAgeing); // Lo añadimos al menu

                //Moles
                List<string> molesValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["moles"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    molesValues.Add(GetConfig.Langs["Moles"] + i);
                }

                btnSelectorMoles = new MenuListItem(GetConfig.Langs["MolesList"], molesValues, 0, GetConfig.Langs["MolesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorMoles); // Lo añadimos al menu

                //Freckles
                List<string> frecklesValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["freckles"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    frecklesValues.Add(GetConfig.Langs["Freckles"] + i);
                }

                btnSelectorFreckles = new MenuListItem(GetConfig.Langs["FrecklesList"], frecklesValues, 0, GetConfig.Langs["FrecklesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorFreckles); // Lo añadimos al menu

                //Freckles
                List<string> grimeValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["grime"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    grimeValues.Add(GetConfig.Langs["Grimes"] + i);
                }

                btnSelectorGrime = new MenuListItem(GetConfig.Langs["GrimesList"], grimeValues, 0, GetConfig.Langs["GrimesDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorGrime); // Lo añadimos al menu

                //Lipsticks
                List<string> lipsticksValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["lipsticks"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    lipsticksValues.Add(GetConfig.Langs["Lipsticks"] + i);
                }

                btnSelectorLipsticks = new MenuListItem(GetConfig.Langs["LipsticksList"], lipsticksValues, 0, GetConfig.Langs["LipsticksDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLipsticks); // Lo añadimos al menu

                //Lipsticks
                List<string> lipsticksColorValues = new List<string>();

                for (float i = 1; i < SkinsUtils.COLOR_PALETTES.Count() + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    lipsticksColorValues.Add(GetConfig.Langs["LipsticksColors"] + i);
                }

                btnSelectorLipsticksColor = new MenuListItem(GetConfig.Langs["LipsticksColorsList"], lipsticksColorValues, 0, GetConfig.Langs["LipsticksColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLipsticksColor); // Lo añadimos al menu

                //Lipsticks
                List<string> lipsticksPColorValues = new List<string>();

                for (float i = 1; i < 255; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    lipsticksPColorValues.Add(GetConfig.Langs["LipsticksPColors"] + i);
                }

                btnSelectorLipsticksPColor = new MenuListItem(GetConfig.Langs["LipsticksPColorsList"], lipsticksPColorValues, 0, GetConfig.Langs["LipsticksPColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorLipsticksPColor); // Lo añadimos al menu

                //Lipsticks
                List<string> shadowsValues = new List<string>();

                for (float i = 1; i < SkinsUtils.overlays_info["shadows"].Count() + 2; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    shadowsValues.Add(GetConfig.Langs["Shadows"] + i);
                }

                btnSelectorShadows = new MenuListItem(GetConfig.Langs["ShadowsList"], shadowsValues, 0, GetConfig.Langs["ShadowsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorShadows); // Lo añadimos al menu

                //Lipsticks
                List<string> shadowsColorValues = new List<string>();

                for (float i = 1; i < SkinsUtils.COLOR_PALETTES.Count() + 1; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    shadowsColorValues.Add(GetConfig.Langs["ShadowsColors"] + i);
                }

                btnSelectorShadowsColor = new MenuListItem(GetConfig.Langs["ShadowsColorsList"], shadowsColorValues, 0, GetConfig.Langs["ShadowsColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorShadowsColor); // Lo añadimos al menu

                //Lipsticks
                List<string> shadowsPColorValues = new List<string>();

                for (float i = 1; i < 255; i++) //Recuerda un +1 a la lista ya que empezamos desde el (INT I = 1)
                {
                    shadowsPColorValues.Add(GetConfig.Langs["ShadowsPColors"] + i);
                }

                btnSelectorShadowsPColor = new MenuListItem(GetConfig.Langs["ShadowsPColorsList"], shadowsPColorValues, 0, GetConfig.Langs["ShadowsPColorsDesc"]); // Añadimos la lista al boton
                skinMenu.AddMenuItem(btnSelectorShadowsPColor); //Lo añadimos al menu 
            }


            skinMenu.OnMenuOpen += (_menu) => {

            };

            skinMenu.OnMenuClose += (_menu) =>
            {

            };

            skinMenu.OnListIndexChange += (_menu, _listItem, _oldIndex, _newIndex, _itemIndex) =>
            {
                switch (_itemIndex)
                {
                    case 0:
                        if (CreateCharacter.model_selected == "mp_male") // Male
                        {
                            UpdateLists(_newIndex, "Male");
                        }
                        else
                        {
                            UpdateLists(_newIndex, "Female");
                        }
                        break;
                    case 1:
                        if (CreateCharacter.model_selected == "mp_male") // Male
                        {
                            CreateCharacter.SetPlayerModelComponent(GetConfig.Config["Male"][btnSelectorBody.ListIndex]["Heads"][_newIndex].ToString(), "HeadType");
                            ReloadTextures();
                        }
                        else
                        {
                            CreateCharacter.SetPlayerModelComponent(GetConfig.Config["Female"][btnSelectorBody.ListIndex]["Heads"][_newIndex].ToString(), "HeadType");
                            ReloadTextures();
                        }
                        break;
                    case 2:
                        if (CreateCharacter.model_selected == "mp_male") // Male
                        {
                            CreateCharacter.SetPlayerModelComponent(GetConfig.Config["Male"][btnSelectorBody.ListIndex]["Body"][_newIndex].ToString(), "BodyType");
                        }
                        else
                        {
                            CreateCharacter.SetPlayerModelComponent(GetConfig.Config["Female"][btnSelectorBody.ListIndex]["Body"][_newIndex].ToString(), "BodyType");
                        }
                        break;
                    case 3:
                        CreateCharacter.SetPlayerBodyComponent((uint)SkinsUtils.BODY_TYPES.ElementAt(_newIndex), "Body");
                        break;
                    case 4:
                        CreateCharacter.SetPlayerBodyComponent((uint)SkinsUtils.WAIST_TYPES.ElementAt(_newIndex), "Waist");
                        break;
                    case 5:
                        if (CreateCharacter.model_selected == "mp_male") // Male 
                        {
                            CreateCharacter.SetPlayerModelComponent(GetConfig.Config["Male"][btnSelectorBody.ListIndex]["Legs"][_newIndex].ToString(), "LegsType");
                        }
                        else
                        {
                            CreateCharacter.SetPlayerModelComponent(GetConfig.Config["Female"][btnSelectorBody.ListIndex]["Legs"][_newIndex].ToString(), "LegsType");
                        }
                        break;
                    case 6:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.SetPlayerModelListComps("Hair", 0, 0x864B03AE);
                        }
                        else if (CreateCharacter.model_selected == "mp_male")
                        {
                            CreateCharacter.SetPlayerModelListComps("Hair", SkinsUtils.HAIR_MALE.ElementAt(_newIndex - 1), 0x864B03AE);
                        }
                        else
                        {
                            CreateCharacter.SetPlayerModelListComps("Hair", SkinsUtils.HAIR_FEMALE.ElementAt(_newIndex - 1), 0x864B03AE);
                        }
                        break;
                    case 7:
                        if (CreateCharacter.model_selected == "mp_male")
                        {
                            CreateCharacter.SetPlayerModelListComps("Eyes", SkinsUtils.EYES_MALE.ElementAt(_newIndex), 0x864B03AE);
                        }
                        else
                        {
                            CreateCharacter.SetPlayerModelListComps("Eyes", SkinsUtils.EYES_FEMALE.ElementAt(_newIndex), 0x864B03AE);
                        }
                        break;
                    case 8:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.SetPlayerModelListComps("Beard", 0, 0xF8016BCA);
                        }
                        else if (CreateCharacter.model_selected == "mp_male")
                        {
                            CreateCharacter.SetPlayerModelListComps("Beard", SkinsUtils.BEARD_MALE.ElementAt(_newIndex - 1), 0xF8016BCA);
                        }
                        break;
                    case 9:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("eyebrows", 0, _newIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("eyebrows", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 10:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("scars", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("scars", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 11:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("spots", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("spots", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 12:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("disc", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("disc", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 13:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("complex", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("complex", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 14:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("acne", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("acne", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 15:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("ageing", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("ageing", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 16:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("moles", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("moles", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 17:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("freckles", 0, _newIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("freckles", 1, _newIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 18:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("grime", 0, _newIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("grime", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 19:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("lipsticks", 0, _newIndex, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("lipsticks", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 20:
                        if (btnSelectorLipsticks.ListIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("lipsticks", 0, btnSelectorLipsticks.ListIndex, 0, 0, 0, 1.0f, 0, _newIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("lipsticks", 1, btnSelectorLipsticks.ListIndex - 1, 0, 0, 0, 1.0f, 0, _newIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 21:
                        if (btnSelectorLipsticks.ListIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("lipsticks", 0, btnSelectorLipsticks.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("lipsticks", 1, btnSelectorLipsticks.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 22:
                        if (_newIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("shadows", 0, _newIndex, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorShadowsPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("shadows", 1, _newIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorShadowsPColor.ListIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                    case 23:
                        if (btnSelectorShadows.ListIndex == 0)
                        {
                            CreateCharacter.toggleOverlayChange("shadows", 0, btnSelectorShadows.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        else
                        {
                            CreateCharacter.toggleOverlayChange("shadows", 1, btnSelectorShadows.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, _newIndex, 0, 0, 0, 1.0f);
                        }
                        break;
                }
            };

        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return skinMenu;
        }

        private static void ReloadTextures() {

            if (btnSelectoreyeBrows.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("eyebrows", 0, btnSelectoreyeBrows.ListIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("eyebrows", 1, btnSelectoreyeBrows.ListIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorScars.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("scars", 0, btnSelectorScars.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("scars", 1, btnSelectorScars.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorSpots.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("spots", 0, btnSelectorSpots.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("spots", 1, btnSelectorSpots.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorDisc.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("disc", 0, btnSelectorDisc.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("disc", 1, btnSelectorDisc.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorComplex.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("complex", 0, btnSelectorComplex.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("complex", 1, btnSelectorComplex.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorAcne.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("acne", 0, btnSelectorAcne.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("acne", 1, btnSelectorAcne.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorAgeing.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("ageing", 0, btnSelectorAgeing.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("ageing", 1, btnSelectorAgeing.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorMoles.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("moles", 0, btnSelectorMoles.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("moles", 1, btnSelectorMoles.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorFreckles.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("freckles", 0, btnSelectorFreckles.ListIndex, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("freckles", 1, btnSelectorFreckles.ListIndex - 1, 0, 0, 1, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorGrime.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("grime", 0, btnSelectorGrime.ListIndex, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("grime", 1, btnSelectorGrime.ListIndex - 1, 0, 0, 0, 1.0f, 0, 0, 0, 0, 0, 0, 1.0f);
            }
            if (btnSelectorLipsticks.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("lipsticks", 0, btnSelectorLipsticks.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("lipsticks", 1, btnSelectorLipsticks.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorLipsticksColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
            if (btnSelectorShadows.ListIndex == 0)
            {
                CreateCharacter.toggleOverlayChange("shadows", 0, btnSelectorShadows.ListIndex, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
            else
            {
                CreateCharacter.toggleOverlayChange("shadows", 1, btnSelectorShadows.ListIndex - 1, 0, 0, 0, 1.0f, 0, btnSelectorShadowsColor.ListIndex, btnSelectorLipsticksPColor.ListIndex, 0, 0, 0, 1.0f);
            }
        }

        private static void UpdateLists(int index, string sex)
        {
            btnSelectorFace.ListItems.Clear();
            btnSelectorTorso.ListItems.Clear();
            btnSelectorLegs.ListItems.Clear();

            //Faces
            List<string> faceValues = new List<string>();
            for (int i = 1; i <= GetConfig.Config[sex][index]["Heads"].Count(); i++)
            {
                btnSelectorFace.ListItems.Add(GetConfig.Langs["FaceValues"] + i);
            }
            btnSelectorFace.ListIndex = 0;

            //Torso
            List<string> torsoValues = new List<string>();
            for (int i = 1; i <= GetConfig.Config[sex][index]["Body"].Count(); i++)
            {
                btnSelectorTorso.ListItems.Add(GetConfig.Langs["TorsoValues"] + i);
            }
            btnSelectorTorso.ListIndex = 0;

            //Legs
            List<string> legsValues = new List<string>();
            for (int i = 1; i <= GetConfig.Config[sex][index]["Legs"].Count(); i++)
            {
                btnSelectorLegs.ListItems.Add(GetConfig.Langs["LegsValues"] + i);
            }
            btnSelectorLegs.ListIndex = 0;

            CreateCharacter.SetPlayerModelComponent(GetConfig.Config[sex][index]["Heads"][0].ToString(), "HeadType");
            CreateCharacter.SetPlayerModelComponent(GetConfig.Config[sex][index]["Body"][0].ToString(), "BodyType");
            CreateCharacter.SetPlayerModelComponent(GetConfig.Config[sex][index]["Legs"][0].ToString(), "LegsType");

            CreateCharacter.texture_types["albedo"] = API.GetHashKey(GetConfig.Config[sex][index]["HeadTexture"].ToString());
            CreateCharacter.skinPlayer["albedo"] = API.GetHashKey(GetConfig.Config[sex][index]["HeadTexture"].ToString());
            ReloadTextures();
        } 
    }
}
