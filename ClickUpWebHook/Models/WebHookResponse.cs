using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClickUpWebHook.Models
{
    public class WebHookResponse
    {
        [JsonPropertyName("task_id")]
        public string TaskId { get; set; }
    }
}
