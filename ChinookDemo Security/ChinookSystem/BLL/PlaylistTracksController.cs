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
                        UserName = username
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
                        tracknumber = (from x in context.PlaylistTracks
                                       where x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username)
                                       select x.TrackNumber).Max();
                        tracknumber++;
                    }
                }

             //create/load/add a playlisttrack
             newtrack = new PlaylistTrack();
             //load of instance data
             //newtrack.PlaylistId = exists.PlaylistId;
             newtrack.TrackId = trackid;
             newtrack.TrackNumber = tracknumber;
             //scenario 1.new playlist
             //scenario 2.existing playlist

             //the solution to both these scenarios is to use navigational properties during the actual .add comand
             //the entityframework will on your behalf ensure that the adding of records to the database will be done in the appropriate order AND add the missing compound primary key value (playlistId) to the child record newtrack.
             exists.PlaylistTracks.Add(newtrack);
                
             //handle the creation of the PlaylistTrack record
             //all validation has been passed??
                    if (errors.Count > 0)
                    {
                        //no, at least one error was found
                        throw new BusinessRuleException("Adding a Track", errors);
                    }
                    else
                    {
                        //good to go
                        //commit all staged work
                        context.SaveChanges();
                    }
                

            }//this ensures that the sql connection closes properly
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 
                //check to see if the playlist exists
                //no:error exception
                //yes:
                ////up:check if on the top
                /////yes:error exception
                /////no:find record above(tracknumber -1)
                /////above record tracknumber modified to tracknumber +1
                /////selected tracknumber modified to tracknumber-1
                ///////down:check if on the bottom
                /////yes:error exception
                /////no:find record below(tracknumber +1)
                /////below record tracknumber modified to tracknumber -1
                /////selected tracknumber modified to tracknumber+1
                //stage record update
                //commit
   
                List<string> errors = new List<string>();//use for businessrule exception
                PlaylistTrack moveTrack = null;
                PlaylistTrack otherTrack = null;
                Playlist exists = (from x in context.Playlists
                                   where x.Name.Equals(playlistname) && x.UserName.Equals(username)
                                   select x).FirstOrDefault();
                if (exists == null)
                {
                    errors.Add("Playlist does not exist.");
                }
                else
                {
                    moveTrack= (from x in context.PlaylistTracks
                                where x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username) && x.TrackId==trackid
                                select x).FirstOrDefault();
                    if (moveTrack == null)
                    {
                        errors.Add("Playlist track does not exist");
                    }
                    else
                    {
                        if (direction.Equals("up"))
                        {
                            //this means the tracknumber of the selected track will decrease
                            //prep for move, check if the track is at the top of the list
                            if (moveTrack.TrackNumber == 1)
                            {
                                errors.Add("Song on play list already at the top.");
                            }
                            else
                            {
                                otherTrack = (from x in context.PlaylistTracks
                                              where x.TrackNumber == (moveTrack.TrackNumber - 1) && x.Playlist.Name.Equals(playlistname) && x.Playlist.UserName.Equals(username)
                                              select x).FirstOrDefault();
                                if (otherTrack == null)
                                {
                                    errors.Add("Missing required other song track record.");
                                }
                                else
                                {
                                    moveTrack.TrackNumber -= 1;
                                    otherTrack.TrackNumber += 1;
                                }
                            }
                        }
                        else
                        {
                            if (moveTrack.TrackNumber == exists.PlaylistTracks.Count)
                            {
                                errors.Add("Song on play list already at the bottom.");
                            }
                            else
                            {//4=>5
                                otherTrack = (from x in context.PlaylistTracks
                                              where x.TrackNumber == (tracknumber + 1)&& x.Playlist.Name.Equals(playlistname)&& x.Playlist.UserName.Equals(username)
                                              select x).FirstOrDefault();
                                if (otherTrack == null)
                                {
                                    errors.Add("Missing required other song track record.");
                                }
                                else
                                {
                                    moveTrack.TrackNumber += 1;
                                    otherTrack.TrackNumber -= 1;
                                }
                            }
                        }
                    }
                }
                if (errors.Count > 0)
                {
                    throw new BusinessRuleException("Move Track", errors);
                }
                else
                {
                    //stage
                    //1)you can stage an update to alter the entire entity(CRUD)
                    //2)You can stage an update to an entity referencing JUST the property to be modified
                    //in this example (b) will be used
                    context.Entry(moveTrack).Property("TrackNumber").IsModified = true;
                    context.Entry(otherTrack).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                    context.SaveChanges();
                }
            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookSystemContext())
            {
                //trx
                //check to see if playlist exists
                //no:error msg
                //yes:
                //create a list of tracks to kept
                //remove the tracks in the incoming list
                //re-sequence the kept tracks
                //commit
                List<string> errors = new List<string>();
                Playlist exists = (from x in context.Playlists
                                   where x.Name.Equals(playlistname) && x.UserName.Equals(username)
                                   select x).FirstOrDefault();
                if (exists == null)
                {
                    errors.Add("Play list does not exist.");
                }
                else
                {
                    //find the songs to keep
                    var trackskept = context.PlaylistTracks
                                   .Where(tr => tr.Playlist.Name.Equals(playlistname) && tr.Playlist.UserName.Equals(username) && !trackstodelete.Any(tod => tod == tr.TrackId))
                                   .OrderBy(tr => tr.TrackNumber)
                                   .Select(tr => tr);
                    //remove the tracks to delete
                    PlaylistTrack item = null;
                    foreach(int deletetrackid in trackstodelete)
                    {
                        item = context.PlaylistTracks
                            .Where(tr => tr.Playlist.Name.Equals(playlistname) && tr.Playlist.UserName.Equals(username) && tr.TrackId == deletetrackid)
                            .Select(tr => tr).FirstOrDefault();
                        if (item != null)
                        {
                            exists.PlaylistTracks.Remove(item);
                        }
                    }
                    //re-sequence
                    int number = 1;
                    foreach(var track in trackskept)
                    {
                        track.TrackNumber = number;
                        context.Entry(track).Property(nameof(PlaylistTrack.TrackNumber)).IsModified = true;
                        number++;
                    }
                    //commit
                    context.SaveChanges();
                }

            }
        }//eom
    }
}
