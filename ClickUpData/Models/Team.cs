using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Team
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}
