using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation;

public class PlaylistRepositoryImpl : IPlaylistRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Playlist> _playlists;

    public PlaylistRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;
        _playlists = context.Set<Playlist>();
    }

    public async Task<IEnumerable<Playlist>> GetPlaylists()
    {
        return await _playlists.Include(playlist => playlist.User)
            .Include(playlist => playlist.Tracks)
            .ThenInclude(track => track.Artist)
            .ToListAsync();
    }

    public async Task<Playlist?> GetPlaylistById(Guid playlistId)
    {
        return await _playlists.Include(playlist => playlist.User)
            .Include(playlist => playlist.Tracks)
            .ThenInclude(track => track.Artist)
            .FirstOrDefaultAsync(playlist => playlist.Id == playlistId);
    }

    public async Task<Playlist> Create(Playlist playlist)
    {
        await _playlists.AddAsync(playlist);
        await _context.SaveChangesAsync();
        return playlist;
    }

    public async Task<Playlist> Update(Playlist playlist)
    {
        _playlists.Update(playlist);
        await _context.SaveChangesAsync();
        return playlist;
    }

    public async Task<Playlist> Delete(Playlist playlist)
    {
        _playlists.Remove(playlist);
        await _context.SaveChangesAsync();
        return playlist;
    }

    public async Task<IEnumerable<Playlist>> GetUserPlaylists(string userId)
    {
        return await _playlists.Include(playlist => playlist.User).Where(playlist => playlist.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Playlist>> GetAllPlaylistsByIds(IEnumerable<Guid> playListIds)
    {
        return await _playlists.Include(playlist => playlist.Tracks)
            .Where(playlist => playListIds.Contains(playlist.Id)).ToListAsync();
    }
}