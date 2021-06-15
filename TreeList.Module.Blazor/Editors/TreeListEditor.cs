using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using TreeList.Module.Blazor.Components.Models;
using TreeList.Module.Blazor.Editors.Adapters;

namespace TreeList.Module.Blazor.Editors {
    public class TreeListEditor : ColumnsListEditor, IComplexListEditor {
        private XafApplication application;
        public TreeListEditor(IModelListView model) : base(model) { }

        public ITreeListEditorAdapter GetTreeListAdapter() => (ITreeListEditorAdapter)Control;
        public string KeyMember => ObjectTypeInfo.KeyMembers.Count > 0 ? ObjectTypeInfo.KeyMembers[0].Name : "";

        #region Columns
        private IList<ColumnWrapper> columnWrappers = new List<ColumnWrapper>();
        public override IList<ColumnWrapper> Columns => columnWrappers;
        protected override ColumnWrapper AddColumnCore(IModelColumn columnInfo) {
            var columnWrapper = new TreeListColumnWrapper();
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
        //internal void UnselectAll() {
        //    //BeginUpdate();
        //    //this.selectedObjectsKeys.Clear();
        //    //EndUpdate();
        //    //OnSelectionChanged();
        //}
        //public void UnselectObjects(IEnumerable<object> objectsToUnselect) {
        //    BeginUpdate();
        //    selectedObjectsKeys = selectedObjectsKeys.Except(objectsToUnselect).ToList();
        //    EndUpdate();
        //}
        //public void BeginUpdate() {
        //    LockSelectionEvents();
        //    //refreshLocker.Lock();
        //}
        //public void EndUpdate() {
        //    UnlockSelectionEvents();
        //    //refreshLocker.Unlock();
        //}
        #endregion
        #region Collections
        private CollectionSourceBase collectionSource;
        internal CollectionSourceBase CollectionSource => collectionSource;
        private IObjectSpace ObjectSpace { get => collectionSource?.ObjectSpace; }
        protected override void AssignDataSourceToControl(object dataSource) { }
        public void Setup(CollectionSourceBase collectionSource, XafApplication application) {
            this.collectionSource = collectionSource;
            this.application = application;
            if(collectionSource != null) {
                collectionSource.CriteriaApplied += CollectionSource_CriteriaApplied;
                //collectionSource.ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
                //collectionSource.ObjectSpace.ObjectDeleted += ObjectSpace_ObjectDeleted;
                //collectionSource.ObjectSpace.Committed += ObjectSpace_Committed;
            }
        }
        private void CollectionSource_CriteriaApplied(object sender, EventArgs e) {
            //Refresh();
        }
        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e) {
            //IObjectSpace objectSpace = (IObjectSpace)sender;
            //if(!objectSpace.IsNewObject(e.Object) && ObjectTypeInfo.Type.IsAssignableFrom(e.Object.GetType())) {
            //    Refresh();
            //}
        }
        private void ObjectSpace_Committed(object sender, EventArgs e) {
            //Refresh();
        }
        private void ObjectSpace_ObjectDeleted(object sender, ObjectsManipulatingEventArgs e) {
            //Type objectType = ObjectType;
            //if(objectType != null) {
            //    List<object> objectsToUnselect = new List<object>();
            //    foreach(object obj in e.Objects) {
            //        if(objectType.IsAssignableFrom(obj.GetType())) {
            //            objectsToUnselect.Add(ObjectTypeInfo.KeyMember.GetValue(obj));
            //        }
            //    }
            //    if(objectsToUnselect.Count > 0) {
            //        UnselectObjects(objectsToUnselect);
            //    }
            //}
        }
        public override void Dispose() {
            if(collectionSource != null) {
                collectionSource.CriteriaApplied -= CollectionSource_CriteriaApplied;
                //collectionSource.ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
                //collectionSource.ObjectSpace.Committed -= ObjectSpace_Committed;
                //collectionSource.ObjectSpace.ObjectDeleted -= ObjectSpace_ObjectDeleted;
                collectionSource = null;
            }
            Disposed?.Invoke(this, EventArgs.Empty);
            base.Dispose();
        }
        #endregion
        #region ControlCreation
        protected override object CreateControlsCore() {
            return CreateComponentAdapter();
        }
        protected ITreeListEditorAdapter CreateComponentAdapter() => CreateComponentAdapter(ObjectType);
        protected static TreeListEditorAdapter<T> CreateComponentAdapter<T>() => new TreeListEditorAdapter<T>(new TreeListEditorModel<T>());
        protected static ITreeListEditorAdapter CreateComponentAdapter(Type objectType) {
            return createComponentAdapterDelegateByType.GetOrAdd(objectType, key => CreateComponentAdapterDelegate(key))();
        }
        private static Func<ITreeListEditorAdapter> CreateComponentAdapterDelegate(Type objectType) {
            Expression createComponentAdapterCall = Expression.Call(typeof(TreeListEditor), nameof(CreateComponentAdapter), new Type[] { objectType });
            return Expression.Lambda<Func<ITreeListEditorAdapter>>(createComponentAdapterCall).Compile();
        }
        private static readonly ConcurrentDictionary<Type, Func<ITreeListEditorAdapter>> createComponentAdapterDelegateByType = new ConcurrentDictionary<Type, Func<ITreeListEditorAdapter>>();
        protected override void OnControlsCreated() {
            if(Control is ITreeListEditorAdapterSetup adapter) {
                adapter.Setup(this);
            }
            ApplyModel();
            base.OnControlsCreated();
        }
        //internal IServiceProvider ServiceProvider => ((BlazorApplication)application).ServiceProvider;
        #endregion
        public override void Refresh() {
            Refreshed?.Invoke(this, EventArgs.Empty);
        }
        public override void ApplyModel() {
            if (Model != null) {
                CancelEventArgs args = new CancelEventArgs();
                OnModelApplying(args);
                if(!args.Cancel) {
                    if(Control != null) {
                        BlazorTreeListModelSynchronizer.ApplyModel(Model, this);
                    }
                    base.ApplyModel();
                    OnModelApplied();
                }
            }
            base.ApplyModel();
        }
        protected override void OnModelApplied() {
            base.OnModelApplied();
            if(Control is ITreeListEditorAdapterColumnsSetup adapter) {
                adapter.SetupFieldNames(this);
            }
        }
        public event EventHandler Refreshed;
        public event EventHandler Disposed;
    }
}
