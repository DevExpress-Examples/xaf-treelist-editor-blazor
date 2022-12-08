using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;

namespace XAFTreeList.Module.BusinessObjects {
    public class Project : Category {
        protected override ITreeNode Parent {
            get { return ProjectGroup; }
        }
        protected override IBindingList Children {
            get { return new BindingList<ProjectArea>(ProjectAreas); }
        }
        public virtual ProjectGroup ProjectGroup { get; set; }
        public virtual IList<ProjectArea> ProjectAreas { get; set; } = new ObservableCollection<ProjectArea>();
    }
}
