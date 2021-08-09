# XAF Blazor - How to implement a TreeList editor

## Scenario

There cases when it is required to display information represented by List Views in a tree-like structure. End-users should be able to collapse/expand nodes and create, update and delete records.

## Solution

The solution consists of two parts. The first part is to create a custom Razor component using the DevExtreme TreeList widget. For this, we created a Razor class library (RCL) project and used the following concepts:
- [Call .NET methods from JavaScript functions in ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-dotnet-from-javascript?view=aspnetcore-5.0)
- [Call JavaScript functions from .NET methods in ASP.NET Core Blazor](https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/call-javascript-from-dotnet?view=aspnetcore-5.0)
- [Tree List > Data Bidning > Load Data on Demand](https://js.devexpress.com/Demos/WidgetsGallery/Demo/TreeList/LoadDataOnDemand/jQuery/Light/)

As a result, we implemented a component with a required API to integrate it into an XAF Blazor application.

The second part is to create a custom List Editor using the created Razor component and recommendations from the following help topic:
- [How to: Use a Custom Component to Implement List Editor (Blazor)](https://docs.devexpress.com/eXpressAppFramework/403258/ui-construction/list-editors/how-to-use-a-custom-component-to-implement-list-editor-blazor)

<img src="./media/example.png" width="600">

## Implementation Details

### Part 1. Create a custom Razor component

**Step 1.** Create a RCL project. Add references to it in your \*.Module.Blazor and \*.Blazor.Server projects. 
**Step 2.** Register DevExtreme libraries in the \*.Blazor.Server/Pages/_Host.csthml page as per as [Add DevExtreme to a jQuery Application](https://js.devexpress.com/Documentation/Guide/jQuery_Components/Add_DevExtreme_to_a_jQuery_Application/).
**Step 3.** Add a Razor component to the project. Declare IJSObjectReference and ElementReference properties. Initialize them using JSInterop in the overridden OnAfterRenderAsync method. This is necessary to use JavaScript code to interact with the DevExtreme TreeList widget.
**Step 4.** Declare the following parameters:
- *GetDataAsync* - a Func<string, Task<IEnumerable<object>>> delegate that will encapsulate a method that fetches data on demand;
- *FieldNames* - an array of string that contains field names;
- *GetFieldDisplayText* - a Func<object, string, string> delegate that will encapsulate a method that returns field captions;
- *GetKey* - a Func<object, string> delegate that will encapsulate a method that returns the current key value;
- *HasChildren* - a Func<object, bool> delegate that will encapsulate a method that determines whether the currently processed node has child nodes;
- *RowClick* - an EventCallback<string> delegate that represents a method that will handle an event when a row is clicked;
- *SelectionChanged* - an EventCallback<string[]> delegate that represents a method that will handle an event when selection is changed.
**Step 5.** Declare the *OnGetDataAsync* method and wrap it with the JSInvokable attribute. It will be called from JavaScript code to fetch data based on the passed parent key value. In this method, create a dictionary of field name-value pairs.
**Step 6.** Declare the *OnRowClick* and *OnSelectionChanged* methods. They will be used to raise the RowClick and SelectionChanged events respectively.
**Step 7.** Declare the *Refresh* method that will call the JavaScript [TreeList.refresh](https://js.devexpress.com/Documentation/ApiReference/UI_Components/dxTreeList/Methods/#refresh) method.
**Step 8.** Create a JavaScript script in the *wwwroot* folder. This script will contain TreeList API.
**Step 9.** In the script, configure TreeList to load data on demand as per as [Load Data on Demand](https://js.devexpress.com/Demos/WidgetsGallery/Demo/TreeList/LoadDataOnDemand/jQuery/Light/). Call the declared .NET *OnGetDataAsync* method using [DotNetObjectReference](https://docs.microsoft.com/en-us/dotnet/api/microsoft.jsinterop.dotnetobjectreference?view=dotnet-plat-ext-6.0) to fetch data.
**Step 10.** Handle the [TreeList.rowClick](https://js.devexpress.com/Documentation/ApiReference/UI_Components/dxTreeList/Configuration/#onRowClick) and [TreeList.selectionChanged](https://js.devexpress.com/Documentation/ApiReference/UI_Components/dxTreeList/Configuration/#onSelectionChanged) events to call the declared .NET *OnRowClick* and *OnSelectionChanged* methods respectively.

### Part 2. Create a custom List Editor

**Step 1.** Create a **ComponentModelBase** descendant. In this class, declare the following properties that describe the component and its interaction with a user:
- *GetDataAsync* - a Func<string, Task<IEnumerable<object>>> delegate that will encapsulate a method that fetches data on demand;
- *FieldNames* - an array of string that contains field names;
- *GetFieldDisplayText* - a Func<object, string, string> delegate that will encapsulate a method that returns field captions;
- *GetKey* - a Func<object, string> delegate that will encapsulate a method that returns the current key value;
- *HasChildren* - a Func<object, bool> delegate that will encapsulate a method that determines whether the currently processed node has child nodes;
- *RowClick*, *SelectionChanged*, *RefreshRequested* - EventCallback delegates that will handle the corresponding events;
- *OnRowClick*, *OnSelectionChanged*, *Refresh* - method that will raise the corresponding events;
**Step 2.** Create EventArgs descendants to pass key values to the *RowClick* and *SelectionChanged* event handlers:

```cs
public class TreeListRowClickEventArgs : EventArgs {
	public TreeListRowClickEventArgs(string key) {
		Key = key;
	}
	public string Key { get; }
}
public class TreeListSelectionChangedEventArgs : EventArgs {
	public TreeListSelectionChangedEventArgs(string[] keys) {
		Keys = keys;
	}
	public string[] Keys { get; }
}
```

**Step 3.** Create a Razor component that will render the TreeList component from the RCL project. Declare the required parameters and implement the *IDisposable* interface.
**Step 4.** Create a **ListEditor** descendant, apply the *ListEditorAttribute* to this class and pass a *ITreeNode* type to it.
**Step 5.** Implement the **IComplexListEditor** interface. In the *IComplexListEditor.Setup* method, initialize an Object Space instance.
**Step 6.** Override the **CreateControlsCore** method to return an instance of the TreeList component. 
**Step 7.** Override the **AssignDataSourceToControl** method. In this method, assign the List Editor’s data source to the component model. If the data source implements the *IBindingList*﻿ interface, handle data change notifications. 
**Step 8.** Override the **OnControlsCreated** method. In this method, pass methods to the created delegates, initialize the arrays of field names and subscribe to the component model’s *RowClick* and *SelectionChanged* events.  
**Step 9.** Override the **BreakLinksToControls** method. In this method, unsubscribe from the component model’s events and reset its data to release resources.  
**Step 10.** Override the **Refresh** method. In this method, call the component model's *Refresh* method to update the ListEditor layout when its data is changed. 
**Step 11.** Override the **SelectionType** property to return *SelectionType.Full*. This setting allows a user to open the Detail View by click. 
**Step 12.** Override the **GetSelectedObjects** method. In this method, return an arrays of selected objects.

<!-- default file list -->
*Files to look at*:

* [TreeList.razor](./BlazorComponents/TreeList.razor)
* [treeListModule.js](./BlazorComponents/wwwroot/treeListModule.js)
* [Component Model](./XAFTreeList.Module.Blazor/Editors/TreeListModel.cs)
* [Component Renderer](./XAFTreeList.Module.Blazor/Editors/TreeListRenderer.razor)  
* [List Editor](./XAFTreeList.Module.Blazor/Editors/TreeListEditor.cs)  
<!-- default file list end -->