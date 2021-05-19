using System;
using DevExpress.ExpressApp.Xpo;

namespace TreeListSample.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
