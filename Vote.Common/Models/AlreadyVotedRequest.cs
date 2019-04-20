namespace Vote.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AlreadyVotedRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public int VoteEventId { get; set; }
    }
}
