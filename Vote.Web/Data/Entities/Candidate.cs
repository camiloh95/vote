namespace Vote.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Candidate : IEntity
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Name { get; set; }

        [MaxLength(250, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Proposal { get; set; }

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImageUrl))
                {
                    return null;
                }

                return $"https://camilovoting.azurewebsites.net{this.ImageUrl.Substring(1)}";
            }
        }

        public int VoteEventId { get; set; }

        public VoteEvent VoteEvent { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public int VotesResult { get; set; }
    }
}
