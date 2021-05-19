using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using DevExpress.Xpo;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace XpoSerialization {

    internal class ConfigureJsonOptions : IConfigureOptions<JsonSerializerOptions>, IServiceProvider {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public ConfigureJsonOptions(
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider) {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
        }

        //public ConfigureJsonOptions(IServiceProvider serviceProvider) {
        //    _serviceProvider = serviceProvider;
        //}

        public void Configure(JsonSerializerOptions options) {
            options.MaxDepth = 64;
            options.ReferenceHandler = ReferenceHandler.Preserve;
            options.Converters.Add(new PersistentBaseConverterFactory(this));
        }

        public object GetService(Type serviceType) {
            return (_httpContextAccessor.HttpContext?.RequestServices ?? _serviceProvider).GetService(serviceType);
        }
    }
}
