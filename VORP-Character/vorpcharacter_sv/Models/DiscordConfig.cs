using System.Collections.Generic;
using System.Runtime.Serialization;
using VORP.Character.Server.Web.Discord.Entity;

namespace VORP.Character.Server.Models
{
    [DataContract]
    public class DiscordConfig
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "webHooks")]
        public List<DiscordWebhook> WebHooks;
    }
}