namespace Vote.Common.Models
{
    using Newtonsoft.Json;
    using System;

    public partial class Candidate
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("proposal")]
        public string Proposal { get; set; }

        [JsonProperty("imageFullPath")]
        public Uri ImageFullPath { get; set; }

        [JsonProperty("voteEventId")]
        public long VoteEventId { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("voteResult")]
        public long VoteResult { get; set; }
    }
}
