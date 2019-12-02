using Microsoft.Extensions.Options;
using SqlWeb.Persistence.SqlServer;

namespace SqlWeb.Persistence
{
    public class ResourceStoreFactory : IResourceStoreFactory
    {
        private readonly IOptions<Options> options;

        public ResourceStoreFactory(IOptions<Options> options)
        {
            this.options = options;
        }
        
        public IResourceStore ResourceStore()
        {
            switch (options.Value.ResourceStoreType.ToLower())
            {
                case "sqlserver":
                    return new SqlServerResourceStore(options.Value.SqlServerConnectionString);
                
                default:
                    return null;
            }
        }
    }
}