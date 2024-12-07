using Domain.Models;

namespace Repository.Interface;

public interface IPlaylistRepository
{
    public Task<IEnumerable<Playlist>> GetPlaylists();
    public Task<Playlist?> GetPlaylistById(Guid playlistId);
    public Task<Playlist> Create(Playlist playlist);
    public Task<Playlist> Update(Playlist playlist);
    public Task<Playlist> Delete(Playlist playlist);
}