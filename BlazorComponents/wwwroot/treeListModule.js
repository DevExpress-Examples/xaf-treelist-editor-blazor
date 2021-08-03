////import 'https://cdn3.devexpress.com/jslib/21.1.4/js/dx.all.js';

export function addTreeListToElement(element, fieldNames, dotNetHelper) {
    var treeList = $(element).dxTreeList({
        keyExpr: "__key",
        rootValue: null,
        parentIdExpr: "__parentKey",
        hasItemsExpr: "__hasChildren",
        columns: fieldNames,
        dataSource: {
            key: "__key",
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