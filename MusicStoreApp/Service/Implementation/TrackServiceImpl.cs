using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class TrackServiceImpl : ITrackService
{
    private readonly ITrackRepository _trackRepository;

    public TrackServiceImpl(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }

    public async Task<IEnumerable<Track>> GetAll()
    {
        return await _trackRepository.GetTracks();
    }

    public async Task<IEnumerable<Track>> GetAllByAlbum(Guid albumId)
    {
        return await _trackRepository.GetTrackByAlbum(albumId);
    }

    public async Task<IEnumerable<Track>> GetAllByIds(IEnumerable<Guid> trackIds)
    {
        return await _trackRepository.GetAllTracksByIds(trackIds);
    }

    public async Task<Track?> GetById(Guid id)
    {
        return await _trackRepository.GetTrackById(id);
    }

    public async Task<Track> Create(Track track)
    {
        return await _trackRepository.Create(track);
    }

    public async Task<Track> Update(Track track)
    {
        return await _trackRepository.Update(track);
    }

    public async Task<Track?> Delete(Guid trackId)
    {
        var track = await GetById(trackId);
        if (track == null)
            return null;
        return await _trackRepository.Delete(track);
    }
}