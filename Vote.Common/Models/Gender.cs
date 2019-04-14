namespace Vote.Common.Models
{
    using Newtonsoft.Json;

    public class Gender
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}