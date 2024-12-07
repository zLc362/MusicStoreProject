using Domain.Models;

namespace Service.Interface;

public interface IArtistService
{
    Task<IEnumerable<Artist>> GetAll();
    Task<Artist?> GetById(Guid id);
    Task<Artist> Create(Artist artist);
    Task<Artist> Update(Artist artist);
    Task<Artist?> Delete(Guid artistId);
}