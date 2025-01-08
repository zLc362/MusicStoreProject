using Domain.Models;
using Domain.Models.Enums;

namespace Domain.DTO;

public class PlaylistDetailsDTO
{
    public Guid Id { get; set; }
    public string PlaylistName { get; set; }
    public string CreatedDate { get; set; }
    public PlaylistType PlaylistType { get; set; }
    public string CreatedBy { get; set; }
    public ICollection<Track> Tracks { get; set; }
    public bool IsCreatedByCurrentUser { get; set; }
}