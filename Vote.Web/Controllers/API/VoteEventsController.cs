namespace Vote.Web.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using Data.Repositories;

    [Route("api/[Controller]")]
    public class VoteEventsController : Controller
    {
        private readonly IVoteEventRepository voteEventRepository;

        public VoteEventsController(IVoteEventRepository voteEventRepository)
        {
            this.voteEventRepository = voteEventRepository;
        }

        [HttpGet]
        public IActionResult GetVoteEvents()
        {
            return Ok(this.voteEventRepository.GetAllWithCandidates());
        }
    }
}