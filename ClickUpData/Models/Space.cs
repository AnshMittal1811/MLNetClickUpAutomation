using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Space
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
