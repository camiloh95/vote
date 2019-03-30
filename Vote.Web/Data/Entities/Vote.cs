namespace Vote.Web.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Vote : IEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CandidateId { get; set; }
    }
}
