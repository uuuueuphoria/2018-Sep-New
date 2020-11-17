﻿using System;
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
    public class PlayListController
    {
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<PlayListItem> PlayList_GetPlayListOfSize(int lowestplaylistsize)
        {
            using(var context=new ChinookSystemContext())
            {
				var results = from x in context.Playlists
							  orderby x.UserName
							  where x.PlaylistTracks.Count() >= lowestplaylistsize
							  select new PlayListItem
							  {
								  Name = x.Name,
								  TrackCount = x.PlaylistTracks.Count(),
								  UserName = x.UserName,
								  Songs = (from y in x.PlaylistTracks
										   orderby y.Track.Genre.Name, y.Track.Name
										   select new PlayListSong
										   {
											   Song = y.Track.Name,
											   GenreName = y.Track.Genre.Name
										   })
							  };
				return results.ToList();
			}
        }
    }
}
