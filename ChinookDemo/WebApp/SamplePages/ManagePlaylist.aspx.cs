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
protected void SelectCheckForException(object sender,                                       ObjectDataSourceStatusEventArgs e)
     {          MessageUserControl.HandleDataBoundException(e);
        }
protected void InsertCheckForException(object sender,                                           ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "Album has been added.");
            }
            else      {MessageUserControl.HandleDataBoundException(e);
            }
        }
protected void UpdateCheckForException(object sender,                                        ObjectDataSourceStatusEventArgs e)
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
        protected void DeleteCheckForException(object sender,
                                                ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Success", "Album has been removed.");
            }
            else          {MessageUserControl.HandleDataBoundException(e);
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


          }

        protected void MediaTypeFetch_Click(object sender, EventArgs e)
        {

                //code to go here

        }

        protected void GenreFetch_Click(object sender, EventArgs e)
        {

                //code to go here

        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ArtistName.Text))
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
            //code to go here
 
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