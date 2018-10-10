using System.Collections.Generic;
using System.Linq;
using FakeCategoryApi.Enteties;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;

public interface ICategoryRepository {
    IEnumerable<Category> GetAllCategories();
    IEnumerable<Category> GetAllCategoriesByLanguage(Language language);
}

public class CategoryRepository : ICategoryRepository
{
    private const string EndpointUri = "https://afakestorage.table.cosmosdb.azure.com:443/";
    private const string PrimaryKey = "p51oHxuvSyfSEOov0vpfTpqPozpqRJcZESh9XxjhOWYRMKjSqUVemnsQNZLYPGUjDXjs2LH7J9vNyCJeXaiJFw==";
    private const string DatabaseName = "GameDb";
    private const string CategoriesCollectionsName = "collections";
    private DocumentClient client;


    public CategoryRepository()
    {
        this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
    }

    private async void SetUpDatabase(){
        await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = DatabaseName });
        await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseName), new DocumentCollection { Id = CategoriesCollectionsName });
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return new List<Category>();
    }

    public IEnumerable<Category> GetAllCategoriesByLanguage(Language language)
    {
        return new List<Category>();
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