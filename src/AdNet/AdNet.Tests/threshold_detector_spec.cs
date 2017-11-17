using AdNet.PointDetectors;
using AdNet.Domain;
using System;
using System.Collections.Generic;
using System.Linq;


using NSpec;
using FluentAssertions;

namespace AdNet.Tests
{
    class threshold_detector_spec : nspec
    {
        Series<double, int> testData;
        ThresholdDetector<double, int> detector;
        List<Anomaly<double, int>> anomalies;

        void before_each()
        {
            testData = new Series<double, int>(new List<DataPoint<double, int>>
            {
                new DataPoint<double, int> { Value = 1.0, Index = 1 },
                new DataPoint<double, int> { Value = 2.0, Index = 2 },
                new DataPoint<double, int> { Value = 10.0, Index = 3 },
                new DataPoint<double, int> { Value = 1.0, Index = 4 },
                new DataPoint<double, int> { Value = -3.0, Index = 5 },
                new DataPoint<double, int> { Value = 1.5, Index = 6 }
            });
        }

        void detects_no_anomalies()
        {
            act = () => anomalies = detector.DetectAnomalies(testData);

            context["when all points are within the thresholds"] = () =>
            {
                before = () =>
                {
                    detector = new ThresholdDetector<double, int>(-20, 20);
                };

                it["detects no anomalies"] = () =>
                {
                    anomalies.Should().BeEmpty();
                };
            };
        }

        void detects_anomalies_in_data()
        {
            act = () => anomalies = detector.DetectAnomalies(testData);

            context["when upper threshold is given"] = () =>
            {
                before = () =>
                {
                    detector = new ThresholdDetector<double, int>(upperThreshold: 5.0);
                };

                it["detects anomalies above the upper threshold"] = () =>
                {
                    anomalies.Should().HaveCount(1);
                    anomalies[0].DataPoint.Value.ShouldBeEquivalentTo(10.0);
                };
            };

            context["when lower threshold is given"] = () =>
            {
                before = () =>
                {
                    detector = new ThresholdDetector<double, int>(lowerThreshold: 0.0);
                };

                it["detects anomalies below the lower threshold"] = () =>
                {
                    anomalies.Should().HaveCount(1);
                    anomalies[0].DataPoint.Value.ShouldBeEquivalentTo(-3.0);
                };
            };

            context["when both thresholds are given"] = () =>
            {
                before = () =>
                {
                    detector = new ThresholdDetector<double, int>(0.0, 5.0);
                };

                it["detects anomalies outside the thresholds"] = () =>
                {
                    anomalies.Should().HaveCount(2);
                };
            };
        }

        void detects_anomalies_with_one_threshold_set()
        {
            act = () => anomalies = detector.DetectAnomalies(testData);

            context["when only upper threshold is set"] = () =>
            {
                before = () =>
                {
                    detector = new ThresholdDetector<double, int>(null, 5.0);
                };

                it["detects anomalies above the upper threshold"] = () =>
                {
                    anomalies.Should().Match(a => a.All(p => p.DataPoint.Value > 5.0));
                };
            };

            context["when only lower threshold is set"] = () =>
            {
                before = () =>
                {
                    detector = new ThresholdDetector<double, int>(0.0, null);
                };

                it["detects anomalies below the lower threshold"] = () =>
                {
                    anomalies.Should().Match(a => a.All(p => p.DataPoint.Value < 0.0));
                };
            };
        }

        void requires_at_least_one_threshold()
        {
            context["when no thresholds are given"] = () =>
            {
                it["throws ArgumentException"] =
                    expect<ArgumentException>(() => new ThresholdDetector<double, int>());
            };
        }
    }
}
