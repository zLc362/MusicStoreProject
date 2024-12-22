using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Service.Interface;

namespace Service.Implementation;

public class UserServiceImpl : IUserService
{
    private readonly ITrackService _trackService;
    private readonly IAlbumService _albumService;
    private readonly IPlaylistService _playlistService;
    private readonly UserManager<MusicStoreUser> _userManager;

    public UserServiceImpl(ITrackService trackService, UserManager<MusicStoreUser> userManager, IAlbumService albumService, IPlaylistService playlistService)
    {
        _trackService = trackService;
        _userManager = userManager;
        _albumService = albumService;
        _playlistService = playlistService;
    }

    public async Task<bool> BuyTrack(string userId, Guid trackId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var track = await _trackService.GetById(trackId);

        if (user == null || track == null)
        {
            return false;
        }

        if (!user.PurchasedItems.Contains(trackId))
        {
            user.PurchasedItems.Add(trackId);
        }
        await _userManager.UpdateAsync(user);
        
        return true;
    }
    public async Task<bool> BuyAlbum(string userId, Guid albumId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var album = await _albumService.GetById(albumId);

        if (user == null || album == null)
        {
            return false;
        }

        if (!user.PurchasedItems.Contains(albumId))
        {
            user.PurchasedItems.Add(albumId);
        }
        await _userManager.UpdateAsync(user);
        
        return true;
    }

    public async Task<IEnumerable<Track>> GetUserTracks(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new List<Track>();
        }
        return await _trackService.GetAllByIds(user.PurchasedItems);
    }


    public List<Track> GetUserTracksInList(string userId)
    {
        var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
        if (user == null)
        {
            return new List<Track>();
        }
        var tracks = _trackService.GetAllByIds(user.PurchasedItems).GetAwaiter().GetResult();
        return tracks.ToList();
    }


    public async Task<IEnumerable<Album>> GetUserAlbums(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new List<Album>();
        }
        return await _albumService.GetAllByIds(user.PurchasedItems);
    }

    public async Task<IEnumerable<Album>> GetUserAlbumsInList(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new List<Album>();
        }
        var albums = _albumService.GetAllByIds(user.PurchasedItems).GetAwaiter().GetResult();
        return albums.ToList();
    }

    public List<Playlist> GetUserPlaylistsInList(string userId)
    {
        var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
        if (user == null)
        {
            return new List<Playlist>();
        }
        var tracks = user.Playlists.ToList();
        return tracks;
    }
}