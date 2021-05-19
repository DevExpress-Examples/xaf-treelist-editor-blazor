using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using TreeList.Module.Blazor.Helpers;
using XpoSerialization;

namespace TreeList.Module.Blazor.Extensions {
    public static class StartupExtensions {
        public static IServiceCollection AddTreeListBlazorModule(this IServiceCollection services) {
            services.AddTransient<ITagHelperComponent, LinkTagHelperComponent>();
            services.ConfigureOptions<ConfigureJsonOptions>();
            DevExpress.ExpressApp.XafTypesInfo.Instance.RegisterEntity(typeof(DevExpress.Persistent.Base.General.ITreeNode));
            return services;
        }
    }
}
