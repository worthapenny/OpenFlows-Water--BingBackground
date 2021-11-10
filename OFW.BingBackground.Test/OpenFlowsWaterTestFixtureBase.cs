/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-11-08 19:31:01
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:43:28
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */


using Haestad.LicensingFacade;
using NUnit.Framework;
using OpenFlows.Water;
using OpenFlows.Water.Domain;
using System.IO;
using static OpenFlows.Water.OpenFlowsWater;

namespace OFW.BingBackground.Test
{
    public abstract class OpenFlowsWaterTestFixtureBase
    {
        #region Constructor
        public OpenFlowsWaterTestFixtureBase()
        {

        }
        #endregion

        #region Setup/Tear-down
        [SetUp]
        public void Setup()
        {
            Assert.AreEqual(LicenseRunStatusEnum.OK, StartSession(WaterProductLicenseType.WaterGEMS));
            Assert.AreEqual(true, IsValid());

            SetupImpl();
        }
        protected virtual void SetupImpl()
        {
        }
        [TearDown]
        public void Teardown()
        {
            if (WaterModel != null)
                WaterModel.Dispose();
            WaterModel = null;

            TeardownImpl();

            EndSession();
        }
        protected virtual void TeardownImpl()
        {

        }
        #endregion

        #region Protected Methods
        protected void OpenModel(string filename)
        {
            WaterModel = Open(filename);
        }
        #endregion

        #region Protected Properties
        protected virtual string BuildTestFilename(string baseFilename)
        {
            return Path.Combine(@"D:\Development\Data\ModelData", baseFilename);
        }
    protected IWaterModel WaterModel { get; private set; }
        #endregion
    }
}
