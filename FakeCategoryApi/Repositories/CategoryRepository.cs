using System;
using System.Collections.Generic;
using System.Linq;
using FakeCategoryApi.Enteties;

public interface ICategoryRepository {
    IEnumerable<Category> GetAllCategories();
    IEnumerable<Category> GetAllCategoriesByLanguage(Language language);
}

public class CategoryRepository : ICategoryRepository
{
    public IEnumerable<Category> GetAllCategories()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Category> GetAllCategoriesByLanguage(Language language)
    {
        throw new System.NotImplementedException();
    }
}

public class FakeCategoryRepository : ICategoryRepository
{
    List<Category> Categories = new List<Category>(){
        new Category { Language = Language.Swedish, Value = "Djur i Afrika", CreatedDateTime = DateTime.Now, Score = 1 },
        new Category { Language = Language.Swedish, Value = "Musikinstrument", CreatedDateTime = DateTime.Now, Score = 1 },
        new Category { Language = Language.Swedish, Value = "Vårdutrustning", CreatedDateTime = DateTime.Now, Score = 1 },
        new Category { Language = Language.Swedish, Value = "Kulturpersonlighet", CreatedDateTime = DateTime.Now, Score = 1 },
        new Category { Language = Language.English, Value = "African animals", CreatedDateTime = DateTime.Now, Score = 1 },
        new Category { Language = Language.English, Value = "Music instruments", CreatedDateTime = DateTime.Now, Score = 1 },
        new Category { Language = Language.English, Value = "Celebreties", CreatedDateTime = DateTime.Now, Score = 1 },
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