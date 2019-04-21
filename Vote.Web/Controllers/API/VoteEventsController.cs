namespace Vote.Web.Controllers.API
{
    using System.Threading.Tasks;
    using Common.Models;
    using Data.Repositories;
    using Data.Entities;
    using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        [Route("GetCandidatesById")]
        public async Task<IActionResult> GetCandidatesByIdAsync([FromBody] Common.Models.VoteEvent request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var candidates = await this.voteEventRepository.GetCandidatesAsync(request.Id);
            if (candidates == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User don't exists."
                });
            }

            return Ok(candidates);
        }

        [HttpPost]
        [Route("SaveVote")]
        public async Task<IActionResult> SaveVote([FromBody] SaveVoteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var entityVote = new Vote
            {
                CandidateId = request.CandidateId,
                UserId = request.UserId
            };

            var newVote = await this.voteEventRepository.CreateVoteAsync(entityVote);
            return Ok(newVote);
        }

        [HttpPost]
        [Route("GetAlreadyVoted")]
        public async Task<IActionResult> GetAlreadyVotedAsync([FromBody] AlreadyVotedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var alreadyVoted = await this.voteEventRepository.GetAlreadyVotedAsync(request.Email, request.VoteEventId);
            return Ok(alreadyVoted);
        }

        [HttpPost]
        [Route("GetVotedCandidate")]
        public async Task<IActionResult> GetVotedCandidateAsync([FromBody] AlreadyVotedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var alreadyVoted = await this.voteEventRepository.GetVotedCandidateAsync(request.Email, request.VoteEventId);
            return Ok(alreadyVoted);
        }
    }
}