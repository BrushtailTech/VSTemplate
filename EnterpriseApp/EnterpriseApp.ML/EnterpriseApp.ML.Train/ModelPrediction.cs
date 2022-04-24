using EnterpriseApp.ML.Train.DataStructure;
using Microsoft.ML;
using System;

namespace EnterpriseApp.ML.Train
{
    public static class ModelPrediction
    {
        private const string ModelPath = @"Models/SentimentModel.zip";

        public static void TestSinglePrediction(MLContext mlContext)
        {
            SentimentIssue sampleStatement = new SentimentIssue { Text = "This is a very rude movie" };

            ITransformer trainedModel = mlContext.Model.Load(ModelPath, out DataViewSchema modelInputSchema);

            // Create prediction engine related to the loaded trained model
            PredictionEngine<SentimentIssue, SentimentPrediction> predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentIssue, SentimentPrediction>(trainedModel);

            //Score
            SentimentPrediction resultPrediction = predictionEngine.Predict(sampleStatement);

            Console.WriteLine($"=============== Single Prediction  ===============");
            Console.WriteLine($"Text: {sampleStatement.Text} | Prediction: {(Convert.ToBoolean(resultPrediction.Prediction) ? "Toxic" : "Non Toxic")} sentiment | Probability of being toxic: {resultPrediction.Probability} ");
            Console.WriteLine($"==================================================");
        }
    }
}
