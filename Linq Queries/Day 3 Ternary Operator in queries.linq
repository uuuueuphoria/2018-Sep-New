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

//conditional statement using if
if(condition)
{
	true path complex logic
}
else
{
	false path complex logic
}
//ternary operator
condition(s)/true value : false value

//nested ternary operator
conition(s)? 
	(condition(s)? true value : false value)
	: (condition(s) ? true value : 
	(condition(s) ? true value : false value))
	
//list all albums by release label. any album with no label should be indicated as unknown, list title and label
var results=from x in Albums
	orderby x.ReleaseLabel
	select new 
	{
		title=x.Title,
		label=x.ReleaseLabel != null ? x.ReleaseLabel : "Unknown"
	};
results.Dump();

//list all albums showing their title, artistName, and decade (oldies, 70's, 80's, 90'x, modern), orderby artist
var albumResult=from x in Albums
	orderby x.Artist.Name
	select new
	{
		title=x.Title,
		artist=x.Artist.Name,
		decade=x.ReleaseYear<1970? "oldies" : 
				x.ReleaseYear<1980? "70's" : 
				x.ReleaseYear<1990? "80's" : 
				x.ReleaseYear<2000? "90's" : "modern"
	};
albumResult.Dump();


//example of using multiple queries to answer a question, 1. find the average, pre-processing 2. using 1's result
//list all tracks indicating whether they are longer, shorter or equal to the average of all tracks lengths
var resultaverage = Tracks.Average (x=> x.Milliseconds);
var trackResult=from x in Tracks
	select new
	{
		name=x.Name,
		length=x.Milliseconds>resultaverage? "longer" : 
				x.Milliseconds==resultaverage? "equal" : "shorter",
		ActualLength=x.Milliseconds
	};
resultaverage.Dump();
trackResult.Dump();