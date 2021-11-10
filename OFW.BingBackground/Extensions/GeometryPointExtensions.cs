/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-27 19:03:06
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:41:05
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using DotSpatial.Projections;
using Haestad.Support.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFW.BingBackground.Extensions
{
    public static class GeometryPointExtensions
    {
        public static double[] ToArray(this GeometryPoint point) => new double[2] {point.X,point.Y};

        public static double[] LatLngArray(this GeometryPoint point, ProjectionInfo from, ProjectionInfo to)
        {
            var latLng = point.ToArray();
            Reproject.ReprojectPoints(latLng, null, from, to, 0, 1);

            return latLng;
        }
        
        public static GeometryPoint LatLngPoint(this GeometryPoint point, ProjectionInfo from, ProjectionInfo to)
        {
            var latLng = point.LatLngArray(from, to);
            return new GeometryPoint(latLng[0], latLng[1]);
        }
    }
}
