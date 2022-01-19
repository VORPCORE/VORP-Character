using CitizenFX.Core;
using System;
using System.Threading.Tasks;
using VorpCharacter.Diagnostics;
using VorpCharacter.Model;

namespace VorpCharacter
{
    public class PluginManager : BaseScript
    {
        public static PluginManager Instance { get; private set; }
        public static VorpPlayer Player;
        public static dynamic CORE;
        public static int MAX_ALLOWED_CHARACTERS = 0;
        public EventHandlerDictionary EventRegistry => EventHandlers;
        public ExportDictionary ExportRegistry => Exports;

        public PluginManager()
        {
            Logger.Info($"VORP Character Init");
            Instance = this;
            Setup();
        }

        private async void Setup()
        {
            await GetCore();

            Player = new VorpPlayer();

            Logger.Info($"VORP Character Started");
        }

        public async Task GetCore()
        {
            while (CORE == null)
            {
                TriggerEvent("getCore", new Action<dynamic>((dic) =>
                {
                    CORE = dic;
                    CORE.RpcCall("vorp_characters:getMaxCharacters", new Action<int>((max) =>
                    {
                        MAX_ALLOWED_CHARACTERS = max;
                    }), "none");
                }));
                await Delay(100);
            }
        }

    }
}