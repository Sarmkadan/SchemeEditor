using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using SchemeEditor.Abstraction.Application.Models;
using SchemeEditor.Abstraction.Application.Services;
using SchemeEditor.Abstraction.DAL.Repositories;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Application.Services
{
    public class SchemeService : ISchemeService
    {
        private readonly ISchemeRepository _schemeRepository;
        private readonly IExecutionContext<User> _executionContext;
        private readonly IAccountService<User, Role> _accountService;

        public SchemeService(ISchemeRepository schemeRepository, IExecutionContext<User> executionContext, IAccountService<User, Role> accountService)
        {
	        _schemeRepository = schemeRepository;
	        _executionContext = executionContext;
	        _accountService = accountService;
        }

        public async Task<List<SchemeView>> List() =>
            (await _schemeRepository.List()).Adapt<List<SchemeView>>();

        public async Task<List<SchemeView>> List(long userId) =>
            (await _schemeRepository.List(userId)).Adapt<List<SchemeView>>();

        public async Task<SchemeView> GetAsync(long schemeId) => 
            (await _schemeRepository.GetAsync(schemeId))?.Adapt<SchemeView>();

        public async Task<SchemeView> CreateAsync(long userId, SchemeView model)
        {
	        var scheme = await _schemeRepository.FindAsync(s => s.CreatedBy == userId 
	                                                            && s.CreatedAt == s.ModifiedAt
	                                                            && string.IsNullOrEmpty(s.Body)
	                                                            && !s.DeletedAt.HasValue);
	        //return scheme != null 
		       // ? scheme.Adapt<SchemeView>() 
		       // : (await _schemeRepository.CreateAsync(userId, model.Adapt<Scheme>())).Adapt<SchemeView>();
               return (await _schemeRepository.CreateAsync(userId, model.Adapt<Scheme>())).Adapt<SchemeView>();
        }

        public async Task<SchemeView> CreateDraftScheme(long userId)
        {
            var scheme = await _schemeRepository.FindAsync(s => s.CreatedBy == userId 
                                                                && s.CreatedAt == s.ModifiedAt
                                                                && string.IsNullOrEmpty(s.Body)
                                                                && !s.DeletedAt.HasValue);
            return scheme != null 
                ? scheme.Adapt<SchemeView>() 
                : (await _schemeRepository.CreateAsync(userId, new Scheme {Name = "Черновик"})).Adapt<SchemeView>();
        }
        
        public async Task<SchemeView> UpdateAsync(long userId, SchemeView scheme) =>
            (await _schemeRepository.UpdateAsync(userId, scheme.Adapt<Scheme>())).Adapt<SchemeView>();

        public async Task<SchemeView> RestoreAsync(long userId, long schemeId) =>
            (await _schemeRepository.RestoreAsync(userId, schemeId)).Adapt<SchemeView>();

        public async Task<SchemeView> DeleteAsync(long schemeId, long userId) =>
            (await _schemeRepository.DeleteAsync(schemeId, userId)).Adapt<SchemeView>();

        public async Task DeletePermanentAsync(long schemeId) =>
            await _schemeRepository.DeletePermanentAsync(schemeId);
    }
}