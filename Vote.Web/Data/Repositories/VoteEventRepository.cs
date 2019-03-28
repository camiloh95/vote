using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vote.Web.Data.Entities;

namespace Vote.Web.Data.Repositories
{
    public class VoteEventRepository : GenericRepository<VoteEvent>, IVoteEventRepository
    {
        private readonly DataContext context;

        public VoteEventRepository(DataContext context) : base(context)
        {
            this.context = context;
        }
    }
}
