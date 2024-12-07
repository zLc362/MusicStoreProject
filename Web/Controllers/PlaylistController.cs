using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Repository;
using Service.Interface;

namespace Web.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        // GET: Playlist
        public async Task<IActionResult> Index()
        {
            return View(await _playlistService.GetAll());
        }

        // GET: Playlist/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistService.GetById(id.Value);
            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // GET: Playlist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Playlist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlaylistName,CreatedDate,PlaylistType")] Playlist playlist)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId == null)
            {
                return Redirect("Identity/Account/Login");
            }
            if (ModelState.IsValid)
            {
                await _playlistService.Create(playlist, userId);
                return RedirectToAction(nameof(Index));
            }
            
            return View(playlist);
        }

        // GET: Playlist/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistService.GetById(id.Value);
            if (playlist == null)
            {
                return NotFound();
            }
            return View(playlist);
        }

        // POST: Playlist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,PlaylistName,CreatedDate,PlaylistType,UserId")] Playlist playlist)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId == null)
            {
                return Redirect("Identity/Account/Login");
            }
            
            if (id != playlist.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _playlistService.Update(playlist);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PlaylistExists(playlist.Id))
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
            return View(playlist);
        }

        // GET: Playlist/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistService.GetById(id.Value);
            if (playlist == null)
            {
                return NotFound();
            }

            return View(playlist);
        }

        // POST: Playlist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var playlist = await _playlistService.GetById(id);
            if (playlist != null)
            {
                await _playlistService.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PlaylistExists(Guid id)
        {
            return await _playlistService.GetById(id) != null;
        }
    }
}
