using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Hangfire;

namespace mbGPT.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class BackGroundController : ViewController
    {
        private readonly IServiceProvider serviceProvider;
        [ActivatorUtilitiesConstructor]
        public BackGroundController(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }

        public BackGroundController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            //_backgroundJobs = backgroundJobs;
            TargetViewType = ViewType.ListView;
            SimpleAction BackgroundAction = new SimpleAction(this, "BackGroundpanel", "Export")// PredefinedCategory.View)
            {
                Caption = "Background",
                ImageName = "Alert_Info"
            };
            BackgroundAction.Execute += BackgroundAction_Execute;
        }

        private void BackgroundAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // _backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            var backgroundService = serviceProvider.GetService<IBackgroundJobClient>();
            if (backgroundService != null)
            {
                backgroundService.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
            }
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
