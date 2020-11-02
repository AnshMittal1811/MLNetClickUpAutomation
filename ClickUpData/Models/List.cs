using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class List
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
