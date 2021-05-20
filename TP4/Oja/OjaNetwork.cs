using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;
using MathNet.Numerics;
using System.IO;
using MathNet.Numerics.Statistics;

namespace TP4.Oja
{
    public class OjaNetwork
    {
        public int N { get; private set; }
        public int variables { get; private set; }
        public Vector<double> W { get; set; }
        public double LearningRate { get; set; }
        public int epochs { get; set; }
        public List<Vector<double>> values{get; set;}


        public OjaNetwork(double learningRate, int epochs, List<Vector<double>> values)
        {
            this.epochs = epochs;

            var columns = CreateMatrix.DenseOfRowVectors(values).EnumerateColumns();
            var mean = CreateVector.DenseOfEnumerable(columns.Select(Statistics.Mean));
            var std = CreateVector.DenseOfEnumerable(columns.Select(Statistics.StandardDeviation));

            this.values = values.Select(v => (v - mean)/std).ToList();
            this.variables = values[1].Count;
            this.W = CreateVector.Random<double>(variables, new ContinuousUniform(-1, 1));

            this.LearningRate = learningRate;

        }
        public Vector<double> TrainOja()
        {
            for (int j = 0; j<this.epochs; j++)
            {
               foreach (var sample in this.values)
               {
                   var activ = W * sample;
                   this.W += this.LearningRate * activ * (sample - (activ * this.W));
               }
            }
            var norm = Math.Sqrt((W * W.ToColumnMatrix()).Aggregate(0.0,(sum, val) => sum + val));
            return W / norm;
        }
    }
}
