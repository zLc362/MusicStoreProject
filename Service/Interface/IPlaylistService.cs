using Domain.Models;

namespace Service.Interface;

public interface IPlaylistService
{
    Task<IEnumerable<Playlist>> GetAll();
    Task<Playlist?> GetById(Guid id);
    Task<Playlist> Create(Playlist playlist,string userId);
    Task<Playlist> Update(Playlist playlist);
    Task<Playlist?> Delete(Guid playlistId);
}