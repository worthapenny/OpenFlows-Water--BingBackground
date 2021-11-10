/**
 * @ Author: Akshaya Niraula
 * @ Modified by: Akshaya Niraula
 * @ Description: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using OFW.BingBackground.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFW.BingBackground.FormModel
{
    public class SearchEpsgCodeFormModel
    {
        #region Constructor
        public SearchEpsgCodeFormModel()
        {            
        }
        #endregion

        #region Public Methods
        public void Search()
        {
            SearchResults = EPSGCodesSearchEngine.GetEpsgCodes(SearchKeyword);            
        }
        #endregion

        #region Public Properties
        public string SearchKeyword { get; set; }
        public List<Epsg> SearchResults { get; private set; } = new List<Epsg>();
        public Epsg SelectedEpsg { get; set; }
        #endregion
    }
}
