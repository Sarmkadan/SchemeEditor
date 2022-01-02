using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Abstraction.DAL.Repositories
{
    public interface ISchemeRepository
    {
        Task<List<Scheme>> List();
        Task<List<Scheme>> List(long userId);
        Task<Scheme> GetAsync(long schemeId);
        Task<Scheme> FindAsync(Expression<Func<Scheme, bool>> predicate);
        Task<Scheme> CreateAsync(long userId, Scheme scheme);
        Task<Scheme> UpdateAsync(long userId, Scheme scheme);
        Task<Scheme> RestoreAsync(long userId, long schemeId);
        Task<Scheme> DeleteAsync(long userId, long schemeId);
        Task DeletePermanentAsync(long schemeId);
    }
}