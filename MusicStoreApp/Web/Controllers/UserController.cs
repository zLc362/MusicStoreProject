using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Library()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        if (userId == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        var userPlaylists = _userService.GetUserPlaylistsInList(userId);
        var tracks = await _userService.GetUserTracks(userId);
        var albums = await _userService.GetUserAlbums(userId);
        ViewData["Playlists"] = userPlaylists;
        ViewData["Tracks"] = tracks;
        ViewData["Albums"] = albums;
        return View();
    }

    public async Task<IActionResult> AddTrackToPlaylist(Guid? trackId, Guid? playlistId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
        if (userId == null || trackId == null || playlistId == null)
        {
            return Redirect("/Identity/Account/Login");
        }

        var result = await _userService.AddTrackToPlaylist(userId, trackId.Value, playlistId.Value);
        if (result)
        {
            return RedirectToAction("Library");
        }

        return BadRequest();
    }
}