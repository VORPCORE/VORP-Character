global using CitizenFX.Core;
global using VORP.Character.Server.Diagnostics;
global using static CitizenFX.Core.Native.API;
using System;
using System.Threading.Tasks;
using VORP.Character.Server.Script;
using VORP.Character.Server.Web;

namespace VORP.Character.Server
{
    public class PluginManager : BaseScript
    {
        public static PluginManager Instance { get; private set; }
        public static PlayerList PlayerList;
        public static dynamic CORE;
        public static int MAX_ALLOWED_CHARACTERS;
        public static bool USE_CORE_EXPORTS = false;

        public EventHandlerDictionary EventRegistry => EventHandlers;
        public ExportDictionary ExportRegistry => Exports;
        string _GHMattiMySqlResourceState => GetResourceState("ghmattimysql");

        readonly public DiscordClient DiscordClient = new();
        readonly public CharacterApi CharacterApi = new();

        public PluginManager()
        {
            Logger.Info($"Init VORP Character");

            Instance = this;
            PlayerList = Players;

            Setup();

            Logger.Debug($"Debugging is enabled.");

            Logger.Info($"VORP Character Loaded");
        }

        async Task VendorReady()
        {
            string dbResource = _GHMattiMySqlResourceState;
            if (dbResource == "missing")
            {
                while (true)
                {
                    Logger.Error($"ghmattimysql resource not found! Please make sure you have the resource!");
                    await Delay(1000);
                }
            }

            while (!(dbResource == "started"))
            {
                await Delay(500);
                dbResource = _GHMattiMySqlResourceState;
            }
        }

        async void Setup()
        {
            await VendorReady(); // wait till ghmattimysql resource has started

            USE_CORE_EXPORTS = GetResourceMetadata(GetCurrentResourceName(), "vorp_core_csharp_new", 0) == "true";

            if (USE_CORE_EXPORTS)
            {
                Logger.Info($"Using VORP-CORE Exports.");
            }

            DiscordClient.Init();
            CharacterApi.Init();

            AddEvents();

            TriggerEvent("getCore", new Action<dynamic>((dic) =>
            {
                Logger.Success($"VORP Core Setup");
                CORE = dic;
                MAX_ALLOWED_CHARACTERS = (int)CORE.maxCharacters;
                CORE.addRpcCallback("vorp_characters:getMaxCharacters", new Action<int, CallbackDelegate, dynamic>((source, cb, args) =>
                {
                    cb(MAX_ALLOWED_CHARACTERS);
                }));
            }));
        }


        void AddEvents()
        {
            EventRegistry.Add("playerJoined", new Action<Player>(([FromSource] player) =>
            {

            }));

            EventRegistry.Add("playerDropped", new Action<Player, string>(([FromSource] player, reason) =>
            {
                string steamIdent = $"steam:{player.Identifiers["steam"]}";
            }));

            EventRegistry.Add("onResourceStart", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;

                Logger.Info($"VORP Character Started");
            }));

            EventRegistry.Add("onResourceStop", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;

                Logger.Info($"Stopping VORP Character");

                UnregisterScript(DiscordClient);
            }));
        }
    }
}

