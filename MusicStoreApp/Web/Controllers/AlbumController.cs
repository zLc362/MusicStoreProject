using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Models.Enums;
using Repository;
using Service.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IArtistService _artistService;
        private readonly IUserService _userService;

        public AlbumController(IAlbumService albumService, IArtistService artistService, IUserService userService)
        {
            _albumService = albumService;
            _artistService = artistService;
            _userService = userService;
        }


        // GET: Album
        public async Task<IActionResult> Index()
        {
            return View(await _albumService.GetAll());
        }

        // GET: Album/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumService.GetById(id.Value);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: Album/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Genres"] = new MultiSelectList(Enum.GetNames<MusicGenre>());
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Album/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Price,Genres,CoverImageUrl,ArtistId")] Album album)
        {
            
            if (ModelState.IsValid)
            {
                await _albumService.Create(album);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Genres"] = new MultiSelectList(Enum.GetNames<MusicGenre>(), album.Genres);
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name", album.ArtistId);
            return View(album);
        }

        // GET: Album/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumService.GetById(id.Value);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["Genres"] = new MultiSelectList(Enum.GetNames<MusicGenre>(), album.Genres);
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name", album.ArtistId);
            return View(album);
        }

        // POST: Album/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,ReleaseDate,Price,Genres,CoverImageUrl,ArtistId")] Album album)
        {
            if (id != album.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _albumService.Update(album);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AlbumExists(album.Id))
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
            ViewData["Genres"] = new MultiSelectList(Enum.GetNames<MusicGenre>(), album.Genres);
            ViewData["ArtistId"] = new SelectList(await _artistService.GetAll(), "Id", "Name", album.ArtistId);
            return View(album);
        }

        // GET: Album/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumService.GetById(id.Value);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Album/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _albumService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AlbumExists(Guid id)
        {
            return await _albumService.GetById(id) != null;
        }
        
        [HttpGet, ActionName("Buy")]
        public async Task<IActionResult> BuyAlbum(Guid? id)
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
            
            var success = await _userService.BuyAlbum(userId, id.Value);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return BadRequest();
        }
    }
}
