/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-28 12:11:30
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:40:47
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using DotSpatial.Projections;
using OFW.BingBackground.FormModel;

namespace OFW.BingBackground.Domain
{
    internal class BingBackgroundMapLayerValidation
    {
        #region Constructor

        public BingBackgroundMapLayerValidation(BingBackgroundLayerFormModel bingBackgroundLayerFormModel)
        {
            BingBackgroundLayerFormModel = bingBackgroundLayerFormModel;
        }        
        #endregion

        #region Public Methods
        public bool IsValid()
        {
            int epsgCode = -1;
            var isValid = int.TryParse(BingBackgroundLayerFormModel.FromEPSGCode, out epsgCode);
            if (isValid)
            {
                try
                {
                    FromProjection = ProjectionInfo.FromEpsgCode(epsgCode);
                    isValid = true;
                }
                catch
                {
                    Message = $"Given EPSG code '{epsgCode}' is not valid.";
                    isValid = false;
                }
            }

            return isValid;
        }
        #endregion

        #region Public Properties
        public string Message { get; private set; }
        public ProjectionInfo FromProjection { get; private set; }
        #endregion

        #region Private Properties
        private BingBackgroundLayerFormModel BingBackgroundLayerFormModel { get; set ; }        
        #endregion

    }
}