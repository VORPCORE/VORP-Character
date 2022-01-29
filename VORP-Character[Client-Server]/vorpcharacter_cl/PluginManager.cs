﻿using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VorpCharacter.Diagnostics;
using VorpCharacter.Model;
using VorpCharacter.Script;
using static CitizenFX.Core.Native.API;

namespace VorpCharacter
{
    public class PluginManager : BaseScript
    {
        public static PluginManager Instance { get; private set; }
        public static dynamic CORE;
        public static int MAX_ALLOWED_CHARACTERS = 0;
        public EventHandlerDictionary EventRegistry => EventHandlers;
        public ExportDictionary ExportRegistry => Exports;

        public static Config Config = new Config();
        public static Dictionary<string, string> Langs = new Dictionary<string, string>();
        public static bool IsLoaded = false;

        public static readonly CreateCharacter _createCharacter = new();
        public static readonly LoadPlayer _loadPlayer = new();
        public static readonly SelectCharacter _selectCharacter = new();

        public PluginManager()
        {
            Logger.Info($"VORP Character Init");
            Instance = this;
            Setup();
        }

        private async void Setup()
        {
            await GetCore();
            LoadDefaultConfig();
            await IsReady();

            RegisterScript(_loadPlayer);
            RegisterScript(_createCharacter);
            RegisterScript(_selectCharacter);

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

        private void LoadDefaultConfig()
        {
            string jsonFile = LoadResourceFile(GetCurrentResourceName(), "config/Config.json");

            if (string.IsNullOrEmpty(jsonFile))
            {
                Logger.Error($"Config.json file is missing.");
                return;
            }

            Config = JsonConvert.DeserializeObject<Config>(jsonFile);

            string languageFile = LoadResourceFile(GetCurrentResourceName(), $"config/{Config.Defaultlang}.json");

            if (string.IsNullOrEmpty(languageFile))
            {
                Logger.Error($"{Config.Defaultlang}.json file is missing.");
                return;
            }

            Langs = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageFile);

            IsLoaded = true;

            Utils.Commands.InitCommands(); // this needs to be changed
        }

        public async Task IsReady()
        {
            while (!IsLoaded)
            {
                await Delay(100);
            }
        }

        void AddEvents()
        {
            EventRegistry.Add("onResourceStart", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;

                Logger.Info($"VORP Character Started");
            }));

            EventRegistry.Add("onResourceStop", new Action<string>(resourceName =>
            {
                if (resourceName != GetCurrentResourceName()) return;

                Logger.Info($"Stopping VORP Character");

                UnregisterScript(_loadPlayer);
                UnregisterScript(_createCharacter);
                UnregisterScript(_selectCharacter);
            }));
        }

    }
}