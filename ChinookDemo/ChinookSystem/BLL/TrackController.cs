using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#region Additional Namespaces
using ChinookSystem.Entities;
using System.ComponentModel; //expose for ODS configuration
using ChinookSystem.DAL;
using ChinookSystem.ViewModels;
using System.Xml.Schema;
#endregion
namespace ChinookSystem.BLL
{
	[DataObject]
	public class TrackController
	{
		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public List<SongItem> Track_FingByArtist(string artistname)
		{
			using (var context = new ChinookSystemContext())
			{
				var results = from x in context.Tracks
							  where x.Album.Artist.Name.Equals(artistname)
							  orderby x.Name
							  select new SongItem
							  {
								  Song = x.Name,
								  AlbumTitle = x.Album.Title,
								  Year = x.Album.ReleaseYear,
								  Length = x.Milliseconds,
								  Price = x.UnitPrice,
								  Genre = x.Genre.Name
							  };
				return results.ToList();
			}
		}
	}
}
