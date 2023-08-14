using DevExpress.Data.Utils;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DocGPT.Module.BusinessObjects;
using DocGPT.Module.Services;
using Hangfire;


namespace DocGPT.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCSendMail : ViewController
    {
        private readonly IServiceProvider serviceProvider;
        [Microsoft.Extensions.DependencyInjection.ActivatorUtilitiesConstructor]
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

        private  void SendMailAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var settingsService = serviceProvider.GetService<SettingsService>();
            var setting = settingsService.GetSettings();
            MailService mailService = serviceProvider.GetService<MailService>();
            mailService.Initialize(setting);
            MailData mailData = new MailData();
 
            var mailSettings = mailService.GetMailSettings();
            mailData.Subject = "Test email";
            mailData.Body = "This is a mail send to test the settings of your DocGPT mail.";
            mailData.From = mailSettings.SenderEmail;
            mailData.DisplayName = mailSettings.SenderDisplayName;
            mailData.ReplyTo = "noreply@brekhof.nl";
            mailData.ReplyToName = "justme";
            // todo: change this
            mailData.To = new List<string>
            {
                "martin@brekhof.nl"
            };
            // var mailResult = await mailService.SendAsync(mailData);
            //CancellationToken ctx = new CancellationToken();
            var mailResult = BackgroundJob.Enqueue(() => SendMailInBackground(mailData, setting));
            //var mailResult =  await mailService.SendAsync(mailData, ctx);
            Console.WriteLine(mailResult);
        }
        public void SendMailInBackground(MailData mailData, Settings setting)
        {
            try
            {
                MailService mailService = serviceProvider.GetService<MailService>();

                //// Initialize the mail service with the passed mail settings
                //mailService.Initialize(setting);

                CancellationToken ctx = new CancellationToken();
                var mailResult = mailService.SendBackGroundAsync(mailData,setting, ctx).GetAwaiter().GetResult();

                Console.WriteLine(mailResult);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                Console.WriteLine("An error occurred while sending the email: " + ex.Message);
            }
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
