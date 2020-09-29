using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.ViewModels
{
    public class AlbumArtist
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; } 
        //DO NOT NEED, FROM DROPDOWN
        //due to the fact that this view will be used in a demonstration of a dropdownlist in a gridview loaded by ODS
        //the artistId will be returned
        public int ReleaseYear { get; set; }
        public string ReleaseLabel { get; set; }
    }
}
