﻿using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using System.Linq;
using MathNet.Numerics.Statistics;

namespace TP4.Kohonen
{
    public class KohonenNetwork
    {
        public Vector<double>[,] W { get; }
        public int N { get; }
        public List<Vector<double>> values;
        private int inputLength;

        public KohonenNetwork(List<Vector<double>> values, int n)
        {
            this.values = Normalize(values);
            W = new Vector<double>[n, n];
            N = n;

            inputLength = values[0].Count;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    W[i, j] = CreateVector.Random<double>(inputLength, new ContinuousUniform(0, 1));
        }

        private List<Vector<double>> Normalize(List<Vector<double>> values)
        {
            var columns = CreateMatrix.DenseOfRowVectors(values).EnumerateColumns();
            var mean = CreateVector.DenseOfEnumerable(columns.Select(Statistics.Mean));
            var std = CreateVector.DenseOfEnumerable(columns.Select(Statistics.StandardDeviation));
            return values.Select(v => (v - mean) / std).ToList();
        }

        private double Distance(Vector<double> weight, Vector<double> input)
        {
            return (weight - input).L2Norm();
        }

        private double Distance((int x, int y) pointA, (int x, int y) pointB)
        {
            return Math.Sqrt((pointA.x - pointB.x) * (pointA.x - pointB.x) + (pointA.y - pointB.y) * (pointA.y - pointB.y));
        }

        private (int,int) FindMin(Vector<double> input)
        {
            int im = 0, jm = 0;
            double min = Distance(W[im, jm], input);
            for(int i = 1; i < N; i++)
                for(int j = 1; j < N; j++)
                {
                    double dist = Distance(W[i, j], input);
                    if(dist < min)
                    {
                        im = i; jm = j;
                        min = dist;
                    }
                }
            return (im, jm);
        }

        private void Update(Vector<double> input, int x, int y, double radius, double rate)
        {
            int imin = Math.Max(0, (int)Math.Floor(x - radius));
            int imax = Math.Min(N-1, (int)Math.Ceiling(x + radius));
            int jmin = Math.Max(0, (int)Math.Floor(y - radius));
            int jmax = Math.Min(N-1, (int)Math.Ceiling(y + radius));

            for (int i = imin; i <= imax ; i++)
                for (int j = jmin; j <= jmax; j++)
                {
                    if (Distance((i, j), (x, y)) < radius)
                        W[i, j] += rate * (input - W[i, j]);
                }
        }

        public void Train(int iterations)
        {
            for(int t = 1; t <= iterations; t++)
            {
                double rate = 1 / t;
                double radius = Math.Max(1, N - (2d / iterations) * t * N);

                var input = values[SystemRandomSource.Default.Next(values.Count)];
                int i, j;
                (i, j) = FindMin(input);

                Update(input, i, j, radius, rate);
            }
        }
    }
}