namespace Vote.Web.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vote.Web.Data.Entities;
    using Vote.Web.Models;

    public interface IVoteEventRepository : IGenericRepository<VoteEvent> 
    {
        Task<IQueryable> GetAllWithCandidates();

        Task<bool> CreateCandidateAsync(CandidateViewModel model, string path);

        Task<IQueryable<Candidate>> GetCandidatesByIdAsync(int Id);

        Task<Candidate> GetCandidateByIdAsync(int id);

        Task<Candidate> UpdateCandidateAsync(CandidateViewModel model, string imaginePath);

        Task<Vote> CreateVoteAsync(Vote model);

        Task<bool> UpdateTotalVotesAsync(VoteEvent voteEvent);

        Task<bool> GetAlreadyVotedAsync(string email, int votEventId);

        Task<Candidate> GetVotedCandidateAsync(string email, int votEventId);

    }
}
