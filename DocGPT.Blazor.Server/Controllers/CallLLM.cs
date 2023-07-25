using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Services;
using DocGPT.Module.BusinessObjects;
using DocGPT.Module.Services;

namespace DocGPT.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CallLLM : ViewController
    {
        private readonly IServiceProvider serviceProvider;
        BlazorApplication BlazorApplication => (BlazorApplication)Application;
        ILoadingIndicatorProvider LoadingIndicatorProvider => (ILoadingIndicatorProvider)BlazorApplication.ServiceProvider.GetService(typeof(ILoadingIndicatorProvider));

        public CallLLM()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(Chat);
            TargetViewType = ViewType.DetailView;

            SimpleAction AskAction = new SimpleAction(this, "Ask a question", "Edit")// PredefinedCategory.View)
            {
                //Specify the Action's button caption.
                Caption = "Ask",
                //Specify the confirmation message that pops up when a user executes an Action.
                //ConfirmationMessage = "Are you sure you can handle the truth?",
                //Specify the icon of the Action's button in the interface.
                ImageName = "Action_Clear"
            };
            //This event fires when a user clicks the Simple Action control. Handle this event to execute custom code.
            AskAction.Execute += AskAction_Execute;
        }

        [ActivatorUtilitiesConstructor]
        public CallLLM(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }

        private async void AskAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Application.ShowViewStrategy.ShowMessage(string.Format("Looking for the answer!"), displayInterval: 5000, position: InformationPosition.Top);
           
            LoadingIndicatorProvider.Hold(nameof(CallLLM));
            
            // should be using: system, user example, assistant example, assistant embeddings, user question
            var serviceOne = serviceProvider.GetRequiredService<OpenAILLMService>();
            await serviceOne.GetAnswer(e);
            ObjectSpace.CommitChanges();
            LoadingIndicatorProvider.Release(nameof(CallLLM));
            Application.ShowViewStrategy.ShowMessage(string.Format("Answered!"));
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

