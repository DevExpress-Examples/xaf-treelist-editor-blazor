window.TreeListEditorComponent = {
    _treeLists: {},
    Init: function (id, keyFieldName, dotnetHelper) {
        var treeList = $(`#${id}`).dxTreeList({
            keyExpr: keyFieldName,
            rootValue: "",
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
            onSelectionChanged: function (e) {
                dotnetHelper.invokeMethodAsync('SelectionChanged', e.selectedRowKeys);
            },
            columnAutoWidth: true,
            wordWrapEnabled: true,
            showRowLines: true,
            showBorders: true,
            columns: [
                { dataField: "Name" }
            ]
        }).dxTreeList("instance");
        window.TreeListEditorComponent._treeLists[id] = { treeList, dotnetHelper };
    },
    Refresh: function (id) {
        window.TreeListEditorComponent._treeLists[id]["treeList"].refresh();
    },
    Dispose: function (id) {
        if (window.TreeListEditorComponent._treeLists[id]) {
            for (const comp in window.TreeListEditorComponent._treeLists[id]) {
                window.TreeListEditorComponent._treeLists[id][comp].dispose();
            }
            delete window.TreeListEditorComponent._treeLists[id];
        }
    }
}