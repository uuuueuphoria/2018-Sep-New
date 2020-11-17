using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Entities;
using System.ComponentModel;
using ChinookSystem.DAL;
using ChinookSystem.ViewModels; //need for wizard implementation of objectDataSource
#endregion

namespace ChinookSystem.BLL
{
    //expose the library class for the wizard
    [DataObject]
    public class ArtistController
    {

        //EXPOSE the class method for the wizard
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SelectionList> Artist_List()
        {
            using (var context=new ChinookSystemContext())
            {
                //due to the fact that the entities will be internal
                //you will not be able to use the entity definations (classes) as the return datatypes
                //instead we will create ViewModel classes that will contain the data defination for your return datatypes

                //to fill these view model classes, we will use Linq queries
                //linq queries return their data as IEnumerable or IQueryable datasets
                //you can use var when declaring your query receiving variables
                //this linq query uses the syntax method for coding
                var results = from x in context.Artists
                              select new SelectionList
                              {
                                  ValueId = x.ArtistId,
                                  DisplayText = x.Name
                              };
                return results.OrderBy(x => x.DisplayText).ToList();
                    
                    //return context.Artists.ToList()  //in 1517;
            }
        }
    }
}
