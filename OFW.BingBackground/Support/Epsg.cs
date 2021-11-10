/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-30 21:29:49
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:42:45
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */


namespace OFW.BingBackground.Support
{
    public class Epsg
    {
        #region Constructor
        public Epsg(string description, string code)
        {
            Description = description;
            Code = code;
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return Description;
        }
        #endregion

        #region Public Properties
        public string Description { get; }
        public string Code { get; }
        #endregion
    }
}
