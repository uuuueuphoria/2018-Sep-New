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

//show all albums for U2, order by year, by title
//a demonstration of using the naviational properties to access data on another table
from x in Albums
orderby x.ReleaseYear, x. Title
where x.Artist.Name.Contains("U2")
select x

Albums
   .OrderBy (x => x.ReleaseYear)
   .ThenBy (x => x.Title)
   .Where (x => x.Artist.Name.Contains ("U2"))

//list all jazz tracks by name
from x in Tracks
where x.Genre.Name.Equals("Jazz")
orderby x.Name
select x

//list all tracks for the artist AC/DC
from x in Tracks
where x.Album.Artist.Name.Equals("AC/DC")
select x