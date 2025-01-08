using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Domain.DTO;
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
        private readonly IArtistService _artistService;

        public AdminController(IUserService userService, IArtistService artistService)
        {
            this._userService = userService;
            this._artistService = artistService;
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

        [HttpPost("[action]")]
        public bool ImportAllArtists(List<ArtistImportDTO> model)
        {
            bool status = true;
            foreach (var artist in model)
            {
                    var neww = new Artist
                    {
                        Id = Guid.NewGuid(),
                        Name = artist.Name,
                        Biography = artist.Biography,
                        Image = artist.Image,
                        Albums = new List<Album>(),
                        Tracks = new List<Track>(),
                    };
                    
                    var result = _artistService.Create(neww).Result;
                    if(result == null)
                {
                    status = false;
                }
            }
            return status;       
        }
    }
}

