using System.Runtime.Serialization;

namespace VorpCharacter.Web.Discord.Entity
{
    [DataContract]
    public class DiscordWebhook
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "avatar")]
        public string Avatar;

        [DataMember(Name = "url")]
        public string Url;
    }
}
