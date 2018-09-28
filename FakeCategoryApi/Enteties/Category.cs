using System;

namespace FakeCategoryApi.Enteties
{
    public class Category{
        public Language Language { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Score { get; set; }
    }

}
