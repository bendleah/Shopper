using Shopper.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Storage
{

    public interface IDataStore<T>
    {
        List<T> ToList();
        bool TryAdd(T item);
        bool Contains(Guid id);
        T Get(Guid id);
        int Count();
    }

    public class InMemoryDataStore<T> : IDataStore<T> where T : IEntity
    {
        private Dictionary<Guid, T> _items;

        public InMemoryDataStore()
        {
            _items = new Dictionary<Guid, T>();
        }

        public bool Contains(Guid id)
        {
            return _items.ContainsKey(id);
        }

        public bool TryAdd(T item)
        {
            return _items.TryAdd(item.Id, item);
        }

        public List<T> ToList()
        {
            return _items.Values.ToList();
        }

        public T Get(Guid id)
        {
            return _items[id];
        }

        public int Count()
        {
            return _items.Values.Count;
        }

    }
}
