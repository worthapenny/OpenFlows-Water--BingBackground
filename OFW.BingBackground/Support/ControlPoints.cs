/**
 * @ Author: Akshaya Niraula
 * @ Modified by: Akshaya Niraula
 * @ Description: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using DotSpatial.Projections;
using Haestad.Domain;
using OFW.BingBackground.Library;
using OpenFlows.Water.Domain;

namespace OFW.BingBackground.Support
{
    public class ControlPoints
    {
        #region Constructor
        public ControlPoints(IWaterModel waterModel)
        {
            WaterModel = waterModel;
        }
        #endregion

        #region Public Methods
        public double[][] GetControlPointsUsingJunctionNodes()
        {
            var xField = WaterModel.Network
                .Junctions.InputFields.FieldByName(StandardFieldName.HmiGeometryXCoordinate);

            var yField = WaterModel.Network
                .Junctions.InputFields.FieldByName(StandardFieldName.HmiGeometryYCoordinate);

            var points = MathLibrary.TriangulatedControlPoints(xField, yField);
            return points;
        }
        public double[][] GetLatLngPoints(double[][] points, ProjectionInfo fromProj)
        {
            var toProj = ProjectionInfo.FromEpsgCode(LatLngEpsgCode);
            var latLngPoints = new double[points.Length][];

            for (int i = 0; i < points.Length; i++)
            {
                latLngPoints[i] = new double[2] {points[i][0], points[i][1]};
                Reproject.ReprojectPoints(latLngPoints[i], null, fromProj, toProj, 0, 1);
            }

            return latLngPoints;
        }


        #endregion


        #region Private Properties
        private IWaterModel WaterModel { get; }
        private int LatLngEpsgCode => 4326; // Projection that supports Lat / Lng
        #endregion

    }
}
