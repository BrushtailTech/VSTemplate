using EnterpriseApp.ML.Consume.DataStructure;
using Microsoft.ML;
using Microsoft.ML.Calibrators;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.Text;
using System;
using System.Diagnostics;

namespace EnterpriseApp.ML.Consume
{
    public static class Program
    {
        private const string DataPath = @"Data/wikiDetoxAnnotated40kRows.tsv";
        private const string ModelPath = @"Data/SentimentModel.zip";

        private static void Main(string[] args)
        {
            //Create MLContext to be shared across the model creation workflow objects 
            //Set a random seed for repeatable/deterministic results across multiple trainings.
            MLContext mlContext = new MLContext(seed: 1);

            // Create, Train, Evaluate and Save a model
            BuildTrainEvaluateAndSaveModel(mlContext);

            ConsoleHelper.ConsoleWriteHeader("=============== End of creating model process, hit any key to finish ===============");
            Console.ReadLine();
        }

        private static ITransformer BuildTrainEvaluateAndSaveModel(MLContext mlContext)
        {
            // STEP 1: Common data loading configuration
            IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentIssue>(DataPath, hasHeader: true);

            DataOperationsCatalog.TrainTestData trainTestSplit =
                mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
            IDataView trainingData = trainTestSplit.TrainSet;
            IDataView testData = trainTestSplit.TestSet;

            // STEP 2: Common data process configuration with pipeline data transformations          
            TextFeaturizingEstimator dataProcessPipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features",
                inputColumnName: nameof(SentimentIssue.Text));

            // (OPTIONAL) Peek data (such as 2 records) in training DataView after applying the ProcessPipeline's transformations into "Features" 
            ConsoleHelper.PeekDataViewInConsole(mlContext, dataView, dataProcessPipeline, 2);
            //Peak the transformed features column
            //ConsoleHelper.PeekVectorColumnDataInConsole(mlContext, "Features", dataView, dataProcessPipeline, 1);

            // STEP 3: Set the training algorithm, then create and config the modelBuilder                            
            SdcaLogisticRegressionBinaryTrainer trainer =
                mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label",
                    featureColumnName: "Features");
            EstimatorChain<BinaryPredictionTransformer<CalibratedModelParametersBase<LinearBinaryModelParameters, PlattCalibrator>>> trainingPipeline = dataProcessPipeline.Append(trainer);

            //Measure training time
            Stopwatch watch = Stopwatch.StartNew();

            // STEP 4: Train the model fitting to the DataSet
            Console.WriteLine("=============== Training the model ===============");
            ITransformer trainedModel = trainingPipeline.Fit(trainingData);

            //Stop measuring time
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"***** Training time: {elapsedMs / 1000} seconds *****");

            // STEP 5: Evaluate the model and show accuracy stats
            Console.WriteLine("===== Evaluating Model's accuracy with Test data =====");
            IDataView predictions = trainedModel.Transform(testData);
            CalibratedBinaryClassificationMetrics metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: "Label",
                scoreColumnName: "Score");

            ConsoleHelper.PrintBinaryClassificationMetrics(trainer.ToString(), metrics);

            // STEP 6: Save/persist the trained model to a .ZIP file
            mlContext.Model.Save(trainedModel, trainingData.Schema, ModelPath);

            Console.WriteLine("The model is saved to {0}", ModelPath);

            return trainedModel;
        }
    }
}