using CitizenFX.Core;
using CitizenFX.Core.Native;
using VorpCharacter.Diagnostics;
using VorpCharacter.Web.Discord.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VorpCharacter.Models.Discord;

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
        public Dictionary<WebhookChannel, DiscordWebhook> Webhooks = new Dictionary<WebhookChannel, DiscordWebhook>();
        static DateTime lastUpdate = DateTime.Now;
        static bool IsDelayRunnning = false;

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
        private async Task OnDiscordWebhookUpdate()
        {
            if (DateTime.Now.Subtract(lastUpdate).TotalSeconds > 120)
            {
                // update every 2 minutes
                lastUpdate = DateTime.Now;
                UpdateWebhooks();
            }

            while (Webhooks.Count == 0)
            {
                // init
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

        private async Task UpdateWebhooks()
        {

        }

        public async Task<RequestResponse> DiscordWebsocket(string method, string url, string jsonData = "")
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", $"Bot {PluginManager.DiscordBotKey}");
            return await request.Http($"{url}", method, jsonData, headers);
        }

        public async Task<RequestResponse> DiscordRequest(string method, string endpoint, string jsonData = "")
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", $"Bot {PluginManager.DiscordBotKey}");
            return await request.Http($"https://discordapp.com/api/{endpoint}", method, jsonData, headers);
        }

        public async Task<string> Avatar(ulong discordId)
        {
            string url = $"https://api.discord.wf/v2/users/{discordId}/avatar";
            RequestResponse requestResponse = await request.Http(url);
            if (requestResponse.status == System.Net.HttpStatusCode.OK)
            {
                DiscordAvatar avatar = JsonConvert.DeserializeObject<DiscordAvatar>(requestResponse.content);
                return avatar.Avatarurl;
            }
            return string.Empty;
        }

        public async Task SendDiscordEmbededMessage(WebhookChannel webhookChannel, string name, string title, string description, DiscordColor discordColor)
        {
            try
            {
                if (!Webhooks.ContainsKey(webhookChannel))
                {
                    Logger.Warn($"SendDiscordEmbededMessage() -> Discord {webhookChannel} Webhook Missing");
                    return;
                }

                if (IsDelayRunnning) return;

                string cleanName = StripUnicodeCharactersFromString(name);

                DiscordWebhook discordWebhook = Webhooks[webhookChannel];

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

                await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                Logger.Error($"SendDiscordEmbededMessage() -> {ex.Message}");
            }
        }

        public async Task SendDiscordSimpleMessage(WebhookChannel webhookChannel, string username, string name, string message)
        {
            try
            {
                DiscordWebhook discordWebhook = Webhooks[webhookChannel];

                Webhook webhook = new Webhook(discordWebhook.Url);

                webhook.AvatarUrl = discordWebhook.Avatar;
                webhook.Content = StripUnicodeCharactersFromString($"{name} > {message.Trim('"')}");
                webhook.Username = StripUnicodeCharactersFromString(username);

                await BaseScript.Delay(0);

                await webhook.Send();

                await Task.FromResult(0);
            }
            catch (Exception ex)
            {
                Logger.Error($"SendDiscordSimpleMessage() -> {ex.Message}");
            }
        }
    }
}
