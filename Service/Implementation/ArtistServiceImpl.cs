using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class ArtistServiceImpl : IArtistService
{
    private readonly IArtistRepository _artistRepository;

    public ArtistServiceImpl(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    public async Task<IEnumerable<Artist>> GetAll()
    {
        return await _artistRepository.GetArtists();
    }

    public async Task<Artist?> GetById(Guid id)
    {
        return await _artistRepository.GetArtistById(id);
    }

    public async Task<Artist> Create(Artist artist)
    {
        return await _artistRepository.Create(artist);
    }

    public async Task<Artist> Update(Artist artist)
    {
        return await _artistRepository.Update(artist);
    }

    public async Task<Artist?> Delete(Guid artistId)
    {
        var artist = await GetById(artistId);
        if (artist == null)
            return null;
        return await _artistRepository.Delete(artist);
    }
    
}