using System;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using TreeList.Module.Blazor.Components;
using TreeList.Module.Blazor.Components.Models;

namespace TreeList.Module.Blazor.Editors.Adapters {
    public interface ITreeListEditorComponentAdapter : IComponentContentHolder { }
    public interface ITreeListEditorComponentAdapter<T> : ITreeListEditorComponentAdapter { }
    interface ITreeListEditorComponentAdapterSetup {
        void Setup(TreeListEditor treeListEditor);
    }
    public class TreeListEditorComponentAdapter<T> : ITreeListEditorComponentAdapter<T>, ITreeListEditorComponentAdapterSetup {
        public TreeListEditorComponentAdapter(TreeListEditorComponentModel<T> componentModel) {
            ComponentModel = componentModel ?? throw new ArgumentNullException(nameof(componentModel));
        }
        public TreeListEditorComponentModel<T> ComponentModel { get; }
        public RenderFragment ComponentContent {
            get { return CreateComponent(); }
        }
        private RenderFragment CreateComponent() => ComponentModelObserver.Create(ComponentModel, TreeListEditorComponent<T>.Create(ComponentModel));
        void ITreeListEditorComponentAdapterSetup.Setup(TreeListEditor treeListEditor) {
            void RowClick(object key) {
                treeListEditor.RowClick(key);
            }
            var enumerable = treeListEditor.CollectionSource.GetEnumerable<T>();
            ComponentModel.Data = enumerable;
            ComponentModel.KeyFieldName = treeListEditor.KeyMember;
            ComponentModel.RowClick = RowClick;
        }
    }
}
