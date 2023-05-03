using System.ComponentModel;
using DevExpress.Persistent.Base.General;

namespace XAFTreeList.Module.BusinessObjects {
    public class ProjectArea : Category {
        protected override ITreeNode Parent {
            get { return Project; }
        }
        protected override IBindingList Children {
            get { return new BindingList<object>(); }
        }
        public virtual Project Project { get; set; }
    }
}
