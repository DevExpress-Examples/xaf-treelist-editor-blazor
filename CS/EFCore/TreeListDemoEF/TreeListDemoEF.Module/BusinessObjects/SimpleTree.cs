using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;


namespace XAFTreeList.Module.BusinessObjects {
    [DefaultClassOptions]
    public class SimpleTree : BaseObject, ITreeNode {
        #region ITreeNode
        IBindingList ITreeNode.Children {
            get { return new BindingList<SimpleTree>(ChildNodes); }
        }
        string ITreeNode.Name {
            get { return Name; }
        }
        ITreeNode ITreeNode.Parent {
            get { return ParentNode; }
        }
    
        #endregion
        public virtual string Name { get; set; }
        public virtual SimpleTree ParentNode { get; set; }
        public virtual IList<SimpleTree> ChildNodes { get; set; } = new ObservableCollection<SimpleTree>();
     
    }
}
