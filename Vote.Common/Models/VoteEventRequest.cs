namespace Vote.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class VoteEventRequest
    {
        [Required]
        public int VoteEventId { get; set; }
    }
}
