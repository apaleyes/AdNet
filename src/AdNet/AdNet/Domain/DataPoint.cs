using System;

namespace AdNet.Domain
{
    public class DataPoint<V, I>
        where V : IComparable
        where I : IComparable
    {
        public V Value { get; set; }
        public I Index { get; set; }
    }
}
