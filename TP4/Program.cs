using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using TP4.Hopfield;

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

            }
        }
    }
}
