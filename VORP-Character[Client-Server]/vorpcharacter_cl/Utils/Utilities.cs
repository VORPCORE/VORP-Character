using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using System.Threading.Tasks;
using VorpCharacter.Diagnostics;
using System;
using VorpCharacter.Enums;

namespace VorpCharacter.Utils
{
    internal class Utilities
    {
        const int MAX_COMPONENT_CHANGE_DELAY = 0;
        public static Random RANDOM = new Random();

        public static void SetAttributeCoreValue(int pedHandle, eAttributeCore attribute, int value)
        {
            Function.Call((Hash)0xC6258F41D86676E0, pedHandle, (int)attribute, value);
        }
        public static int GetAttributeCoreValue(int pedHandle, eAttributeCore attribute)
        {
            return Function.Call<int>((Hash)0xC6258F41D86676E0, pedHandle, (int)attribute);
        }

        public static bool IsPedReadyToRender(int pedHandle)
        {
            return Function.Call<bool>((Hash)0xA0BC8FAED8CFEB3C, pedHandle);
        }

        public static void RemoveTagFromMetaPed(int pedHandle, uint component, int p2)
        {
            Function.Call((Hash)0xD710A5007C2AC539, pedHandle, component, p2);
        }

        public static void SetPedOutfitPreset(int pedHandle, int presetId)
        {
            Function.Call((Hash)0x77FF8D35EEC6BBC4, pedHandle, presetId, 0);
        }

        public static void SetPedBodyComponent(int pedHandle, uint hash)
        {
            Function.Call((Hash)0x1902C4CFCC5BE57C, pedHandle, hash);
        }

        public static async Task SetPedFaceFeature(int pedHandle, uint index, float value, bool updateVariation = false)
        {
            Function.Call((Hash)0x5653AB26C82938CF, pedHandle, index, value);
            if (updateVariation) UpdatePedVariation(pedHandle);
            await BaseScript.Delay(MAX_COMPONENT_CHANGE_DELAY);
        }

        public static async Task ApplyShopItemToPed(int pedHandle, uint componentHash, bool immediately = true, bool isMultiplayer = true, bool p4 = true)
        {
            Function.Call((Hash)0xD3A7B003ED343FD9, pedHandle, componentHash, immediately, isMultiplayer, p4);
            UpdatePedVariation(pedHandle);
            await BaseScript.Delay(MAX_COMPONENT_CHANGE_DELAY);
        }

        public async static Task<int> SetPlayerModel(uint hash)
        {
            Function.Call((Hash)0xED40380076A31506, PlayerId(), hash, true);
            await BaseScript.Delay(100);
            int playerPedHandle = Cache.PlayerPedId;
            UpdatePedVariation(playerPedHandle);
            return playerPedHandle;
        }

        public static void UpdatePedVariation(int pedHandle)
        {
            Function.Call((Hash)0xCC8CA3E88256E58F, pedHandle, 0, 1, 1, 1, false);
        }

        public static async Task SetPedScale(int pedHandle, float scale)
        {
            await BaseScript.Delay(10);
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
