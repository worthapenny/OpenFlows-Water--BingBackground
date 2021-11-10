/**
 * @ Author: Akshaya Niraula
 * @ Modified by: Akshaya Niraula
 * @ Description: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using Haestad.Drawing.Domain;
using OFW.BingBackground.Domain;
using OpenFlows.Water.Domain;
using System;

namespace OFW.BingBackground.FormModel
{
    public class BingBackgroundLayerFormModel
    {
        #region Events / Delegate

        public event BingBackgroundLayerFormEventHandler BingBackgroundLayerEvents;
        public delegate void BingBackgroundLayerFormEventHandler(object sender, BingBackgroundLayerEventArgs events);
        #endregion

        #region Constructor
        public BingBackgroundLayerFormModel(IWaterModel waterModel, IGraphicalProject graphicalProject)
        {                
            WaterModel = waterModel;
            GraphicalProject = graphicalProject;
            Validation = new BingBackgroundMapLayerValidation(this);
        }
        #endregion

        #region Public Methods
        public void Apply()
        {         
            if(!Validate())     
                return;

            BingBackgroundLayerEvents?.Invoke(this, new BingBackgroundLayerEventArgs(BingBackgroundLayerEventsEnum.BackgroundApplyStarted));

            try
            {
                var bgMapper = new BackgroundMapper();
                bgMapper.AddBingMapLayer(WaterModel, GraphicalProject, Validation.FromProjection);
            }
            catch (Exception ex)
            {
                BingBackgroundLayerEvents?.Invoke(this, 
                    new BingBackgroundLayerEventArgs(BingBackgroundLayerEventsEnum.BackgroundApplyFailed) { 
                        Message = $"Failed to apply the background layer. \n{ex.Message}"});
            }

            BingBackgroundLayerEvents?.Invoke(this, new BingBackgroundLayerEventArgs(BingBackgroundLayerEventsEnum.BackgroundApplySucceeded));
        }
        public bool Validate()
        {
            BingBackgroundLayerEvents?.Invoke(this, new BingBackgroundLayerEventArgs(BingBackgroundLayerEventsEnum.ValidationStarted));

            if (!Validation.IsValid())
            {
                var eventArgs = new BingBackgroundLayerEventArgs(BingBackgroundLayerEventsEnum.ValidationFailed);
                eventArgs.Message = Validation.Message;
                BingBackgroundLayerEvents?.Invoke(this, eventArgs);
                return false;
            }

            BingBackgroundLayerEvents?.Invoke(this, new BingBackgroundLayerEventArgs(BingBackgroundLayerEventsEnum.ValidationSucceeded));
            return true;
        }
        #endregion

        #region Public Properties
        public string FromEPSGCode { get; set; }
        #endregion

        #region Private Properties
        private IWaterModel WaterModel { get; set; }
        private IGraphicalProject GraphicalProject { get; set; }
        private BingBackgroundMapLayerValidation Validation { get; set; }
        #endregion
    }

    #region Helper EventArgs

    public class BingBackgroundLayerEventArgs: EventArgs
    {
        #region Constructor
        public BingBackgroundLayerEventArgs(BingBackgroundLayerEventsEnum eventsEnum)
        {
            BingBackgroundLayerEventsEnum = eventsEnum;
        }
        #endregion

        #region Public Properties
        public BingBackgroundLayerEventsEnum BingBackgroundLayerEventsEnum { get; private set; }
        public string Message { get; set; }
        #endregion
    }
    #endregion

    #region Enum

    public enum BingBackgroundLayerEventsEnum
    {
        ValidationStarted,
        ValidationFailed,
        ValidationSucceeded,
        BackgroundApplyStarted,
        BackgroundApplyFailed,
        BackgroundApplySucceeded
    }
    #endregion
}
