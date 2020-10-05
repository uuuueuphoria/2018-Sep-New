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

//aggregrates
//.Count(), .Sum(), .Max(), .Min(), .Average()
//.Sum(), .Max(), .Min(), .Average() require a delegate expression

//query syntax
//  (from ...
//		....
//   ). Max()


//method syntax
//    collection.Max(x=>x.collectionfield)
//collectionfield could also  be a calculation
//  .Sum(x=>x.quantity*x.price)

//IMPORTANT!! aggregrates work only on a collectionn of data
//not on a single row
//a collection can have 0, 1 or more rows

//the delegate of .Sum(), .Max(), .Min(), .Average() cannot be null

//.Count() does not need a delegate, it counts occurances

//bad example of using aggregate, aggregate is against a single row
from x in Tracks //x is a single, not collection, cannot use avg
select new
	{
		Name=x.Name,
		AvgLength=x.Average(x=>x.Milliseconds)
	}
	
	
(from x in Tracks 
select x.Milliseconds
).Average()   //query syntax

//ok, the list of all milliseconds in Tracks
//is created then the aggrregrate is applied

Tracks.Average(x=>x.Milliseconds)   //method syntax


//List all albums showing the title, artist name and very aggregate values
//for albums containing tracks. For each albums, show number of tracks, the longest track length, the shortest track length, the total price of the tracks, and the average track length

from x in Albums
where x.Tracks.Count()>1
select new
{
	title=x.Title,
	artist=x.Artist.Name,
	trackCount=x.Tracks.Count(),
	//querytrackcount=(from y in x.Tracks select x).Count(),
	querytrackcount=(from y in Tracks
						where x.AlbumId==y.AlbumId
						select x).Count(),
	AvgLength=x.Tracks.Average(y=>y.Milliseconds),
	MaxLength=x.Tracks.Max(y=>y.Milliseconds),
	MinLength=x.Tracks.Min(y=>y.Milliseconds),
	AlbumPrice=x.Tracks.Sum(y=>y.UnitPrice),
	BadPrice=x.Tracks.Count() * 0.99
}
