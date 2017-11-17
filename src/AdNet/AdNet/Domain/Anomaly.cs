using System;

namespace AdNet.Domain
{
    /// <summary>
    /// Describes a single anomalous data point.
    /// </summary>
    /// <typeparam name="V">Data point value</typeparam>
    /// <typeparam name="I">Data point index</typeparam>
    public class Anomaly<V, I>
        where V : IComparable where I : IComparable
    {
        public DataPoint<V, I> DataPoint
        {
            get;
            private set;
        }

        public double Confidence
        {
            get;
            private set;
        }

        public Anomaly(DataPoint<V, I> dataPoint, double confidence = 1.0)
        {
            DataPoint = dataPoint;
            Confidence = confidence;
        }
    }
}
