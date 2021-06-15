using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.Persistent.Base.General;

namespace TreeList.Module.Blazor.Components.Models {
    public class TreeListEditorModel<T> : TreeListEditorModel {
        public IEnumerable<T> Data {
            get => GetPropertyValue<IEnumerable<T>>();
            set => SetPropertyValue(value);
        }
        internal TreeListEditorComponent<T> TreeList {
            get => GetPropertyValue<TreeListEditorComponent<T>>();
            set => SetPropertyValue(value);
        }
    }
    public abstract class TreeListEditorModel : ComponentModelBase {
        public string KeyFieldName {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }
        internal object RootValue {
            get => GetPropertyValue<object>() != null ? GetPropertyValue<object>() : string.Empty;
            set => SetPropertyValue(value);
        }
        internal Type RootValueType {
            get => GetPropertyValue<Type>();
            set => SetPropertyValue(value);
        }
        internal object RootObject {
            get => GetPropertyValue<object>();
            set => SetPropertyValue(value);
        }
        public IEnumerable<string> FieldNames {
            get => GetPropertyValue<IEnumerable<string>>();
            set => SetPropertyValue(value);
        }
        internal Action<object> RowClick {
            get => GetPropertyValue<Action<object>>();
            set => SetPropertyValue(value);
        }
        internal Action<IEnumerable<object>> SelectionChanged {
            get => GetPropertyValue<Action<IEnumerable<object>>>();
            set => SetPropertyValue(value);
        }
    }
}
