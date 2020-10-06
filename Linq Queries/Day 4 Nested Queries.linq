<Query Kind="Expression">
  <Connection>
    <ID>17f2f030-1e49-4ed8-9803-4ff7836b36a0</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//nested queries
//simply put queries with queries

//list all sales support employees showing their full name (lastname, firstname), their title and the number of customers each support. Order by fullname

//in addition, show a list of the customers for each exployee. Show the customer fullname, city and state
from x in Employees
where x.Title.Contains("Support")
orderby x.LastName, x.FirstName
select new
{
	name=x.LastName + ", " + x.FirstName,
	title=x.Title,
	//clientcount=x.SupportRepCustomers.Count(),
	clientcount=(from y in x.SupportRepCustomers
					select y).Count(),
	//clientlist=from y in x.SupportRepCustomers
	//	orderby y.LastName, y.FirstName
	//	select new
	//	{
	//		name=y.LastName + ", " + y.FirstName,
	//		city=y.City,
	//		state=y.State
	//	}
		clientlist=from y in Customers
		where y.SupportRepId==x.EmployeeId
		orderby y.LastName, y.FirstName
		select new
		{
			name=y.LastName + ", " + y.FirstName,
			city=y.City,
			state=y.State
		}
}

//Create a list of albums showing their title and artist. Show albums with 25 or more tracks only, show the songs on the albums (name and length)

//the inner query create an IEnumberable collection
from x in Albums
where x.Tracks.Count()>=25
orderby x.Title
select new
{
	title=x.Title,
	artist=x.Artist.Name
	songs=(from y in x.Tracks
			orderby y.Name
			select new{
			name=y.Name,
			length=y.Milliseconds})
}

//create a playlist report that shows the playlist name, the number of songs on the playlist, the user name belonging to the playlist and the songs on the playlist with their Genre
from x in Playlists
where x.PlaylistTracks.Count()>15
orderby x.Name
select new
{
	name=x.Name,
	numberofsongs=x.PlaylistTracks.Count(),
	username=x.UserName,
	songs=(from y in x.PlaylistTracks
	        select new{
			track=y.Track.Name,
			genre=y.Track.Genre.Name})
}
	

