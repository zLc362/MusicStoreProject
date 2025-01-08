using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Models.Enums;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Repository;
using Service.Interface;

namespace Web.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IUserService _userService;

        public PlaylistController(IPlaylistService playlistService, IUserService userService)
        {
            _playlistService = playlistService;
            _userService = userService;
        }

        // GET: Playlist
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var userPlaylists = new List<Playlist>();
            if (userId != null)
            {
                userPlaylists = _userService.GetUserPlaylistsInList(userId);
            }

            var allPlaylists = await _playlistService.GetAll();
            var publicPlaylists = allPlaylists.Where(p => p.PlaylistType == PlaylistType.Public && p.UserId != userId);
            ViewBag.UserPlaylists = userPlaylists;
            ViewBag.PublicPlaylists = publicPlaylists;
            return View();
        }

        // GET: Playlist/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (id == null)
            {
                return NotFound();
            }

            var playlist = await _playlistService.GetById(id.Value);
            if (playlist == null)
            {
                return NotFound();
            }

            var playlistDetailsDto = new PlaylistDetailsDTO
            {
                Id = playlist.Id,
                PlaylistName = playlist.PlaylistName,
                PlaylistType = playlist.PlaylistType,
                CreatedDate = playlist.CreatedDate.ToShortDateString(),
                CreatedBy = playlist.User?.FirstName + " " + playlist.User?.LastName,
                Tracks = playlist.Tracks,
                IsCreatedByCurrentUser = playlist.User?.Id == userId,
            };
            return View(playlistDetailsDto);
        }

        // GET: Playlist/Create
        public IActionResult Create()
        {
            ViewData["PlaylistTypes"] = new MultiSelectList(Enum.GetNames<PlaylistType>());
            return View();
        }

        // POST: Playlist/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaylistName,PlaylistType")] PlaylistCreateDTO playlistCreateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId == null)
            {
                return Redirect("Identity/Account/Login");
            }

            var playlist = new Playlist
            {
                PlaylistName = playlistCreateDto.PlaylistName,
                PlaylistType = playlistCreateDto.PlaylistType,
            };

            if (ModelState.IsValid)
            {
                await _playlistService.Create(playlist, userId);
                return RedirectToAction(nameof(Index));
            }

            ViewData["PlaylistTypes"] = new MultiSelectList(Enum.GetNames<PlaylistType>());
            return View();
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

            var playlistEditDto = new PlaylistEditDTO
            {
                Id = playlist.Id,
                PlaylistName = playlist.PlaylistName,
                PlaylistType = playlist.PlaylistType,
            };
            ViewData["PlaylistTypes"] = new MultiSelectList(Enum.GetNames<PlaylistType>());
            return View(playlistEditDto);
        }

        // POST: Playlist/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Id,PlaylistName,PlaylistType")] PlaylistEditDTO playlistEditDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId == null)
            {
                return Redirect("Identity/Account/Login");
            }

            if (id != playlistEditDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var playlist = await _playlistService.GetById(id);
                if (playlist == null)
                    return NotFound();
                playlist.PlaylistName = playlistEditDto.PlaylistName;
                playlist.PlaylistType = playlistEditDto.PlaylistType;
                await _playlistService.Update(playlist);
                return RedirectToAction(nameof(Index));
            }

            ViewData["PlaylistTypes"] = new MultiSelectList(Enum.GetNames<PlaylistType>());
            return View(playlistEditDto);
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

        public async Task<IActionResult> RemoveTrackFromPlaylist(Guid? playlistId, Guid? trackId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            if (userId == null)
            {
                return Redirect("Identity/Account/Login");
            }

            if (playlistId == null || trackId == null)
            {
                return BadRequest("Playlist and track are required");
            }

            var playlist = await _playlistService.GetById(playlistId.Value);
            if (playlist == null)
            {
                return NotFound("Playlist not found");
            }

            var track = playlist.Tracks.FirstOrDefault(t => t.Id == trackId.Value);
            if (track == null)
            {
                return NotFound("Track not found in playlist");
            }

            playlist.Tracks.Remove(track);
            await _playlistService.Update(playlist);
            return RedirectToAction(nameof(Details), new { id = playlistId });
        }
    }
}