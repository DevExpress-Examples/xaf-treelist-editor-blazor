using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;

namespace TreeList.Module.Blazor.Editors {
    public class TreeListColumnWrapper : ColumnWrapper {
        bool visible;
        string id;
        string propertyName;
        //public string FieldName { get; set; }
        public override bool Visible { get => visible; }
        public override string Id { get => id; }
        public override string PropertyName { get => propertyName; }
        public override void ApplyModel(IModelColumn columnInfo) {
            base.ApplyModel(columnInfo);
            visible = columnInfo.Index >= 0 || columnInfo.GroupIndex >= 0;
            Caption = columnInfo.Caption;
            //FieldName = columnInfo.FieldName;
            id = columnInfo.Id;
            propertyName = columnInfo.PropertyName;
        }
    }
}
