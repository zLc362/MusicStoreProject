using System.ComponentModel.DataAnnotations;
using Domain.Models.Enums;

namespace Domain.Models;

public class Track
{
    [Required]
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public TimeSpan Duration { get; set; }
    [Required]
    public double Price { get; set; }

    public string? FileUrl { get; set; }
    
    public virtual List<MusicGenre> Genres { get; set; } = new List<MusicGenre>();

    public Guid? AlbumId { get; set; }
    public virtual Album? Album { get; set; }
    
    public Guid ArtistId { get; set; }
    public virtual Artist? Artist { get; set; }
}