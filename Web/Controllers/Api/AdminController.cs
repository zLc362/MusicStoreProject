using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Security.Claims;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("[action]")]
        public List<Track> GetAllTracksOfUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            return _userService.GetUserTracksInList(userId);
        }

        [HttpGet("[action]")]
        public List<Track> GetAllAlbumsOfUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            return _userService.GetUserAlbumsInList(userId);
        }

        [HttpGet("[action]")]
        public List<Playlist> GetAllPlaylistsOfUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            return _userService.GetUserPlaylistsInList(userId);
        }
    }
}
