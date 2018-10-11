using System.Collections.Generic;
using System.Linq;
using FakeCategoryApi.Enteties;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Options;

namespace FakeCategoryApi {
    public interface ICategoryRepository {
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Category> GetAllCategoriesByLanguage(Language language);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private DocumentClient client;
        private DatabaseSettings _databaseSettings;

        public CategoryRepository(IOptions<DatabaseSettings> settings)
        {
            _databaseSettings = settings.Value;
            this.client = new DocumentClient(new Uri(_databaseSettings.EndpointUri), _databaseSettings.PrimaryKey);

            try
            {
                SetUpDatabase();
            }
            catch (DocumentClientException de)
            {
                    Exception baseException = de.GetBaseException();
            }
            catch (Exception e)
            {
                    Exception baseException = e.GetBaseException();
            }
        }

        private async void SetUpDatabase(){
                await client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseSettings.DatabaseName });
                await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseSettings.DatabaseName), new DocumentCollection { Id = _databaseSettings.CategoriesCollectionsName });
        }

        public IEnumerable<Category> GetAllCategories()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            var all = client.CreateDocumentQuery<Category>(UriFactory.CreateDocumentCollectionUri(_databaseSettings.DatabaseName, _databaseSettings.CategoriesCollectionsName), queryOptions);

            return all;
        }

        public IEnumerable<Category> GetAllCategoriesByLanguage(Language language)
        {
            return GetAllCategories().Where(c => c.Language == language);
        }


        private async Task CreateCategoryDocumentIfNotExists(string databaseName, string collectionName, Category category)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, category.Id));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), category);
                }
                else
                {
                    throw;
                }
            }
        }
    }

    // public class FakeCategoryRepository : ICategoryRepository
    // {
    //     List<Category> Categories = new List<Category>(){
    //         new Category (Language.Swedish, "Barn"),
    //         new Category (Language.Swedish, "Barnprogram"),
    //         new Category (Language.Swedish, "Bilar"),
    //         new Category (Language.Swedish, "Brott"),
    //         new Category (Language.Swedish, "Butikskedjor"),
    //         new Category (Language.Swedish, "Böcker"),
    //         new Category (Language.Swedish, "Cirkus"),
    //         new Category (Language.Swedish, "Datorer"),
    //         new Category (Language.Swedish, "Djur"),
    //         new Category (Language.Swedish, "Djur i Afrika"),
    //         new Category (Language.Swedish, "Djur på film"),
    //         new Category (Language.Swedish, "Drycker"),
    //         new Category (Language.Swedish, "Fantasy"),
    //         new Category (Language.Swedish, "Fest"),
    //         new Category (Language.Swedish, "Film"),
    //         new Category (Language.Swedish, "Flyg"),
    //         new Category (Language.Swedish, "Fobier"),
    //         new Category (Language.Swedish, "Frukost"),
    //         new Category (Language.Swedish, "Frukt"),
    //         new Category (Language.Swedish, "Fåglar"),
    //         new Category (Language.Swedish, "Förvaring"),
    //         new Category (Language.Swedish, "Geografi"),
    //         new Category (Language.Swedish, "Grönsaker"),
    //         new Category (Language.Swedish, "Helg"),
    //         new Category (Language.Swedish, "Historia"),
    //         new Category (Language.Swedish, "Hjältar"),
    //         new Category (Language.Swedish, "Hobby"),
    //         new Category (Language.Swedish, "Höst"),
    //         new Category (Language.Swedish, "Internet"),
    //         new Category (Language.Swedish, "Jul"),
    //         new Category (Language.Swedish, "Kalla saker"),
    //         new Category (Language.Swedish, "Kläder"),
    //         new Category (Language.Swedish, "Kroppsdelar"),
    //         new Category (Language.Swedish, "Kulturpersonlighet"),
    //         new Category (Language.Swedish, "Kända duos"),
    //         new Category (Language.Swedish, "Kända musiker"),
    //         new Category (Language.Swedish, "Kända svenskar"),
    //         new Category (Language.Swedish, "Köksredskap"),
    //         new Category (Language.Swedish, "Leksaker"),
    //         new Category (Language.Swedish, "Länder"),
    //         new Category (Language.Swedish, "Mord"),
    //         new Category (Language.Swedish, "Musik"),
    //         new Category (Language.Swedish, "Musikinstrument"),
    //         new Category (Language.Swedish, "Ovanor"),
    //         new Category (Language.Swedish, "Politiker"),
    //         new Category (Language.Swedish, "Påsk"),
    //         new Category (Language.Swedish, "Reptiler"),
    //         new Category (Language.Swedish, "Sci-Fi"),
    //         new Category (Language.Swedish, "Semester"),
    //         new Category (Language.Swedish, "Serier"),
    //         new Category (Language.Swedish, "Sidekicks"),
    //         new Category (Language.Swedish, "Skola"),
    //         new Category (Language.Swedish, "Skräpmat"),
    //         new Category (Language.Swedish, "Skurkar"),
    //         new Category (Language.Swedish, "Smultronställen"),
    //         new Category (Language.Swedish, "Sommar"),
    //         new Category (Language.Swedish, "Spel"),
    //         new Category (Language.Swedish, "Sport"),
    //         new Category (Language.Swedish, "TV-spel"),
    //         new Category (Language.Swedish, "Trådlöst"),
    //         new Category (Language.Swedish, "Under jorden"),
    //         new Category (Language.Swedish, "Vapen"),
    //         new Category (Language.Swedish, "Varumärken"),
    //         new Category (Language.Swedish, "Vinter"),
    //         new Category (Language.Swedish, "Växter"),
    //         new Category (Language.Swedish, "Vår"),
    //         new Category (Language.Swedish, "Vårdutrustning"),

    //         new Category (Language.English, "African animals"),
    //         new Category (Language.English, "Music instruments"),
    //         new Category (Language.English, "Celebreties"),

    //     };

    //     public IEnumerable<Category> GetAllCategories()
    //     {
    //         return Categories;
    //     }

    //     public IEnumerable<Category> GetAllCategoriesByLanguage(Language language)
    //     {
    //         return Categories.Where(c => c.Language == language);
    //     }
    // }   
}