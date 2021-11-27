/**
 * @ Author: Akshaya Niraula
 * @ Create Time: 2021-10-28 16:17:04
 * @ Modified by: Akshaya Niraula
 * @ Modified time: 2021-11-09 19:41:37
 * @ Copyright: Copyright (c) 2021 Akshaya Niraula. See LICENSE for details
 */

using Haestad.Drawing;
using Haestad.Drawing.Control.Adapting;
using Haestad.Drawing.Control.Application;
using Haestad.Drawing.Control.Components;
using Haestad.Drawing.Domain;
using Haestad.Drawing.Support;
using Haestad.Drawing.Windows.Forms.Components;
using Haestad.Framework.Application;
using Haestad.Framework.Windows.Forms.Components;
using Haestad.Framework.Windows.Forms.Forms;
using Haestad.Framework.Windows.Forms.Resources;
using Haestad.Framework.Windows.Forms.Support;
using Haestad.Support.Support;
using Haestad.WaterProduct.Application;
using OFW.BingBackground.FormModel;
using OpenFlows.Application;
using OpenFlows.Water;
using OpenFlows.Water.Application;
using OpenFlows.Water.Domain;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace OFW.BingBackground.Forms
{
    public partial class BingBackgroundLayerForm : HaestadParentForm, IParentFormSurrogate
    {
        #region Constructor
        public BingBackgroundLayerForm(HaestadParentFormModel parentFormModel)
            : base(parentFormModel)
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        public void SetParentWindowHandle(long handle)
        {
            //no-op
        }
        #endregion

        #region Private Methods
        private BingBackgroundLayerFormModel NewBingBackgroundLayerFormModel() =>
            new BingBackgroundLayerFormModel(WaterModel, ParentFormModel.CurrentProject as IGraphicalProject);
        private void DoLazyInitialization(bool lazyInitialize)
        {
            if (lazyInitialize && !IsLazyInitialized)
            {
                AnimationFormManager.Current.StartAnimation(TextManager.Current["InitializingUserInterface"], this);

                try
                {
                    WaterApplicationManager.GetInstance().ParentFormUIModel.Initialize();
                    InitializeDockingManagers();

                    WaterApplicationManager.GetInstance().ParentFormUIModel.DoLazyInitialization();
                }
                finally
                {
                    AnimationFormManager.Current.StopAnimation();
                }

                IsLazyInitialized = true;
            }
        }
        private void InitializeDockingManagers()
        {
            var bgLayerProxy = WaterApplicationManager.GetInstance().ParentFormUIModel.BackgroundLayerProxy;
            bgLayerProxy.Dock = DockStyle.Fill;
            splitContainerDrawing.Panel1.Controls.Add(bgLayerProxy);

        }
        private void OpenDrawingDocument(IProject aproject)
        {
            AddDocument(aproject);
            GLDrawingControl.Tag = ParentFormModel.CurrentProject;
            GLDrawingControl.ResumeLayout(true);
            (ParentFormModel.CurrentProject as IGraphicalProject).Drawing.AllowRefresh = true;
            GLDrawingControl.ResumeDrawing(true);
            Cursor = Cursors.WaitCursor;
        }
        private void AddDocument(IProject aproject)
        {
            GLDrawingControl = GetNewDocumentControl() as GLDrawingControl;
            splitContainerDrawing.Panel2.Controls.Add(GLDrawingControl);
        }
        private Control GetNewDocumentControl()
        {
            GLDrawingControl documentControl = new GLDrawingControl(this);
            documentControl.AllowDrop = false;

            documentControl.SuspendLayout();
            (WaterApplicationManager.GetInstance().ParentFormModel.CurrentProject as IGraphicalProject).Drawing.AllowRefresh = true;
            documentControl.SuspendDrawing();
            documentControl.BackColor = Color.White;
            documentControl.Dock = DockStyle.Fill;
            documentControl.Location = new Point(0, 0);
            documentControl.Name = "GLDrawingControl";
            documentControl.Dock = DockStyle.Fill;

            MDIDrawingFormModelBase drawingFormModel = (ParentFormModel as WaterProductParentFormModel).GraphicalEditorFormModelFactory.NewMDIDrawingFormModel(
                WaterApplicationManager.GetInstance().ParentFormModel.CurrentProject as IDomainProject) as MDIDrawingFormModelBase;
            documentControl.LoadUserControl(drawingFormModel.GLDrawingControlModel);
            documentControl.GraphicalProject = ParentFormModel.CurrentProject as IGraphicalProject;
            documentControl.DrawingToolManager = WaterApplicationManager.GetInstance().ParentFormUIModel.LayoutController.DrawingToolManager;

            return documentControl;
        }
        private void SelectTool(IDrawingTool atool)
        {
            if (LayoutController != null
                && LayoutController.DrawingToolManager != null
                && LayoutController.DrawingToolManager.CurrentTool != null)
            {
                if (WaterModel != null && GLDrawingControl != null)
                {
                    GLDrawingControl.ResetToAppropriateCursor();
                }
                Cursor = Cursors.Default;
                Cursor.Current = Cursors.Default;
            }
        }
        private void EnableToolStripButtons(bool enable)
        {
            toolStripButtonSave.Enabled = enable;
            toolStripButtonSaveAs.Enabled = enable;
            toolStripButtonClose.Enabled = enable;
            toolStripButtonAddBingMap.Enabled = enable;
            toolStripTextBoxEpsgCode.Enabled = enable;
            toolStripButtonSearch.Enabled = enable;
        }
        private void RefreshBackgroundLayerTreeNode()
        {
            BackgroundLayerTreeControl backgroundLayerControl =  (BackgroundLayerTreeControl)((BackgroundLayerUserControlProxy)splitContainerDrawing.Panel1.Controls[0]).WrappedControl;
            TreeViewControl treeViewControl = backgroundLayerControl.Controls["treeViewControl"] as TreeViewControl;
            BackgroundLayerTreeEditorModel backgroundTreeModel = (BackgroundLayerTreeEditorModel)treeViewControl.TreeModel;
            backgroundTreeModel.RefreshTreeNodes();
        }
        #endregion

        #region Protected Overrides
        protected override void DisposeEvents()
        {
            if (BingBackgroundLayerFormModel != null)
                BingBackgroundLayerFormModel.BingBackgroundLayerEvents -= BingBackgroundLayerFormModel_BingBackgroundLayerEvents;
        }
        protected override void InitializeVisually()
        {
            this.Icon = (Icon)GraphicResourceManager.Current[StandardGraphicResourceNames.BingMaps];
            toolStripButtonOpen.Image = ((Icon)GraphicResourceManager.Current[StandardGraphicResourceNames.Open]).ToBitmap();
            toolStripButtonSave.Image = ((Icon)GraphicResourceManager.Current[StandardGraphicResourceNames.Save]).ToBitmap();
            toolStripButtonSaveAs.Image = ((Icon)GraphicResourceManager.Current[StandardGraphicResourceNames.SaveAs]).ToBitmap();
            toolStripButtonClose.Image = ((Icon)GraphicResourceManager.Current[StandardGraphicResourceNames.Close]).ToBitmap();
            toolStripButtonAddBingMap.Image = ((Icon)GraphicResourceManager.Current[StandardGraphicResourceNames.BingMaps]).ToBitmap();
            toolStripButtonSearch.Image = ((Icon)GraphicResourceManager.Current[StandardGraphicResourceNames.Search]).ToBitmap();

            EnableToolStripButtons(false);
        }
        protected override void HaestadForm_Load(object sender, EventArgs e)
        {
            //OpenFlowsWater.StartSession(ParentFormModel.LicensedFeatureSet);

            base.HaestadForm_Load(sender, e);
        }
        public override OpenFileDialog NewOpenFileDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.CheckFileExists = true;
            open.CheckPathExists = true;
            open.DefaultExt = WaterAppParentFormModel.ApplicationDescription.LeadFileExtension;
            open.Filter = ((WaterProductApplicationDescription)WaterAppParentFormModel.ApplicationDescription).MultiExtensionOpenFileFilter;
            open.ShowReadOnly = false;
            return open;
        }
        #endregion

        #region Event Handlers
        private void BingBackgroundLayerFormModel_BingBackgroundLayerEvents(object sender, BingBackgroundLayerEventArgs events)
        {
            var message = string.Empty;
            switch (events.BingBackgroundLayerEventsEnum)
            {
                case BingBackgroundLayerEventsEnum.ValidationStarted:
                    AnimationFormManager.Current.StartAnimation($"Validating inputs...", this);
                    break;

                case BingBackgroundLayerEventsEnum.ValidationFailed:
                    AnimationFormManager.Current.StopAnimation();
                    message = $"Validation failed: \n{events.Message}";
                    MessageBox.Show(this, message, "Validation Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    toolStripTextBoxEpsgCode.SelectAll();
                    toolStripTextBoxEpsgCode.Focus();
                    break;

                case BingBackgroundLayerEventsEnum.ValidationSucceeded:
                    AnimationFormManager.Current.StopAnimation();
                    break;


                case BingBackgroundLayerEventsEnum.BackgroundApplyStarted:
                    AnimationFormManager.Current.StartAnimation($"Applying Bing background...", this);
                    break;

                case BingBackgroundLayerEventsEnum.BackgroundApplyFailed:
                    AnimationFormManager.Current.StopAnimation();
                    message = $"For some reason, Bing map background could not be applied. \nMessage: {events.Message}";
                    MessageBox.Show(this, message, "Bing background", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case BingBackgroundLayerEventsEnum.BackgroundApplySucceeded:
                    AnimationFormManager.Current.StopAnimation();
                    GLDrawingControl.RefreshBackground();
                    GLDrawingControl.RefreshBackgroundLayers();
                    GLDrawingControl.ZoomExtents();
                    // Need help here
                    // How can I refresh the Background Layer TreeView?

                    WaterApplicationManager.GetInstance().ParentFormUIModel.BackgroundLayerProxy.Invalidate();
                    MessageBox.Show(this, "Bing background added!", "Bing background", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

            }
        }
        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            CloseCurrentFile();

            var open = NewOpenFileDialog();
            if (open.ShowDialog(this) == DialogResult.OK)
            {
                DoLazyInitialization(true);
                OpenFile(open.FileName);
                WaterModel = OpenFlowsWater.GetModel(ParentFormModel.CurrentProject);

                if (ParentFormModel.CurrentProject != null)
                {
                    EnableToolStripButtons(true);
                    Text = $"Bing Background Layer - {WaterModel.ModelInfo.Filename}";

                    OpenDrawingDocument(ParentFormModel.CurrentProject);
                    GLDrawingControl.ZoomExtents();
                    SelectTool(LayoutController.DrawingToolManager.ToolNamed(PaletteNames.PaletteSelect));

                    BingBackgroundLayerFormModel = NewBingBackgroundLayerFormModel();
                    BingBackgroundLayerFormModel.BingBackgroundLayerEvents += BingBackgroundLayerFormModel_BingBackgroundLayerEvents;

                    Application.DoEvents();
                }
            }


        }
        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveCurrentFile();
        }
        private void toolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            if (PromptSaveAs(ParentFormModel.CurrentProject) == DialogResult.OK)
            {
                OpenFlowsWater.SetMaxProjects(5);
                WaterModel = OpenFlowsWater.GetModel(ParentFormModel.CurrentProject);
                BingBackgroundLayerFormModel = NewBingBackgroundLayerFormModel();
            }
        }
        private void toolStripButtonAddBingMap_Click(object sender, EventArgs e)
        {
            BingBackgroundLayerFormModel.Apply();

            RefreshBackgroundLayerTreeNode();
        }
        private void toolStripTextBoxEpsgCode_TextChanged(object sender, EventArgs e)
        {
            BingBackgroundLayerFormModel.FromEPSGCode = (sender as ToolStripTextBox).Text;
        }
        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            EnableToolStripButtons(false);
            CloseCurrentFile();

            GLDrawingControl.Dispose();
            splitContainerDrawing.Panel2.Controls.Clear();
        }
        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            var formModel = new SearchEpsgCodeFormModel();
            var form = new SearchEpsgCodeForm(formModel);
            if (form.ShowDialog(this) == DialogResult.OK)
                toolStripTextBoxEpsgCode.Text = formModel.SelectedEpsg.Code;
        }
        #endregion

        #region Private Properties
        private IWaterModel WaterModel { get; set; }
        private WaterProductParentFormModel WaterAppParentFormModel => ParentFormModel as WaterProductParentFormModel;
        private BingBackgroundLayerFormModel BingBackgroundLayerFormModel { get; set; }
        private GLDrawingControl GLDrawingControl { get; set; }
        private bool IsLazyInitialized { get; set; }
        private LayoutControllerBase LayoutController => WaterApplicationManager.GetInstance().ParentFormUIModel.LayoutController;

        #endregion

    }
}
