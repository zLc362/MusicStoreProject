using Domain.Models;
using Domain.Models.Enums;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class AlbumServiceImpl : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly ITrackService _trackService;

    public AlbumServiceImpl(IAlbumRepository albumRepository, ITrackService trackService)
    {
        _albumRepository = albumRepository;
        _trackService = trackService;
    }

    public async Task<IEnumerable<Album>> GetAll()
    {
        return await _albumRepository.GetAlbums();
    }

    public async Task<Album?> GetById(Guid id)
    {
        return await _albumRepository.GetAlbumById(id);
    }

    public async Task<IEnumerable<Album>> GetAllByIds(IEnumerable<Guid> albumIds)
    {
        return await _albumRepository.GetAllAlbumsByIds(albumIds);
    }

    public async Task<Album> Create(Album album)
    {
        album.Id = Guid.NewGuid();
        return await _albumRepository.Create(album);
    }

    public async Task<Album> Update(Album album)
    {
        return await _albumRepository.Update(album);
    }

    public async Task<Album?> Delete(Guid albumId)
    {
        var album = await GetById(albumId);
        if (album == null)
            return null;
        return await _albumRepository.Delete(album);
    }
}