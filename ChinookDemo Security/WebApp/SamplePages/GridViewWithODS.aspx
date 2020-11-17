<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GridViewWithODS.aspx.cs" Inherits="WebApp.SamplePages.GridViewWithODS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>GridView using ODS</h1>
    <div class="row">
        <div class="offset-1">
            <asp:GridView ID="AlbumList" runat="server" AutoGenerateColumns="False" DataSourceID="AlbumListODS" AllowPaging="True" Caption="List of Artists">

                <Columns>
                    <asp:BoundField DataField="AlbumId" HeaderText="AlbumId" SortExpression="AlbumId"></asp:BoundField>
                    <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title"></asp:BoundField>
                    <asp:BoundField DataField="ArtistId" HeaderText="Artist" SortExpression="ArtistId"></asp:BoundField>
                    <asp:BoundField DataField="ReleaseYear" HeaderText="Year" SortExpression="ReleaseYear"></asp:BoundField>
                    <asp:BoundField DataField="ReleaseLabel" HeaderText="Label" SortExpression="ReleaseLabel"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="AlbumListODS" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="Album_List" TypeName="ChinookSystem.BLL.AlbumController">

            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
