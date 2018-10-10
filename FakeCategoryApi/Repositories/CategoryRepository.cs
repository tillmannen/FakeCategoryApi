using System.Collections.Generic;
using System.Linq;
using FakeCategoryApi.Enteties;
using Microsoft.Azure.Storage; // Namespace for StorageAccounts
using Microsoft.Azure.CosmosDB.Table; // Namespace for Table storage types

public interface ICategoryRepository {
    IEnumerable<Category> GetAllCategories();
    IEnumerable<Category> GetAllCategoriesByLanguage(Language language);
}

public class CategoryRepository : ICategoryRepository
{
    private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=afakestorage;AccountKey=p51oHxuvSyfSEOov0vpfTpqPozpqRJcZESh9XxjhOWYRMKjSqUVemnsQNZLYPGUjDXjs2LH7J9vNyCJeXaiJFw==;TableEndpoint=https://afakestorage.table.cosmosdb.azure.com:443/;";
    private CloudStorageAccount _storageAccount;
    private CloudTableClient _tableClient;
    CloudTable _categoriesTable;

    public CategoryRepository()
    {
        _storageAccount = CloudStorageAccount.Parse(connectionString);
        _tableClient = _storageAccount.CreateCloudTableClient();
        _categoriesTable = _tableClient.GetTableReference("categories");
    }
    public IEnumerable<Category> GetAllCategories()
    {
        TableQuery<Category> query = new TableQuery<Category>();

        IEnumerable<Category> results;

        TableContinuationToken token = null;
        do
        {
            TableQuerySegment<Category> resultSegment = _categoriesTable.ExecuteQuerySegmentedAsync(query, token).Result;
            token = resultSegment.ContinuationToken;

            results = resultSegment.Results;
        } while (token != null);

        return results;
    }

    public IEnumerable<Category> GetAllCategoriesByLanguage(Language language)
    {
        TableQuery<Category> query = new TableQuery<Category>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, language.ToString()));

        IEnumerable<Category> results;

        TableContinuationToken token = null;
        do
        {
            TableQuerySegment<Category> resultSegment = _categoriesTable.ExecuteQuerySegmentedAsync(query, token).Result;
            token = resultSegment.ContinuationToken;

            results = resultSegment.Results;
        } while (token != null);

        return results;
    }
}

public class FakeCategoryRepository : ICategoryRepository
{
    List<Category> Categories = new List<Category>(){
        new Category (Language.Swedish, "Djur i Afrika"),
        new Category (Language.Swedish, "Musikinstrument"),
        new Category (Language.Swedish, "Vårdutrustning"),
        new Category (Language.Swedish, "Kulturpersonlighet"),
        new Category (Language.English, "African animals"),
        new Category (Language.English, "Music instruments"),
        new Category (Language.English, "Celebreties")
    };

    public IEnumerable<Category> GetAllCategories()
    {
        return Categories;
    }

    public IEnumerable<Category> GetAllCategoriesByLanguage(Language language)
    {
        return Categories.Where(c => c.Language == language);
    }
}