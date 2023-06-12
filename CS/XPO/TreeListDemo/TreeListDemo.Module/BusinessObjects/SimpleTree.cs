using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace XAFTreeList.Module.BusinessObjects {
    [DefaultClassOptions]
    public class SimpleTree : BaseObject, ITreeNode {
        #region ITreeNode
        IBindingList ITreeNode.Children {
            get { return ChildNodes; }
        }
        string ITreeNode.Name {
            get { return Name; }
        }
        ITreeNode ITreeNode.Parent {
            get { return ParentNode; }
        }
        #endregion
        private string name;
        public string Name {
            get { return name; }
            set { SetPropertyValue("Name", ref name, value); }
        }
        private SimpleTree _parentNode;

        public SimpleTree(Session session) : base(session) {
        }

        [Association]
        public SimpleTree ParentNode {
            get { return _parentNode; }
            set { SetPropertyValue(nameof(ParentNode), ref _parentNode, value); }
        }
        [Association]
        public XPCollection<SimpleTree> ChildNodes {
            get { return GetCollection<SimpleTree>(nameof(ChildNodes)); }
        }
    }
}
