using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base.General;
using TreeList.Module.Blazor.Editors;
using TreeList.Module.Blazor.Editors.Adapters;

namespace TreeList.Module.Blazor.Controllers {
    public class BlazorTreeListEditorRootValueController : ViewController {
        PropertyCollectionSource propertyCollectionSource = null;
        ITreeListEditorAdapter treeListEditorAdapter;
        public BlazorTreeListEditorRootValueController() {
            TargetViewType = ViewType.ListView;
            TargetViewNesting = Nesting.Nested;
        }
        protected override void OnActivated() {
            base.OnActivated();
            propertyCollectionSource = ((ListView)View).CollectionSource as PropertyCollectionSource;
            if(propertyCollectionSource != null) {
                propertyCollectionSource.CollectionChanged += new EventHandler(propertyCollectionSource_CollectionChanged);
                propertyCollectionSource.MasterObjectChanged += new EventHandler(propertyCollectionSource_MasterObjectChanged);
            }
        }
        private void propertyCollectionSource_CollectionChanged(object sender, EventArgs e) {
            SetRootValue();
        }
        private void propertyCollectionSource_MasterObjectChanged(object sender, EventArgs e) {
            SetRootValue();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
            if(((ListView)View).Editor is TreeListEditor treeListEditor) {
                treeListEditorAdapter = treeListEditor.GetTreeListAdapter();
                SetRootValue();
            }
        }
        protected virtual void SetRootValue() {
            if(treeListEditorAdapter != null) {
                object rootValue = null;
                Type rootValueType = null;
                ITypeInfo rootValueTypeInfo = null;
                if(propertyCollectionSource != null) {
                    object owner = propertyCollectionSource.MemberInfo.GetOwnerInstance(propertyCollectionSource.MasterObject);
                    if(typeof(ITreeNode).IsAssignableFrom(propertyCollectionSource.MemberInfo.ListElementType)) {
                        rootValueTypeInfo = propertyCollectionSource.MemberInfo.ListElementTypeInfo;
                        rootValueType = propertyCollectionSource.MemberInfo.ListElementType;
                        while(rootValueTypeInfo.Base != null && typeof(ITreeNode).IsAssignableFrom(rootValueType.BaseType)) {
                            rootValueTypeInfo = rootValueTypeInfo.Base;
                            rootValueType = rootValueTypeInfo.Type;
                        }
                    }
                    if(owner != null && propertyCollectionSource.MemberInfo.ListElementType != null && (owner.GetType().IsAssignableFrom(propertyCollectionSource.MemberInfo.ListElementType) ||
                         propertyCollectionSource.MemberInfo.ListElementType.IsAssignableFrom(owner.GetType()) ||
                         (rootValueType.IsAssignableFrom(propertyCollectionSource.MemberInfo.ListElementType) && rootValueType.IsAssignableFrom(owner.GetType())))) {
                        rootValue = rootValueTypeInfo.KeyMember.GetValue(owner);
                    }
                }
                treeListEditorAdapter.TreeListModel.RootValue = rootValue;
                treeListEditorAdapter.TreeListModel.RootValueType = rootValueType;
            }
        }
        protected override void OnDeactivated() {
            if(propertyCollectionSource != null) {
                propertyCollectionSource.CollectionChanged -= new EventHandler(propertyCollectionSource_CollectionChanged);
                propertyCollectionSource.MasterObjectChanged -= new EventHandler(propertyCollectionSource_MasterObjectChanged);
                propertyCollectionSource = null;
            }
            base.OnDeactivated();
        }
    }
}
