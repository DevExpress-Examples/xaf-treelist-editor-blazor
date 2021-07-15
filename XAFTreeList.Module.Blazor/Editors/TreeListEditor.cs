using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base.General;
using Microsoft.AspNetCore.Components;

namespace XAFTreeList.Module.Blazor.Editors {
    [ListEditor(typeof(ITreeNode))]
    public class TreeListEditor : ListEditor {
        public class TreeListViewHolder : IComponentContentHolder {
            private RenderFragment componentContent;
            public TreeListViewHolder(TreeListViewModel componentModel) {
                ComponentModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
            }
            private RenderFragment CreateComponent() => ComponentModelObserver.Create(ComponentModel, TreeListComponent.Create(ComponentModel));
            public TreeListViewModel ComponentModel { get; }
            RenderFragment IComponentContentHolder.ComponentContent => componentContent ??= CreateComponent();
        }
        private ITreeNode[] selectedObjects = Array.Empty<ITreeNode>();
        public TreeListEditor(IModelListView model) : base(model) { }
        protected override object CreateControlsCore() => new TreeListViewHolder(new TreeListViewModel());
        protected override void AssignDataSourceToControl(object dataSource) {
            if(Control is TreeListViewHolder holder) {
                if(holder.ComponentModel.Data is IBindingList bindingList) {
                    bindingList.ListChanged -= BindingList_ListChanged;
                }
                holder.ComponentModel.Data = (dataSource as IEnumerable)?.OfType<ITreeNode>().OrderBy(i => i.Name);
                if(dataSource is IBindingList newBindingList) {
                    newBindingList.ListChanged += BindingList_ListChanged;
                }
            }
        }
        protected override void OnControlsCreated() {
            if(Control is TreeListViewHolder holder) {
                holder.ComponentModel.RowClick += ComponentModel_RowClick;
                holder.ComponentModel.SelectionChanged += ComponentModel_SelectionChanged;
            }
            base.OnControlsCreated();
        }
        public override void BreakLinksToControls() {
            if(Control is TreeListViewHolder holder) {
                holder.ComponentModel.RowClick -= ComponentModel_RowClick;
                holder.ComponentModel.SelectionChanged -= ComponentModel_SelectionChanged;
            }
            AssignDataSourceToControl(null);
            base.BreakLinksToControls();
        }
        public override void Refresh() {
            if(Control is TreeListViewHolder holder) {
                holder.ComponentModel.OnRefresh();
            }
        }
        private void BindingList_ListChanged(object sender, ListChangedEventArgs e) {
            Refresh();
        }
        private void ComponentModel_RowClick(object sender, TreeListRowClickEventArgs e) {
            selectedObjects = new ITreeNode[] { e.Item };
            OnSelectionChanged();
            OnProcessSelectedItem();
        }
        private void ComponentModel_SelectionChanged(object sender, TreeListSelectionChangedEventArgs e) {
            selectedObjects = e.Items;
            OnSelectionChanged();
        }
        public override SelectionType SelectionType => SelectionType.Full;
        public override IList GetSelectedObjects() => selectedObjects;
    }
}
