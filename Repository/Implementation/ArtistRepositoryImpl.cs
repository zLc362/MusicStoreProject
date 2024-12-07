using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Implementation;

public class ArtistRepositoryImpl : IArtistRepository
{
    private readonly ApplicationDbContext _context;
    private DbSet<Artist> _artists;

    public ArtistRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;
        _artists = context.Set<Artist>();
    }

    public async Task<IEnumerable<Artist>> GetArtists()
    {
        return await _artists.ToListAsync();
    }

    public async Task<Artist?> GetArtistById(Guid artistId)
    {
        return await _artists.FindAsync(artistId);
    }

    public async Task<Artist> Create(Artist artist)
    {
        await _artists.AddAsync(artist);
        await _context.SaveChangesAsync();
        return artist;
    }

    public async Task<Artist> Update(Artist artist)
    {
        _artists.Update(artist);
        await _context.SaveChangesAsync();
        return artist;
    }

    public async Task<Artist> Delete(Artist artist)
    {
        _artists.Remove(artist);
        await _context.SaveChangesAsync();
        return artist;
    }
}