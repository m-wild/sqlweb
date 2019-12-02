namespace SqlWeb.Persistence
{
    public interface IResourceStoreFactory
    {
        IResourceStore ResourceStore();
    }
}