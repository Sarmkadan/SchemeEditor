using LinqToDB.Data;

namespace SchemeEditor.Abstraction.DAL.Services
{
    public interface IConnectionService<T> where T : DataConnection
    {
        T GetNewDefaultConnection();
    }
}