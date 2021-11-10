/**
 * @ Author: Akshaya Niraula
 * @ Modified by: Akshaya Niraula
 * @ Description: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using Haestad.Domain;
using OpenFlows.Domain.ModelingElements.Support;

namespace OFW.BingBackground.Library
{
    public class MathLibrary
    {
        public static double[][] TriangulatedControlPoints(IFieldInfo xField, IFieldInfo yField)
        {
            var xValues = (xField.Field as IFieldStatistics).GetStatistics(
                new StatisticType[] {
                    StatisticType.Minimum,
                    StatisticType.Mean,
                    StatisticType.Maximum });

            var yValues = (yField.Field as IFieldStatistics).GetStatistics(
                new StatisticType[] {
                    StatisticType.Minimum,
                    StatisticType.Maximum });


            // create a control triangle points
            var points = new double[3][] {
                new double[2] {xValues[0], yValues[0] },
                new double[2] {xValues[1], yValues[1] },
                new double[2] {xValues[2], yValues[1] }
            };

            return points;
        }
    }
}
