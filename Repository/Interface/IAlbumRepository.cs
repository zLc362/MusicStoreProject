using Domain.Models;

namespace Repository.Interface;

public interface IAlbumRepository
{
    public Task<IEnumerable<Album>> GetAlbums();
    public Task<Album?> GetAlbumById(Guid albumId);
    public Task<IEnumerable<Album>> GetAllAlbumsByIds(IEnumerable<Guid> albumIds);
    public Task<Album> Create(Album album);
    public Task<Album> Update(Album album);
    public Task<Album> Delete(Album album);
}