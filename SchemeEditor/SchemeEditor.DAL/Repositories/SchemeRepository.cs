using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB;
using Mapster;
using SchemeEditor.Abstraction.DAL.Repositories;
using SchemeEditor.Abstraction.DAL.Services;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.DAL.Repositories
{
    public class SchemeRepository : ISchemeRepository
    {
        private readonly IConnectionService<SchemeEditorContext> _connectionService;
        private readonly IExecutionContext<User> _executionContext;

        public SchemeRepository(IConnectionService<SchemeEditorContext> connectionService, IExecutionContext<User> executionContext)
        {
	        _connectionService = connectionService;
	        _executionContext = executionContext;
        }

        public async Task<List<Scheme>> List()
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                return await context.Schemes.ToListAsync();
            }
        }

        public async Task<List<Scheme>> List(long userId)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                return await context.Schemes
                    .LoadWith(x => x.Author)
                    .Where(s => s.CreatedBy == userId)
                    .ToListAsync();
            }
        }

        public async Task<Scheme> GetAsync(long schemeId)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                return await context.Schemes.FirstOrDefaultAsync(s => s.Id == schemeId);
            }
        }

        public async Task<Scheme> FindAsync(Expression<Func<Scheme, bool>> predicate)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                return await context.Schemes.FirstOrDefaultAsync(predicate);
            }
        }
        
        public async Task<Scheme> CreateAsync(long userId, Scheme scheme)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                var createdAt = this._executionContext.Now;
                var newScheme = scheme.Adapt<Scheme>();
                newScheme.CreatedAt = createdAt;
                newScheme.ModifiedAt = createdAt;
                newScheme.CreatedBy = userId;
                newScheme.ModifiedBy = userId;
                newScheme.Id = await context.InsertWithInt64IdentityAsync(newScheme);
                return newScheme;
            }
        }

        public async Task<Scheme> UpdateAsync(long userId, Scheme scheme)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                var schemeFind = await context.Schemes.FirstAsync(s => s.Id == scheme.Id);
                
                var schemeUpdate = scheme.Adapt<Scheme>();
                schemeUpdate.CreatedAt = schemeFind.CreatedAt;
                schemeUpdate.CreatedBy = schemeFind.CreatedBy;
                schemeUpdate.ModifiedAt = this._executionContext.Now;
                schemeUpdate.ModifiedBy = userId;
                
                await context.UpdateAsync(schemeUpdate);

                return schemeUpdate;
            }
        }

        public async Task<Scheme> RestoreAsync(long userId, long schemeId)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                var scheme = await context.Schemes.FirstAsync(s => s.Id == schemeId);
                scheme.ModifiedAt = this._executionContext.Now;
                scheme.ModifiedBy = userId;
                scheme.DeletedBy = null;
                scheme.DeletedAt = null;

                await context.UpdateAsync(scheme);

                return scheme;
            }
        }

        public async Task<Scheme> DeleteAsync(long userId, long schemeId)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                var scheme = await context.Schemes.FirstAsync(s => s.Id == schemeId);

                scheme.DeletedAt = this._executionContext.Now;
                scheme.DeletedBy = userId;

                await context.UpdateAsync(scheme);

                return scheme;
            }
        }

        public async Task DeletePermanentAsync(long schemeId)
        {
            using (var context = _connectionService.GetNewDefaultConnection())
            {
                await context.Schemes.DeleteAsync(s => s.Id == schemeId);
            }
        }
    }
}