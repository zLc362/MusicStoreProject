using System.Collections;
using Domain.Models;

namespace Service.Interface;

public interface ITrackService
{
    Task<IEnumerable<Track>> GetAll();
    Task<IEnumerable<Track>> GetAllByAlbum(Guid albumId);
    Task<IEnumerable<Track>> GetAllByIds(IEnumerable<Guid> trackIds);
    Task<Track?> GetById(Guid id);
    Task<Track> Create(Track track);
    Task<Track> Update(Track track);
    Task<Track?> Delete(Guid trackId);
}