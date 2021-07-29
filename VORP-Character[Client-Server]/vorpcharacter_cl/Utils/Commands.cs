using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcharacter_cl.Utils
{
    class Commands : BaseScript
    {
        public static void InitCommands()
        {
            API.RegisterCommand(GetConfig.Langs["CommandHat"], new Action(Hat), false);
            API.RegisterCommand(GetConfig.Langs["CommandEyeWear"], new Action(EyeWear), false);
            API.RegisterCommand(GetConfig.Langs["CommandMask"], new Action(Mask), false);
            API.RegisterCommand(GetConfig.Langs["CommandNeckWear"], new Action(NeckWear), false);
            API.RegisterCommand(GetConfig.Langs["CommandNeckTies"], new Action(NeckTies), false);
            API.RegisterCommand(GetConfig.Langs["CommandShirt"], new Action(Shirt), false);
            API.RegisterCommand(GetConfig.Langs["CommandSuspender"], new Action(Suspender), false);
            API.RegisterCommand(GetConfig.Langs["CommandVest"], new Action(Vest), false);
            API.RegisterCommand(GetConfig.Langs["CommandCoat"], new Action(Coat), false);
            API.RegisterCommand(GetConfig.Langs["CommandPoncho"], new Action(Poncho), false);
            API.RegisterCommand(GetConfig.Langs["CommandCloak"], new Action(Cloak), false);
            API.RegisterCommand(GetConfig.Langs["CommandGlove"], new Action(Glove), false);
            API.RegisterCommand(GetConfig.Langs["CommandRings"], new Action(Rings), false);
            API.RegisterCommand(GetConfig.Langs["CommandBracelet"], new Action(Bracelet), false);
            API.RegisterCommand(GetConfig.Langs["CommandBelt"], new Action(Belt), false);
            API.RegisterCommand(GetConfig.Langs["CommandBuckle"], new Action(Buckle), false);
            API.RegisterCommand(GetConfig.Langs["CommandPant"], new Action(Pant), false);
            API.RegisterCommand(GetConfig.Langs["CommandSkirt"], new Action(Skirt), false);
            API.RegisterCommand(GetConfig.Langs["CommandChap"], new Action(Chap), false);
            API.RegisterCommand(GetConfig.Langs["CommandBoots"], new Action(Boots), false);
            API.RegisterCommand(GetConfig.Langs["CommandSpurs"], new Action(Spurs), false);
            API.RegisterCommand(GetConfig.Langs["CommandUndress"], new Action(Undress), false);
            API.RegisterCommand(GetConfig.Langs["CommandDress"], new Action(Dress), false);
            API.RegisterCommand(GetConfig.Langs["CommandSpats"], new Action(Spats), false);
            API.RegisterCommand(GetConfig.Langs["CommandGunbeltAccs"], new Action(GunbeltAccs), false);
            API.RegisterCommand(GetConfig.Langs["CommandGauntlets"], new Action(Gauntlets), false);
            API.RegisterCommand(GetConfig.Langs["CommandLoadouts"], new Action(Loadouts), false);
            API.RegisterCommand(GetConfig.Langs["CommandAccessories"], new Action(Accessories), false);
            API.RegisterCommand(GetConfig.Langs["CommandSatchels"], new Action(Satchels), false);

        }

        private static void Hat()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x9925C067))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x9925C067, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Hat"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void EyeWear()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x5E47CA6))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x5E47CA6, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["EyeWear"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Mask()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x7505EF42))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x7505EF42, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Mask"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void NeckWear()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x5FC29285))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x5FC29285, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckWear"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void NeckTies()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x7A96FACA))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x7A96FACA, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckTies"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Shirt()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x2026C46D))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x2026C46D, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Shirt"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Suspender()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x877A2CF7))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x877A2CF7, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Suspender"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Vest()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x485EE834))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x485EE834, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Vest"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Coat()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xE06D30CE))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xE06D30CE, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Coat"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Poncho()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xAF14310B))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xAF14310B, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Poncho"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Cloak()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x3C1A74CD))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x3C1A74CD, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Cloak"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Glove()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xEABE0032))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xEABE0032, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Glove"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Rings()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x7A6BBD0B))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x7A6BBD0B, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingRh"].ToString()), true, true, false);
            }
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xF16A1D23))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xF16A1D23, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingLh"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Bracelet()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x7BC10759))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x7BC10759, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Bracelet"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }


        private static void Belt()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x9B2C8B89))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x9B2C8B89, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gunbelt"].ToString()), true, true, false);
            }
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xB6B6122D))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xB6B6122D, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Holster"].ToString()), true, true, false);
            }
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xA6D134C6))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xA6D134C6, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Belt"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Buckle()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xFAE9107F))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xFAE9107F, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Buckle"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Pant()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x1D4C528A))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x1D4C528A, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Pant"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Skirt()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xA0E3AB7F))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xA0E3AB7F, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Skirt"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Chap()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x3107499B))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x3107499B, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Chap"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static void Boots()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x777EC6EF))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x777EC6EF, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Boots"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }
        private static void Spurs()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x18729F39))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x18729F39, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spurs"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }
        private static void Spats()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x514ADCEA))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x514ADCEA, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spats"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }
        private static void GunbeltAccs()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0xF1542D11))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xF1542D11, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["GunbeltAccs"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }
        private static void Gauntlets()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x91CE9B20))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x91CE9B20, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gauntlets"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }
        private static void Loadouts()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x83887E88))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x83887E88, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Loadouts"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }
        private static void Accessories()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x79D7DF96))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x79D7DF96, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Accessories"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }
        private static void Satchels()
        {
            if (Function.Call<bool>((Hash)0xFB4891BD7578CDC1, API.PlayerPedId(), 0x94504D26))
            {
                Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x94504D26, 0);
            }
            else
            {
                Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Accessories"].ToString()), true, true, false);
            }
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static async void Undress()
        {

            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x9925C067, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x5E47CA6, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x5FC29285, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x7A96FACA, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x2026C46D, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x877A2CF7, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x485EE834, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xE06D30CE, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xAF14310B, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x3C1A74CD, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xEABE0032, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x7A6BBD0B, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xF16A1D23, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x7BC10759, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x9B2C8B89, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xA6D134C6, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xFAE9107F, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xB6B6122D, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x1D4C528A, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xA0E3AB7F, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x3107499B, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x777EC6EF, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x18729F39, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0xF1542D11, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x514ADCEA, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x91CE9B20, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x83887E88, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x79D7DF96, 0);
            Function.Call((Hash)0xD710A5007C2AC539, API.PlayerPedId(), 0x94504D26, 0);
            await Delay(100);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

        private static async void Dress()
        {

            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Hat"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["EyeWear"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckWear"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["NeckTies"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Shirt"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Suspender"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Vest"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Coat"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Poncho"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Cloak"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Glove"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingRh"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["RingLh"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Bracelet"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Gunbelt"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Belt"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Buckle"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Holster"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Pant"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Skirt"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Chap"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Boots"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["Spurs"].ToString()), true, true, false);
            Function.Call((Hash)0xD3A7B003ED343FD9, API.PlayerPedId(), LoadPlayer.ConvertValue(LoadPlayer.cache_cloths["GunbeltAccs"].ToString()), true, true, false);
            await Delay(100);
            Function.Call((Hash)0xCC8CA3E88256E58F, API.PlayerPedId(), 0, 1, 1, 1, 0);
        }

    }
}