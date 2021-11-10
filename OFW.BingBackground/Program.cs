/**
 * @ Author: Akshaya Niraula
 * @ Modified by: Akshaya Niraula
 * @ Description: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */


using OFW.BingBackground.Forms;
using OpenFlows.Application;
using OpenFlows.Water.Application;
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
            ApplicationManager.SetApplicationManager(new WaterAppManager());
            ApplicationManager.GetInstance().SetParentFormSurrogateDelegate(
                new ParentFormSurrogateDelegate((fm) =>
                {
                    return new BingBackgroundLayerForm(fm);
                }));

            ApplicationManager.GetInstance().Start();
            ApplicationManager.GetInstance().Stop();

            return 0;
        }
    }

    public class WaterAppManager : WaterApplicationManager
    {
        protected override bool IsHeadless => false;
    }
}

