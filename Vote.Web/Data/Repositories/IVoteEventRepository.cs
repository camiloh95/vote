namespace Vote.Web.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vote.Web.Data.Entities;
    using Vote.Web.Models;

    public interface IVoteEventRepository : IGenericRepository<VoteEvent> 
    {
        IQueryable GetAllWithCandidates();

        Task<bool> CreateCandidateAsync(CandidateViewModel model, string path);

        Task<IQueryable<Candidate>> GetCandidatesAsync(int Id);

        Task<Candidate> GetCandidateByIdAsync(int id);

        Task<Candidate> UpdateCandidateAsync(CandidateViewModel model, string imaginePath);

        Task<bool> UpdateTotalVotesAsync(VoteEvent voteEvent);

        Task<Vote> CreateVoteAsync(Vote model);

    }
}
