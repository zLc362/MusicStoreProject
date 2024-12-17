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
        
        var tracks = await _userService.GetUserTracks(userId);
        Console.WriteLine(tracks);
        var albums = await _userService.GetUserAlbums(userId);
        ViewData["Tracks"] = tracks;
        ViewData["Albums"] = albums;
        return View();
    }
}