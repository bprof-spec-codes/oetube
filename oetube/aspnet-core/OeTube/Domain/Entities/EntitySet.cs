using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace OeTube.Entities
{
    public interface IHasAtomicKey<TKey>
        where TKey : notnull
    {
        public TKey AtomicKey { get; }
    }

    public interface IReadOnlyEntitySet<TEntity, TKey> : IReadOnlyCollection<TEntity>
        where TEntity : IEntity, IHasAtomicKey<TKey>
        where TKey : notnull
    {
        TEntity Get(TKey key);
        bool Contains(TKey key);
        bool Remove(TKey key);
    }

    public class EntitySet<TEntity, TKey> : ICollection<TEntity>, IReadOnlyEntitySet<TEntity, TKey>
        where TEntity : IEntity, IHasAtomicKey<TKey>
        where TKey : notnull
    {
        private Dictionary<TKey, TEntity> _dict;
        public bool IsReadOnly => false;
        public int Count => _dict.Count;

        public EntitySet()
        {
            _dict = new Dictionary<TKey, TEntity>();
        }
        public EntitySet(IEnumerable<TEntity> entities) : this()
        {
            foreach (var item in entities)
            {
                Add(item);
            }
        }

        public bool Add(TEntity entity)
        {
            return _dict.TryAdd(entity.AtomicKey, entity);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public bool Contains(TEntity item)
        {
            return _dict.ContainsKey(item.AtomicKey);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            foreach (var item in _dict)
            {
                array[arrayIndex++] = item.Value;
            }
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _dict.Values.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }

        public bool Remove(TEntity item)
        {
            return _dict.Remove(item.AtomicKey);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<TEntity>.Add(TEntity item)
        {
            _dict.TryAdd(item.AtomicKey, item);
        }

        public TEntity Get(TKey key)
        {
            return _dict[key];
        }
    }

}
