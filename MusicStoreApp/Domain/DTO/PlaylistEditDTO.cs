using Domain.Models.Enums;

namespace Domain.DTO;

public class PlaylistEditDTO
{
    public Guid Id { get; set; }
    public string PlaylistName { get; set; }
    public PlaylistType PlaylistType { get; set; } 
}