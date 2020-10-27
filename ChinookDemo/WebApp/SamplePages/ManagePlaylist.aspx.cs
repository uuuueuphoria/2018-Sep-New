﻿using System;
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
#endregion

namespace WebApp.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
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
            string username = "HansenB";
            //validate that a string exists in the playlist name
            if(string.IsNullOrEmpty(PlaylistName.Text))
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
                },"Playlist","View the current songs on the playlist");
                
            }
 
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            //call BLL to move track
 
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            //code to go here
            
        }

    }
}