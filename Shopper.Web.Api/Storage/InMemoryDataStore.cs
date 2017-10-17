using Shopper.Web.Api.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopper.Web.Api.Storage
{
    public class InMemoryDataStore<T> : IDataStore<T> where T : IEntity
    {
        private ConcurrentDictionary<Guid, T> _items;

        public InMemoryDataStore()
        {
            _items = new ConcurrentDictionary<Guid, T>();
        }

        public bool Contains(Guid id)
        {
            return _items.ContainsKey(id);
        }

        public bool TryAdd(T item)
        {
            return _items.TryAdd(item.Id, item);
        }

        public List<T> GetAll()
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

        public bool Remove(Guid id)
        {
            T item = default(T);
            return _items.TryRemove(id, out item);
        }
    }
}
