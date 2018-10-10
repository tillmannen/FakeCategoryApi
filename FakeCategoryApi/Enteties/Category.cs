using System;
using Newtonsoft.Json;

namespace FakeCategoryApi.Enteties
{
    public class Category {
        
        public Category(Language language, string value, int? score = null)
        {
            this.Language = language;
            this.Value = value;
            this.Timestamp = DateTime.Now;
            this.Score = score ?? 1;

            this.Id = Value + "_" + Language.ToString();
        }

        public Category()
        {
        }

        [JsonProperty(PropertyName="id")]
        public string Id { get; set; }
        public Language Language { get; set; }
        public string Value { get; set; }
        public int Score { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
