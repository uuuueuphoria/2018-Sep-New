<Query Kind="Program">
  <Connection>
    <ID>17f2f030-1e49-4ed8-9803-4ff7836b36a0</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DeferDatabasePopulation>true</DeferDatabasePopulation>
    <Database>Chinook</Database>
  </Connection>
</Query>

void Main()
{
//change the anonymous datatype to a strongly-typed datatype
//define a class and use it use the new
    var results=BLL_Query("AC/DC");
	results.Dump();
	
}

//use C# program environment, you can define classes and methods
// Define other methods, classes and namespaces here
public class SongItem
{
	public string Song{get;set;}
	public string AlbumTitle{get;set;}
	public int Year{get;set;}
	public int Length{get;set;}
	public decimal Price{get;set;}
	public string Genre{get;set;}	
}

//create a method to simulate the BLL method
public List<SongItem> BLL_Query(string artistname)
{
	var results = from x in Tracks
					where x.Album.Artist.Name.Equals(artistname)
					orderby x.Name
					select new SongItem
					{
						Song=x.Name,
						AlbumTitle=x.Album.Title,
						Year=x.Album.ReleaseYear,
						Length=x.Milliseconds,
						Price=x.UnitPrice,
						Genre=x.Genre.Name
					};
	return results.ToList();
	
}