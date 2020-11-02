using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Task
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("tags")]
        public List<Tag> Tags { get; set; }
    }
}
