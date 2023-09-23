using System;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using Microsoft.AspNetCore.Components;

namespace mbGPT.Blazor.Server.Editors
{
    /// <summary>
    /// Adapter for DevExtreme HTML Edit control.
    /// 
    /// See: https://supportcenter.devexpress.com/ticket/details/t943982/blazor-how-to-integrate-a-custom-devextreme-component-and-bind-it-to-a-data-source
    /// </summary>
    public class HTMLEditAdapter : ComponentAdapterBase, IDisposable
    {
        public HTMLEditAdapter(HTMLEditModel componentModel)
        {
            ComponentModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
            ComponentModel.ValueChanged += ComponentModel_ValueChanged;
        }

        public HTMLEditModel ComponentModel { get; }

        public override void SetAllowEdit(bool allowEdit)
        {
            ComponentModel.ReadOnly = !allowEdit;
        }

        public override object GetValue()
        {
            return ComponentModel.Value;
        }

        public override void SetValue(object value)
        {
            ComponentModel.Value = (string)value;
        }

        protected override RenderFragment CreateComponent()
        {
            return ComponentModelObserver.Create(ComponentModel, HTMLEditRenderer.Create(ComponentModel));
        }

        private void ComponentModel_ValueChanged(object sender, EventArgs e) => RaiseValueChanged();
        public override void SetAllowNull(bool allowNull) { /* ...*/ }
        public override void SetDisplayFormat(string displayFormat) { /* ...*/ }
        public override void SetEditMask(string editMask) { /* ...*/ }
        public override void SetEditMaskType(EditMaskType editMaskType) { /* ...*/ }
        public override void SetErrorIcon(ImageInfo errorIcon) { /* ...*/ }
        public override void SetErrorMessage(string errorMessage) { /* ...*/ }
        public override void SetIsPassword(bool isPassword) { /* ...*/ }
        public override void SetMaxLength(int maxLength) { /* ...*/ }
        public override void SetNullText(string nullText) { /* ...*/ }

        #region Dispose

        void IDisposable.Dispose()
        {
            ComponentModel.ValueChanged -= ComponentModel_ValueChanged;
        }

        #endregion
    }
}
