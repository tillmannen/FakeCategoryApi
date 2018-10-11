using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeCategoryApi.Enteties;
using Microsoft.AspNetCore.Mvc;

namespace FakeCategoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepository _categoryRepository { get; }

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> GetRandom()
        {
            var categories = _categoryRepository.GetAllCategoriesByLanguage(Language.Swedish).ToList();

            Random r = new Random((int)DateTime.Now.Ticks); 
            var index = r.Next(categories.Count() -1);

            return categories[index].Value;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAll(){
            var categories = _categoryRepository.GetAllCategories();

            return categories.Select(c => c.Value).ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllByLanguage(string languageString){
            Enum.TryParse<Language>(languageString, true, out var language);
            var categories = _categoryRepository.GetAllCategoriesByLanguage(language);

            return categories.Select(c => c.Value).ToList();
        }

        // // POST api/values
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }

}
