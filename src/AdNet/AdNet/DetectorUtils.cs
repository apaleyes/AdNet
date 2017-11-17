using System;

namespace AdNet
{
    internal static class DetectorUtils
    {
        /// <summary>
        /// Allows detectors to assert that data point values can be cast to a specific type.
        /// </summary>
        /// <typeparam name="SourceType">Type of data point value</typeparam>
        /// <typeparam name="TargetType">Type required by the detector</typeparam>
        public static void EnsureCast<SourceType, TargetType>()
        {
            SourceType sourceTypeValue = default(SourceType);
            try
            {
                Convert.ChangeType(sourceTypeValue, typeof(TargetType));
            }
            catch (InvalidCastException)
            {
                string message = String.Format(
                    "Data point value type {0} is not compatible with type {1} required by this detector",
                    typeof(SourceType).ToString(), typeof(TargetType).ToString());
                throw new ArgumentException(message);
            }
        }
    }
}
