using Microsoft.ML;
using Microsoft.ML.AutoML;
using System;
using System.IO;

namespace ClickUpAutoML
{
    class Program
    {
        static void Main(string[] args)
        {
            var csvPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "tasks.csv");

            var context = new MLContext();

            var data = context.Data.LoadFromTextFile<TaskInput>(csvPath, hasHeader: true, separatorChar: ',');

            var settings = new MulticlassExperimentSettings
            {
                MaxExperimentTimeInSeconds = 600,
                OptimizingMetric = MulticlassClassificationMetric.LogLoss
            };

            var experiment = context.Auto().CreateMulticlassClassificationExperiment(settings);

            var result = experiment.Execute(data, new ColumnInformation { LabelColumnName = "Tags" });

            var bestModel = result.BestRun.Model;

            var predictionEngine = context.Model.CreatePredictionEngine<TaskInput, TaskOutput>(bestModel);

            var prediction = predictionEngine.Predict(new TaskInput { TaskName = "Introduction to ML.NET" });

            Console.WriteLine($"Predicted label - {prediction.PredictedLabel}");

            context.Model.Save(bestModel, data.Schema, "./clickup-model.zip");
        }
    }
}
