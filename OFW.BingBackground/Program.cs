/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-28 16:20:37
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:43:06
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using OFW.BingBackground.Forms;
using OpenFlows.Application;
using OpenFlows.Water;
using OpenFlows.Water.Application;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;

namespace OFW.BingBackground
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static int Main()
        {
            ApplicationManagerBase.SetApplicationManager(new WaterApplicationManager());
            WaterApplicationManager.GetInstance().SetParentFormSurrogateDelegate(
                new ParentFormSurrogateDelegate((fm) =>
                {
                    return new BingBackgroundLayerForm(fm);
                }));

            OpenFlowsWater.StartSession(WaterProductLicenseType.WaterGEMS);


            // Set up the logging mechanism            
            string logTemplate = "{Timestamp:MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}";
            var logLevelSwitch = new LoggingLevelSwitch();
            logLevelSwitch.MinimumLevel = LogEventLevel.Debug;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logLevelSwitch)
                .WriteTo.Console(outputTemplate: logTemplate)
                .CreateLogger();


            WaterApplicationManager.GetInstance().Start();
            WaterApplicationManager.GetInstance().Stop();

            return 0;
        }
    }

    //public class WaterAppManager : WaterApplicationManager
    //{
    //    protected override bool IsHeadless => false;
    //}
}

