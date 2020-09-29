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

//connents are entered as C# comments

//hotkeys for comments control + k, c
//control + k u   uncomment

//there are 2 styles of coding linq queries
//Query syntax (very sql-ish)
//method syntax (very C#-ish)

//in the expression environment you can code multiple quieries, but you must highlight the query to execute

//in the statement environment you can code multiple queries as C# statements an run the entire physical file without highlighting the query

//in program enviromnment you can code multiple queries and class definations or programs methods which are tested in a main() program


//Query syntax expression, from clause is 1st and select clause is last
from x in Albums
orderby x.Title ascending
select x

//method syntax
Albums.OrderBy(x=>x.Title)

from x in Albums
orderby x.ReleaseYear descending, x.Title
select x


Albums
	.OrderByDescending(x=>x.ReleaseYear)
	.ThenBy(x=>x.Title)
	
	//filtering of data
//where clause
//list artists with a Q in their name
from x in Artists
where x.Name.Contains("Q")
select x

//show all albums released in the 90's
from x in Albums
where x.ReleaseYear>1989 && x.ReleaseYear<2000
select x


Albums
   .Where (x => ((x.ReleaseYear > 1989) && (x.ReleaseYear < 2000)))
   
//list all customers in alphabeti corder by last name who live in the USA, the customer must have an yahoo email
from x in Customers
where x.Country.Equals("USA") && x.Email.Contains("yahoo") //=="USA"
orderby x.LastName
select x