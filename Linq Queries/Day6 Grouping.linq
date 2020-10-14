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

//grouping

//basically,grouping is the technique of placing a large pile of data into smaller piles of data depending on a criteria
//navigational properties allow for natural grouping of parent to child (pkey/fkey) collections

// pinstance..childnavproperty.Count()   counts all the child records associate with the parent instance

//problem: what if there is no navigational property for the grouping of the data collection
//here you can use the group clause to create a set of smaller collections based on the desired criteria
//it is important to remember that once the smaller groups are created, ALL reporting MUST use the smaller groups as the collection reference

//grouping is NOT same as ORDERING

//syntax
// group instance by criteria [into group reference name]

//the instance is one record from the original pile of data
//the criteria can be:
//                    a) A single attribute ......
//					  b) A multiple attributes new{...,...,.....}
//					  c) A class classname

//if you wish to do processing on the smaller group you will place the grouping results ito a smaller pile of data referenced by a specified name

//groups have 2 components
//  a)key component
//  b)the data component

//report albums by ReleaseYear showing the year and the number of albums for the year. Order by the most albums, then by the year within count

//my process to creating group queries
//a)create and display the grouping
from x in Albums
group x by x.ReleaseYear into gYear
select gYear
//b)create the reporting row for a group
from x in Albums
group x by x.ReleaseYear into gYear
select new
{
	year=gYear.Key,
	albumcount=gYear.Count()
}
//c)complete any additional report customization
from x in Albums
group x by x.ReleaseYear into gYear
orderby gYear.Count() descending, gYear.Key
select new
{
	year=gYear.Key,
	albumcount=gYear.Count()
}

//report albums by ReleaseYear showing the year and the number of albums for the year. Order by the most albums, then by the year within count
//report the album title, artist name and number of album tracks. Report only albums of the 90s
//process of filtering for the 90s can be done a)on the original pile of data   b)on the grouped piles of data
from x in Albums
//where x.ReleaseYear>1989 && x.ReleaseYear<2000  better group earlier if possible
group x by x.ReleaseYear into gYear
where gYear.Key>1989 && gYear.Key<2000
orderby gYear.Count() descending, gYear.Key
select new
{
	year=gYear.Key,
	albumcount=gYear.Count(),
	albumdata= from grow in gYear
				select new
				{
					title=grow.Title,
					artist=grow.Artist.Name,
					trackcount=grow.Tracks.Count(trk=>trk.AlbumId == grow.AlbumId)
				}
}


//list tracks for albums produced after 2010 by Genre name
//Count tracks for the Name
from x in Tracks
where x.Album.ReleaseYear>2010
group x by x.Genre.Name into gTem
orderby gTem.Count()
select new
{
	genre=gTem.Key,
	numberof=gTem.Count()
}

//same report but using the entity as the group criteria, then change select item

//when you group on an entity, the entire entity instance becomes the content of your key value use normal object referencing (dot operator)
from x in Tracks
where x.Album.ReleaseYear>2010
group x by x.Genre into gTem
orderby gTem.Count()
select new
{
	genre=gTem.Key.Name,
	numberof=gTem.Count()
}

//using group techniques, create a list of custoemrs by employee support individual showing the employee name (last, first (phone)); the number of customers for this employee, and a customer list for the employee
//in the customer list show the state, city and customer name (last, first). Order the customer list by state then city
from x in Customers
where x.SupportRep!=null
orderby x.State, x.City
group x by x.SupportRep into gTemp
select new
{
	employeeName=(from em in gTemp
					select em.SupportRep.LastName + ", "+ em.SupportRep.FirstName+"("+em.SupportRep.Phone+")").Distinct(),
	customerNumber=(from em in gTemp
					select em.SupportRep.SupportRepCustomers.Count()).Distinct(),
	customerlist=from em in gTemp
					orderby em.State, em.City
					select new
					{
					state=em.State,
					city=em.City,
					customerName=em.LastName+", "+em.FirstName
					}
}