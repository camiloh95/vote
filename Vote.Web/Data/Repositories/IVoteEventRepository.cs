using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vote.Web.Data.Entities;

namespace Vote.Web.Data.Repositories
{
    public interface IVoteEventRepository : IGenericRepository<VoteEvent> 
    {
    }
}
