using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Blazor.Components.Models;

namespace XAFTreeList.Module.Blazor.Editors {
    public class TreeListModel : ComponentModelBase {
        public Func<string, Task<IEnumerable<object>>> GetDataAsync {
            get => GetPropertyValue<Func<string, Task<IEnumerable<object>>>>();
            set => SetPropertyValue(value);
        }
        public string[] FieldNames {
            get => GetPropertyValue<string[]>();
            set => SetPropertyValue(value);
        }
        public Func<object, string, string> GetFieldDisplayText {
            get => GetPropertyValue<Func<object, string, string>>();
            set => SetPropertyValue(value);
        }
        public Func<object, string> GetKey {
            get => GetPropertyValue<Func<object, string>>();
            set => SetPropertyValue(value);
        }
        public Func<object, bool> HasChildren {
            get => GetPropertyValue<Func<object, bool>>();
            set => SetPropertyValue(value);
        }
        public void Refresh() => RefreshRequested?.Invoke(this, EventArgs.Empty);
        public void OnRowClick(string key) => RowClick?.Invoke(this, new TreeListRowClickEventArgs(key));
        public void OnSelectionChanged(string[] keys) => SelectionChanged?.Invoke(this, new TreeListSelectionChangedEventArgs(keys));
        public event EventHandler RefreshRequested;
        public event EventHandler<TreeListRowClickEventArgs> RowClick;
        public event EventHandler<TreeListSelectionChangedEventArgs> SelectionChanged;
    }
    public class TreeListRowClickEventArgs : EventArgs {
        public TreeListRowClickEventArgs(string key) {
            Key = key;
        }
        public string Key { get; }
    }
    public class TreeListSelectionChangedEventArgs : EventArgs {
        public TreeListSelectionChangedEventArgs(string[] keys) {
            Keys = keys;
        }
        public string[] Keys { get; }
    }
}
