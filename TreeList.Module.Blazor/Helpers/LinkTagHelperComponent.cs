using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TreeList.Module.Blazor.Helpers {
    [HtmlTargetElement("head")]
    class LinkTagHelperComponent : TagHelperComponent {
        private string jQuery = "<script type=\"text/javascript\" src=\"https://code.jquery.com/jquery-3.5.1.min.js\"></script>";
        private string style = "<link rel=\"stylesheet\" href=\"https://cdn3.devexpress.com/jslib/20.2.7/css/dx.common.css\" />";
        private string theme = "<link rel=\"stylesheet\" href=\"https://cdn3.devexpress.com/jslib/20.2.7/css/dx.light.css\">";
        private string script = "<script type=\"text/javascript\" src=\"https://cdn3.devexpress.com/jslib/20.2.7/js/dx.all.js\"></script>";
        private string componentScript = "<script src=\"_content/TreeList.Module.Blazor/script.js\" ></script>";
        private string componentStyles = "<link rel=\"stylesheet\" href=\"_content/TreeList.Module.Blazor/styles.css\">";
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            if(string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase)) {
                output.PostContent.AppendHtml(jQuery).AppendLine();
                output.PostContent.AppendHtml(style).AppendLine();
                output.PostContent.AppendHtml(theme).AppendLine();
                output.PostContent.AppendHtml(script).AppendLine();
                output.PostContent.AppendHtml(componentScript).AppendLine();
                output.PostContent.AppendHtml(componentStyles).AppendLine();
            }
            return Task.CompletedTask;
        }
    }
}
