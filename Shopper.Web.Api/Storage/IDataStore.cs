using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Storage
{
    public interface IDataStore<T>
    {
        bool TryAdd(T item);
        bool Contains(Guid id);
        T Get(Guid id);
        List<T> GetAll();
        int Count();
        bool Remove(Guid id);
    }
}
