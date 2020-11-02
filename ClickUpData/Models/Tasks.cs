using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Tasks
    {
        [JsonPropertyName("tasks")]
        public List<Task> TasksList { get; set; }
    }
}
