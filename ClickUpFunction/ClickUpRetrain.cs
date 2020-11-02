using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.ML.AutoML;

namespace ClickUpFunction
{
    public static class ClickUpRetrain
    {
        [FunctionName("ClickUpRetrain")]
        public static void Run([BlobTrigger("clickup/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log, ExecutionContext context)
        {
            var blobData = string.Empty;
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var blobConnection = config.GetSection("AzureWebJobsStorage");

            var mlContext = new MLContext();

            using (var reader = new StreamReader(myBlob))
            {
                blobData = reader.ReadToEnd();
            }

            var parsedData = blobData
                .Split("\r\n")
                .Skip(1)
                .Select(line => line.Split(','))
                .TakeWhile(row => !string.IsNullOrWhiteSpace(row[0]))
                .Select(row => new TaskInput 
                {
                    TaskName = row[0],
                    Tags = row[1]
                });

            var data = mlContext.Data.LoadFromEnumerable(parsedData);

            var settings = new MulticlassExperimentSettings
            {
                MaxExperimentTimeInSeconds = 600,
                OptimizingMetric = MulticlassClassificationMetric.LogLoss
            };

            var experiment = mlContext.Auto().CreateMulticlassClassificationExperiment(settings);

            var result = experiment.Execute(data, new ColumnInformation { LabelColumnName = "Tags" });

            var bestModel = result.BestRun.Model;

            mlContext.Model.Save(bestModel, data.Schema, "./clickup-model.zip");

            var storage = CloudStorageAccount.Parse(blobConnection.Value);

            var storageClient = storage.CreateCloudBlobClient();

            var container = storageClient.GetContainerReference("models");

            var modelRef = container.GetBlockBlobReference("clickup-model.zip");
            modelRef.UploadFromFile("clickup-model.zip");
        }
    }
}
