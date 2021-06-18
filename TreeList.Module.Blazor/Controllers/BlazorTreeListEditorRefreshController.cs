using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using TreeList.Module.Blazor.Editors;

namespace TreeList.Module.Blazor.Controllers {
    public class BlazorTreeListEditorRefreshController : RefreshController {
        public BlazorTreeListEditorRefreshController() {
            TargetViewType = ViewType.ListView;
            RefreshAction.ExecuteCompleted += RefreshAction_ExecuteCompleted;
        }
        private void RefreshAction_ExecuteCompleted(object sender, DevExpress.ExpressApp.Actions.ActionBaseEventArgs e) {
            if(((ListView)View).Editor is TreeListEditor treeListEditor) {
                treeListEditor.Refresh();
            }
        }
    }
}
