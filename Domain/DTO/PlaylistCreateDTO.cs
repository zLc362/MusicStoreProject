using Domain.Models.Enums;

namespace Domain.DTO;

public class PlaylistCreateDTO
{
    public string PlaylistName { get; set; }
    public PlaylistType PlaylistType { get; set; } 
}