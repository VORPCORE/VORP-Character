using Newtonsoft.Json;

namespace VorpCharacter.Web.Discord.Entity
{
    public class EmbedThumbnail
    {
        [JsonProperty(PropertyName = "url", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url;
    }
}
