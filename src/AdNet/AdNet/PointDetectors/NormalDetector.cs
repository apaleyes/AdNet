using System;
using System.Collections.Generic;
using System.Linq;

namespace AdNet.PointDetectors
{
    public class NormalDetector<V, I> : BasePointDetector<V, I>
        where V : IComparable
        where I : IComparable
    {
        public double? LowerDeviationFactor { get; protected set; }

        public double? UpperDeviationFactor { get; protected set; }

        public NormalDetector(double? lowerDeviationFactor = 3.0, double? upperDeviationFactor = 3.0)
        {
            DetectorUtils.EnsureCast<V, double>();

            if (lowerDeviationFactor == null && upperDeviationFactor == null)
            {
                throw new ArgumentException("At least one of the deviation factors must be set.");
            }

            if (lowerDeviationFactor < 0 || upperDeviationFactor < 0)
            {
                string message = String.Format(
                    "Deviation factors must be positive, received: lower {}; upper {}",
                    lowerDeviationFactor, upperDeviationFactor);
                throw new ArgumentException(message);
            }

            LowerDeviationFactor = lowerDeviationFactor;
            UpperDeviationFactor = upperDeviationFactor;
        }

        protected override List<int> DetectAnomaliesInData(List<V> data)
        {
            var dataCastWithIndex = data.Select((d, i) => new { Index = i, Value = (double)(object)d }).ToList();

            var mean = dataCastWithIndex.Select(dp => dp.Value).Average();
            var variance = dataCastWithIndex.Select(dp => dp.Value * dp.Value).Average() - mean * mean;
            var stdDev = Math.Sqrt(variance);

            return dataCastWithIndex
                      .Where
                      (point =>
                          (LowerDeviationFactor.HasValue && point.Value < mean - stdDev * LowerDeviationFactor)
                          ||
                          (UpperDeviationFactor.HasValue && point.Value > mean + stdDev * UpperDeviationFactor)
                      )
                      .Select(d => d.Index)
                      .ToList();
        }
    }
}
