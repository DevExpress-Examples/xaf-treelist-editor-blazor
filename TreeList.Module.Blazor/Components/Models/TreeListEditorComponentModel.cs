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
        public Action<object> RowClick {
            get => GetPropertyValue<Action<object>>();
            set => SetPropertyValue(value);
        }
    }
}
