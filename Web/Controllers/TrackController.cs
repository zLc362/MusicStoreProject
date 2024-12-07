using System.Security.Claims;
using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Interface;

namespace Web.Controllers
{
    public class TrackController : Controller
    {
        private readonly ITrackService _trackService;
        private readonly IAlbumService _albumService;
        private readonly IArtistService _artistService;
        private readonly IUserService _userService;

        public TrackController(ITrackService trackService, IArtistService artistService, IAlbumService albumService, IUserService userService)
        {
            _trackService = trackService;
            _artistService = artistService;
            _albumService = albumService;
            _userService = userService;
        }

        // GET: Track
        public async Task<IActionResult> Index()
        {
            return View(await _trackService.GetAll());
        }

        // GET: Track/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _trackService.GetById(id.Value);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Track/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Genres"] = new MultiSelectList(Enum.GetNames<MusicGenre>());
            ViewData["AlbumId"] = new SelectList(await _albumService.GetAll(), "Id", "Title");
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Track/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Duration,Price,FileUrl,Genres,AlbumId,ArtistId")] Track track)
        {
            if (ModelState.IsValid)
            {
                track.Id = Guid.NewGuid();
                await _trackService.Create(track);
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(await _albumService.GetAll(), "Id", "Title", track.AlbumId);
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name", track.ArtistId);
            return View(track);
        }

        // GET: Track/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _trackService.GetById(id.Value);
            if (track == null)
            {
                return NotFound();
            }
            ViewData["Genres"] = new MultiSelectList(Enum.GetNames<MusicGenre>(), track.Genres);
            ViewData["AlbumId"] = new SelectList(await _albumService.GetAll(), "Id", "Title", track.AlbumId);
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name", track.ArtistId);
            return View(track);
        }

        // POST: Track/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Duration,Price,FileUrl,Genres,AlbumId,ArtistId")] Track track)
        {
            if (id != track.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _trackService.Update(track);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TrackExists(track.Id))
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
            ViewData["AlbumId"] = new SelectList(await _albumService.GetAll(), "Id", "Title", track.AlbumId);
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name", track.ArtistId);
            return View(track);
        }

        // GET: Track/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _trackService.GetById(id.Value);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Track/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _trackService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TrackExists(Guid id)
        {
            return await _trackService.GetById(id)!=null;
        }

        [HttpGet, ActionName("Buy")]
        public async Task<IActionResult> BuyTrack(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId == null)
            {
                return Redirect("Identity/Account/Login");
            }

            if (id == null)
            {
                return BadRequest();
            }
            
            var success = await _userService.BuyTrack(userId, id.Value);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }
    }
}
