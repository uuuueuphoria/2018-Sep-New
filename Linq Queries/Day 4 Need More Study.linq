<Query Kind="Statements">
  <Connection>
    <ID>17f2f030-1e49-4ed8-9803-4ff7836b36a0</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

//.Distinct()
//create a list of customer countries

var distinctResults = (from x in Customers
orderby x.Country
select x.Country).Distinct();

//boolean filters .Any() and .All()
//.Any() method iterates through the entire collection, if any of the items match the specified condition,
//true is returned, boolean filters return NO data, just true or false
//an instance of the collection that receives a true on the condition, is selected for processing

//show genres that have tracks which are not on any playlist
var anyResults = from x in Genres
				where x.Tracks.Any(trk=>trk.PlaylistTracks.Count()==0)
				orderby x.Name
				select new
				{
					genre=x.Name,
					tracksingenre=x.Tracks.Count(),
					broingracks=from y in x.Tracks
								where y.PlaylistTracks.Count()==0
								select y.Name
				};
anyResults.Dump();