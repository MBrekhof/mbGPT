using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using mbGPT.Module.BusinessObjects;
using mbGPT.Module.Services;
using Microsoft.Extensions.DependencyInjection;

namespace mbGPT.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCSendMail : ViewController
    {
        private readonly IServiceProvider serviceProvider;
        [ActivatorUtilitiesConstructor]
        public VCSendMail(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }
        public VCSendMail()
        {
            InitializeComponent();
            TargetObjectType = typeof(Settings);
            TargetViewType = ViewType.DetailView;

            SimpleAction SendMailAction = new SimpleAction(this, "SendMailAction", "EmailCategory")
            {
                Caption = "Send mail",
                ConfirmationMessage = "A test email will be send.",
            };
            SendMailAction.Execute += SendMailAction_Execute;
        }

        private async void SendMailAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            MailService mailService = serviceProvider.GetService<MailService>();
            MailData mailData = new MailData();
            var mailSettings = await mailService.GetMailSettings();
            mailData.Subject = "Test email";
            mailData.Body = "This is a mail send to test the settings of your DocGPT mail.";
            mailData.From = mailSettings.SenderEmail;
            mailData.DisplayName = mailSettings.SenderDisplayName;
            // todo: change this
            mailData.To = new List<string>
            {
                "martin@brekhof.nl"
            };
            var mailResult = await mailService.SendAsync(mailData);

        }

        protected override void OnActivated()
        {
            base.OnActivated();
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
