using System.Collections.Generic;
using System.Threading.Tasks;
using SchemeEditor.Abstraction.Application.Models;

namespace SchemeEditor.Abstraction.Application.Services
{
    public interface ISchemeService
    {
        Task<SchemeView> GetAsync(long schemeId);
        Task<SchemeView> CreateAsync(long userId, SchemeView model);
        // Task<SchemeView> CreateDraftScheme(long userId);
        Task<SchemeView> UpdateAsync(long userId, SchemeView model);
        Task<SchemeView> RestoreAsync(long userId, long schemeId);
        Task<SchemeView> DeleteAsync(long userId, long schemeId);
        Task DeletePermanentAsync(long schemeId);
        Task<List<SchemeView>> List();
        Task<List<SchemeView>> List(long userId);
    }
}