using ShamsAlShamoos01.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public interface IDapperGenericRepository
    {
        List<T> List<T>(string sp, object param, int? timeout);
        Task<List<T>> ListAsync<T>(string sp, object param, int? timeout);

        T Single<T>(string sp, object param, int? timeout);
        Task<T> SingleAsync<T>(string sp, object param, int? timeout);

        void Execute(string sp, object param, int? timeout);
        Task ExecuteAsync(string sp, object param, int? timeout);

        Task<VartextAllModel> ListVartextAllAsync(string sp, object param, int? timeout);
    }
}
