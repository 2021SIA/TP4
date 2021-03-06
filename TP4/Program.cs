using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using TP4.Hopfield;
using TP4.Oja;
using System.IO;
using TP4.Kohonen;
using System.Linq;

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
                        string[] values = line.Split(',');
                        Array.Copy(values, 1, values2, 0, values.Length - 1);
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
                    var networkOja = new OjaNetwork(configuration.LearningRate, configuration.Epochs, ParseCsv(configuration.Csv));
                    var W = networkOja.TrainOja();
                    Console.WriteLine(W);
                    break;
                case "kohonen":
                    var values = ParseCsv(configuration.Csv);
                    var kohonen = new KohonenNetwork(values, configuration.KohonenK, configuration.WeightEntries);
                    kohonen.Train(configuration.Epochs);
                    var i = 0;
                    var countries = new string[] {"Austria", "Belgium", "Bulgaria", "Croatia", "Czech", "Denmark", "Estonia", "Finland", "Germany", "Greece", "Hungary", "Iceland", "Ireland", "Italy", "Latvia", "Lithuania", "Luxembourg", "Netherlands", "Norway", "Poland", "Portugal", "Slovakia", "Slovenia", "Spain", "Sweden", "Switzerland", "Ukraine", "United Kingdom" };
                    var groups = new List<(int x, int y)>();
                    foreach (var city in kohonen.values)
                    {
                        (int x, int y) group = kohonen.Classify(city);
                        Console.WriteLine($"{countries[i++]}: {group.x},{group.y}");
                        groups.Add(group);
                    }
                    await File.WriteAllLinesAsync("classification.csv", groups.Select((v, index) => $"{v.x},{v.y}"));
                    var weights = from Vector<double> weight in kohonen.W select weight;
                    await File.WriteAllLinesAsync("weights.csv", weights.Select((v, index) => v.Aggregate("",(str,n) => str + n + ",")));
                    var distances = new List<double>();
                    for(i = 0; i < kohonen.N; i++)
                    {
                        for(var j = 0; j < kohonen.N; j++)
                        {
                            int imin = Math.Max(0, i - 1);
                            int imax = Math.Min(kohonen.N - 1, i + 1);
                            int jmin = Math.Max(0, j - 1);
                            int jmax = Math.Min(kohonen.N - 1, j + 1);

                            var neuronDist = new List<double>();
                            for (int x = imin; x <= imax; x++)
                                for (int y = jmin; y <= jmax; y++)
                                {
                                    if ((i != x || j != y) && kohonen.Distance((i, j), (x, y)) <= 1)
                                        neuronDist.Add(kohonen.Distance(kohonen.W[i, j], kohonen.W[x, y]));
                                }
                            distances.Add(neuronDist.Average());
                        }
                    }
                    await File.WriteAllLinesAsync("distances.csv", distances.Select(v => v.ToString()));

                    break;

            }
        }
    }
}
