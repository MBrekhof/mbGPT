using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Utils;
using mbGPT.Module.BusinessObjects;
using mbGPT.Module.Services;
using Microsoft.JSInterop;

namespace mbGPT.Blazor.Server.Controllers
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
            //Application.ShowViewStrategy.ShowMessage(string.Format("Looking for the answer!"), displayInterval: 5000, position: InformationPosition.Top);


            IJSRuntime jsRuntime = (IJSRuntime)((BlazorApplication)Application).ServiceProvider.GetService(typeof(IJSRuntime));
            await jsRuntime.InvokeVoidAsync("setLoadingText", "Searching..");
            LoadingIndicatorProvider.Hold("Searching");


            var serviceOne = serviceProvider.GetRequiredService<OpenAILLMService>();
            var target = (Chat)e.CurrentObject;
            try
            {
                var result = await serviceOne.GetAnswer(target, ObjectSpace);
                if (result)
                {
                    Application.ShowViewStrategy.ShowMessage(string.Format("Answered using local knowledge!"));
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage(string.Format("Answered!"));
                }
                ObjectSpace.CommitChanges();
            }
            catch (Exception)
            {
                Application.ShowViewStrategy.ShowMessage(string.Format("There was an error please try again later!"),type:InformationType.Error);
                //throw;
            }
            finally
            {
                LoadingIndicatorProvider.Release("Searching");
                await jsRuntime.InvokeVoidAsync("setLoadingText", CaptionHelper.GetLocalizedText("VisualComponents/LoadingIndicator", "Loading"));
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

