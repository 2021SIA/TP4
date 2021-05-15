using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP4
{
    public class HopfieldUtils
    {
        public static List<Vector<double>> ParsePatterns(string path, int rows, int columns)
        {
            List<Vector<double>> trainingInput = new List<Vector<double>>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = "";
                    for (var i = 0; i < rows && !reader.EndOfStream; i++)
                        line += reader.ReadLine().PadRight(columns, ' ').Substring(0, columns);
                    var values = line.ToCharArray().Select(val => val == '*' ? 1.0 : -1.0); ;
                    trainingInput.Add(Vector<double>.Build.Dense(values.ToArray()));
                }
            }
            return trainingInput;
        }
        public static List<Vector<double>> Test(HopfieldNetwork network, List<Vector<double>> patterns, int testPattern, double noise)
        {
            Random rand = new Random();
            var input = patterns[testPattern].Map((v) => rand.NextDouble() < noise ? v * -1.0 : v);
            return network.GetPattern(input);
        }
        public static Task SaveNoiseAccuracyMetrics(HopfieldNetwork network, List<Vector<double>> patterns, int repetitions, int testPattern)
        {
            var accuracy = new List<double>();
            Random rand = new Random();
            for (float n = 0; n < 1.1; n += 0.1f)
            {
                var success = 0;
                for (int i = 0; i < repetitions; i++)
                {
                    var result = Test(network,patterns,testPattern,n);
                    if (patterns.IndexOf(result[result.Count - 1]) == testPattern) success++;
                }
                accuracy.Add(success / (double)repetitions);
            }
            return File.WriteAllLinesAsync("accuracy.csv", accuracy.Select((v, index) => $"{index * 0.1},{v}"));
        }
        public static async Task SaveEnergyMetrics(HopfieldNetwork network, List<Vector<double>> patterns, int repetitions, int testPattern, double noise)
        {
            var energyValues = new List<List<double>>();
            Random rand = new Random();
            for (var i = 0; i < repetitions; i++)
            {
                var input = patterns[testPattern].Map((v) => rand.NextDouble() < noise ? v * -1.0 : v);
                energyValues.Add(network.GetEnergy(input));
            }
            for (var i = 0; i < energyValues.Count; i++)
                await File.WriteAllLinesAsync($"energy_{i}.csv", energyValues[i].Select((v, index) => $"{index},{v}"));
        }
    }
}
