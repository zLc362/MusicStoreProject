using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation;

public class AlbumRepositoryImpl : IAlbumRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Album> _albums;

    public AlbumRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;
        _albums = context.Set<Album>();
    }

    public async Task<IEnumerable<Album>> GetAlbums()
    {
        return await _albums.Include(album=>album.Artist).ToListAsync();
    }

    public async Task<Album?> GetAlbumById(Guid albumId)
    {
        return await _albums.Include(album=>album.Artist).FirstOrDefaultAsync(album => album.Id == albumId);
    }

    public async Task<IEnumerable<Album>> GetAllAlbumsByIds(IEnumerable<Guid> albumIds)
    {
        return await _albums.Include(album => album.Artist).Where(album => albumIds.Contains(album.Id)).ToListAsync();
    }

    public async Task<Album> Create(Album album)
    {
        await _albums.AddAsync(album);
        await _context.SaveChangesAsync();
        return album;
    }

    public async Task<Album> Update(Album album)
    {
        _albums.Update(album);
        await _context.SaveChangesAsync();
        return album;
    }

    public async Task<Album> Delete(Album album)
    {
        _albums.Remove(album);
        await _context.SaveChangesAsync();
        return album;
    }
    
}