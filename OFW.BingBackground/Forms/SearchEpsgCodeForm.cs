/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-30 20:00:45
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:42:01
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using OFW.BingBackground.FormModel;
using OFW.BingBackground.Support;
using System.Windows.Forms;

namespace OFW.BingBackground.Forms
{
    public partial class SearchEpsgCodeForm : Form
    {
        #region Constructor
        public SearchEpsgCodeForm()
        {
            InitializeComponent();
        }
        public SearchEpsgCodeForm(SearchEpsgCodeFormModel formModel):this()
        { 
            SearchEpsgCodeFormModel = formModel;
            textBoxKeyword.DataBindings.Add("Text", SearchEpsgCodeFormModel, "SearchKeyword", true, DataSourceUpdateMode.OnPropertyChanged);
            textBoxKeyword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) SearchEpsgCodes(); };
            buttonSearch.Click += (s, e) => SearchEpsgCodeFormModel.Search();
            buttonSearch.Click += (s, e) => SearchEpsgCodes();
            listBoxEpsgCodeResults.DoubleClick += (s, e) => SelectEpsgCode();
        }
        #endregion

        #region Private Methods
        private void SearchEpsgCodes()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SearchEpsgCodeFormModel.Search();

                listBoxEpsgCodeResults.DataSource = SearchEpsgCodeFormModel.SearchResults;
                listBoxEpsgCodeResults.DisplayMember = "Description";
                listBoxEpsgCodeResults.ValueMember = "Code";
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        private void SelectEpsgCode()
        {
            SearchEpsgCodeFormModel.SelectedEpsg = listBoxEpsgCodeResults.SelectedItem as Epsg;
            DialogResult = DialogResult.OK;
            Close();
        }
        #endregion

        #region Private Properties
        private SearchEpsgCodeFormModel SearchEpsgCodeFormModel { get; set; }
        #endregion

    }
}
