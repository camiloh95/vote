namespace Vote.Web.Controllers.API
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Data.Entities;
    using Helpers;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Data.Repositories;

    [Route("api/[Controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            return Ok(this.voteEventRepository.GetAll());
        }
    }
}