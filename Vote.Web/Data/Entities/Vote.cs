namespace Vote.Web.Data.Entities
{
    using System;

    public class Vote : IEntity
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int CandidateId { get; set; }

        public Candidate Candidate { get; set; }
    }
}
