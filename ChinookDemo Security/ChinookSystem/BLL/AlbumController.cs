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
    public class AlbumController
    {
        #region Queries
        public List<AlbumArtist> Album_FindByArtist(int artistId)
        {
            using(var context=new ChinookSystemContext())
            {
                var results = from x in context.Albums
                              where x.ArtistId == artistId
                              select new AlbumArtist
                              {
                                  AlbumId = x.AlbumId,
                                  Title = x.Title,
                                  ArtistId = x.ArtistId,
                                  ReleaseYear = x.ReleaseYear,
                                  ReleaseLabel = x.ReleaseLabel
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<AlbumList> Album_List()
        {
            using (var context = new ChinookSystemContext())
            {
                //return all album records
                var results = from x in context.Albums
                              select new AlbumList
                              {
                                  AlbumId = x.AlbumId,
                                  Title = x.Title,
                                  ArtistId = x.ArtistId,
                                  ReleaseYear = x.ReleaseYear,
                                  ReleaseLabel = x.ReleaseLabel
                              };
                return results.ToList();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public AlbumList Album_FindByID(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {

                //return Album record matching supplied PK or null if not found
                var results = (from x in context.Albums
                               where x.AlbumId == albumid
                               select new AlbumList
                               {
                                   AlbumId = x.AlbumId,
                                   Title = x.Title,
                                   ArtistId = x.ArtistId,
                                   ReleaseYear = x.ReleaseYear,
                                   ReleaseLabel = x.ReleaseLabel
                               }).FirstOrDefault();
                return results;
            }
        }
        #endregion
        #region CRUD methods ADD, Update, Delete
        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public void Album_Add(AlbumList item)
        {
            //ADD DO NOT NEED PRIMARY KEY
            using(var context=new ChinookSystemContext())
            {
                Album addItem = new Album
                {
                    Title = item.Title,
                    ArtistId=item.ArtistId,
                    ReleaseYear=item.ReleaseYear,
                    ReleaseLabel=item.ReleaseLabel
                };
                context.Albums.Add(addItem);
                context.SaveChanges(); //causes entity validation to run
            }
        }
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Album_Update(AlbumList item)
        {
            using (var context = new ChinookSystemContext())
            {

                //moving the data from the external viewmodel instance into an internal instance of the entity
                Album updateItem = new Album
                {
                    //Remember for an update, I need to supply the primary key
                    AlbumId= item.AlbumId, 
                    Title = item.Title,
                    ArtistId = item.ArtistId,
                    ReleaseYear = item.ReleaseYear,
                    ReleaseLabel = item.ReleaseLabel
                };
                context.Entry(updateItem).State=System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Album_Delete(AlbumList item)
        {
           Album_Delete(item.AlbumId);
        }
        public void Album_Delete (int albumid)
        {
            using(var context = new ChinookSystemContext())
            {
                var exists = context.Albums.Find(albumid);
                context.Albums.Remove(exists);
                context.SaveChanges();
            }
        }
        #endregion
    }
}
