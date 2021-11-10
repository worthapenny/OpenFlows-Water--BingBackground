/**
 * @ Author: Akshaya Niraula
 * @ Modified by: Akshaya Niraula
 * @ Description: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using DotSpatial.Projections;
using Haestad.Drawing.Domain;
using Haestad.Maps.BingMaps.Adapting;
using OFW.BingBackground.Support;
using OpenFlows.Water.Domain;

namespace OFW.BingBackground.Domain
{
    public class BackgroundMapper
    {
        #region Constructor
        public BackgroundMapper()
        {
        }
        #endregion

        #region Public Methods
        public  void AddBingMapLayer(
            IWaterModel waterModel,
            IGraphicalProject project,
            ProjectionInfo fromProj)
        {
            // Get the coordinate of control points 
            var controlPoints = new ControlPoints(waterModel);            
            var controlPointVertices = controlPoints.GetControlPointsUsingJunctionNodes();


            // Get the Lat Lng of the control points
            var controlLatLngPointVerticies = controlPoints.GetLatLngPoints(controlPointVertices, fromProj);
            

            // Define Bing Map
            var bingMapLayerData = project.BingMapsBackgroundDefinition;

            // Add Bing map layer in to the project
            bingMapLayerData.Visible = true;
            bingMapLayerData.Active = true;


            // update the bing map properties
            bingMapLayerData.ControlPointManager.NumberOfControlPoints = controlPointVertices.Length;
            bingMapLayerData.BingMapsImagerySet = (int)EnumBingMapsImagerySet.Road;
            bingMapLayerData.Visible = true;


            // Add lat, Lng and X, Y to each row
            for (int i = 0; i < controlPointVertices.Length; i++)
            {
                VirtualMapControlPointElement row = project
                    .BingMapsBackgroundDefinition
                    .ControlPointManager
                    .Elements()[i] as VirtualMapControlPointElement;

                row.Latitude = controlLatLngPointVerticies[i][1];
                row.Longitude = controlLatLngPointVerticies[i][0];
                row.X = controlPointVertices[i][0];
                row.Y = controlPointVertices[i][1];

            }

            // initialize virtual map so that it will get loaded in the drawing
            var virtualMaps = bingMapLayerData.VirtualMaps;            
            if (!virtualMaps.IsInitialized)
                virtualMaps.Initialize();
            if (!virtualMaps.VirtualMapsProvider.IsInitialized)
                virtualMaps.VirtualMapsProvider.Initialize();


            // Make sure the properties are applied correctly
            bingMapLayerData.ApplyChanges();
            bingMapLayerData.Active = true;
            
        }
        #endregion

    }
}
