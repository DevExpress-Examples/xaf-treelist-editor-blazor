using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFTreeList.Module.BusinessObjects {
    [NavigationItem]
    public abstract class Category : BaseObject, ITreeNode {
        protected abstract ITreeNode Parent { get; }
        protected abstract IBindingList Children { get; }
        public virtual string Name { get; set; }
        #region ITreeNode
        IBindingList ITreeNode.Children {
            get { return Children; }
        }
        string ITreeNode.Name {
            get { return Name; }
        }
        ITreeNode ITreeNode.Parent {
            get { return Parent; }
        }
        #endregion
    }
}
