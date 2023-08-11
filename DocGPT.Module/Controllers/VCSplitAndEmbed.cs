

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Pdf;
using DevExpress.Persistent.Base;
using DevExpress.XtraRichEdit;
using DocGPT.Module.BusinessObjects;
using DocGPT.Module.Services;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.Embeddings;
using Pgvector;

namespace DocGPT.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class VCSplitAndEmbed : ViewController
    {

        public VCSplitAndEmbed()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            TargetObjectType = typeof(FileSystemStoreObject);
            TargetViewType = ViewType.ListView; // of eerst saven!

            PopupWindowShowAction SplitAndEmbedAction = new PopupWindowShowAction(this, "SplitAndEmbedAction", PredefinedCategory.View)
            {
                Caption = "Spit and Embed document" 
            };

            SplitAndEmbedAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            SplitAndEmbedAction.CustomizePopupWindowParams += SplitAndEmbedAction_CustomizePopupWindowParams;
            SplitAndEmbedAction.Execute += SplitAndEmbedAction_Execute;
        }


        private void SplitAndEmbedAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace newObjectSpace = Application.CreateObjectSpace(typeof(SplitAndEmbed));
            SplitAndEmbed target = newObjectSpace.CreateObject<SplitAndEmbed>();
            var currentFile = ((FileSystemStoreObject)View.CurrentObject);
            target.FileName = currentFile.File.FileName;
            target.FileSize = currentFile.File.Size;
            target.RealFileName = currentFile.File.RealFileName;
            var view = Application.CreateDetailView(newObjectSpace,target);
            view.ViewEditMode = ViewEditMode.View;
            e.View = view;
            // Ok/Cancel button
            var dialogController = Application.CreateController<DialogController>();
            dialogController.Accepting += DialogController_Accepting;
            dialogController.Cancelling += DialogController_Cancelling;
            e.DialogController = dialogController;

        }

        private void DialogController_Cancelling(object sender, EventArgs e)
        {
            var selected = (FileSystemStoreObject)this.Application.MainWindow.View.CurrentObject;
            var x = 1;
        }

        private void DialogController_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
            var selected = (FileSystemStoreObject)this.Application.MainWindow.View.CurrentObject;
            var x = 2;
            //throw new NotImplementedException();
        }

        private void SplitAndEmbedAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            SplitAndEmbed currentFile = (SplitAndEmbed)e.PopupWindowViewCurrentObject;
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



    public class ActionInPopupController : ViewController
    {
        private readonly IServiceProvider serviceProvider;
        [ActivatorUtilitiesConstructor]
        public ActionInPopupController(IServiceProvider serviceProvider) : this()
        {
            this.serviceProvider = serviceProvider;
        }
        public ActionInPopupController()
        {
            TargetObjectType = typeof(SplitAndEmbed);
            SimpleAction actionInPopup = new SimpleAction(this,
                "Split",
                DevExpress.Persistent.Base.PredefinedCategory.PopupActions
            );
            //Refer to the https://docs.devexpress.com/eXpressAppFramework/112815 help article to see how to reorder Actions within the PopupActions container.
            actionInPopup.Execute += actionInPopup_Execute;
        }
        async void actionInPopup_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var target = (SplitAndEmbed)e.CurrentObject;
            var content = "";
            var doctype = Path.GetExtension(target.RealFileName).ToUpper();
            Application.ShowViewStrategy.ShowMessage(string.Format("Splitting {0}!", target.FileName));

            switch (doctype)
            {
                case ".PDF":
                    using (PdfDocumentProcessor source = new PdfDocumentProcessor())
                    {
                        source.LoadDocument(target.RealFileName);
                        content = String.Empty;
                        for(int i=1;i<=source.Document.Pages.Count;i++)
                        {
                            content += source.GetPageText(i);
                        }
                    }
                    break;
                case ".DOC":
                case".DOCX":
                    using (var Wordprocessor = new RichEditDocumentServer())
                    {
                        Wordprocessor.LoadDocument(target.RealFileName);
                        content = Wordprocessor.Text;
                    }
                        break;
                default:
                    return; // needs a message?
                    //break;
            }
            var serviceOne = serviceProvider.GetRequiredService<VectorService>();
            var settingsService = serviceProvider.GetRequiredService<SettingsService>();
            var settings = await settingsService.GetSettingsAsync();
            target.DocChunks = serviceOne.SplitArticleIntoChunks(target.FileName, content, target.ChunkSize); // ProcessString(content, target.ChunkSize, target.OverlapSize);
            if (target.DocChunks != null)
            {
                Application.ShowViewStrategy.ShowMessage(string.Format("Creating Article and ArticleDetail for {0}!", target.FileName));
                IObjectSpace ArticleObjectSpace = Application.CreateObjectSpace(typeof(Article));
                var newArticle = ArticleObjectSpace.CreateObject<Article>();
                newArticle.ArticleName = target.FileName;
                newArticle.Description = "";

                ArticleObjectSpace.CommitChanges();
                var teller =0;
                //// Create an instance of the OpenAI client
                var api = new OpenAIClient(new OpenAIAuthentication(settings.OpenAIKey));

                //// Get the model details
                var model = await api.ModelsEndpoint.GetModelDetailsAsync(settings.EmbeddingModel.Name);

                foreach (string docChunk in target.DocChunks)
                {
                    teller++;
                    var newArticleDetail = ArticleObjectSpace.CreateObject <ArticleDetail>();
                    newArticleDetail.ArticleContent = "Source: "+target.FileName+ " "+docChunk;
                    newArticleDetail.ArticleSequence = teller;
                    //newArticleDetail.ArticleId = newArticle.ArticleId; // ?? why ??
                    newArticle.ArticleDetail.Add(newArticleDetail);
                }
                ArticleObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage(string.Format("Finished splitting"));
                //// create the embeddings
                foreach (var articleDet in newArticle.ArticleDetail)
                {
                    var embeddings = await api.EmbeddingsEndpoint.CreateEmbeddingAsync(articleDet.ArticleContent, model);
                    var x = new Vector("[" + String.Join(",", embeddings.Data[0].Embedding) + "]");
                    articleDet.VectorDataString = x;//"[" + String.Join(",", embeddings.Data[0].Embedding) + "]";
                    articleDet.Tokens = (int)embeddings.Usage.TotalTokens;
                    // Get Embedding Vectors for this chunk
                    //var EmbeddingVectors = embeddings.Data[0].Embedding.Select(d => (float)d).ToArray();
                    // Instert all Embedding Vectors
                    //for (int i = 0; i < EmbeddingVectors.Length; i++)
                    //{
                    //    var embeddingVector = ArticleObjectSpace.CreateObject<ArticleVectorData>();

                    //    embeddingVector.ArticleDetailId = articleDet.ArticleDetailId;
                    //    embeddingVector.VectorValueId = i;
                    //    embeddingVector.VectorValue = EmbeddingVectors[i];

                    //    articleDet.ArticleVectorData.Add(embeddingVector);
                    //}
                    ArticleObjectSpace.CommitChanges();
                }               
            }

            // the object in the main listview, this only works in SDI mode!
            var selected = (FileSystemStoreObject)this.Application.MainWindow.View.CurrentObject;
            selected.Processed = true;
            Application.ShowViewStrategy.ShowMessage(string.Format("Finished Embedding"));
        }

     }
}
