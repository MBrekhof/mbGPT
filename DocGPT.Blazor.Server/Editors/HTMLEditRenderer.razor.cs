using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DocGPT.Blazor.Server.Editors
{
    /// <summary>
    /// Renderer for DevExtreme HTML Edit control.
    /// 
    /// See: https://supportcenter.devexpress.com/ticket/details/t943982/blazor-how-to-integrate-a-custom-devextreme-component-and-bind-it-to-a-data-source
    /// </summary>
    public class HTMLEditRendererCode : ComponentBase, IDisposable
    {
        //private IObjectSpace _objectSpace;
        private HtmlEditUpdateInvokeHelper _gridUpdateInvokeHelper;

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        [Inject]
        protected DevExpress.ExpressApp.Blazor.Services.IXafApplicationProvider ApplicationProvider { get; set; }

        [Parameter]
        public HTMLEditModel ComponentModel { get; set; }

        protected override void OnInitialized()
        {
            _gridUpdateInvokeHelper = new HtmlEditUpdateInvokeHelper(UpdateEditor);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await JSRuntime.InvokeVoidAsync("JsFunctions.RefreshDevExtremeHtmlEditor", GetData());
                return;
            }
            //XafApplication application = ApplicationProvider.GetApplication();
            //_objectSpace = application.CreateObjectSpace(typeof(BlazorTest.Module.BusinessObjects.DomainObject1));
            await JSRuntime.InvokeVoidAsync("JsFunctions.InitDevExtremeHtmlEditor", GetData(), "FieldNameTODO", DotNetObjectReference.Create(_gridUpdateInvokeHelper));
        }

        private async void UpdateEditor(UpdateItem param)
        {
            //var obj = _objectSpace.GetObjectByKey<Projects.Standard.Module.DomainObject1>(param.Key);
            //if (obj == null)
            //{
            //  return;
            //}
            //obj.Name = param.Values.Name;
            //_objectSpace.CommitChanges();

            ComponentModel.SetValueFromUI(param.Value);

            await JSRuntime.InvokeVoidAsync("JsFunctions.RefreshDevExtremeHtmlEditor", GetData());
        }

        private string GetData()
        {
            return ComponentModel.Value; //_objectSpace.GetObjects<Projects.Standard.Module.DomainObject1>().Select(item => new Item { Oid = item.Oid, Name = item.Name });
        }

        #region CreateReadOnly

        /// <summary>
        /// Render raw html.
        /// 
        /// See: https://docs.devexpress.com/Blazor/401753/common-concepts/customize-and-reuse-components
        /// See also: https://github.com/dotnet/aspnetcore/issues/19640
        /// </summary>
        public static RenderFragment CreateReadOnly(HTMLEditModel componentModel)
        {
            return builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddMarkupContent(1, componentModel.Value);
                builder.CloseElement();
            };
        }

        ///// <summary>
        ///// Alternative builder - build of components.
        ///// </summary>
        //public static RenderFragment CreateReadOnly(HTMLEditModel componentModel)
        //{
        //  return RenderTextBox(componentModel.Value);
        //}

        //public static RenderFragment RenderTextBox(string content)
        //{
        //  RenderFragment item = builder =>
        //  {

        //    builder.OpenComponent<DevExpress.Blazor.DxTextBox>(0);
        //    builder.AddAttribute(1, "Text", content);
        //    builder.CloseComponent();
        //  };
        //  return item;
        //}

        #endregion

        #region IDisposable members

        void IDisposable.Dispose()
        {
            //_objectSpace?.Dispose();
        }

        #endregion
    }

    public class HtmlEditUpdateInvokeHelper
    {
        private Action<UpdateItem> _action;

        public HtmlEditUpdateInvokeHelper(Action<UpdateItem> action)
        {
            _action = action;
        }

        [JSInvokable]
        public void UpdateEditorCaller(UpdateItem param)
        {
            _action.Invoke(param);
        }
    }

    //public class Item
    //{
    //  public Guid Oid { get; set; }
    //  public string Name { get; set; }
    //}

    public class UpdateItem
    {
        public string FieldName { get; set; }
        public string Value { get; set; }

        //public Guid Key { get; set; }
        //public Item Values { get; set; }
    }
}
