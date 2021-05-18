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
            Vector<double> auxW = CreateVector.Random<double>(variables, new ContinuousUniform(0, 1));
            //Vector<double> f = CreateVector.Dense<double>(new[] { 4.0, 31, 29,33,4,10,3});

            this.LearningRate = learningRate;
            double aux = 0;
            foreach( double w in auxW)
            {
                aux += Math.Pow(w, 2);
            }
            this.W = auxW/ (double)Math.Sqrt(aux);
            //this.W = f / Math.Sqrt(aux);

        }
        public Vector<double> TrainOja()
        {
            for (int j = 0; j<this.epochs; j++)
            {
               foreach (var sample in this.values)
               {
                   var activ = predict(sample);
                   this.W += this.LearningRate * activ * (sample - (activ * this.W));
               }
            }
            return W;
        }

        private double predict(Vector<double> sample){
            double activ = 0;
            for (int i = 0; i < this.variables; i++)
            {
                activ += this.W[i] * sample[i];
            }
            return activ;
        } 
    }
}
