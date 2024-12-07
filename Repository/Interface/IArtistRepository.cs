using Domain.Models;

namespace Repository.Interface;

public interface IArtistRepository
{
    public Task<IEnumerable<Artist>> GetArtists();
    public Task<Artist?> GetArtistById(Guid artistId);
    public Task<Artist> Create(Artist artist);
    public Task<Artist> Update(Artist artist);
    public Task<Artist> Delete(Artist artist);
}