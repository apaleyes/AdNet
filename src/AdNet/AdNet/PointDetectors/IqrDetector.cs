using System;
using System.Collections.Generic;
using System.Linq;

using AdNet.Domain;

namespace AdNet.PointDetectors
{
    /// <summary>
    /// Detects outliers using Inter Quartile Range method
    /// Details: https://en.wikipedia.org/wiki/Interquartile_range#Outliers
    /// </summary>
    /// <typeparam name="I"></typeparam>
    public class IqrDetector<V, I> : BasePointDetector<V, I>
        where V : IComparable
        where I : IComparable
    {
        public double LowerPercentileRank { get; protected set; }

        public double UpperPercentileRank { get; protected set; }

        public IqrDetector(double? lowerPercentileRank = 25.0, double? upperPercentileRank = 75.0)
        {
            DetectorUtils.EnsureCast<V, double>();

            if (lowerPercentileRank == null || upperPercentileRank == null)
            {
                throw new ArgumentException("Both percentile ranks must be set.");
            }

            if (lowerPercentileRank < 0 || lowerPercentileRank > 100 ||
                upperPercentileRank < 0 || upperPercentileRank > 100)
            {
                string message = String.Format(
                    "Percentile must be between 0 and 100, received: lower {}; upper {}",
                    lowerPercentileRank, upperPercentileRank);
                throw new ArgumentException(message);
            }

            LowerPercentileRank = lowerPercentileRank ?? 0.0;
            UpperPercentileRank = upperPercentileRank ?? 100.0;
        }

        protected override List<int> DetectAnomaliesInData(List<V> data)
        {
            var dataCastWithIndex = data.Select((d, i) => new { Index = i, Value = (double)(object)d }).ToList();
            
            double lowerPercentile = StatisticUtils.GetPercentile(
                dataCastWithIndex.Select(d => d.Value), LowerPercentileRank);
            double upperPercentile = StatisticUtils.GetPercentile(
                dataCastWithIndex.Select(d => d.Value), UpperPercentileRank);
            double median = StatisticUtils.GetMedian(dataCastWithIndex.Select(d => d.Value));

            double lowerThreshold = median - 1.5 * (median - lowerPercentile);
            double upperThreshold = median + 1.5 * (upperPercentile - median);

            return dataCastWithIndex
                      .Where
                      (point =>
                          lowerThreshold.CompareTo(point.Value) > 0
                          ||
                          upperThreshold.CompareTo(point.Value) < 0
                      )
                      .Select(d => d.Index)
                      .ToList();
        }
    }
}
