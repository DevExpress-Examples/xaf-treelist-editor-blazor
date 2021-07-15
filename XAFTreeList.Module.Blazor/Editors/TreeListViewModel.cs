using System;
using System.Collections.Generic;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.Persistent.Base.General;

namespace XAFTreeList.Module.Blazor.Editors {
    public class TreeListViewModel : ComponentModelBase {
        public string[] PropertyNames {
            get => GetPropertyValue<string[]>();
            set => SetPropertyValue(value);
        }
        public IEnumerable<ITreeNode> Data {
            get => GetPropertyValue<IEnumerable<ITreeNode>>();
            set => SetPropertyValue(value);
        }
        //public void Refresh() => RaiseChanged();

        public void OnRowClick(ITreeNode item) => RowClick?.Invoke(this, new TreeListRowClickEventArgs(item));
        public void OnSelectionChanged(ITreeNode[] items) => SelectionChanged?.Invoke(this, new TreeListSelectionChangedEventArgs(items));
        public void OnRefresh() => Refreshed?.Invoke(this, EventArgs.Empty);

        public event EventHandler<TreeListRowClickEventArgs> RowClick;
        public event EventHandler<TreeListSelectionChangedEventArgs> SelectionChanged;
        public event EventHandler Refreshed;
    }
    public class TreeListRowClickEventArgs : EventArgs {
        public TreeListRowClickEventArgs(ITreeNode item) {
            Item = item;
        }
        public ITreeNode Item { get; }
    }
    public class TreeListSelectionChangedEventArgs : EventArgs {
        public TreeListSelectionChangedEventArgs(ITreeNode[] items) {
            Items = items;
        }
        public ITreeNode[] Items { get; }
    }
}
