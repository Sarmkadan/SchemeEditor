using LinqToDB;
using LinqToDB.Data;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.DAL
{
    public class SchemeEditorContext : DataConnection
    {
	    public SchemeEditorContext(string connectionString)
		    : base(ProviderName.PostgreSQL95, connectionString)
	    {
	    }

        public ITable<Scheme> Schemes => GetTable<Scheme>();
        public ITable<User> Users => GetTable<User>();
        public ITable<Role> Roles => GetTable<Role>();
        public ITable<UserRole> UserRoles => GetTable<UserRole>();
        public ITable<Message> Messages => GetTable<Message>();
        public ITable<MessageUser> UserMessages => GetTable<MessageUser>();
    }
}