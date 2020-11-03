using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.ViewModels;
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using System.ComponentModel;
using DMIT2018Common.UserControls;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookSystemContext())
            {

                var results = from x in context.PlaylistTracks
                              where x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username)
                              orderby x.TrackNumber
                              select new UserPlaylistTrack
                              {
                                  TrackID = x.TrackId,
                                  TrackNumber = x.TrackNumber,
                                  TrackName = x.Track.Name,
                                  Milliseconds = x.Track.Milliseconds,
                                  UnitPrice = x.Track.UnitPrice
                              };

                return results.ToList();
            }
        }//eom
        public void Add_TrackToPLaylist(string playlistname, string username, int trackid)
        {
            using (var context = new ChinookSystemContext())
            {
                //the code within this using will be done as a transaction which means there will be ONLY ONE .SaveChanges() within this code. if the .SaveChangs is NOT executed successful, all work within this method will be rollback automatically.
                //transaction
                //query:Playlist to see if list name exists
                //if not
                //create an instance of playlist
                //load with data, stage the instance for adding
                //set the tracknumber to 1
                //if yes, check to see if track already exists on Playlist
                //if found, yes: throw an error (stop processing trx) BUSINESS RULE
                //no: determine the current max tracknumber, increment++

                //create an instance of the PlaylistTrack
                //load with data, stage the instance for adding
                //commit the work via entityframework(ADO.net) to the database
                int tracknumber = 0;
                PlaylistTrack newtrack = null;
                List<string> errors = new List<string>();//use for businessrule exception
                Playlist exists = (from x in context.Playlists
                                   where x.Name.Equals(playlistname) && x.UserName.Equals(username)
                                   select x).FirstOrDefault();
                if (exists == null)
                {
                    exists = new Playlist()
                    {
                        //pkey is an identity int key
                        Name = playlistname,
                        UserName=username
                    };
                    context.Playlists.Add(exists);
                    tracknumber = 1;
                }
                else
                {//does the track already exist on the playlist
                    newtrack = (from x in context.PlaylistTracks
                                where x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username) && x.TrackId == trackid
                                select x).FirstOrDefault();
                    if (newtrack != null)
                    {
                        //throw new Exception("Track already on the playlist. Duplicates not allowed");
                        errors.Add("Track already on the playlist. Duplicates are not allowed");
                    }
                    else
                    {
                        tracknumber= (from x in context.PlaylistTracks
                                      where x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username)
                                      select x.TrackNumber).Max();
                        tracknumber++;
                    }
                    //handle the creation of the PlaylistTrack record
                    //all validation has been passed??
                    if(errors.Count>0)
                    {
                        //no, at least one error was found
                        throw new BusinessRuleException("Adding a Track", errors);
                    }
                    else
                    {
                        //good to go
                    }
                }
             
            }//this ensures that the sql connection closes properly
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookSystemContext())
            {
               //code to go here


            }
        }//eom
    }
}
