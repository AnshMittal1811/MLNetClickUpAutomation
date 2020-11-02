using Microsoft.ML.Data;

namespace ClickUpWebHook
{
    public class TaskOutput
    {
        public string PredictedLabel { get; set; }
        public float[] Score { get; set; }
    }
}