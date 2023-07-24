using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel.DataAnnotations.Schema;
using HtmlAgilityPack;
using static DevExpress.XtraPrinting.Native.ExportOptionsPropertiesNames;

namespace DocGPT.Module.BusinessObjects
{
    // Register this entity in your DbContext (usually in the BusinessObjects folder of your project) with the "public DbSet<WebSiteData> WebSiteDatas { get; set; }" syntax.

    [DefaultClassOptions]
    [NavigationItem("Code")]
    public class WebSiteData : BaseObjectNoID
    {
        public WebSiteData()
        {
            // In the constructor, initialize collection properties, e.g.: 
            // this.AssociatedEntities = new ObservableCollection<AssociatedEntityObject>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [VisibleInDetailView(false)]
        public virtual int WebSiteDataId { get; set; }

        [FieldSize(200)]
        public virtual string Website { get; set; }

        [FieldSize(200)]

        public virtual string URL { get; set; }

        [Action(AutoCommit = true, Caption = "Get it", ConfirmationMessage = "Get it?", ToolTip = "Eerst de URL invullen.", TargetObjectsCriteria = "Not IsNullOrEmpty([URL])")]
        public async Task GetTheWebsiteAsync()
        {
            var url = URL;
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();

                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                // Removing scripts
                htmlDocument.DocumentNode.Descendants()
                             .Where(n => n.Name == "script")
                             .ToList()
                             .ForEach(n => n.Remove());

                // Removing styles
                htmlDocument.DocumentNode.Descendants()
                             .Where(n => n.Name == "style")
                             .ToList()
                             .ForEach(n => n.Remove());

                var cleanedHtml = htmlDocument.DocumentNode.OuterHtml;
                Text = cleanedHtml;
            }
        }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual string Text { get; set; }
    }
}