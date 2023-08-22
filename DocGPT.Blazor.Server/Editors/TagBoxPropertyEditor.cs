using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using System.Collections;

namespace DocGPT.Blazor.Server.Editors
{
    [PropertyEditor(typeof(IEnumerable), nameof(TagBoxPropertyEditor), false)]
    public class TagBoxPropertyEditor : BlazorPropertyEditorBase
    {
        public TagBoxPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }
        protected override bool IsMemberSetterRequired() => false;
        protected override IComponentAdapter CreateComponentAdapter()
        {
            DxTagBoxModel<DataItem<string>, string> componentModel = new DxTagBoxModel<DataItem<string>, string>();
            List<DataItem<string>> data = new List<DataItem<string>>();
            IObjectSpace objectSpace = View.ObjectSpace;
            ITypeInfo itemTypeInfo = MemberInfo.ListElementTypeInfo;
            foreach (object item in objectSpace.GetObjects(itemTypeInfo.Type))
            {
                string objectHandle = objectSpace.GetObjectHandle(item);
                string displayText = itemTypeInfo.DefaultMember.GetValue(item)?.ToString();
                data.Add(new DataItem<string>(objectHandle, displayText));
            }
            componentModel.Data = data;
            componentModel.ValueFieldName = nameof(DataItem<string>.Value);
            componentModel.TextFieldName = nameof(DataItem<string>.Text);
            return new DxTagBoxAdapter<DataItem<string>, string>(componentModel);
        }
        protected override void ReadValueCore()
        {
            if (Control is DxTagBoxAdapter<DataItem<string>, string> adapter && PropertyValue is IList propertyList)
            {
                List<string> values = new List<string>();
                IObjectSpace objectSpace = View.ObjectSpace;
                foreach (object obj in propertyList)
                {
                    string objectHandle = objectSpace.GetObjectHandle(obj);
                    values.Add(objectHandle);
                }
                adapter.ComponentModel.Values = values;
            }
        }
        protected override void WriteValueCore()
        {
            if (Control is DxTagBoxAdapter<DataItem<string>, string> adapter && PropertyValue is IList propertyList)
            {
                IObjectSpace objectSpace = View.ObjectSpace;
                List<object> actualObjects = new List<object>();
                if (adapter.ComponentModel.Values != null)
                {
                    foreach (string objectHandle in adapter.ComponentModel.Values)
                    {
                        actualObjects.Add(objectSpace.GetObjectByHandle(objectHandle));
                    }
                }
                List<object> objectsToLink = new List<object>(actualObjects);
                List<object> objectsToUnlink = new List<object>();
                foreach (object obj in propertyList)
                {
                    objectsToLink.Remove(obj);
                    if (!actualObjects.Contains(obj))
                    {
                        objectsToUnlink.Add(obj);
                    }
                }
                foreach (object obj in objectsToUnlink)
                {
                    propertyList.Remove(obj);
                }
                foreach (object obj in objectsToLink)
                {
                    propertyList.Add(obj);
                }
                objectSpace.SetModified(CurrentObject);
            }
        }
    }
}