using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnterpriseApp.ML.Consume.DataStructure
{
    public class ConsoleHelper
    {
        public static void PeekDataViewInConsole(MLContext mlContext, IDataView dataView, IEstimator<ITransformer> pipeline, int numberOfRows = 4)
        {
            string msg = $"Peek data in DataView: Showing {numberOfRows.ToString()} rows with the columns";
            ConsoleWriteHeader(msg);

            //https://github.com/dotnet/machinelearning/blob/master/docs/code/MlNetCookBook.md#how-do-i-look-at-the-intermediate-data
            ITransformer transformer = pipeline.Fit(dataView);
            IDataView transformedData = transformer.Transform(dataView);

            // 'transformedData' is a 'promise' of data, lazy-loading. call Preview  
            //and iterate through the returned collection from preview.

            DataDebuggerPreview preViewTransformedData = transformedData.Preview(maxRows: numberOfRows);

            foreach (DataDebuggerPreview.RowInfo row in preViewTransformedData.RowView)
            {
                KeyValuePair<string, object>[] columnCollection = row.Values;
                string lineToPrint = columnCollection.Aggregate("Row--> ", (current, column) => current + $"| {column.Key}:{column.Value}");
                Console.WriteLine(lineToPrint + "\n");
            }
        }
        public static void ConsoleWriteHeader(params string[] lines)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ");
            foreach (string line in lines)
                Console.WriteLine(line);
            int maxLength = lines.Select(x => x.Length).Max();
            Console.WriteLine(new string('#', maxLength));
            Console.ForegroundColor = defaultColor;
        }
        public static void PrintBinaryClassificationMetrics(string name, CalibratedBinaryClassificationMetrics metrics)
        {
            Console.WriteLine($"************************************************************");
            Console.WriteLine($"*       Metrics for {name} binary classification model      ");
            Console.WriteLine($"*-----------------------------------------------------------");
            Console.WriteLine($"*       Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"*       Area Under Curve:      {metrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"*       Area under Precision recall Curve:  {metrics.AreaUnderPrecisionRecallCurve:P2}");
            Console.WriteLine($"*       F1Score:  {metrics.F1Score:P2}");
            Console.WriteLine($"*       LogLoss:  {metrics.LogLoss:#.##}");
            Console.WriteLine($"*       LogLossReduction:  {metrics.LogLossReduction:#.##}");
            Console.WriteLine($"*       PositivePrecision:  {metrics.PositivePrecision:#.##}");
            Console.WriteLine($"*       PositiveRecall:  {metrics.PositiveRecall:#.##}");
            Console.WriteLine($"*       NegativePrecision:  {metrics.NegativePrecision:#.##}");
            Console.WriteLine($"*       NegativeRecall:  {metrics.NegativeRecall:P2}");
            Console.WriteLine($"************************************************************");
        }
    }
}