namespace Vote.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
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
        private readonly IUserHelper userHelper;

        public VoteEventsController(IVoteEventRepository voteEventRepository, IUserHelper userHelper)
        {
            this.voteEventRepository = voteEventRepository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
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
            }

            return View(voteEvent);
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
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\VoteEvents",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/VoteEvents/{file}";
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
                return NotFound();
            }

            var voteEvent = await this.voteEventRepository.GetByIdAsync(id.Value);
            if (voteEvent == null)
            {
                return NotFound();
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
                Description = voteEvent.Description,
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VoteEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageUrl;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\VoteEvents",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/VoteEvents/{file}";
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

        public IActionResult VoteEventNotFound()
        {
            return this.View();
        }
    }
}