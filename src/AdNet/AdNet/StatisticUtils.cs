using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdNet
{
    internal static class StatisticUtils
    {
        public static double GetPercentile(IEnumerable<double> data, double percentileRank)
        {
            List<double> sortedData = data.OrderBy(x => x).ToList();
            int percentileIndex = (int)(sortedData.Count * percentileRank / 100.0);
            return sortedData[percentileIndex];
        }

        public static double GetMedian(IEnumerable<double> data)
        {
            return GetPercentile(data, 50.0);
        }
    }
}
