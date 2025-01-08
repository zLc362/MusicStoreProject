using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class PlaylistServiceImpl : IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistServiceImpl(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<IEnumerable<Playlist>> GetAll()
    {
        return await _playlistRepository.GetPlaylists();
    }

    public async Task<Playlist?> GetById(Guid id)
    {
        return await _playlistRepository.GetPlaylistById(id);
    }

    public async Task<Playlist> Create(Playlist playlist, string userId)
    {
        playlist.Id = Guid.NewGuid();
        playlist.UserId = userId;
        playlist.CreatedDate = DateTime.Now;
        return await _playlistRepository.Create(playlist);
    }

    public async Task<Playlist> Update(Playlist playlist)
    {
        return await _playlistRepository.Update(playlist);
    }

    public async Task<Playlist?> Delete(Guid playlistId)
    {
        var playlist = await GetById(playlistId);
        if (playlist == null)
            return null;
        return await _playlistRepository.Delete(playlist);
    }

    public async Task<IEnumerable<Playlist>> GetUserPlaylists(string userId)
    {
        return await _playlistRepository.GetUserPlaylists(userId);
    }

    public async Task<IEnumerable<Playlist>> GetAllByIds(IEnumerable<Guid> playListIds)
    {
        return await _playlistRepository.GetAllPlaylistsByIds(playListIds);
    }
}