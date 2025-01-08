using System.ComponentModel.DataAnnotations;
using Domain.Models.Enums;
using Newtonsoft.Json;

namespace Domain.Models;

public class Album
{
    [Required]
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public DateTime ReleaseDate { get; set; }
    [Required]
    public double Price { get; set; }
    
    public List<MusicGenre> Genres { get; set; } = new List<MusicGenre>();

    public string? CoverImageUrl { get; set; } =
        "https://www.huber-online.com/daisy_website_files/_processed_/8/0/csm_no-image_d5c4ab1322.jpg";
    
    [Required]
    public Guid ArtistId { get; set; }
    public Artist? Artist { get; set; }
    [JsonIgnore]
    public ICollection<Track> Tracks { get; set; } = new HashSet<Track>();
}