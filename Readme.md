<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/398001179/22.2.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1023129)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# XAF Blazor - How to Implement a TreeList Editor to Display Hierarchical Data

> **Note**  
> The description of this example is currently under construction and may not match the code in the example. We are currently working to provide you with up-to-date content.

Implement the following components to add the TreeList Editor to your ASP.NET Core Blazor application:

* A Razor component based on the DevExtreme TreeList widget.
* A component model that changes the state of the component.
* A component renderer that binds the component model with the component.
* A List Editor that integrates the component into your XAF application.

The following image demonstrates the result:

![example](https://user-images.githubusercontent.com/14300209/235937849-de05ae88-f1cf-4ff8-baf1-833d78769016.png)


## Alternative Solutions

### Group List View Data Using DxGridListEditor
If this DevExtreme-based solution does not meet your requirements (for instance, you do not want to implement the ITreeNode contract in your data model from scratch or do not want to inherit your class from our HCategory), you can consider using the data grid and group by the 'Parent' property to emulate a tree: [Group List View Data](https://docs.devexpress.com/eXpressAppFramework/403248/getting-started/in-depth-tutorial-blazor/customize-data-display-and-view-layout/group-list-view-data). In this case, it is also natural to enable a 'Split View' (MasterDetailMode = ListAndDetailView): [Enable Split Layout in a List View](https://docs.devexpress.com/eXpressAppFramework/404203/getting-started/in-depth-tutorial-blazor/customize-data-display-and-view-layout/display-a-detail-view-with-a-list-view).

### Implement a Custom List Editor or View Item from Scratch
Of course, you can also build your own List Editor or View Item to display your custom data model using suitable DevExtreme or Blazor components: [Using a Custom Control that is not Integrated by Default](https://docs.devexpress.com/eXpressAppFramework/113610/ui-construction/using-a-custom-control-that-is-not-integrated-by-default/using-a-custom-control-that-is-not-integrated-by-default).

## Implementation Details

### ITreeNode-based Data Model

XAF has a built-in ITreeNode interface, which can be implemented by business objects to visualize them as a tree in a ListView (refer to the Category and related classes in this example). You can find more example code of ITreeNode-based data models below OR you can inherit your business class from our built-in HCategory class):
 - **EF Core**: "c:\Program Files\DevExpress 2X.Y\Components\Sources\DevExpress.Persistent\DevExpress.Persistent.BaseImpl.EFCore\HCategory.cs" 
 - **XPO**: "c:\Program Files\DevExpress 2X.Y\Components\Sources\DevExpress.Persistent\DevExpress.Persistent.BaseImpl.Xpo\HCategory.cs" 

This hierarchical data visualization is currently supported for WinForms and ASP.NET WebForms out-of-the-box (hence this example for ASP.NET Core Blazor).

### Razor Component

1. Create a Razor class library (RCL) project (_BlazorComponents_). Reference it in your _TreeListDemoEF.Blazor.Server_ project. 

2. Register DevExtreme libraries in the _TreeListDemoEF.Blazor.Server/Pages/[\_Host.cshtml](./CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/Pages/_Host.cshtml#L16-L19)_ page as described in the following topic: [Add DevExtreme to a jQuery Application](https://js.devexpress.com/Documentation/Guide/jQuery_Components/Add_DevExtreme_to_a_jQuery_Application/).  
3. Add the [TreeList.razor](./CS/EFCore/TreeListDemoEF/BlazorComponents/TreeList.razor) Razor component to the _BlazorComponents_ project. 
	
	The following table describes the APIs implemented in this component: 
	
	| API | Type | Description |
	| --- | ---- | ----------- |
	| GetDataAsync | parameter | Encapsulates a method that fetches data on demand. |
	| FieldNames | parameter | Stores an array of field names. |
	| GetFieldDisplayText | parameter | Encapsulates a method that returns field captions. |
	| GetKey | parameter | Encapsulates a method that returns the current key value. |
	| HasChildren | parameter | Encapsulates a method that determines whether the currently processed node has child nodes. |
	| RowClick | parameter | Encapsulates a method that handles an event when users click a row. |
	| SelectionChanged | parameter | Encapsulates a method that handles an event when users change selection. |  
	| OnRowClick and OnSelectionChanged | methods | Used to raise the **RowClick** and **SelectionChanged** events. |
	| OnAfterRenderAsync | method | Initializes the necessary [IJSObjectReference](https://docs.microsoft.com/en-us/dotnet/api/microsoft.jsinterop.ijsobjectreference), [ElementReference](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.elementreference), and [DotNetObjectReference<TreeList>](https://docs.microsoft.com/en-us/dotnet/api/microsoft.jsinterop.dotnetobjectreference-1) fields for interaction with the DevExtreme TreeList widget. |
	| OnGetDataAsync | method | Creates a dictionary of field name-value pairs. This method is called from JavaScript code to fetch data based on the passed parent key value. |  
	| Refresh | method | Calls the JavaScript [TreeList.refresh](https://js.devexpress.com/Documentation/ApiReference/UI_Components/dxTreeList/Methods/#refresh) method. | 

4. Add the [treeListModule.js](./CS/EFCore/TreeListDemoEF/BlazorComponents/wwwroot/treeListModule.js ) script with the TreeList API to the _BlazorComponents\wwwroot_ folder. In the script, configure TreeList to load data on demand as described in the following article: [Load Data on Demand](https://js.devexpress.com/Demos/WidgetsGallery/Demo/TreeList/LoadDataOnDemand/jQuery/Light/). Use the **DotNetObjectReference** object to call the declared .NET **OnGetDataAsync** method and fetch data. Handle the [TreeList.rowClick](https://js.devexpress.com/Documentation/ApiReference/UI_Components/dxTreeList/Configuration/#onRowClick) and [TreeList.selectionChanged](https://js.devexpress.com/Documentation/ApiReference/UI_Components/dxTreeList/Configuration/#onSelectionChanged) events to call the declared .NET **OnRowClick** and **OnSelectionChanged** methods.

**See also**:
* [Call .NET methods from JavaScript functions in ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-5.0)
* [Call JavaScript functions from .NET methods in ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-javascript-from-dotnet?view=aspnetcore-5.0)
* [Tree List > Data Binding > Load Data on Demand](https://js.devexpress.com/Demos/WidgetsGallery/Demo/TreeList/LoadDataOnDemand/jQuery/Light/)

### Component Model 

1. In the Blazor project (_TreeListDemoEF.Blazor.Server_), create the **ComponentModelBase** descendant and name it [TreeListModel.cs](./CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/Editors/TreeListEditor/TreeListModel.cs). 

	The following table describes the APIs implemented in this component: 

	| API | Type | Description |
	| --- | ---- | ----------- | 
	| GetDataAsync | property | Encapsulates a method that fetches data on demand. |
	| FieldNames | property | Stores an array of field names. |
	| GetFieldDisplayText | property | Encapsulates a method that returns field captions.|
	| GetKey | property | Encapsulates a method that returns the current key value. |
	| HasChildren | property | Encapsulates a method that determines whether the currently processed node has child nodes. |
	| RowClick, SelectionChanged, RefreshRequested | events | Occur when users click a row and change selection. |
	| OnRowClick, OnSelectionChanged, Refresh | methods | Used to raise the corresponding events. |

2. Create **EventArgs** descendants to pass key values to the *RowClick* and *SelectionChanged* event handlers. See these classes in the following file: [TreeListModel.cs](./CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/Editors/TreeListEditor/TreeListModel.cs#L35-L46).

### Component Renderer

1. In the Blazor project (_TreeListDemoEF.Blazor.Server_), create a new Razor component and name it [TreeListRenderer.razor](./CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/Editors/TreeListEditor/TreeListRenderer.razor). This component renders the **TreeList** component from the RCL project. 
2. Ensure that the component’s [Build Action](https://docs.microsoft.com/en-us/visualstudio/ide/build-actions) property is set to **Content**.
3. Declare the required parameters and implement the *IDisposable* interface. 

### List Editor

1. Create a [ListEditor](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Editors.ListEditor) descendant, apply the [ListEditorAttribute](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Editors.ListEditorAttribute) to this class, and pass an [ITreeNode](https://docs.devexpress.com/eXpressAppFramework/DevExpress.Persistent.Base.General.ITreeNode) type as a parameter.  
2. Implement the [IComplexListEditor](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Editors.IComplexListEditor) interface. In the [IComplexListEditor.Setup](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Editors.IComplexListEditor.Setup(DevExpress.ExpressApp.CollectionSourceBase-DevExpress.ExpressApp.XafApplication)) method, initialize an Object Space instance. 

The following table describes the API implemented in this List Editor:

| API | Type | Description | 
| --- | ---- | ----------- | 
| SelectionType | property | Returns [SelectionType.Full](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.SelectionType). This setting allows users to open the Detail View by click. |
| CreateControlsCore | method | Returns an instance of the TreeList component. |
| AssignDataSourceToControl | method | Assigns the List Editor’s data source to the component model. If the data source implements the [IBindingList](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.ibindinglist) interface, this method handles data change notifications. |
| OnControlsCreated | method | Passes methods to the created delegates, initializes the arrays of field names, and subscribes to the component model’s *RowClick* and *SelectionChanged* events. |
| BreakLinksToControls | method | Unsubscribes from the component model’s events and resets its data to release resources. |
| Refresh | method | Calls the component model's *Refresh* method to update the List Editor layout when its data is changed. |
| GetSelectedObjects | method | Returns an array of selected objects. |

### Supported Data Access mode

As the created tree list editor supports only the 'Client' [Data Access mode](https://docs.devexpress.com/eXpressAppFramework/113683/ui-construction/views/list-view-data-access-modes), you need to use the [RegisterEditorSupportedModes](https://docs.devexpress.com/eXpressAppFramework/113683/ui-construction/views/list-view-data-access-modes) method as shown here:

https://github.com/DevExpress-Examples/xaf-treelist-editor-blazor/blob/34ef5255cd98d894d4f48360943adbebe11babfe/CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/BlazorModule.cs#L21

## Files to Review

EF Core:

* [BlazorComponents/TreeList.razor](./CS/EFCore/TreeListDemoEF/BlazorComponents/TreeList.razor)
* [BlazorComponents/wwwroot/treeListModule.js](./CS/EFCore/TreeListDemoEF/BlazorComponents/wwwroot/treeListModule.js)
* [EFCore/XAFTreeList.Module.Blazor/Editors/TreeListModel.cs](./CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/Editors/TreeListEditor/TreeListModel.cs )
* [EFCore/XAFTreeList.Module.Blazor/Editors/TreeListRenderer.razor](./CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/Editors/TreeListEditor/TreeListRenderer.razor)  
* [EFCore/XAFTreeList.Module.Blazor/Editors/TreeListEditor.cs](./CS/EFCore/TreeListDemoEF/TreeListDemoEF.Blazor.Server/Editors/TreeListEditor/TreeListEditor.cs)  



## Documentation

* [How to: Use a Custom Component to Implement List Editor (Blazor)](https://docs.devexpress.com/eXpressAppFramework/403258/ui-construction/list-editors/how-to-use-a-custom-component-to-implement-list-editor-blazor)
* [Using a Custom Control that is not Integrated by Default](https://docs.devexpress.com/eXpressAppFramework/113610/ui-construction/using-a-custom-control-that-is-not-integrated-by-default/using-a-custom-control-that-is-not-integrated-by-default)

## More Examples

[Use a Custom Component to Implement List Editor (Blazor)](https://github.com/DevExpress-Examples/xaf-custom-list-editor-blazor)
