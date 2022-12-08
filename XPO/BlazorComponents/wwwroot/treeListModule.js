export function addTreeListToElement(element, fieldNames, dotNetHelper) {
    const keyExpression = "__key";
    const parentKeyExpression = "__parentKey";
    const hasChildrenExpression = "__hasChildren";
    var treeList = $(element).dxTreeList({
        keyExpr: keyExpression,
        rootValue: null,
        parentIdExpr: parentKeyExpression,
        hasItemsExpr: hasChildrenExpression,
        columns: fieldNames,
        dataSource: {
            key: keyExpression,
            load: function (options) {
                var parentKeys = null;
                if (options.parentIds) {
                    parentKeys = options.parentIds[0];
                }
                return dotNetHelper.invokeMethodAsync('OnGetDataAsync', parentKeys);
            }
        },
        remoteOperations: {
            filtering: true
        },
        selection: {
            mode: "multiple"
        },
        onRowClick: function (e) {
            if (!e.event.target.parentElement.classList.contains("dx-treelist-expanded") && !e.event.target.parentElement.classList.contains("dx-treelist-collapsed")) {
                dotNetHelper.invokeMethodAsync('OnRowClick', e.key);
            }
        },
        onSelectionChanged: function (e) {
            dotNetHelper.invokeMethodAsync('OnSelectionChanged', e.selectedRowKeys);
        },
        columnAutoWidth: true,
        wordWrapEnabled: true,
        showRowLines: true,
        showBorders: true
    }).dxTreeList('instance');
    return treeList;
}
export function refresh(element) {
    $(element).dxTreeList('instance').refresh();
}
export function dispose(element) {
    if (element) {
        $(element).dxTreeList('dispose');
    }
}