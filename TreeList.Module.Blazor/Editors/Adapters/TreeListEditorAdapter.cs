using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using Microsoft.AspNetCore.Components;
using TreeList.Module.Blazor.Components;
using TreeList.Module.Blazor.Components.Models;

namespace TreeList.Module.Blazor.Editors.Adapters {
    public interface ITreeListEditorAdapter : IComponentContentHolder {
        TreeListEditorModel TreeListModel { get; }
    }
    public interface ITreeListEditorAdapter<T> : ITreeListEditorAdapter {
        new TreeListEditorModel<T> TreeListModel { get; }
    }
    interface ITreeListEditorAdapterSetup {
        void Setup(TreeListEditor treeListEditor);
    }
    interface ITreeListEditorAdapterColumnsSetup {
        void SetupFieldNames(TreeListEditor treeListEditor);
    }
    public class TreeListEditorAdapter<T> : ITreeListEditorAdapter<T>, ITreeListEditorAdapterSetup, ITreeListEditorAdapterColumnsSetup, IDisposable {
        private TreeListEditor editor;
        public TreeListEditorAdapter(TreeListEditorModel<T> componentModel) {
            TreeListModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
        }
        public TreeListEditorModel<T> TreeListModel { get; }
        public RenderFragment ComponentContent {
            get { return CreateComponent(); }
        }
        TreeListEditorModel ITreeListEditorAdapter.TreeListModel => TreeListModel;
        private RenderFragment CreateComponent() => ComponentModelObserver.Create(TreeListModel, TreeListEditorComponent<T>.Create(TreeListModel));
        void ITreeListEditorAdapterSetup.Setup(TreeListEditor treeListEditor) {
            editor = treeListEditor;
            void RowClick(object key) {
                treeListEditor.RowClick(key);
            }
            void SelectionChanged(IEnumerable<object> keys) {
                treeListEditor.SetSelectedObjectsKeys(keys);
            }
            TreeListModel.KeyFieldName = editor.KeyMember;
            TreeListModel.RowClick = RowClick;
            TreeListModel.SelectionChanged = SelectionChanged;
            SubscribeToEditorEvents(editor);
            SetupData(editor, out _);
        }
        private void SubscribeToEditorEvents(TreeListEditor treeListEditor) {
            treeListEditor.Refreshed += TreeListEditor_Refreshed;
            treeListEditor.Disposed += TreeListEditor_Disposed;
        }
        private void SetupData(TreeListEditor editor, out bool needRefresh) {
            var enumerable = editor.CollectionSource.GetEnumerable<T>();
            needRefresh = false;
            if(TreeListModel.Data == enumerable && !(enumerable is INotifyCollectionChanged)) {
                needRefresh = true;
            }
            //if(ComponentModel.Data != enumerable) {
            //    if(ComponentModel.Data is INotifyCollectionChanged oldNotifier) {
            //        oldNotifier.CollectionChanged -= OnCollectionChanged;
            //    }
            //    if(enumerable is INotifyCollectionChanged newNotifier) {
            //        newNotifier.CollectionChanged += OnCollectionChanged;
            //    }
            //}
            TreeListModel.Data = enumerable;
            //void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            //    if(e.Action == NotifyCollectionChangedAction.Remove) {
            //        editor.UnselectAll();
            //    }
            //}
        }
        void ITreeListEditorAdapterColumnsSetup.SetupFieldNames(TreeListEditor treeListEditor) {
            TreeListModel.FieldNames = editor.GetVisibleColumns().Select(x => x.PropertyName);
        }
        private async void TreeListEditor_Refreshed(object sender, EventArgs e) {
            SetupData(editor, out bool needRefresh);
            if(needRefresh && TreeListModel.TreeList != null) {
                await TreeListModel.TreeList.Refresh();
            }
        }
        private void TreeListEditor_Disposed(object sender, EventArgs e) {
            Invalidate();
        }
        void IDisposable.Dispose() {
            Invalidate();
        }
        private void Invalidate() {
            if(editor != null) {
                editor.Refreshed -= TreeListEditor_Refreshed;
                editor.Disposed -= TreeListEditor_Disposed;
            }
            //notifier.Unsubscribe(DataGridModel);
            //notifier.Unsubscribe(DataGridSelectionColumnModel);
            editor = null;
        }
    }
}
