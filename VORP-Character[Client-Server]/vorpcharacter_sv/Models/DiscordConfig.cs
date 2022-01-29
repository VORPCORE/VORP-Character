using System.Collections.Generic;
using System.Runtime.Serialization;
using VorpCharacter.Web.Discord.Entity;

namespace VorpCharacter.Models
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