namespace Vote.Web.Data.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Vote.Web.Helpers;
    using Models;

    public class VoteEventRepository : GenericRepository<VoteEvent>, IVoteEventRepository
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;

        public VoteEventRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this.context = context;
            this.userHelper = userHelper;

        }

        public IQueryable GetAllWithCandidates()
        {
            return this.context.VoteEvents.Include(p => p.Candidates);
        }

        public async Task<bool> CreateCandidateAsync(CandidateViewModel model, string path)
        {
            var candidate = new Candidate
            {
                Name = model.Name,
                Proposal = model.Proposal,
                ImageUrl = path,
                VoteEventId = model.VoteEventId
            };

            this.context.Candidates.Add(candidate);
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<IQueryable<Candidate>> GetCandidatesAsync(int id)
        {
            var voteEvent = await this.GetByIdAsync(id);
            if (voteEvent == null)
            {
                return null;
            }

            return this.context.Candidates
                .Where(o => o.VoteEventId == id)
                .OrderBy(o => o.Name);
        }

        public async Task<IQueryable<Vote>> GetVotesAsync(int id)
        {
            var candidate = await this.GetCandidateByIdAsync(id);
            if (candidate == null)
            {
                return null;
            }

            return this.context.Votes
                .Where(o => o.CandidateId == id);
        }

        public async Task<Candidate> UpdateCandidateAsync(CandidateViewModel model, string imaginePath)
        {
            var candidate = await this.context.Candidates.FindAsync(model.Id);
            if (candidate == null)
            {
                return null;
            }

            candidate.Name = model.Name;
            candidate.Proposal = model.Proposal;
            candidate.ImageUrl = imaginePath;

            this.context.Candidates.Update(candidate);
            await this.context.SaveChangesAsync();
            return candidate;
        }

        public async Task<Candidate> GetCandidateByIdAsync(int id)
        {
            return await this.context.Candidates.AsNoTracking()
                                                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> UpdateTotalVotesAsync(VoteEvent voteEvent)
        {
            foreach (var candidate in voteEvent.Candidates)
            {
                var votes = this.context.Votes.Where(c => c.CandidateId == candidate.Id).Count();

                candidate.VotesResult = votes;

                this.context.Candidates.Update(candidate);
            }
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<Vote> CreateVoteAsync(Vote model)
        {
            this.context.Votes.Add(model);
            await this.context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> GetAlreadyVotedAsync(string email, int voteEventId)
        {
            var user = await this.userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var candidate = await this.context.Candidates.AsNoTracking().AnyAsync(c => c.VoteEventId == voteEventId);

            if(candidate)
            {
                var Vote = await this.context.Votes.AsNoTracking().AnyAsync(v => v.UserId.ToString() == user.Id);
                return Vote;
            }
            return false;
        }

        public async Task<Candidate> GetVotedCandidateAsync(string email, int voteEventId)
        {
            var user = await this.userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var possibleCandidates = this.context.Candidates.Where(c => c.VoteEventId == voteEventId);

            foreach(var candidate in possibleCandidates)
            {
                var vote = this.context.Votes.Where(v => v.CandidateId == candidate.Id)
                                             .Where(v => v.UserId.ToString() == user.Id);
                if (vote.Any())
                {
                    return candidate;
                }
            }
            return null;
        }
    }
}