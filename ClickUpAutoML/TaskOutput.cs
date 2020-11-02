using Microsoft.ML.Data;

namespace ClickUpAutoML
{
    public class TaskOutput
    {
        public string PredictedLabel { get; set; }
        public float[] Score { get; set; }
    }
}