using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using TP4.Hopfield;
using TP4.Oja;
using System.IO;

namespace TP4
{
    class Program
    {
        static void PrintPatterns(List<Vector<double>> patterns, int columns)
        {
            for (int i = 0; i < patterns.Count; i++)
            {
                Vector<double> pattern = patterns[i];
                Console.WriteLine($"State {i}: ");
                for (int j = 0; j < pattern.Count; j++)
                {
                    if (j % columns == 0)
                        Console.WriteLine();
                    Console.Write(pattern[j] == 1 ? "*" : " ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        static List<Vector<double>> ParseCsv(string path)
        {
            List<Vector<double>> trainingInput = new List<Vector<double>>();
            bool first = true;
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] values2 = new string[7];
                    if (!first)
                    {
                        string[] values = line.Split(';');
                        Array.Copy(values, 1, values2, 0, values.Length - 1);
                        Console.WriteLine(String.Join(";", values2));
                        trainingInput.Add(Vector<double>.Build.Dense(Array.ConvertAll(values2, s => double.Parse(s))));
                    }
                    else
                        first = false;
                }
            }
            return trainingInput;
        }
        /// <summary>
        /// Unsupervised Learning Engine
        /// </summary>
        /// <param name="config">Path to the configuration file</param>
        static async void Main(string config)
        {
            Configuration configuration = Configuration.FromYamlFile(config);
            switch (configuration.Network)
            {
                case "hopfield":
                    //Parse network patterns
                    var patterns = HopfieldUtils.ParsePatterns(configuration.Patterns, configuration.PatternRows, configuration.PatternColumns);
                    //Initialize Hopfield network
                    var network = new HopfieldNetwork(patterns);
                    //Test network and get closest pattern
                    Console.WriteLine($"Testing Pattern: {configuration.TestPattern}; Noise: {configuration.Noise}\n");
                    var result = HopfieldUtils.Test(network, patterns, configuration.TestPattern, configuration.Noise);
                    //Print network intermediate states
                    PrintPatterns(result, configuration.PatternColumns);
                    //Check if network got to test pattern successfully
                    if (patterns.IndexOf(result[result.Count - 1]) == configuration.TestPattern)
                        Console.WriteLine("Found pattern!\n");
                    else
                        Console.WriteLine("Could not find pattern.\n");

                    if (configuration.Metrics == "all" || configuration.Metrics == "noise")
                    {
                        //Get network noise accuracy metrics
                        await HopfieldUtils.SaveNoiseAccuracyMetrics(network, patterns, configuration.Repetitions, configuration.TestPattern);
                        Console.WriteLine($"Noise metrics stored in accuracy.csv [Repetitions: {configuration.Repetitions}]");
                    }
                    if (configuration.Metrics == "all" || configuration.Metrics == "energy")
                    {
                        //Get network energy metrics
                        await HopfieldUtils.SaveEnergyMetrics(network, patterns, configuration.Repetitions, configuration.TestPattern, configuration.Noise);
                        Console.WriteLine("Energy metrics stored in energy_{i}.csv" + $" [Repetitions: {configuration.Repetitions}]");
                    }
                    break;
                case "oja":
                    var epochs = 1000;
                    Console.WriteLine(configuration.LearningRate);

                    var networkOja = new OjaNetwork(configuration.LearningRate, epochs, ParseCsv(configuration.Csv));
                    var W = networkOja.TrainOja();
                    Console.WriteLine(W);
                    break;

            }
        }
    }
}
