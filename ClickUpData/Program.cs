using ClickUpData.Models;
using System;
using System.Net.Http;
using System.Linq;
using System.Text.Json;
using System.IO;
using CsvHelper;
using System.Globalization;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace ClickUpData
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var csvPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "tasks.csv");
            var apiKey = Environment.GetEnvironmentVariable("CLICKUP_API_KEY", EnvironmentVariableTarget.User);
            var storageConnection = Environment.GetEnvironmentVariable("STORAGE_CONNECTION", EnvironmentVariableTarget.User);

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://api.clickup.com/api/v2/")
            };

            client.DefaultRequestHeaders.Add("Authorization", apiKey);

            var teamsCall = await client.GetAsync("team");

            var teamsContent = await teamsCall.Content.ReadAsStringAsync();

            var teams = JsonSerializer.Deserialize<Teams>(teamsContent);

            var team = teams.TeamsList.First();

            var spacesCall = await client.GetAsync($"team/{team.Id}/space");

            var spacesContent = await spacesCall.Content.ReadAsStringAsync();

            var spaces = JsonSerializer.Deserialize<Spaces>(spacesContent);

            var space = spaces.SpacesList.Where(s => s.Name.Contains("Jon")).First();

            var foldersCall = await client.GetAsync($"space/{space.Id}/folder");

            var foldersContent = await foldersCall.Content.ReadAsStringAsync();

            var folders = JsonSerializer.Deserialize<Folders>(foldersContent);

            var folder = folders.FoldersList.First(f => f.Name.Contains("Personal"));

            var list = folder.Lists.First(l => l.Name == "YouTube");

            var tasksCall = await client.GetAsync($"list/{list.Id}/task?archived=false&include_closed=true");

            var taskContent = await tasksCall.Content.ReadAsStringAsync();

            var tasks = JsonSerializer.Deserialize<Tasks>(taskContent);

            var csvData = tasks.TasksList.Where(t => t.Tags.Count == 1).Select(t => new CsvData
            {
                TaskName = t.Name,
                Tags = String.Join(",", t.Tags.Select(t => t.Name).FirstOrDefault())
            });

            await using (var writer = new StreamWriter(csvPath))
            await using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync(csvData);
            }

            var storage = CloudStorageAccount.Parse(storageConnection);

            var storageClient = storage.CreateCloudBlobClient();

            var container = storageClient.GetContainerReference("clickup");

            var fileRef = container.GetBlockBlobReference("tasks.csv");
            await fileRef.UploadFromFileAsync(csvPath);
        }
    }
}
