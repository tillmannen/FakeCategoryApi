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
            return new List<Category>();
        }

        public IEnumerable<Category> GetAllCategoriesByLanguage(Language language)
        {
            return new FakeCategoryRepository().GetAllCategoriesByLanguage(language);
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
}