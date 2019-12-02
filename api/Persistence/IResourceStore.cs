using System.Collections.Generic;
using SqlWeb.Types;

namespace SqlWeb.Persistence
{
    public interface IResourceStore
    {
        Resource GetResource(string resourceId);

        List<Resource> GetAllResources();
    }
}