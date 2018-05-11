using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sasa.DataAccess
{
    //IDataService is to allow all entities to have the common method of add remove...
    //interface always start with I
    public interface IDataService<T> where T : class //T is the entity
    {
        void Add(T record); //void coz it returns nothing

        void AddRange(List<T> records);

        void Remove(T record);

        void RemoveRange(List<T> records);

        void Update(T record);

        void UpdateRange(List<T> records);

        //List<T> GetRange(Func<List<T>, bool> predicate); //because it returns a list of the entity

        //T Get(Func<T, bool> predicate); //T because it returns the entity



        T Get(Expression<Func<T, bool>> condition); //Func is the pointer to a method that can return a value. pointer is the 'reference'
                                                    //T inside func is the return parameterb. ool is the parameter only

        List<T> GetRange(Expression<Func<T, bool>> condition);

        List<T> GetRange();
    }
}
