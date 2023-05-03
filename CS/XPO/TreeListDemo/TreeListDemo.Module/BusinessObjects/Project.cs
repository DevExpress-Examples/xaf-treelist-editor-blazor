using System.ComponentModel;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;

namespace XAFTreeList.Module.BusinessObjects {
    public class Project : Category {
        private ProjectGroup projectGroup;
        protected override ITreeNode Parent {
            get { return projectGroup; }
        }
        protected override IBindingList Children {
            get { return ProjectAreas; }
        }
        public Project(Session session) : base(session) { }
        public Project(Session session, string name) : base(session) {
            this.Name = name;
        }
        [Association("ProjectGroup-Projects")]
        public ProjectGroup ProjectGroup {
            get { return projectGroup; }
            set { SetPropertyValue("ProjectGroup", ref projectGroup, value); }
        }
        [Association("Project-ProjectAreas"), Aggregated]
        public XPCollection<ProjectArea> ProjectAreas {
            get { return GetCollection<ProjectArea>("ProjectAreas"); }
        }
    }
}
