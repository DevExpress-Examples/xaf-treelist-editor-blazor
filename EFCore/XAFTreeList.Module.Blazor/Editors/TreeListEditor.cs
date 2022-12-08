using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base.General;
using Microsoft.AspNetCore.Components;

namespace XAFTreeList.Module.Blazor.Editors
{
    [ListEditor(typeof(ITreeNode))]
    public class TreeListEditor : ListEditor, IComplexListEditor {
        public class TreeListHolder : IComponentContentHolder {
            private RenderFragment componentContent;
            public TreeListHolder(TreeListModel componentModel) {
                ComponentModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
            }
            private RenderFragment CreateComponent() => ComponentModelObserver.Create(ComponentModel, TreeListRenderer.Create(ComponentModel));
            public TreeListModel ComponentModel { get; }
            RenderFragment IComponentContentHolder.ComponentContent => componentContent ??= CreateComponent();
        }
        private ITreeNode[] selectedObjects = Array.Empty<ITreeNode>();
        private IObjectSpace objectSpace;
        private IEnumerable<object> data;
        public TreeListEditor(IModelListView model) : base(model) { }
        void IComplexListEditor.Setup(CollectionSourceBase collectionSource, XafApplication application) {
            objectSpace = collectionSource.ObjectSpace;
        }
        protected override object CreateControlsCore() => new TreeListHolder(new TreeListModel());
        protected override void AssignDataSourceToControl(object dataSource) {
            if(Control is TreeListHolder holder) {
                if(data is IBindingList bindingList) {
                    bindingList.ListChanged -= BindingList_ListChanged;
                }
                data = (dataSource as IEnumerable)?.Cast<ITreeNode>();
                if(dataSource is IBindingList newBindingList) {
                    newBindingList.ListChanged += BindingList_ListChanged;
                }
            }
        }
        protected override void OnControlsCreated() {
            if(Control is TreeListHolder holder) {
                holder.ComponentModel.GetDataAsync = GetDataAsync;
                holder.ComponentModel.FieldNames = new string[] { nameof(ITreeNode.Name) };
                holder.ComponentModel.GetFieldDisplayText = GetFieldDisplayText;
                holder.ComponentModel.GetKey = GetKey;
                holder.ComponentModel.HasChildren = HasChildren;
                holder.ComponentModel.RowClick += ComponentModel_RowClick;
                holder.ComponentModel.SelectionChanged += ComponentModel_SelectionChanged;
            }
            base.OnControlsCreated();
        }

        private Task<IEnumerable<object>> GetDataAsync(string parentKey) {
            bool IsRoot(object obj) {
                return obj is ITreeNode node && (node.Parent == null || !data.Contains(node.Parent));
            }
            if(parentKey is null) {
                IEnumerable<object> rootData = data?.Where(n => IsRoot(n)) ?? Array.Empty<object>();
                return Task.FromResult(rootData);
            }
            ITreeNode parent = GetNode(parentKey);
            return Task.FromResult(parent.Children.Cast<object>());
        }
        private string GetFieldDisplayText(object item, string field) => ObjectTypeInfo.FindMember(field).GetValue(item)?.ToString();
        private string GetKey(object item) => objectSpace.GetObjectHandle(item);
        private ITreeNode GetNode(string key) => (ITreeNode)objectSpace.GetObjectByHandle(key);
        private bool HasChildren(object item) => ((ITreeNode)item).Children.Count > 0;

        public override void BreakLinksToControls() {
            if(Control is TreeListHolder holder) {
                holder.ComponentModel.RowClick -= ComponentModel_RowClick;
                holder.ComponentModel.SelectionChanged -= ComponentModel_SelectionChanged;
            }
            AssignDataSourceToControl(null);
            base.BreakLinksToControls();
        }
        public override void Refresh() {
            if(Control is TreeListHolder holder) {
                holder.ComponentModel.Refresh();
            }
        }
        private void BindingList_ListChanged(object sender, ListChangedEventArgs e) {
            Refresh();
        }
        private void ComponentModel_RowClick(object sender, TreeListRowClickEventArgs e) {
            selectedObjects = new ITreeNode[] { GetNode(e.Key) };
            OnSelectionChanged();
            OnProcessSelectedItem();
        }
        private void ComponentModel_SelectionChanged(object sender, TreeListSelectionChangedEventArgs e) {
            var items = e.Keys.Select(key => GetNode(key)).ToArray();
            selectedObjects = items;
            OnSelectionChanged();
        }
        public override SelectionType SelectionType => SelectionType.Full;
        public override IList GetSelectedObjects() => selectedObjects;
    }
}
