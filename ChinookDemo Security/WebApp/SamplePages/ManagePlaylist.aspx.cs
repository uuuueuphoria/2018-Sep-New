using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;
using ChinookSystem.Entities;
using System.Globalization;
using System.Web.Services.Description;
using DMIT2018Common.UserControls;
using System.Runtime.InteropServices;
#endregion

namespace WebApp.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
            if (Request.IsAuthenticated)
            {
                
                //do you have the right to be on this page
                if (User.IsInRole("Customers") && User.IsInRole("Employees"))
                {
                    MessageUserControl.ShowInfo("Security","You are allowed on the page");
                }
                else
                {
                    //not allowed
                    Response.Redirect("~/SamplePages/AccessDenied.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        /// your MessageUsercontrol ODS methods go here
        protected void SelectCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }
        protected void InsertCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "Album has been added.");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        protected void UpdateCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "Album has been updated.");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        protected void DeleteCheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "Album has been removed.");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }


        protected void ArtistFetch_Click(object sender, EventArgs e)
        {

            //validate that data exists if not put out a message
            if (string.IsNullOrEmpty(ArtistName.Text))
            {
                MessageUserControl.ShowInfo("Search entry error", "Enter a artist name or partial artist name, then press your button.");
                SearchArg.Text = "dddfgg";
            }
            TracksBy.Text = "Artist";
            SearchArg.Text = ArtistName.Text;
            //to force the listview to rebind to execute again
            TracksSelectionList.DataBind();
        }


        protected void MediaTypeFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "MediaType";
            SearchArg.Text = MediaTypeDDL.SelectedValue;
            //selected value returns contents as a string
            //no need for valdation
            TracksSelectionList.DataBind();
        }




        protected void GenreFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Genre";
            SearchArg.Text = GenreDDL.SelectedValue;
            //selected value returns contents as a string
            //no need for valdation
            TracksSelectionList.DataBind();

        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(AlbumTitle.Text))
            {
                MessageUserControl.ShowInfo("Search entry error", "Enter a album title or partial album title, then press your button.");
                SearchArg.Text = "dddfgg";
            }
            TracksBy.Text = "Album";
            SearchArg.Text = AlbumTitle.Text;
            //to force the listview to rebind to execute again
        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            //security is yet to be implemented
            //this page needs to know the username of currently logged user
            //temoirarily we will hard code the username
            //string username = "HansenB";
            string username = User.Identity.Name;
            //validate that a string exists in the playlist name
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing data", "enter the playlist name.");
            }
            else
            {
                //how do we do error handling using messageUserControl if the code executing is NOT part of ODS
                //you could use Try/Catch (but we won't)
                //we wish to use MessageUserControl
                //if you examine the source code for MessageUserControl, you will find embedded within the code the Try/Catch
                //The syntax:
                //MessageUserControl.TryRun(()=>{your code block});
                //MessageUserControl.TryRun(()=>{your code block},"Success Title","Success message");
                MessageUserControl.TryRun(() =>
                {
                    //standard lookup

                    //connect to controller
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    //issue the controller call
                    List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                    //assign the results to the control
                    PlayList.DataSource = info;
                    //bind results to control
                    PlayList.DataBind();
                }, "Playlist", "View the current songs on the playlist");

            }

        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            //code to go here
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Track Movement", "You must have a play list name.");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track Movement", "You must have a play list showin g.");
                }
                else
                {
                    //was anything actually selected?
                    CheckBox songSelected = null;
                    int rowsSelected = 0;//count number selected
                    int trackid = 0;//trackid of the song to move
                    int tracknumber = 0;//tracknumber of the song to move
                    //traverse the song list
                    //only 1 song may be selected for movement
                    for (int index = 0; index < PlayList.Rows.Count; index++)
                    {
                        //point to a checkbox on the gridview row
                        songSelected = PlayList.Rows[index].FindControl("Selected") as CheckBox;
                        //selected??
                        if (songSelected.Checked)
                        {
                            rowsSelected++;
                            trackid = int.Parse((PlayList.Rows[index].FindControl("TrackId") as Label).Text);
                            tracknumber = int.Parse((PlayList.Rows[index].FindControl("TrackNumber") as Label).Text);
                        }
                    }
                    if (rowsSelected != 1)
                    {
                        MessageUserControl.ShowInfo("Track Movement", "You must select a single song to move.");
                    }
                    else
                    {
                        //is this the top row?
                        if (tracknumber == PlayList.Rows.Count)
                        {
                            MessageUserControl.ShowInfo("Track Movement", "Song is at the bottom of list already. No move is necessary.");
                        }
                        else
                        {
                            MoveTrack(trackid, tracknumber, "down");
                        }
                    }
                }
            }

        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            //code to go here
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Track Movement", "You must have a play list name.");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track Movement", "You must have a play list showin g.");
                }
                else
                {
                    //was anything actually selected?
                    CheckBox songSelected = null;
                    int rowsSelected = 0;//count number selected
                    int trackid = 0;//trackid of the song to move
                    int tracknumber = 0;//tracknumber of the song to move
                    //traverse the song list
                    //only 1 song may be selected for movement
                    for (int index = 0; index < PlayList.Rows.Count; index++)
                    {
                        //point to a checkbox on the gridview row
                        songSelected = PlayList.Rows[index].FindControl("Selected") as CheckBox;
                        //selected??
                        if (songSelected.Checked)
                        {
                            rowsSelected++;
                            trackid = int.Parse((PlayList.Rows[index].FindControl("TrackId") as Label).Text);
                            tracknumber = int.Parse((PlayList.Rows[index].FindControl("TrackNumber") as Label).Text);
                        }
                    }
                    if (rowsSelected != 1)
                    {
                        MessageUserControl.ShowInfo("Track Movement", "You must select a single song to move.");
                    }
                    else
                    {
                        //is this the top row?
                        if (tracknumber == 1)
                        {
                            MessageUserControl.ShowInfo("Track Movement", "Song is at the top of list already. No move is necessary.");
                        }
                        else
                        {
                            MoveTrack(trackid, tracknumber, "up");
                        }
                    }
                }
            }

        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            //call BLL to move track
            string username = User.Identity.Name;
            MessageUserControl.TryRun(() =>
            {
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                sysmgr.MoveTrack(username, PlaylistName.Text, trackid, tracknumber, direction);
                List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                //assign the results to the control
                PlayList.DataSource = info;
                //bind results to control
                PlayList.DataBind();
            }, "Move Track", "Track has been moved");

        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Track Removal", "You must have a play list name.");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Track Removal", "You must have a play list to view.");
                }
                else
                {
                    //collect the tracks to remove
                    List<int> trackids = new List<int>();
                    int rowSelected = 0;
                    CheckBox trackSelection = null;
                    for (int i = 0; i < PlayList.Rows.Count; i++)
                    {
                        trackSelection = PlayList.Rows[i].FindControl("Selected") as CheckBox;
                        if (trackSelection.Checked)
                        {
                            rowSelected++;
                            trackids.Add(int.Parse((PlayList.Rows[i].FindControl("TrackId") as Label).Text));
                        }
                    }
                    if (rowSelected == 0)
                    {
                        MessageUserControl.ShowInfo("Track Removal", "You must select at least one track to remove");
                    }
                    else
                    {
                        MessageUserControl.TryRun(() =>
                        {
                            string username = User.Identity.Name;
                            PlaylistTracksController sysmgr = new PlaylistTracksController();
                            sysmgr.DeleteTracks(username, PlaylistName.Text, trackids);
                            List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                            PlayList.DataSource = info;
                            PlayList.DataBind();
                        }, "Track Removal", "Selected track has been removed from the play list");
                    }
                }
            }
        }
        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            string username = User.Identity.Name;
            //validation of incoming data
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing Data", "Enter the playlist name");
            }
            else
            {
                //reminder:MessageUserControl will do the error handling
                MessageUserControl.TryRun(() =>
                {
                    //coding block for your logic to be run under the error handling control of MessengeUserControl
                    //a standard add to the database
                    //connect to controller
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    sysmgr.Add_TrackToPLaylist(PlaylistName.Text, username, int.Parse(e.CommandArgument.ToString()));
                    //refresh playlist
                    List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
                    PlayList.DataSource = info;
                    PlayList.DataBind();
                }, "Add track to Playlist", "Track has been added to the playlist");
            }
            
        }

    }
}