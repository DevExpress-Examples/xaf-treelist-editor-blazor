using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using TreeList.Module.Blazor.Components.Models;
using TreeList.Module.Blazor.Editors.Adapters;

namespace TreeList.Module.Blazor.Editors {
    public class TreeListEditor : ColumnsListEditor, IComplexListEditor {
        private XafApplication application;
        public TreeListEditor(IModelListView model) : base(model) {
        }

        public string KeyMember => ObjectTypeInfo.KeyMembers.Count > 0 ? ObjectTypeInfo.KeyMembers[0].Name : "";

        #region Columns
        private IList<ColumnWrapper> columnWrappers = new List<ColumnWrapper>();
        public override IList<ColumnWrapper> Columns => columnWrappers;
        protected override ColumnWrapper AddColumnCore(IModelColumn columnInfo) {
            var columnWrapper = new ColumnWrapper();
            columnWrapper.ApplyModel(columnInfo);
            Columns.Add(columnWrapper);
            return columnWrapper;
        }
        #endregion
        #region Selection
        public override SelectionType SelectionType => SelectionType.MultipleSelection;
        private IList<object> selectedObjectsKeys = new List<object>();
        public override IList GetSelectedObjects() {
            return selectedObjectsKeys.Select(k => GetObjectByKey(k)).Where(obj => obj != null).ToList();
        }
        private object GetObjectByKey(object key) {
            if(collectionSource == null || collectionSource.DataAccessMode == CollectionSourceDataAccessMode.DataView || ObjectSpace == null || ObjectSpace.IsDisposed) {
                return null;
            }
            object result = FindObjectByKey(ObjectSpace, collectionSource.ObjectTypeInfo, key);
            if(result == null && collectionSource.List != null) {
                result = collectionSource.List.OfType<object>().SingleOrDefault(obj => object.Equals(ObjectSpace.GetKeyValue(obj), key));
            }
            return result;
        }
        private object FindObjectByKey(IObjectSpace objectSpace, ITypeInfo typeInfo, object key) {
            return objectSpace.GetObjectByKey(typeInfo.Type, key) ?? objectSpace.FindObject(typeInfo.Type, BaseObjectSpace.CreateByKeyCriteria(typeInfo, key));
        }
        internal void RowClick(object key) {
            IList<object> previousSelectedObjectsKeys = new List<object>(selectedObjectsKeys);
            SetSelectedObjectsKeys(new List<object>() { key });
            OnProcessSelectedItem();
            SetSelectedObjectsKeys(previousSelectedObjectsKeys);
        }
        internal void SetSelectedObjectsKeys(IEnumerable<object> selectedObjectsKeys) {
            this.selectedObjectsKeys = selectedObjectsKeys.ToList();
            OnSelectionChanged();
        }
        #endregion
        #region Collections
        public override void Refresh() { }

        private CollectionSourceBase collectionSource;
        internal CollectionSourceBase CollectionSource => collectionSource;
        private IObjectSpace ObjectSpace { get => collectionSource?.ObjectSpace; }
        protected override void AssignDataSourceToControl(object dataSource) { }
        public void Setup(CollectionSourceBase collectionSource, XafApplication application) {
            this.collectionSource = collectionSource;
            this.application = application;
        }
        #endregion
        #region ControlCreation
        protected override object CreateControlsCore() {
            return CreateComponentAdapter();
        }
        protected ITreeListEditorComponentAdapter CreateComponentAdapter() => CreateComponentAdapter(ObjectType);
        protected static TreeListEditorComponentAdapter<T> CreateComponentAdapter<T>() => new TreeListEditorComponentAdapter<T>(new TreeListEditorComponentModel<T>());
        protected static ITreeListEditorComponentAdapter CreateComponentAdapter(Type objectType) {
            return createComponentAdapterDelegateByType.GetOrAdd(objectType, key => CreateComponentAdapterDelegate(key))();
        }
        private static Func<ITreeListEditorComponentAdapter> CreateComponentAdapterDelegate(Type objectType) {
            Expression createComponentAdapterCall = Expression.Call(typeof(TreeListEditor), nameof(CreateComponentAdapter), new Type[] { objectType });
            return Expression.Lambda<Func<ITreeListEditorComponentAdapter>>(createComponentAdapterCall).Compile();
        }
        private static readonly ConcurrentDictionary<Type, Func<ITreeListEditorComponentAdapter>> createComponentAdapterDelegateByType = new ConcurrentDictionary<Type, Func<ITreeListEditorComponentAdapter>>();
        protected override void OnControlsCreated() {
            if(Control is ITreeListEditorComponentAdapterSetup adapter) {
                adapter.Setup(this);
            }
            base.OnControlsCreated();
        }
        internal IServiceProvider ServiceProvider => ((BlazorApplication)application).ServiceProvider;
        #endregion
    }
}
