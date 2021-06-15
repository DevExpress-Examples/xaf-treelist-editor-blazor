using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using TreeList.Module.Blazor.Components;
using TreeList.Module.Blazor.Components.Models;

namespace TreeList.Module.Blazor.Editors.Adapters {
    public interface ITreeListEditorComponentAdapter : IComponentContentHolder { }
    public interface ITreeListEditorComponentAdapter<T> : ITreeListEditorComponentAdapter { }
    interface ITreeListEditorComponentAdapterSetup {
        void Setup(TreeListEditor treeListEditor);
    }
    public class TreeListEditorComponentAdapter<T> : ITreeListEditorComponentAdapter<T>, ITreeListEditorComponentAdapterSetup {
        private TreeListEditor editor;
        public TreeListEditorComponentAdapter(TreeListEditorComponentModel<T> componentModel) {
            ComponentModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
        }
        public TreeListEditorComponentModel<T> ComponentModel { get; }
        public RenderFragment ComponentContent {
            get { return CreateComponent(); }
        }
        private RenderFragment CreateComponent() => ComponentModelObserver.Create(ComponentModel, TreeListEditorComponent<T>.Create(ComponentModel));
        void ITreeListEditorComponentAdapterSetup.Setup(TreeListEditor treeListEditor) {
            editor = treeListEditor;
            void RowClick(object key) {
                treeListEditor.RowClick(key);
            }
            void SelectionChanged(IEnumerable<object> keys) {
                treeListEditor.SetSelectedObjectsKeys(keys);
            }
            ComponentModel.KeyFieldName = editor.KeyMember;
            ComponentModel.RowClick = RowClick;
            ComponentModel.SelectionChanged = SelectionChanged;
            SubscribeToEditorEvents(editor);
            SetupData(editor, out _);
        }
        private void SubscribeToEditorEvents(TreeListEditor treeListEditor) {
            treeListEditor.Refreshed += TreeListEditor_Refreshed;
        }
        private void SetupData(TreeListEditor editor, out bool needRefresh) {
            var enumerable = editor.CollectionSource.GetEnumerable<T>();
            needRefresh = false;
            if(ComponentModel.Data == enumerable && !(enumerable is INotifyCollectionChanged)) {
                needRefresh = true;
            }
            if(ComponentModel.Data != enumerable) {
                if(ComponentModel.Data is INotifyCollectionChanged oldNotifier) {
                    oldNotifier.CollectionChanged -= OnCollectionChanged;
                }
                if(enumerable is INotifyCollectionChanged newNotifier) {
                    newNotifier.CollectionChanged += OnCollectionChanged;
                }
            }
            ComponentModel.Data = enumerable;
            void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
                if(e.Action == NotifyCollectionChangedAction.Remove) {
                    editor.UnselectAll();
                }
            }
        }
        private async void TreeListEditor_Refreshed(object sender, EventArgs e) {
            SetupData(editor, out bool needRefresh);
            if(needRefresh && ComponentModel.TreeList != null) {
                await ComponentModel.TreeList.Refresh();
            }
        }
    }
}
