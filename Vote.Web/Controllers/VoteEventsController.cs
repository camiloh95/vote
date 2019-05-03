namespace Vote.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Entities;
    using Data.Repositories;
    using Helpers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class VoteEventsController : Controller
    {
        private readonly IVoteEventRepository voteEventRepository;

        public VoteEventsController(IVoteEventRepository voteEventRepository, IUserHelper userHelper)
        {
            this.voteEventRepository = voteEventRepository;
        }

        public IActionResult Index()
        {
            return View(this.voteEventRepository.GetAll().OrderBy(p => p.Name));
        }

        public IActionResult IndexResults()
        {
            return View(this.voteEventRepository.GetAll().OrderBy(p => p.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VoteEventNotFound");
            }

            var voteEvent = await this.voteEventRepository.GetByIdAsync(id.Value);
            if (voteEvent == null)
            {
                return new NotFoundViewResult("VoteEventNotFound");
            } else {
                voteEvent.Candidates = await this.voteEventRepository.GetCandidatesByIdAsync(id.Value);
            }

            if (voteEvent.EndDate <= DateTime.Today)
            {
                return View("Results", voteEvent);
            }
            else
            { 
                return View(voteEvent);

            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(VoteEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await this.pathVoteEventCreation(model);
                }

                var eventvote = this.ToVoteEvent(model, path);
                await this.voteEventRepository.CreateAsync(eventvote);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private VoteEvent ToVoteEvent(VoteEventViewModel model, string path)
        {
            return new VoteEvent
            {
                Id = model.Id,
                ImageUrl = path,
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
            };
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("VoteEventNotFound");
            }

            var voteEvent = await this.voteEventRepository.GetByIdAsync(id.Value);
            if (voteEvent == null)
            {
                return new NotFoundViewResult("VoteEventNotFound");
            }
            else {
                voteEvent.Candidates = await this.voteEventRepository.GetCandidatesByIdAsync(id.Value);
            }

            var model = this.ToVoteEventViewModel(voteEvent);
            return View(model);
        }

        private VoteEventViewModel ToVoteEventViewModel(VoteEvent voteEvent)
        {
            return new VoteEventViewModel
            {
                Id = voteEvent.Id,
                ImageUrl = voteEvent.ImageUrl,
                Name = voteEvent.Name,
                StartDate = voteEvent.StartDate,
                EndDate = voteEvent.EndDate,
                Candidates = voteEvent.Candidates,
                Description = voteEvent.Description
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> edit(VoteEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await this.pathVoteEventCreation(model);
                    }

                    var voteEvent = this.ToVoteEvent(model, path);
                    await this.voteEventRepository.UpdateAsync(voteEvent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.voteEventRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private async Task<string> pathVoteEventCreation(VoteEventViewModel model)
        {
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot\\images\\VoteEvents",
                file);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }

            path = $"~/images/VoteEvents/{file}";
            return path;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var voteEvent = await this.voteEventRepository.GetByIdAsync(id.Value);
            if (voteEvent == null)
            {
                return NotFound();
            }

            return View(voteEvent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var voteEvent = await this.voteEventRepository.GetByIdAsync(id);
            await this.voteEventRepository.DeleteAsync(voteEvent);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var candidate = await this.voteEventRepository.GetCandidateByIdAsync(id);
            await this.voteEventRepository.DeleteCandidateAsync(candidate);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult VoteEventNotFound()
        {
            return this.View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CreateCandidate(int id)
        {
            var model = new CandidateViewModel
            {
                VoteEventId = id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createCandidate(CandidateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await this.pathCandidateCreation(model);
                }

                await this.voteEventRepository.CreateCandidateAsync(model, path);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCandidate(int id)
        {
            var candidate = await this.voteEventRepository.GetCandidateByIdAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }
            
            var model = this.ToCandidateViewModel(candidate);
            return View(model);
        }

        private CandidateViewModel ToCandidateViewModel(Candidate candidate)
        {
            return new CandidateViewModel
            {
                Id = candidate.Id,
                Name = candidate.Name,
                Proposal = candidate.Proposal,
                ImageUrl = candidate.ImageUrl,
                VoteEventId = candidate.VoteEventId
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> editCandidate(CandidateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = model.ImageUrl;

                try
                {
                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await this.pathCandidateCreation(model);
                    }
                } catch (DbUpdateConcurrencyException) {
                    if (!await this.voteEventRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    } else {
                        throw;
                    }
                }

                await this.voteEventRepository.UpdateCandidateAsync(model, path);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private async Task<string> pathCandidateCreation(CandidateViewModel model)
        {
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot\\images\\Candidates",
                file);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }

            path = $"~/images/Candidates/{file}";
            return path;
        }
    }
}