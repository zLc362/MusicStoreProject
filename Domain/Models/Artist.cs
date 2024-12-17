using Newtonsoft.Json;
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

    [JsonIgnore]
    public virtual ICollection<Album> Albums { get; set; } = new HashSet<Album>();
    [JsonIgnore]
    public virtual ICollection<Track> Tracks { get; set; } = new HashSet<Track>();
}