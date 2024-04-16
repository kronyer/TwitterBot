using Newtonsoft.Json;

namespace Twitter_Bot.Models
{
    public class PostTweetRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; } = string.Empty;
    }
}
