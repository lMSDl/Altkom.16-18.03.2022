using Models;
using Services.Bogus.Fakers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Bogus
{
    public class CrudService<T> : ICrudService<T> where T : Entity
    {
        protected ICollection<T> _entities;

        public CrudService(EntityFaker<T> faker)
        {
            _entities = faker.Generate(100);
        }

        public Task<T> CreateAsync(T entity)
        {
            entity.Id = _entities.Max(x => x.Id) + 1;
            _entities.Add(entity);
            return Task.FromResult(entity);
        }

        public async Task DeleteAsync(int id)
        {
            _entities.Remove(await ReadAsync(id));
        }

        public Task<IEnumerable<T>> ReadAsync()
        {
            return Task.FromResult(_entities.AsEnumerable());
        }

        public Task<T> ReadAsync(int id)
        {
            return Task.FromResult( _entities.SingleOrDefault(x => x.Id == id));
        }

        public async Task UpdateAsync(int id, T entity)
        {
            await DeleteAsync(id);
            entity.Id = id;
            _entities.Add(entity);
        }
    }
}
