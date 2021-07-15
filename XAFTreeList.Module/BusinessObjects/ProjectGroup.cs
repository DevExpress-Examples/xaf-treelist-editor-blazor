using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;

namespace XAFTreeList.Module.BusinessObjects {
    public class ProjectGroup : Category {
        protected override ITreeNode Parent {
            get { return null; }
        }
        protected override IBindingList Children {
            get { return Projects; }
        }
        public ProjectGroup(Session session) : base(session) { }
        public ProjectGroup(Session session, string name) : base(session) {
            this.Name = name;
        }
        [Association("ProjectGroup-Projects"), Aggregated]
        public XPCollection<Project> Projects {
            get { return GetCollection<Project>("Projects"); }
        }
    }
}
