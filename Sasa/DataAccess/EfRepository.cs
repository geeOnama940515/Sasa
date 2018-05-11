using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sasa.DataAccess.EF
{
    public class EfRepository:IRepository
    {
        public IDataService<Person> Person { get; } = new EfDataService<Person>();
        public IDataService<Purok> Purok { get; } = new EfDataService<Purok>();
        public IDataService<Accessor> Accessor { get; } = new EfDataService<Accessor>();
        public IDataService<Clearance> Clearance { get; } = new EfDataService<Clearance>(); 
        public IDataService<Household> Household { get; } = new EfDataService<Household>();
        public IDataService<Case> Case { get; } = new EfDataService<Case>();
        public IDataService<Employee> Employee { get; } = new EfDataService<Employee>();
        public IDataService<Livestock> Livestock { get; } = new EfDataService<Livestock>();
    }
}
