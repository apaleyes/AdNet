using System;
using System.Collections.Generic;
using System.Linq;

namespace AdNet.Domain
{
    public class Series<V, I>
        where V : IComparable
        where I : IComparable
    {
        private List<DataPoint<V, I>> dataPoints = new List<DataPoint<V, I>>();
        
        public object ID
        {
            get; set;
        }

        public Series(IList<DataPoint<V, I>> dataPoints)
        {
            if (dataPoints == null)
            {
                throw new ArgumentNullException("dataPoints");
            }

            var orderedDataPoints = dataPoints.OrderBy(dp => dp.Index);
            this.dataPoints.AddRange(orderedDataPoints);
        }

        public IList<DataPoint<V, I>> getDataPoints()
        {
            return dataPoints.AsReadOnly();
        }

        public int Count
        {
            get { return dataPoints.Count; }
        }
    }
}
