using System.Collections;
using Domain.Models;

namespace Service.Interface;

public interface IUserService
{
    public Task<bool> BuyTrack(string userId, Guid trackId);
    public Task<bool> BuyAlbum(string userId, Guid albumId);
    
    public Task<IEnumerable<Track>> GetUserTracks(string userId);

    public List<Track> GetUserTracksInList(string userId);
    public List<Track> GetUserAlbumsInList(string userId);
    public List<Playlist> GetUserPlaylistsInList(string userId);
    public Task<IEnumerable<Album>> GetUserAlbums(string userId);
    public Task<MusicStoreUser> GetUserById(string userId);
}