using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using VorpCharacter.Extensions;
using VorpCharacter.Script;

namespace VorpCharacter.Utils
{
    class Commands : BaseScript
    {
        static int playerPedId => Cache.PlayerPedId;

        public static void InitCommands()
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

        private static void CoatClosed()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x0662AC34))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x0662AC34, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["CoatClosed"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Hat()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x9925C067))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x9925C067, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Hat"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void EyeWear()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x5E47CA6))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x5E47CA6, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["EyeWear"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Mask()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x7505EF42))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x7505EF42, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Mask"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void NeckWear()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x5FC29285))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x5FC29285, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckWear"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void NeckTies()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x7A96FACA))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x7A96FACA, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckTies"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Shirt()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x2026C46D))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x2026C46D, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Shirt"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Suspender()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x877A2CF7))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x877A2CF7, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Suspender"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Vest()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x485EE834))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x485EE834, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Vest"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Coat()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xE06D30CE))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xE06D30CE, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Coat"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Poncho()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xAF14310B))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xAF14310B, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Poncho"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Cloak()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x3C1A74CD))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x3C1A74CD, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Cloak"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Glove()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xEABE0032))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xEABE0032, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Glove"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Rings()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x7A6BBD0B))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x7A6BBD0B, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingRh"].ToString()), true, true, false);
            }
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xF16A1D23))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xF16A1D23, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingLh"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Bracelet()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x7BC10759))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x7BC10759, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Bracelet"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }


        private static void Belt()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x9B2C8B89))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x9B2C8B89, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gunbelt"].ToString()), true, true, false);
            }
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xB6B6122D))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xB6B6122D, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Holster"].ToString()), true, true, false);
            }
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xA6D134C6))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xA6D134C6, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Belt"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Buckle()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xFAE9107F))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xFAE9107F, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Buckle"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Pant()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x1D4C528A))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x1D4C528A, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Pant"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Skirt()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xA0E3AB7F))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xA0E3AB7F, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Skirt"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Chap()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x3107499B))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x3107499B, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Chap"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static void Boots()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x777EC6EF))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x777EC6EF, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Boots"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }
        private static void Spurs()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x18729F39))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x18729F39, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spurs"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }
        private static void Spats()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x514ADCEA))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x514ADCEA, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spats"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }
        private static void GunbeltAccs()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0xF1542D11))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0xF1542D11, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["GunbeltAccs"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }
        private static void Gauntlets()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x91CE9B20))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x91CE9B20, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gauntlets"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }
        private static void Loadouts()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x83887E88))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x83887E88, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Loadouts"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }
        private static void Accessories()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x79D7DF96))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x79D7DF96, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Accessories"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }
        private static void Satchels()
        {
            if (Utilities.IsMetapedUsingComponent(playerPedId, 0x94504D26))
            {
                Utilities.RemoveTagFromMetaPed(playerPedId, 0x94504D26, 0);
            }
            else
            {
                Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Accessories"].ToString()), true, true, false);
            }
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static async void Undress()
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
            await Delay(100);
            Utilities.UpdatePedVariation(playerPedId);
        }

        private static async void Dress()
        {

            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Hat"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["EyeWear"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckWear"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckTies"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Shirt"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Suspender"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Vest"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Coat"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["CoatClosed"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Poncho"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Cloak"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Glove"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingRh"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingLh"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Bracelet"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gunbelt"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Belt"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Buckle"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Holster"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Pant"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Skirt"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Chap"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Boots"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spurs"].ToString()), true, true, false);
            Utilities.ApplyShopItemToPed(playerPedId, LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["GunbeltAccs"].ToString()), true, true, false);
            await Delay(100);
            Utilities.UpdatePedVariation(playerPedId);
        }

    }
}