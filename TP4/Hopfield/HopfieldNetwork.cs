using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TP4.Hopfield
{
    public class HopfieldNetwork
    {
        private List<Vector<double>> patterns;
        public Matrix<double> W { get; }

        public HopfieldNetwork(List<Vector<double>> patterns)
        {
            this.patterns = patterns;
            var N = patterns[0].Count;
            var K = CreateMatrix.DenseOfColumnVectors(patterns);
            this.W = (1.0 / N) * K.TransposeAndMultiply(K);
            this.W.SetDiagonal(CreateVector.Dense(N, 0.0));
        }

        public List<Vector<double>> GetPattern(Vector<double> pattern)
        {
            Vector<double> S = pattern, previous = null;
            List<Vector<double>> patterns = new List<Vector<double>>();
            while (!S.Equals(previous) && !patterns.Contains(S))
            {
                patterns.Add(S);
                previous = S;
                S = CreateVector.DenseOfEnumerable((W * S).Select((v,index) => v != 0 ? Math.Sign(v) : S[index]));
            }
            return patterns;
        }
        public List<double> GetEnergy(Vector<double> pattern)
        {
            Vector<double> S = pattern, previous = null;
            List<double> energy = new List<double>();
            while (!S.Equals(previous) && !patterns.Contains(S))
            {
                energy.Add(-0.5 * (W * S * S));
                previous = S;
                S = CreateVector.DenseOfEnumerable((W * S).Select((v, index) => v != 0 ? Math.Sign(v) : S[index]));
            }
            return energy;
        }
    }
}
