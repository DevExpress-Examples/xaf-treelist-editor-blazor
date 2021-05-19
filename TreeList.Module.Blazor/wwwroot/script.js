window.TreeListEditorComponent = {
    Init: function (id, keyFieldName, dotnetHelper) {
        //var jsonData = JSON.parse(data);
        var treeList = $(`#${id}`).dxTreeList({
            keyExpr: keyFieldName,
            dataSource: {
                load: function (options) {
                    return dotnetHelper.invokeMethodAsync('GetData', options.parentIds.join(","));
                },
                key: keyFieldName
            },
            remoteOperations: {
                filtering: true
            },
            //parentIdExpr: "parentId",
            //hasItemsExpr: "hasItems",
            rootValue: null,
            selection: {
                mode: "multiple"
            },
            onRowClick: function (e) {
                //dotnetHelper.invokeMethodAsync('RowClick', e.key);
            },
            columnAutoWidth: true,
            wordWrapEnabled: true,
            showBorders: true
            //columns: [
            //    { dataField: "name" },
            //    {
            //        dataField: "size", width: 100,
            //        customizeText: function (e) {
            //            if (e.value !== null) {
            //                return Math.ceil(e.value / 1024) + " KB";
            //            }
            //        }
            //    },
            //    { dataField: "createdDate", dataType: "date", width: 150 },
            //    { dataField: "modifiedDate", dataType: "date", width: 150 }
            //]
        }).dxTreeList("instance");
    }
}