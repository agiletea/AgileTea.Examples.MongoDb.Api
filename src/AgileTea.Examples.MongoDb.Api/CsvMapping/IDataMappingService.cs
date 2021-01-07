namespace AgileTea.Examples.MongoDb.Api.CsvMapping
{
    public interface IDataMappingService
    {
        IDataMap CreateMappings<T>(T[] items) where T : class, IDataMappable, new();

        T[] CreateMappedObjects<T>(IDataMap mappings) where T : class, IDataMappable, new();
    }
}