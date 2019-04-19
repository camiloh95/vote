    namespace Vote.Web.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.EntityFrameworkCore;
    using Helpers;
    using Models;

    public class VoteEventRepository : GenericRepository<VoteEvent>, IVoteEventRepository
    {
        private readonly DataContext context;

        public VoteEventRepository(DataContext context) : base(context)
        {
            this.context = context;
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

        public async Task<int> GetTotalVotesAsync(int id)
        {
            return await this.context.Votes.Where(v => v.Id == id).CountAsync();
        }

        public async Task<bool> UpdateTotalVotesAsync(VoteEvent voteEvent)
        {
            foreach (var candidate in voteEvent.Candidates)
            {
                var votes = this.context.Votes.Where(c => c.CandidateId == candidate.Id).Count();

                candidate.VoteResult = votes;

                this.context.Candidates.Update(candidate);
            }
            await this.context.SaveChangesAsync();
            return true;
        }
    }
}
