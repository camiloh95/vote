namespace Vote.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SaveVoteRequest
    {
        [Required]
        public int CandidateId { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
