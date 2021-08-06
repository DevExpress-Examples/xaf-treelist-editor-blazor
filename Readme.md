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

### Part 1. Creating a custom Razor component

**Step 1.**
**Step 2.**

### Part 2. Creating a custom List Editor

**Step 1.**
**Step 2.**

<!-- default file list -->
*Files to look at*:

* [TreeList.razor](./BlazorComponents/TreeList.razor)
* [treeListModule.js](./BlazorComponents/wwwroot/treeListModule.js)

* [Component Model](./XAFTreeList.Module.Blazor/Editors/TreeListModel.cs)
* [Component Renderer](./XAFTreeList.Module.Blazor/Editors/TreeListRenderer.razor)  
* [List Editor](./XAFTreeList.Module.Blazor/Editors/TreeListEditor.cs)  
<!-- default file list end -->