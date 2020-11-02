using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Spaces
    {
        [JsonPropertyName("spaces")]
        public List<Space> SpacesList { get; set; }
    }
}
