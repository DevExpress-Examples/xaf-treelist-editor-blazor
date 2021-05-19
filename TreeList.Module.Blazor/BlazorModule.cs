using System;
using System.Linq;
using System.Text;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base.General;
using TreeList.Module.Blazor.Editors;

namespace TreeList.Module.Blazor {
    [ToolboxItemFilter("Xaf.Platform.Blazor")]
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class TreeListSampleBlazorModule : ModuleBase {
        public TreeListSampleBlazorModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
        protected override void RegisterEditorDescriptors(EditorDescriptorsFactory editorDescriptorsFactory) {
            editorDescriptorsFactory.RegisterListEditorAlias(EditorAliases.TreeListEditor, typeof(ITreeNode), true);
            editorDescriptorsFactory.RegisterListEditor(EditorAliases.TreeListEditor, typeof(ITreeNode), typeof(TreeListEditor), true);
        }
    }
}
