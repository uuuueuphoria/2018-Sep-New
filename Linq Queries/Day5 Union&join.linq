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

//union
//will combine 2 or mroe queries into one result
//each query needs to have the same number of columns
//each query should have the same associated data within the column
//each query column needs to be the same datatype between queries

//syntax
//(query1).union(query2).union(queryn).OrderBy(first sort).ThenBy(nth sort)
//sorting is done using the column name from the union

//Generate a report convering all albums showing their title, track count, album price, an average track length
//orderby the number of tracks on the album,, then by album title
//remember datatypes of coluns must match
//sum add up a decimal field, average returns a double
Albums.Count().Dump();
var unionresults=(from x in Albums
					where x.Tracks.Count() > 0
					select new
					{
						title=x.Title,
						trackcount=x.Tracks.Count(),
						albumprice=x.Tracks.Sum(y => y.UnitPrice),
						averagelength=x.Tracks.Average(y => y.Milliseconds/1000.0)
					}).Union(from x in Albums
						where x.Tracks.Count() == 0
						select new
					{
						title=x.Title,
						trackcount=x.Tracks.Count(),
						albumprice=0.00m,
						averagelength=0.0
					}).OrderBy(y=>y.trackcount).ThenBy(y=>y.title);
unionresults.Dump();

//joins

//www.dotnetlearners.com/linq
//avoid joins if there is an acceptable navagational property available
//joins can be jused where navigational property DOES NOT EXIST
//joins can be used between assiciated entities
//scenario fkey<==>pkey

//left side of the join, I use the support data
//right side of the join, I use the processing record collection

//****assume there is NO navigational property between artist and album****

//syntax
//leftside entity join rightside entity on leftside.pkey==rightside.fkey
//supportside join processide on supportkey==processfkey
var joinResults=from supportside in Artists
				join processside in Albums
				on supportside.ArtistId equals processside.ArtistId
				select new
				{
					title=processside.Title,
					year=processside.ReleaseYear,
					label=processside.ReleaseLabel==null? "Unknown" : processside.ReleaseLabel,
					artist=supportside.Name,
					trackcount=processside.Tracks.Count()
				};
joinResults.Dump();