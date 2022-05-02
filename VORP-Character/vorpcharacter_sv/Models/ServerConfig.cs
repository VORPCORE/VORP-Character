using System.Runtime.Serialization;

namespace VORP.Character.Server.Models
{
    [DataContract]
    public class ServerConfig
    {
        [DataMember(Name = "discord")]
        public DiscordConfig DiscordConfig;
    }
}
