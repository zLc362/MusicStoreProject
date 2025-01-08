using Domain.Models;

namespace Repository.Interface;

public interface ITrackRepository
{
    public Task<IEnumerable<Track>> GetTracks();
    public Task<Track?> GetTrackById(Guid trackId);
    public Task<IEnumerable<Track>> GetAllTracksByIds(IEnumerable<Guid> trackIds);
    public Task<IEnumerable<Track>> GetTrackByAlbum(Guid albumId);
    public Task<Track> Create(Track track);
    public Task<Track> Update(Track track);
    public Task<Track> Delete(Track track);
}