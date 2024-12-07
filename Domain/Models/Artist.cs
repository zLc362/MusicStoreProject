using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Artist
{
    [Required]
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Biography { get; set; } = String.Empty;
    
    public virtual ICollection<Album> Albums { get; set; } = new HashSet<Album>();
    public virtual ICollection<Track> Tracks { get; set; } = new HashSet<Track>();
}