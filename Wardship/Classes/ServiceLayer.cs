
namespace ServiceLayer
{
    public interface IUnitOfWorkDataStore
    {
        object this[string key] { get; set; }
    }
    public static class UnitOfWorkHelper
    {
        public static IUnitOfWorkDataStore CurrentDataStore;
    }
}