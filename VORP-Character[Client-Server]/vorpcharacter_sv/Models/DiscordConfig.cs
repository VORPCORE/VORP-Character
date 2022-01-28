using System.Collections.Generic;
using System.Runtime.Serialization;
using VorpCharacter.Web.Discord.Entity;

namespace VorpCharacter.Models
{
    [DataContract]
    public class DiscordConfig
    {
        [DataMember(Name = "webHooks")]
        public List<DiscordWebhook> WebHooks;
    }
}