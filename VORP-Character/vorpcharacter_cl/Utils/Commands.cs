using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using VORP.Character.Client.Extensions;
using VORP.Character.Client.Script;

namespace VORP.Character.Client.Utils
{
    public class Commands : BaseScript
    {
        static int playerPedId => Cache.PlayerPedId;

        public Commands()
        {
            API.RegisterCommand(Common.GetTranslation("CommandHat"), new Action(Hat), false);
            API.RegisterCommand(Common.GetTranslation("CommandEyeWear"), new Action(EyeWear), false);
            API.RegisterCommand(Common.GetTranslation("CommandMask"), new Action(Mask), false);
            API.RegisterCommand(Common.GetTranslation("CommandNeckWear"), new Action(NeckWear), false);
            API.RegisterCommand(Common.GetTranslation("CommandNeckTies"), new Action(NeckTies), false);
            API.RegisterCommand(Common.GetTranslation("CommandShirt"), new Action(Shirt), false);
            API.RegisterCommand(Common.GetTranslation("CommandSuspender"), new Action(Suspender), false);
            API.RegisterCommand(Common.GetTranslation("CommandVest"), new Action(Vest), false);
            API.RegisterCommand(Common.GetTranslation("CommandCoat"), new Action(Coat), false);
            API.RegisterCommand(Common.GetTranslation("CommandCoatClosed"), new Action(CoatClosed), false);
            API.RegisterCommand(Common.GetTranslation("CommandPoncho"), new Action(Poncho), false);
            API.RegisterCommand(Common.GetTranslation("CommandCloak"), new Action(Cloak), false);
            API.RegisterCommand(Common.GetTranslation("CommandGlove"), new Action(Glove), false);
            API.RegisterCommand(Common.GetTranslation("CommandRings"), new Action(Rings), false);
            API.RegisterCommand(Common.GetTranslation("CommandBracelet"), new Action(Bracelet), false);
            API.RegisterCommand(Common.GetTranslation("CommandBelt"), new Action(Belt), false);
            API.RegisterCommand(Common.GetTranslation("CommandBuckle"), new Action(Buckle), false);
            API.RegisterCommand(Common.GetTranslation("CommandPant"), new Action(Pant), false);
            API.RegisterCommand(Common.GetTranslation("CommandSkirt"), new Action(Skirt), false);
            API.RegisterCommand(Common.GetTranslation("CommandChap"), new Action(Chap), false);
            API.RegisterCommand(Common.GetTranslation("CommandBoots"), new Action(Boots), false);
            API.RegisterCommand(Common.GetTranslation("CommandSpurs"), new Action(Spurs), false);
            API.RegisterCommand(Common.GetTranslation("CommandUndress"), new Action(Undress), false);
            API.RegisterCommand(Common.GetTranslation("CommandDress"), new Action(Dress), false);
            API.RegisterCommand(Common.GetTranslation("CommandSpats"), new Action(Spats), false);
            API.RegisterCommand(Common.GetTranslation("CommandGunbeltAccs"), new Action(GunbeltAccs), false);
            API.RegisterCommand(Common.GetTranslation("CommandGauntlets"), new Action(Gauntlets), false);
            API.RegisterCommand(Common.GetTranslation("CommandLoadouts"), new Action(Loadouts), false);
            API.RegisterCommand(Common.GetTranslation("CommandAccessories"), new Action(Accessories), false);
            API.RegisterCommand(Common.GetTranslation("CommandSatchels"), new Action(Satchels), false);
        }

        static void ToggleComponent(uint component, uint item)
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, component))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, component);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, item);
            }
        }

        private static void CoatClosed() => ToggleComponent(0x0662AC34, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["CoatClosed"].ToString()));
        private static void Hat() => ToggleComponent(0x9925C067, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Hat"].ToString()));
        private static void EyeWear() => ToggleComponent(0x5E47CA6, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["EyeWear"].ToString()));
        private static void Mask() => ToggleComponent(0x7505EF42, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Mask"].ToString()));
        private static void NeckWear() => ToggleComponent(0x5FC29285, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckWear"].ToString()));
        private static void NeckTies() => ToggleComponent(0x7A96FACA, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckTies"].ToString()));
        private static void Shirt() => ToggleComponent(0x2026C46D, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Shirt"].ToString()));
        private static void Suspender() => ToggleComponent(0x877A2CF7, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Suspender"].ToString()));
        private static void Vest() => ToggleComponent(0x485EE834, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Vest"].ToString()));
        private static void Coat() => ToggleComponent(0xE06D30CE, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Coat"].ToString()));
        private static void Poncho() => ToggleComponent(0xAF14310B, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Poncho"].ToString()));
        private static void Cloak() => ToggleComponent(0x3C1A74CD, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Cloak"].ToString()));
        private static void Glove() => ToggleComponent(0xEABE0032, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Glove"].ToString()));

        private static void Rings()
        {
            ToggleComponent(0x7A6BBD0B, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingRh"].ToString()));
            ToggleComponent(0xF16A1D23, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingLh"].ToString()));
        }

        private static void Bracelet() => ToggleComponent(0x7BC10759, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Bracelet"].ToString()));

        private static void Belt()
        {
            ToggleComponent(0x9B2C8B89, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gunbelt"].ToString()));
            ToggleComponent(0xB6B6122D, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Holster"].ToString()));
            ToggleComponent(0xA6D134C6, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Belt"].ToString()));
        }

        private static void Buckle() => ToggleComponent(0xFAE9107F, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Buckle"].ToString()));
        private static void Pant() => ToggleComponent(0x1D4C528A, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Pant"].ToString()));
        private static void Skirt() => ToggleComponent(0xA0E3AB7F, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Skirt"].ToString()));
        private static void Chap() => ToggleComponent(0x3107499B, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Chap"].ToString()));
        private static void Boots() => ToggleComponent(0x777EC6EF, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Boots"].ToString()));
        private static void Spurs() => ToggleComponent(0x18729F39, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spurs"].ToString()));
        private static void Spats() => ToggleComponent(0x514ADCEA, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spats"].ToString()));
        private static void GunbeltAccs() => ToggleComponent(0xF1542D11, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["GunbeltAccs"].ToString()));
        private static void Gauntlets() => ToggleComponent(0x91CE9B20, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gauntlets"].ToString()));
        private static void Loadouts() => ToggleComponent(0x83887E88, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Loadouts"].ToString()));
        private static void Accessories() => ToggleComponent(0x79D7DF96, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Accessories"].ToString()));
        private static void Satchels() => ToggleComponent(0x83887E88, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Satchels"].ToString()));

        private static void Undress()
        {
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x9925C067, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x5E47CA6, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x5FC29285, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x7A96FACA, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x2026C46D, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x877A2CF7, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x485EE834, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xE06D30CE, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x0662AC34, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xAF14310B, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x3C1A74CD, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xEABE0032, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x7A6BBD0B, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xF16A1D23, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x7BC10759, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x9B2C8B89, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xA6D134C6, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xFAE9107F, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xB6B6122D, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x1D4C528A, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xA0E3AB7F, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x3107499B, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x777EC6EF, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x18729F39, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0xF1542D11, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x514ADCEA, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x91CE9B20, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x83887E88, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x79D7DF96, 0);
            Utilities.RemoveTagFromMetaPed(playerPedId, 0x94504D26, 0);
        }

        private static void Dress()
        {
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Hat"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["EyeWear"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckWear"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckTies"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Shirt"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Suspender"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Vest"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Coat"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["CoatClosed"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Poncho"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Cloak"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Glove"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingRh"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingLh"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Bracelet"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gunbelt"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Belt"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Buckle"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Holster"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Pant"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Skirt"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Chap"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Boots"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spurs"].ToString()));
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["GunbeltAccs"].ToString()));
        }

    }
}