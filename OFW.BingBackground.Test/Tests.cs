/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-11-08 19:32:58
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:43:43
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */


using DotSpatial.Projections;
using Haestad.Drawing.Domain;
using NUnit.Framework;
using OFW.BingBackground.FormModel;
using OFW.BingBackground.Support;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/* NOTE:
 * The TEST project is not running ATM unless 10.4 is available
 */

namespace OFW.BingBackground.Test
{
    [TestFixture]
    public class Tests: OFWAppTestFixtureBase
    {
        #region Constructor
        public Tests()
        {
        }
        #endregion

        #region Setup/Teardown
        protected override void SetupImpl()
        {
            string filepath = Path.GetFullPath(BuildTestFilename($@"NCAR\NCAR.wtg"));
            OpenModel(filepath);
        }
        #endregion

        #region Tests
        [Test]
        public void ControlPointsTest()
        {
            var controlPoints = new ControlPoints(WaterModel);
            Assert.NotNull(controlPoints);

            var points = controlPoints.GetControlPointsUsingJunctionNodes();
            Assert.NotNull(points);
            Assert.AreEqual(3, points.Length);

            var lowerLeftPoint = points[0];
            var upperMidPoint = points[1];
            var lowerRightPoint = points[2];

            Assert.NotNull(lowerLeftPoint);
            Assert.NotNull(upperMidPoint);
            Assert.NotNull(lowerRightPoint);

            Assert.AreEqual(3063082.6926068268, lowerLeftPoint[0], 0.001);
            Assert.AreEqual(1234961.4915434157, lowerLeftPoint[1], 0.001);
            Assert.AreEqual(3063427.0521996939, upperMidPoint[0], 0.001);
            Assert.AreEqual(1235643.6180421796, upperMidPoint[1], 0.001);
            Assert.AreEqual(3063703.763052071, lowerRightPoint[0], 0.001);
            Assert.AreEqual(1235643.6180421796, lowerRightPoint[1], 0.001);

            var fromProj = ProjectionInfo.FromEpsgCode(2231); // Colorado North
            var latLngPoints = controlPoints.GetLatLngPoints(points, fromProj);
            Assert.NotNull(latLngPoints);

            Assert.AreEqual(-105.27490011754958, latLngPoints[0][0], 0.001);
            Assert.AreEqual(39.978133447798982, latLngPoints[0][1], 0.001);
            Assert.AreEqual(-105.27366512141187, latLngPoints[1][0], 0.001);
            Assert.AreEqual(39.980003604103359, latLngPoints[1][1], 0.001);
            Assert.AreEqual(-105.27267770293332, latLngPoints[2][0], 0.001);
            Assert.AreEqual(39.980001661000024, latLngPoints[2][1], 0.001);

        }

        [Test]
        public void BingBackgroundLayerFormModelFailureValidationTest()
        {
            IGraphicalProject graphicalProject = null;
            var formModel = new BingBackgroundLayerFormModel(WaterModel, graphicalProject);
            Assert.IsNotNull(formModel);

            var eventList = new List<BingBackgroundLayerEventArgs>();
            formModel.BingBackgroundLayerEvents += delegate (object sender, BingBackgroundLayerEventArgs events)
            {
                eventList.Add(events);
            };

            formModel.FromEPSGCode = "1";
            formModel.Apply();

            var flags = string.Empty;
            for (int i = 0; i < eventList.Count; i++)
            {
                BingBackgroundLayerEventArgs eventArg = eventList[i];
                Assert.IsNotNull(eventArg);

                switch (eventArg.BingBackgroundLayerEventsEnum)
                {
                    case BingBackgroundLayerEventsEnum.ValidationStarted:
                        flags += "vs";
                        break;
                    case BingBackgroundLayerEventsEnum.ValidationFailed:
                        flags += "vf";
                        Assert.IsTrue(eventArg.Message.Contains("not valid"));
                        break;

                    case BingBackgroundLayerEventsEnum.ValidationSucceeded:
                        flags += "vsd";
                        break;
                    case BingBackgroundLayerEventsEnum.BackgroundApplyStarted:
                        flags += "bas";
                        break;
                    case BingBackgroundLayerEventsEnum.BackgroundApplyFailed:
                        flags += "baf";
                        Assert.IsTrue(eventArg.Message.Contains("Failed"));
                        break;
                    case BingBackgroundLayerEventsEnum.BackgroundApplySucceeded:
                        flags += "basd";
                        break;

                    default:
                        break;
                }
            }

            Assert.AreEqual("vsvf", flags);
        }

        [Test]
        public void BingBackgroundLayerFormModelApplyTestTest()
        {
            IGraphicalProject graphicalProject = Project as IGraphicalProject;
            var formModel = new BingBackgroundLayerFormModel(WaterModel, graphicalProject);
            Assert.IsNotNull(formModel);

            var eventList = new List<BingBackgroundLayerEventArgs>();
            formModel.BingBackgroundLayerEvents += delegate (object sender, BingBackgroundLayerEventArgs events)
            {
                eventList.Add(events);
            };

            formModel.FromEPSGCode = "2231"; // Colorado North
            formModel.Apply();

            var flags = string.Empty;
            for (int i = 0; i < eventList.Count; i++)
            {
                BingBackgroundLayerEventArgs eventArg = eventList[i];
                Assert.IsNotNull(eventArg);

                switch (eventArg.BingBackgroundLayerEventsEnum)
                {
                    case BingBackgroundLayerEventsEnum.ValidationStarted:
                        flags += "vs";
                        break;
                    case BingBackgroundLayerEventsEnum.ValidationFailed:
                        flags += "vf";
                        Assert.IsTrue(eventArg.Message.Contains("not valid"));
                        break;
                    case BingBackgroundLayerEventsEnum.ValidationSucceeded:
                        flags += "vsd";
                        break;
                    case BingBackgroundLayerEventsEnum.BackgroundApplyStarted:
                        flags += "bas";
                        break;
                    case BingBackgroundLayerEventsEnum.BackgroundApplyFailed:
                        flags += "baf";
                        Assert.IsTrue(eventArg.Message.Contains("Failed"));
                        break;
                    case BingBackgroundLayerEventsEnum.BackgroundApplySucceeded:
                        flags += "basd";
                        break;

                    default:
                        break;
                }
            }

            Assert.AreEqual("vsvsdbasbasd", flags);
        }

        [Test]
        public void EpsgCodeSerachTest()
        {
            var formModel = new SearchEpsgCodeFormModel();
            Assert.IsNotNull(formModel);

            formModel.SearchKeyword = "Colorado";
            formModel.Search();
            var codes = formModel.SearchResults;
            Assert.IsTrue(codes.Count > 0);
            Assert.AreEqual(21, codes.Count);
            Assert.IsTrue(codes.Where(c => c.Code == "2231").Any());

        }

        #endregion

    }
}
