using System;
using DevExpress.ExpressApp.Blazor.Components.Models;

namespace mbGPT.Blazor.Server.Editors
{
    /// <summary>
    /// Component model for DevExtreme HTML Edit control.
    /// 
    /// See: https://supportcenter.devexpress.com/ticket/details/t943982/blazor-how-to-integrate-a-custom-devextreme-component-and-bind-it-to-a-data-source
    /// </summary>
    public class HTMLEditModel : ComponentModelBase
    {
        /// <summary>
        /// Note that it does NOT raise ValueChanged event. Use SetValueFromUI to update the UI.
        /// </summary>
        public string Value
        {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }

        public bool ReadOnly
        {
            get => GetPropertyValue<bool>();
            set => SetPropertyValue(value);
        }

        public void SetValueFromUI(string value)
        {
            SetPropertyValue(value, notify: false, nameof(Value));
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ValueChanged;
    }
}
