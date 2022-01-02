using Microsoft.Extensions.Configuration;
using SchemeEditor.Abstraction.DAL.Services;

namespace SchemeEditor.DAL.Services
{
    public class ConnectionService : IConnectionService<SchemeEditorContext>
    {
        private readonly IConfiguration _config;
        
        public ConnectionService(IConfiguration config) => _config = config;
        
        public SchemeEditorContext GetNewDefaultConnection() 
            => new SchemeEditorContext(_config.GetConnectionString("SchemeEditor"));
    }
}