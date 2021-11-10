/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-30 20:00:45
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:42:10
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

namespace OFW.BingBackground.Forms
{
    partial class SearchEpsgCodeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxKeyword = new System.Windows.Forms.TextBox();
            this.listBoxEpsgCodeResults = new System.Windows.Forms.ListBox();
            this.labelEpsgCode = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.labelNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxKeyword
            // 
            this.textBoxKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxKeyword.Location = new System.Drawing.Point(195, 12);
            this.textBoxKeyword.Name = "textBoxKeyword";
            this.textBoxKeyword.Size = new System.Drawing.Size(196, 20);
            this.textBoxKeyword.TabIndex = 0;
            // 
            // listBoxEpsgCodeResults
            // 
            this.listBoxEpsgCodeResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxEpsgCodeResults.FormattingEnabled = true;
            this.listBoxEpsgCodeResults.Location = new System.Drawing.Point(12, 69);
            this.listBoxEpsgCodeResults.Name = "listBoxEpsgCodeResults";
            this.listBoxEpsgCodeResults.Size = new System.Drawing.Size(379, 199);
            this.listBoxEpsgCodeResults.TabIndex = 1;
            // 
            // labelEpsgCode
            // 
            this.labelEpsgCode.AutoSize = true;
            this.labelEpsgCode.Location = new System.Drawing.Point(12, 15);
            this.labelEpsgCode.Name = "labelEpsgCode";
            this.labelEpsgCode.Size = new System.Drawing.Size(170, 13);
            this.labelEpsgCode.TabIndex = 2;
            this.labelEpsgCode.Text = "Location (State/Province/Country)";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(291, 38);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(100, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "&Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Location = new System.Drawing.Point(12, 274);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(325, 13);
            this.labelNote.TabIndex = 2;
            this.labelNote.Text = "Note: Search results are obtained from https://spatialreference.org/";
            // 
            // SearchEpsgCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 296);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.labelEpsgCode);
            this.Controls.Add(this.listBoxEpsgCodeResults);
            this.Controls.Add(this.textBoxKeyword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(419, 335);
            this.Name = "SearchEpsgCodeForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search for Epsg Code";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxKeyword;
        private System.Windows.Forms.ListBox listBoxEpsgCodeResults;
        private System.Windows.Forms.Label labelEpsgCode;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label labelNote;
    }
}