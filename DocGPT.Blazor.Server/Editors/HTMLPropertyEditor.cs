using System;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;

namespace DocGPT.Blazor.Server.Editors
{
    /// <summary>
    /// Property editor for DevExtreme HTML Edit control.
    /// 
    /// Inspired on: https://supportcenter.devexpress.com/ticket/details/t943982/blazor-how-to-integrate-a-custom-devextreme-component-and-bind-it-to-a-data-source
    /// See also: https://docs.devexpress.com/eXpressAppFramework/402189/task-based-help/property-editors/how-to-implement-a-property-editor-based-on-custom-components-blazor?p=netstandard&utm_source=SupportCenter&utm_medium=website&utm_campaign=docs-feedback&utm_content=T980328
    /// </summary>
    [PropertyEditor(typeof(string), EditorAliases.HtmlPropertyEditor, false)]
    public class HTMLPropertyEditor : BlazorPropertyEditorBase
    {
        public HTMLPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model)
        {

        }

        protected override void OnControlCreated()
        {
            base.OnControlCreated();

            if (View?.ObjectSpace != null)
                View.ObjectSpace.Reloaded += ObjectSpace_Reloaded;
        }

        protected override IComponentAdapter CreateComponentAdapter() => new HTMLEditAdapter(new HTMLEditModel());

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            HTMLEditModel componentModel = new HTMLEditModel();
            componentModel.Value = Convert.ToString(this.GetPropertyValue(dataContext));
            return HTMLEditRenderer.CreateReadOnly(componentModel);
            //return base.CreateViewComponentCore(dataContext);
        }

        private void ObjectSpace_Reloaded(object sender, EventArgs e)
        {
            HTMLEditAdapter htmlEditAdapter = Control as HTMLEditAdapter;
            if (htmlEditAdapter != null)
                htmlEditAdapter.ComponentModel.SetValueFromUI(htmlEditAdapter.ComponentModel.Value); // TODO change this line as it does not refresh the control.
        }

        protected override void Dispose(bool disposing)
        {
            if (View?.ObjectSpace != null)
                View.ObjectSpace.Reloaded -= ObjectSpace_Reloaded;

            base.Dispose(disposing);
        }
    }
}
