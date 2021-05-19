window.TreeListEditorComponent = {
    Init: function (id, keyFieldName, dotnetHelper) {
        var treeList = $(`#${id}`).dxTreeList({
            keyExpr: keyFieldName,
            rootValue: null,
            parentIdExpr: "ParentId",
            hasItemsExpr: "HasChildren",
            dataSource: {
                load: function (options) {
                    return dotnetHelper.invokeMethodAsync('GetData', options.parentIds.join(","));
                },
                key: keyFieldName
            },
            remoteOperations: {
                filtering: true
            },
            selection: {
                mode: "multiple"
            },
            onRowClick: function (e) {
                if (!e.event.target.parentElement.classList.contains("dx-treelist-expanded") && !e.event.target.parentElement.classList.contains("dx-treelist-collapsed")) {
                    dotnetHelper.invokeMethodAsync('RowClick', e.key);
                }
            },
            columnAutoWidth: true,
            wordWrapEnabled: true,
            showRowLines: true,
            showBorders: true,
            columns: [
                { dataField: "Name" }
            ]
        }).dxTreeList("instance");
    }
}