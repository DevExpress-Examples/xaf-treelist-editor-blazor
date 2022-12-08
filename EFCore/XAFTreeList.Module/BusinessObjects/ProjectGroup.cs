using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;

namespace XAFTreeList.Module.BusinessObjects {
    public class ProjectGroup : Category {
        protected override ITreeNode Parent {
            get { return null; }
        }
        protected override IBindingList Children {
            get { return new BindingList<Project>(Projects); }
        }
        public virtual IList<Project> Projects { get; set; } = new ObservableCollection<Project>();
    }
}
