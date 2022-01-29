using System.Runtime.Serialization;

namespace VorpCharacter.Models
{
    [DataContract]
    public class ServerConfig
    {
        [DataMember(Name = "discord")]
        public DiscordConfig DiscordConfig;
    }
}
