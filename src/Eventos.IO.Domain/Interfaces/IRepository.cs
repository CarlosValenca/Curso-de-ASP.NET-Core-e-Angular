using Eventos.IO.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Eventos.IO.Domain.Interfaces
{
    // Qualquer classe que herde de Entity pode trabalhar neste repositório
    // E neste repositório eu posso ter os métodos que eu quiser
    // É considerado um repositório genérico de implementação de todos os comportamentos
    // básicos relacionados a Entity (CRUD)
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity<TEntity>
    {
        void Add(TEntity obj);

        TEntity GetById(Guid id);

        IEnumerable<TEntity> GetAll();

        void Update(TEntity obj);

        void Remove(Guid id);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        int SaveChanges();

    }
}
