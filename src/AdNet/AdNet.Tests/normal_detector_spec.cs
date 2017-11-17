using System;
using System.Collections.Generic;
using System.Linq;

using NSpec;
using FluentAssertions;
using AdNet.Domain;
using AdNet.PointDetectors;

namespace AdNet.Tests
{
    class normal_detector_spec : nspec
    {
        List<DataPoint<double, int>> testData;
        Series<double, int> testSeries;
        NormalDetector<double, int> detector;
        List<Anomaly<double, int>> anomalies;

        void before_each()
        {
            testData = new List<DataPoint<double, int>>
            {
                new DataPoint<double, int> { Value = 1.0, Index = 1 },
                new DataPoint<double, int> { Value = 1.0, Index = 2 },
                new DataPoint<double, int> { Value = 1.0, Index = 3 },
                new DataPoint<double, int> { Value = 1.0, Index = 4 },
                new DataPoint<double, int> { Value = 1.0, Index = 5 },
                new DataPoint<double, int> { Value = 1.0, Index = 6 },
                new DataPoint<double, int> { Value = 1.0, Index = 7 },
                new DataPoint<double, int> { Value = 1.0, Index = 8 },
                new DataPoint<double, int> { Value = 1.0, Index = 9 },
                new DataPoint<double, int> { Value = 1.0, Index = 10 }
            };
        }

        void detects_anomalies_in_data()
        {
            act = () =>
            {
                detector = new NormalDetector<double, int>(2.0, 2.0);
                anomalies = detector.DetectAnomalies(testSeries);
            };

            context["when given data with anomaly in the beginning"] = () =>
            {
                before = () =>
                {
                    var newData = testData.Select(p => p).ToList();
                    newData[0].Value = 10.0;
                    testSeries = new Series<double, int>(newData);
                };

                it["detects one anomaly"] = () =>
                {
                    anomalies.Should().HaveCount(1);
                    anomalies[0].DataPoint.Index.ShouldBeEquivalentTo(1);
                };
            };

            context["when given data with anomaly in the end"] = () =>
            {
                before = () =>
                {
                    var newData = testData.Select(p => p).ToList();
                    newData[testData.Count - 1].Value = 10.0;
                    testSeries = new Series<double, int>(newData);
                };

                it["detects one anomaly"] = () =>
                {
                    anomalies.Should().HaveCount(1);
                    anomalies[0].DataPoint.Index.ShouldBeEquivalentTo(testData.Count);
                };
            };

            context["when given data with anomaly in the middle"] = () =>
            {
                before = () =>
                {
                    var newData = testData.Select(p => p).ToList();
                    newData[4].Value = 10.0;
                    testSeries = new Series<double, int>(newData);
                };

                it["detects one anomaly"] = () =>
                {
                    anomalies.Should().HaveCount(1);
                    anomalies[0].DataPoint.Index.ShouldBeEquivalentTo(5);
                };
            };

            context["when given data with multiple anomalies"] = () =>
            {
                before = () =>
                {
                    var newData = testData.Select(p => p).ToList();
                    newData[0].Value = 10.0;
                    newData.AddRange(
                        newData.Select(p => new DataPoint<double, int>
                        {
                            Index = p.Index + testData.Count,
                            Value = p.Value
                        }).ToList());
                    testSeries = new Series<double, int>(newData);
                };

                it["detects all anomalies"] = () =>
                {
                    anomalies.Should().HaveCount(2);
                    anomalies[0].DataPoint.Index.ShouldBeEquivalentTo(1);
                    anomalies[1].DataPoint.Index.ShouldBeEquivalentTo(testData.Count + 1);
                };
            };

            context["when all points are within the thresholds"] = () =>
            {
                before = () =>
                {
                    testSeries = new Series<double, int>(testData);
                    detector = new NormalDetector<double, int>(2.0, 2.0);
                };

                it["detects no anomalies"] = () =>
                {
                    anomalies.Should().BeEmpty();
                };
            };
        }

        public void requires_at_least_one_deviation_factor()
        {
            context["when no deviation factors are given"] = () =>
            {
                it["throws ArgumentException"] =
                    expect<ArgumentException>(() => new NormalDetector<double, int>(null, null));
            };
        }

        public void requires_double_compatible_type()
        {
            context["when type non-castable to double is used"] = () =>
            {
                it["throws ArgumentException"] =
                    expect<ArgumentException>(() => new IqrDetector<string, int>(null, null));
            };
        }
    }
}
