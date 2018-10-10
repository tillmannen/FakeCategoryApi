using System;

namespace FakeCategoryApi.Enteties
{
    public class Category {
        
        public Category(Language language, string value, int? score = null)
        {
            this.Language = language;
            this.Value = value;
            this.Timestamp = DateTime.Now;
            this.Score = score ?? 1;
        }

        public Category()
        {
        }

        public Language Language { get; set; }
        public string Value { get; set; }
        public int Score { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
