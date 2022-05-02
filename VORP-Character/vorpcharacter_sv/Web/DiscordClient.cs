using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VorpCharacter.Diagnostics;
using VorpCharacter.Models;
using VorpCharacter.Web.Discord.Entity;

namespace VorpCharacter.Web
{
    public enum WebhookChannel
    {
        ServerEvent = 1
    }

    public enum DiscordColor : int
    {
        White = 16777215,
        Black = 0,
        Red = 16738657,
        Green = 7855479,
        Blue = 11454159,
        Orange = 16757575
    }

    public class DiscordClient : BaseScript
    {
        static Request request = new Request();
        public Dictionary<string, DiscordWebhook> Webhooks = new Dictionary<string, DiscordWebhook>();
        static long lastUpdate;
        static bool IsDelayRunnning = false;
        public static string DiscordBotKey;

        private static Regex _compiledUnicodeRegex = new Regex(@"[^\u0000-\u007F]", RegexOptions.Compiled);

        internal DiscordClient()
        {
            UpdateWebhooks();
        }

        public String StripUnicodeCharactersFromString(string inputValue)
        {
            return _compiledUnicodeRegex.Replace(inputValue, String.Empty);
        }

        [Tick]
        private async Task OnDiscordWebhookUpdateAsync()
        {
            if ((API.GetGameTimer() - lastUpdate) > 120000)
            {
                lastUpdate = API.GetGameTimer();
                UpdateWebhooks();
            }

            while (Webhooks.Count == 0)
            {
                UpdateWebhooks();
                await BaseScript.Delay(1000);
                if (Webhooks.Count == 0)
                {
                    Logger.Error($"No Discord Webhooks returned, trying again in five seconds.");
                    await BaseScript.Delay(5000);
                }
            }

            await BaseScript.Delay(10000);
        }

        private void UpdateWebhooks()
        {
            try
            {
                string serverConfigFile = API.LoadResourceFile(API.GetCurrentResourceName(), "/Resources/server-config.json");
                ServerConfig serverConfig = JsonConvert.DeserializeObject<ServerConfig>(serverConfigFile);
                DiscordBotKey = serverConfig.DiscordConfig.Key;
                serverConfig.DiscordConfig.WebHooks.ForEach(x =>
                {
                    if (!Webhooks.ContainsKey(x.Name))
                        Webhooks.Add(x.Name, x);

                    if (Webhooks.ContainsKey(x.Name))
                        Webhooks[x.Name] = x;
                });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error when trying to load Server Configuration, please check server-config.json exists.");
            }
        }

        public async Task SendDiscordEmbededMessageAsync(string webHookName, string name, string title, string description, DiscordColor discordColor)
        {
            try
            {
                if (!Webhooks.ContainsKey(webHookName))
                {
                    Logger.Warn($"SendDiscordEmbededMessage() -> Discord '{webHookName}' Webhook Missing from the config.");
                    return;
                }

                if (IsDelayRunnning) return;

                string cleanName = StripUnicodeCharactersFromString(name);

                DiscordWebhook discordWebhook = Webhooks[webHookName];

                Webhook webhook = new Webhook(discordWebhook.Url);

                webhook.AvatarUrl = discordWebhook.Avatar;
                webhook.Username = cleanName;

                Embed embed = new Embed();
                embed.Author = new EmbedAuthor { Name = cleanName, IconUrl = discordWebhook.Avatar };
                embed.Title = StripUnicodeCharactersFromString(title);
                embed.Description = StripUnicodeCharactersFromString(description);
                embed.Color = (int)discordColor;
                embed.Thumbnail = new EmbedThumbnail { Url = discordWebhook.Avatar };

                webhook.Embeds.Add(embed);
                await BaseScript.Delay(0);
                await webhook.Send();

                await BaseScript.Delay(0);
            }
            catch (Exception ex)
            {
                Logger.Error($"SendDiscordEmbededMessage() -> {ex.Message}");
            }
        }
    }
}
