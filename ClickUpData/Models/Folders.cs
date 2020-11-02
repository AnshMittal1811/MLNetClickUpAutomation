using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ClickUpData.Models
{
    public class Folders
    {
        [JsonPropertyName("folders")]
        public List<Folder> FoldersList { get; set; }
    }
}
