/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-11-08 19:31:01
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:43:28
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */


using Haestad.Framework.Application;
using NUnit.Framework;
using OpenFlows.Application;
using OpenFlows.Water;
using OpenFlows.Water.Application;
using OpenFlows.Water.Domain;
using System.IO;

namespace OFW.BingBackground.Test
{


    public abstract class OFWAppTestFixtureBase
    {
        #region Constructor
        public OFWAppTestFixtureBase()
        {

        }
        #endregion

        #region Setup/Tear-down
        [SetUp]
        public void Setup()
        {
            ApplicationManagerBase.SetApplicationManager(new WaterAppManager());

            // By passing in false, this will suppress the primary user interface.
            // Make sure you are logged into CONNECTION client.
            WaterApplicationManager.GetInstance().Start(false);
            OpenFlowsWater.SetMaxProjects(5);

            SetupImpl();
        }
        protected virtual void SetupImpl()
        {
        }
        [TearDown]
        public void Teardown()
        {
            TeardownImpl();

            WaterApplicationManager.GetInstance().Stop();
        }
        protected virtual void TeardownImpl()
        {
        }
        #endregion

        #region Protected Methods
        protected void OpenModel(string filename)
        {
            ProjectProperties pp = ProjectProperties.Default;
            pp.NominalProjectPath = filename;

            WaterApplicationManager.GetInstance().ParentFormModel.OpenProject(pp);
        }
        protected virtual string BuildTestFilename(string baseFilename)
        {
            return Path.Combine(@"D:\Development\Data\ModelData", baseFilename);
        }
        #endregion

        #region Protected Properties
        protected IWaterModel WaterModel => WaterApplicationManager.GetInstance().CurrentWaterModel;
        protected IProject Project => WaterApplicationManager.GetInstance().ParentFormModel.CurrentProject;
        #endregion
    }
}
