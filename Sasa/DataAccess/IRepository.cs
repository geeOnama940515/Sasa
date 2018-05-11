using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sasa.DataAccess.EF;

namespace Sasa.DataAccess
{
    public interface IRepository
    {
        IDataService<Person> Person { get; } 
        IDataService<Purok> Purok { get; } 
        IDataService<Accessor> Accessor { get; } 
       
        IDataService<Household> Household { get; } 
        IDataService<Case> Case { get; } 
        IDataService<Employee> Employee { get; } 
        IDataService<Livestock> Livestock { get; } 
        IDataService<Clearance> Clearance { get; } 
    }
}
