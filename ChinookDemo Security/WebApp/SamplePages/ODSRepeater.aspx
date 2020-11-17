<%@ Page Title="ODS Repeater" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ODSRepeater.aspx.cs" Inherits="WebApp.SamplePages.ODSRepeater" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Repeater using ODS with nested query</h1>
    <div class="row">
        <div class="col-12">
            <uc1:messageusercontrol runat="server" id="MessageUserControl" />
        </div>
    </div>
    <%--Set up the parameter search input area--%>
    <div class="offset-1">
        <asp:Label ID="Label1" runat="server" Text="Enter the lowest playlist track size desired: "></asp:Label>&nbsp;&nbsp;
        <asp:TextBox ID="PlayListSizeArg" runat="server"
            TextMode="Number"
            step="1"
            min="0"
            placeholder="0"
            ToolTip="Enter the desired playlist minimum size"></asp:TextBox>
        <asp:Button ID="Fetch" runat="server" Text="Fetch" OnClick="Button1_Click" CssClass="btn btn-primary" />
    </div>

    <div class="row">
        <div class="offset-3">
            <%-- item type will alow to select the defination of the object that the data is using (classname)
                if you use ItemType, you can use Item. in referencing your properties as you develop your control--%>
            <asp:Repeater ID="ClientPlayList" runat="server" DataSourceID="ClientPlayListODS" ItemType="ChinookSystem.ViewModels.PlayListItem">
                <HeaderTemplate>
                <h3>Client Playlist</h3>
                </HeaderTemplate>
                <ItemTemplate>
                    <%-- flat info on ODS --%>
                    <h4>PlayList Name:<%# Item.Name %>(<%#Item.TrackCount %>)</h4> 
                    <br />
                    <h5>Owner: <%#Item.UserName %></h5>
                    <%-- the internal list on each record in the ODS --%>
                    <%-- gridview --%>
                    <asp:GridView ID="PlayListSong" runat="server" DataSource="<%#Item.Songs %>" CssClass="table"></asp:GridView>
                    <%--<asp:ListView ID="PlayListSongs" runat="server"DataSource="<%#Item.Songs %>" ItemType="ChinookSystem.ViewModels.PlayListSong">
                        <ItemTemplate>
                         <span style="background-color:silver">
                             <%#Item.Song %> &nbsp;&nbsp;(<%#Item.GenreName %>)
                             <br />
                         </span>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <span style="background-color:gold">
                             <%#Item.Song %> &nbsp;&nbsp;(<%#Item.GenreName %>)
                         </span>
                        </AlternatingItemTemplate>
                        <LayoutTemplate>
                            <span runat="server" id="itemPlaceHolder">
                            </span><br />
                        </LayoutTemplate>
                    </asp:ListView>--%>
                    <table>
                    <asp:Repeater ID="PlayListSongs" runat="server" DataSource="<%#Item.Songs %>" ItemType="ChinookSystem.ViewModels.PlayListSong">
                        <ItemTemplate>
                            <tr>
                                <td style="background-color:pink">
                                    <%#Item.Song %> 
                                </td>
                                <td style="background-color:pink">(<%# Item.GenreName %>)</td>
                                </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr>
                                <td style="background-color:silver">
                                    <%#Item.Song %> 
                                </td>
                           
                                <td style="background-color:silver">(<%# Item.GenreName %>)</td>
                                </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                         </table>
                </ItemTemplate>
                <SeparatorTemplate>
                    <hr style="height:5px;" />
                </SeparatorTemplate>
            </asp:Repeater>
           
        </div>
    </div>
    <%-- ODS CONTROLS --%>
    <asp:ObjectDataSource ID="ClientPlayListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="PlayList_GetPlayListOfSize" OnSelected="SelectCheckForException" TypeName="ChinookSystem.BLL.PlayListController">

        <SelectParameters>
            <asp:ControlParameter ControlID="PlayListSizeArg" PropertyName="Text" DefaultValue="0" Name="lowestplaylistsize" Type="Int32"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
