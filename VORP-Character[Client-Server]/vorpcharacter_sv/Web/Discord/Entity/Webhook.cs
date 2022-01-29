﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VorpCharacter.Web.Discord.Entity
{
    public class Webhook
    {
        static Request request = new Request();

        public string Url;

        [JsonProperty(PropertyName = "avatar_url")]
        public string AvatarUrl;

        [JsonProperty(PropertyName = "content")]
        public string Content;

        [JsonProperty(PropertyName = "username")]
        public string Username;

        [JsonProperty(PropertyName = "embeds")]
        public List<Embed> Embeds = new List<Embed>();

        public Webhook(string uri)
        {
            Url = uri;
        }

        public async Task Send()
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Authorization", $"Bot {DiscordClient.DiscordBotKey}");
            await request.Http($"{Url}", "POST", JsonConvert.SerializeObject(this), headers);
        }
    }
}
