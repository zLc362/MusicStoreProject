using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicStoreAdminApp.Models
{
    public class Artist
    {
        public string Name { get; set; }
        public string? Biography { get; set; }
        public string? Image { get; set; }
    
    }
}
