using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.Persistent.Base.General;

namespace TreeList.Module.Blazor.Components.Models {
    public class TreeListEditorComponentModel<T> : ComponentModelBase {
        public string KeyFieldName {
            get => GetPropertyValue<string>();
            set => SetPropertyValue(value);
        }
        public IEnumerable<T> Data {
            get => GetPropertyValue<IEnumerable<T>>();
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
        internal TreeListEditorComponent<T> TreeList {
            get => GetPropertyValue<TreeListEditorComponent<T>>();
            set => SetPropertyValue(value);
        }
    }
}
