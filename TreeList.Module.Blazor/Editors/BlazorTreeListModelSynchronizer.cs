using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace TreeList.Module.Blazor.Editors {
    public class BlazorTreeListModelSynchronizer {
        public static void ApplyModel(IModelListView model, TreeListEditor listEditor) {
            new ColumnsListEditorModelSynchronizer(listEditor, model).ApplyModel();
            //if(model is IModelListViewBlazor modelListViewBlazor) {
            //    ApplyPagingModel(modelListViewBlazor, listEditor.GetDataGridAdapter());
            //}
        }
    }
}
