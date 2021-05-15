using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TP4
{
    class Program
    {
        static List<Vector<double>> ParsePatterns(string path, int rows, int columns)
        {
            List<Vector<double>> trainingInput = new List<Vector<double>>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = "";
                    for (var i = 0; i < rows && !reader.EndOfStream; i++)
                        line += reader.ReadLine().PadRight(columns,' ').Substring(0, columns);
                    var values = line.ToCharArray().Select(val => val == '*' ? 1.0 : -1.0); ;
                    trainingInput.Add(Vector<double>.Build.Dense(values.ToArray()));
                }
            }
            return trainingInput;
        }
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
        static void Main(string config)
        {
            Configuration configuration = Configuration.FromYamlFile(config);
            if(configuration.Network == "hopfield")
            {
                var patterns = ParsePatterns(configuration.Patterns, configuration.PatternRows, configuration.PatternColumns);
                var network = new HopfieldNetwork(patterns);
                Random rand = new Random();
                var input = patterns[configuration.TestPattern].Map((v) => rand.NextDouble() < configuration.Noise ? v * -1.0 : v);
                var result = network.GetPattern(input);
                PrintPatterns(result, configuration.PatternColumns);
            }
        }
    }
}
