////import 'https://cdn3.devexpress.com/jslib/21.1.4/js/dx.all.js';

export function addTreeListToElement(element, fieldNames, dotnetHelper) {
    var treeList = $(element).dxTreeList({
        keyExpr: "__key",
        rootValue: null,
        parentIdExpr: "__parentKey",
        hasItemsExpr: "__hasChild",
        dataSource: {
            load: function (options) {
                var parentKeys = null;
                if (options.parentIds != null) {
                    //parentKeys = options.parentIds.join(",");
                    parentKeys = options.parentIds[0];
                }
                return dotnetHelper.invokeMethodAsync('OnGetDataAsync', parentKeys);
            },
            key: "__key"
        },
        remoteOperations: {
            filtering: true
        },
        selection: {
            mode: "multiple"
        },
        onRowClick: function (e) {
            if (!e.event.target.parentElement.classList.contains("dx-treelist-expanded") && !e.event.target.parentElement.classList.contains("dx-treelist-collapsed")) {
                dotnetHelper.invokeMethodAsync('OnRowClick', e.key);
            }
        },
        onSelectionChanged: function (e) {
            dotnetHelper.invokeMethodAsync('OnSelectionChanged', e.selectedRowKeys);
        },
        columnAutoWidth: true,
        wordWrapEnabled: true,
        showRowLines: true,
        showBorders: true,
        columns: fieldNames
    }).dxTreeList('instance');
    return treeList;
}
export function refresh(element) {
    $(element).dxTreeList('instance').refresh();
}