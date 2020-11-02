using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClickUpAutoML
{
    public class TaskInput
    {
        [LoadColumn(0)]
        public string TaskName { get; set; }

        [LoadColumn(1)]
        public string Tags { get; set; }
    }
}
