/**
 * @ Author: Akshaya Niraula
 * @ Modified by: Akshaya Niraula
 * @ Description: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
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
