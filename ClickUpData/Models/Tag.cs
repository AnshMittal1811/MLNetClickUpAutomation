using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Tag
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
