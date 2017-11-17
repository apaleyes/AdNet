using System;
using System.Collections.Generic;
using System.Linq;

namespace AdNet.PointDetectors
{
    public class ThresholdDetector<V, I> : BasePointDetector<V, I>
        where V : struct, IComparable where I : IComparable
    {
        public V? LowerThreshold { get; protected set; }

        public V? UpperThreshold { get; protected set; }

        public ThresholdDetector(V? lowerThreshold = null, V? upperThreshold = null)
        {
            if (lowerThreshold == null && upperThreshold == null)
            {
                throw new ArgumentException("At least one of the thresholds must be set.");
            }

            LowerThreshold = lowerThreshold;
            UpperThreshold = upperThreshold;
        }

        protected override List<int> DetectAnomaliesInData(List<V> data)
        {
            return data.Select((d, i) => new { Index = i, Value = d })
                       .Where
                        (point =>
                            (LowerThreshold.HasValue && LowerThreshold.Value.CompareTo(point.Value) > 0)
                            ||
                            (UpperThreshold.HasValue && UpperThreshold.Value.CompareTo(point.Value) < 0)
                        )
                        .Select(point => point.Index)
                        .ToList();
        }
    }
}
