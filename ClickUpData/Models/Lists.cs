using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Lists
    {
        [JsonPropertyName("lists")]
        public List<List> ListsList { get; set; }
    }
}
