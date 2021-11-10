/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-30 17:12:22
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:42:54
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace OFW.BingBackground.Support
{
    public static class EPSGCodesSearchEngine
    {
        #region Public Static Methods
        public static List<Epsg> GetEpsgCodes(string keyword)
        {
            var codes = SearchEpsgCodes(keyword);
            return codes;
        }

        #endregion
        private static List<Epsg> SearchEpsgCodes(string keyword)
        {
            var codes = new List<Epsg>();

            var uri = new Uri($"{EPSG_WEBISITE}{keyword}");
            var response = httpClient.GetAsync(uri).Result;
            if(response != null && response.IsSuccessStatusCode)
            {
                var rawData = response.Content.ReadAsStringAsync().Result;
                var regExPattern = @"<li.*\/epsg\/.*>(?'EpsgCode'.*)<\/a>(?'Description':.*)<\/li>";
                var regEx = new Regex(regExPattern);

                foreach(Match match in regEx.Matches(rawData))
                {
                    var code = match.Groups["EpsgCode"].Value;
                    var description = match.Groups["Description"].Value;
                    codes.Add(new Epsg($"{code} {description}", code.Replace("EPSG:", "")));
                }
            }

            return codes;
        }

       

        #region Fields
        private static string EPSG_WEBISITE = "https://www.spatialreference.org/ref/?search=";
        private static HttpClient httpClient => new HttpClient();
        #endregion

    }
}
