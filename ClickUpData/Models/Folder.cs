using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Folder
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lists")]
        public List<List> Lists { get; set; }
    }
}
