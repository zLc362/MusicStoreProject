using Domain.Models;

namespace Service.Interface;

public interface IAlbumService
{
    Task<IEnumerable<Album>> GetAll();
    Task<Album?> GetById(Guid id);
    Task<IEnumerable<Album>> GetAllByIds(IEnumerable<Guid> albumIds);
    Task<Album> Create(Album album);
    Task<Album> Update(Album album);
    Task<Album?> Delete(Guid albumId);
}