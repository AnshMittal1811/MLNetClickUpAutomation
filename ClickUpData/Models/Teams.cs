using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Teams
    {
        [JsonPropertyName("teams")]
        public List<Team> TeamsList { get; set; }
    }
}
