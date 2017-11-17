using System;
using System.Collections.Generic;
using System.Linq;

using AdNet.Domain;

namespace AdNet.PointDetectors
{
    /// <summary>
    /// Detects anomalous points in a single time series
    /// </summary>
    /// <typeparam name="V">Type of data point values</typeparam>
    /// <typeparam name="I">Type of data point indexes (e.g. integer, date)</typeparam>
    public abstract class BasePointDetector<V, I>
        where V : IComparable
        where I : IComparable
    {
        protected abstract List<int> DetectAnomaliesInData(List<V> data);

        public List<Anomaly<V, I>> DetectAnomalies(Series<V, I> dataSeries)
        {
            List<V> data = dataSeries.getDataPoints().Select(dp => dp.Value).ToList();
            List<int> anomalyIndexes = DetectAnomaliesInData(data);
            var anomalies = anomalyIndexes.Select(i => dataSeries.getDataPoints()[i])
                .Select(dp => new Anomaly<V, I>(dp))
                .ToList();
            return anomalies;
        }
    }
}
