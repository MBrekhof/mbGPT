using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Grid;

namespace mbGPT.Blazor.Server.Controllers
{
    public partial class GroupPanelVisibilityController : ViewController<ListView>
    {
        public GroupPanelVisibilityController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetViewType = ViewType.ListView;
            SimpleAction PanelAction = new SimpleAction(this, "Grouppanel", "Edit")// PredefinedCategory.View)
            {
                Caption = "GroupPanel",
                ImageName = "Ungroup"
            };
            //This event fires when a user clicks the Simple Action control. Handle this event to execute custom code.
            PanelAction.Execute += PanelAction_Execute;
        }

        private void PanelAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.Editor is DxGridListEditor gridListEditor)
            {
                IDxGridAdapter dataGridAdapter = gridListEditor.GetGridAdapter();
                dataGridAdapter.GridModel.ShowGroupPanel = !dataGridAdapter.GridModel.ShowGroupPanel;
                View.Refresh();
            }
        }

        protected override void OnActivated()
        {
            {
                base.OnActivated();
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
