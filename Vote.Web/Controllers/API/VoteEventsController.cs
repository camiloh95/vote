namespace Vote.Web.Controllers.API
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Data.Repositories;
    using Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
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
        public async Task<IActionResult> GetUserByEmail([FromBody] VoteEvent request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.voteEventRepository.GetCandidatesAsync(request.Id);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "User don't exists."
                });
            }

            return Ok(user);
        }
    }
}