using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ShoppingCart
    {
        public List<Album> Albums { get; set; } = new List<Album>();
        public List<Track> Tracks { get; set; } = new List<Track>();
        public double Total = 0;
    }

}
