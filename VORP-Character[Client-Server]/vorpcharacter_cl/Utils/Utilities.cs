using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VorpCharacter.Diagnostics;
using VorpCharacter.Enums;
using static CitizenFX.Core.Native.API;

namespace VorpCharacter.Utils
{
    internal class Utilities
    {
        const int MAX_COMPONENT_CHANGE_DELAY = 10;
        public static Random RANDOM = new Random();

        public static void SetAttributeCoreValue(int pedHandle, eAttributeCore attribute, int value)
        {
            Function.Call((Hash)0xC6258F41D86676E0, pedHandle, (int)attribute, value);
        }

        public static int GetAttributeCoreValue(int pedHandle, eAttributeCore attribute)
        {
            return Function.Call<int>((Hash)0xC6258F41D86676E0, pedHandle, (int)attribute);
        }

        public static bool IsMetapedUsingComponent(int pedHandle, ePedComponent component) => IsMetapedUsingComponent(pedHandle, (uint)component);
        public static bool IsMetapedUsingComponent(int pedHandle, uint component)
        {
            return Function.Call<bool>((Hash)0xFB4891BD7578CDC1, pedHandle, component);
        }

        public static bool IsPedHogtied(int pedHandle)
        {
            return Function.Call<bool>((Hash)0x3AA24CCC0D451379, pedHandle);
        }

        public static bool IsPedCuffed(int pedHandle)
        {
            return Function.Call<bool>((Hash)0x74E559B3BC910685, pedHandle);
        }

        public static bool IsPedReadyToRender(int pedHandle)
        {
            return Function.Call<bool>((Hash)0xA0BC8FAED8CFEB3C, pedHandle);
        }

        public static void RemoveTagFromMetaPed(int pedHandle, uint component, int p2 = 0, int delay = MAX_COMPONENT_CHANGE_DELAY)
        {
            Function.Call((Hash)0xD710A5007C2AC539, pedHandle, component, p2);
            UpdatePedVariation(pedHandle);
        }

        public static void SetPedOutfitPreset(int pedHandle, int presetId)
        {
            Function.Call((Hash)0x77FF8D35EEC6BBC4, pedHandle, presetId, 0);
        }

        public static void SetPedBodyComponent(int pedHandle, uint hash)
        {
            Function.Call((Hash)0x1902C4CFCC5BE57C, pedHandle, hash);
            UpdatePedVariation(pedHandle);
        }

        public static void SetPedFaceFeature(int pedHandle, ePedFaceFeature pedFaceFeature, Dictionary<string, string> skin, string key, bool updateVariation = false)
        {
            if (!skin.TryGetValue(key, out string keyValue))
            {
                Logger.Error($"SetPedFaceFeature: Skin model is missing key '{key}'.");
                return;
            }

            if (!float.TryParse(keyValue, out float value))
            {
                Logger.Error($"SetPedFaceFeature: Skin model, key '{key}' is not a valid float.");
                return;
            }

            Function.Call((Hash)0x5653AB26C82938CF, pedHandle, (uint)pedFaceFeature, value);
            if (updateVariation) UpdatePedVariation(pedHandle);
        }

        public static async Task SetPedFaceFeature(int pedHandle, ePedFaceFeature pedFaceFeature, float value, bool updateVariation = false, int delay = MAX_COMPONENT_CHANGE_DELAY)
        {
            Function.Call((Hash)0x5653AB26C82938CF, pedHandle, (uint)pedFaceFeature, value);
            if (updateVariation) UpdatePedVariation(pedHandle);
            await BaseScript.Delay(delay);
        }

        public static void ApplyShopItemToPed(int pedHandle, long componentHash, bool immediately = true, bool isMultiplayer = true, bool p4 = false)
        {
            Function.Call((Hash)0xD3A7B003ED343FD9, pedHandle, componentHash, immediately, isMultiplayer, p4);
            UpdatePedVariation(pedHandle);
        }

        public async static Task<int> SetPlayerModel(uint hash)
        {
            Function.Call((Hash)0xED40380076A31506, PlayerId(), hash, true);
            await BaseScript.Delay(100);
            int playerPedHandle = Cache.PlayerPedId;
            UpdatePedVariation(playerPedHandle);
            return playerPedHandle;
        }

        public static void UpdatePedVariation(int pedHandle, bool p1 = false, bool p5 = false)
        {
            Function.Call((Hash)0xCC8CA3E88256E58F, pedHandle, p1, true, true, true, p5);
        }

        public static async Task SetPedScale(int pedHandle, float scale)
        {
            await BaseScript.Delay(500); // wait 500ms before setting, speed is not in the nature of the wild west
            Function.Call((Hash)0x25ACFC650B65C538, pedHandle, scale);
        }

        public static async Task RequestModel(uint hash)
        {
            if (Function.Call<bool>(Hash.IS_MODEL_VALID, hash))
            {
                Function.Call(Hash.REQUEST_MODEL, hash);
                while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
                {
                    await BaseScript.Delay(100);
                }
            }
            else
            {
                Debug.WriteLine($"Model {hash} is not valid!");
            }
        }

        public static async Task FadeOutScreen(int duration)
        {
            DoScreenFadeOut(duration);
            while (IsScreenFadingOut())
            {
                await BaseScript.Delay(0);
            }
        }

        public static async Task FadeInScreen(int duration, int timeToWait = 500)
        {
            await BaseScript.Delay(timeToWait);
            DoScreenFadeIn(duration);
            while (IsScreenFadingIn())
            {
                await BaseScript.Delay(0);
            }
        }
    }
}
