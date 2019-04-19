namespace Vote.Web.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;

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

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Votes")]
        public int VoteResult { get; set; }
    }
}
