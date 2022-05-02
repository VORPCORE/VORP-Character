using Newtonsoft.Json;

namespace VORP.Character.Server.Web.Discord.Entity
{
    public class EmbedThumbnail
    {
        [JsonProperty(PropertyName = "url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url;
    }
}
