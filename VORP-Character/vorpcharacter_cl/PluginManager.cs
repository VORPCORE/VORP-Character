﻿using CitizenFX.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VORP.Character.Client.Diagnostics;
using VORP.Character.Client.Model;
using static CitizenFX.Core.Native.API;

namespace VORP.Character.Client
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

        public PluginManager()
        {
            LoadDefaultConfig();
            Logger.Info($"VORP Character Init");
            Instance = this;
            Load();
        }

        private async void Load()
        {
            await GetCore();

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
            Logger.Success($"getCore Loaded");
        }

        private void LoadDefaultConfig()
        {
            string jsonFile = LoadResourceFile(GetCurrentResourceName(), "/config/Config.json");

            if (string.IsNullOrEmpty(jsonFile))
            {
                Logger.Error($"Config.json file is missing.");
                return;
            }

            Config = JsonConvert.DeserializeObject<Config>(jsonFile);

            string languageFile = LoadResourceFile(GetCurrentResourceName(), $"/config/{Config.Defaultlang}.json");

            if (string.IsNullOrEmpty(languageFile))
            {
                Logger.Error($"{Config.Defaultlang}.json file is missing, defaulting to 'En.json'.");
                languageFile = LoadResourceFile(GetCurrentResourceName(), $"/config/En.json"); ;
            }
            else
            {
                Logger.Success($"{Config.Defaultlang}.json file has been loaded.");
            }

            Langs = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageFile);

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
            }));
        }

        public void AttachTickHandler(Func<Task> task)
        {
            Tick += task;
        }

        public void DetachTickHandler(Func<Task> task)
        {
            Tick -= task;
        }

    }
}