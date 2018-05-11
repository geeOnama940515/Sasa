using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sasa.DataAccess.EF;

namespace Sasa.DataAccess
{
    public class EfDataService<T> : IDataService<T> where T : class, new()
    {
        public void Add(T record)
        {
            using (var context = new BarangaySasaContext())
            {
                context.Entry(record).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void AddRange(List<T> records)
        {
            using (var context = new BarangaySasaContext())
            {
                foreach (var record in records)
                {
                    context.Entry(record).State = EntityState.Added;
                }
                context.SaveChanges();
            }
        }

        public void Remove(T record)
        {
            using (var context = new BarangaySasaContext())
            {
                context.Entry(record).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void RemoveRange(List<T> records)
        {
            using (var context = new BarangaySasaContext())
            {
                foreach (var record in records)
                {
                    context.Entry(record).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }

        public void Update(T record)
        {
            using (var context = new BarangaySasaContext())
            {
                context.Entry(record).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void UpdateRange(List<T> records)
        {
            using (var context = new BarangaySasaContext())
            {
                foreach (var record in records)
                {
                    context.Entry(record).State = EntityState.Modified;
                }
                context.SaveChanges();
            }
        }

        public T Get(Expression<Func<T, bool>> condition)
        {
            using (var context = new BarangaySasaContext())
            {
                var record = context.Set<T>().FirstOrDefault(condition);
                return record;
            }


        }

        public List<T> GetRange(Expression<Func<T, bool>> condition)
        {
            using (var context = new BarangaySasaContext())
            {
                var records = context.Set<T>().Where(condition).ToList();
                return records;
            }
        }

        public List<T> GetRange()
        {
            using (var context = new BarangaySasaContext())
            {
                var records = context.Set<T>().ToList();
                return records;
            }
        }
    }
}
