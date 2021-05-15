using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TP4
{
    public class HopfieldNetwork
    {
        public Matrix<double> W { get; }

        public HopfieldNetwork(List<Vector<double>> patterns)
        {
            var N = patterns[0].Count;
            var K = CreateMatrix.DenseOfColumnVectors(patterns);
            this.W = (1.0 / N) * K.TransposeAndMultiply(K);
            this.W.SetDiagonal(CreateVector.Dense(N, 0.0));
        }

        public List<Vector<double>> GetPattern(Vector<double> pattern)
        {
            Vector<double> S = pattern, previous = null;
            List<Vector<double>> patterns = new List<Vector<double>>();
            while (!S.Equals(previous))
            {
                patterns.Add(S);
                previous = S;
                S = CreateVector.DenseOfEnumerable((W * S).Select((v,index) => v != 0 ? Math.Sign(v) : S[index]));
            }
            return patterns;
        }
    }
}
