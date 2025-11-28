
#region "This"





#region "This"

using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public interface IDapperGenericRepository
    {
        string ConnectionString { get; set; }
        void Execute(string name, object param = null, int? commandTimeout = null);
        List<T> ListFilter<T>(string name, object param, int? commandTimeout = null);
        List<T> List<T>(string name, int? commandTimeout = null);
        List<T> List<T>(string name, int id, int? commandTimeout = null);
        List<T> List<T>(string name, object param, int? commandTimeout = null);
        Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> List<T1, T2, T3>(string name, object param, int? commandTimeout = null);
        // 🟩 اضافه کن:
        (List<string> Vartext01, List<string> Vartext02, List<string> Vartext03, List<string> Vartext04,
         List<string> Vartext05, List<string> Vartext06, List<string> Vartext07, List<string> Vartext08,
         List<string> Vartext09, List<string> Vartext10, List<string> Vartext11, List<string> Vartext12,
         List<string> Vartext13, List<string> Vartext14, List<string> Vartext15, List<string> Vartext16,
         List<string> Vartext17, List<string> Vartext18, List<string> Vartext19, List<string> Vartext20)
         ListVartextAll(string storedProcedure, object param = null, int? commandTimeout = null);

        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string name, object param, int? commandTimeout = null);
        void QueryExecute(string name, object param = null, int? commandTimeout = null);
        T Single<T>(string name, int id, int? commandTimeout = null);
        T Single<T>(string name, object param, int? commandTimeout = null);
        Task<List<T>> ListFilterAsync<T>(string name, object param, int? commandTimeout = null);

    }



    public class DapperGenericRepository : IDapperGenericRepository
    {
        public string ConnectionString { get; set; }

        public DapperGenericRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // -------------------------
        // Synchronous implementations
        // -------------------------
        public void Execute(string name, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            cnn.Execute(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
        }

        public void QueryExecute(string name, object param = null, int? commandTimeout = null) => Execute(name, param, commandTimeout);

        public List<T> List<T>(string name, object param, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = cnn.Query<T>(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            return result?.ToList() ?? new List<T>();
        }

        public List<T> List<T>(string name, int? commandTimeout = null) => List<T>(name, null, commandTimeout);

        public List<T> List<T>(string name, int id, int? commandTimeout = null) => List<T>(name, new { id }, commandTimeout);

        public List<T> ListFilter<T>(string name, object param, int? commandTimeout = null) => List<T>(name, param, commandTimeout);

        public T Single<T>(string name, object param, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            return cnn.Query<T>(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout).FirstOrDefault();
        }

        public T Single<T>(string name, int id, int? commandTimeout = null) => Single<T>(name, new { id }, commandTimeout);

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string name, object param, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = cnn.QueryMultiple(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(result.Read<T1>().ToList(), result.Read<T2>().ToList());
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> List<T1, T2, T3>(string name, object param, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = cnn.QueryMultiple(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(
                result.Read<T1>().ToList(), result.Read<T2>().ToList(), result.Read<T3>().ToList());
        }
        public (List<string> Vartext01, List<string> Vartext02, List<string> Vartext03, List<string> Vartext04,
        List<string> Vartext05, List<string> Vartext06, List<string> Vartext07, List<string> Vartext08,
        List<string> Vartext09, List<string> Vartext10, List<string> Vartext11, List<string> Vartext12,
        List<string> Vartext13, List<string> Vartext14, List<string> Vartext15, List<string> Vartext16,
        List<string> Vartext17, List<string> Vartext18, List<string> Vartext19, List<string> Vartext20)
    ListVartextAll(string storedProcedure, object param = null, int? commandTimeout = null)
{
    using var cnn = new SqlConnection(ConnectionString);
    cnn.Open();

    using var multi = cnn.QueryMultiple(storedProcedure, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);

    var result = (
        Vartext01: multi.Read<string>().ToList(),
        Vartext02: multi.Read<string>().ToList(),
        Vartext03: multi.Read<string>().ToList(),
        Vartext04: multi.Read<string>().ToList(),
        Vartext05: multi.Read<string>().ToList(),
        Vartext06: multi.Read<string>().ToList(),
        Vartext07: multi.Read<string>().ToList(),
        Vartext08: multi.Read<string>().ToList(),
        Vartext09: multi.Read<string>().ToList(),
        Vartext10: multi.Read<string>().ToList(),
        Vartext11: multi.Read<string>().ToList(),
        Vartext12: multi.Read<string>().ToList(),
        Vartext13: multi.Read<string>().ToList(),
        Vartext14: multi.Read<string>().ToList(),
        Vartext15: multi.Read<string>().ToList(),
        Vartext16: multi.Read<string>().ToList(),
        Vartext17: multi.Read<string>().ToList(),
        Vartext18: multi.Read<string>().ToList(),
        Vartext19: multi.Read<string>().ToList(),
        Vartext20: multi.Read<string>().ToList()
    );

    return result;
}

        // -------------------------
        // Async implementations
        // -------------------------
        public async Task ExecuteAsync(string name, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            await cnn.ExecuteAsync(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
        }

        public async Task QueryExecuteAsync(string name, object param = null, int? commandTimeout = null) => await ExecuteAsync(name, param, commandTimeout);

        public async Task<List<T>> ListAsync<T>(string name, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = await cnn.QueryAsync<T>(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            return result?.ToList() ?? new List<T>();
        }

        public async Task<T> SingleAsync<T>(string name, object param, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = await cnn.QueryAsync<T>(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            return result.FirstOrDefault();
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> ListAsync<T1, T2>(string name, object param, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = await cnn.QueryMultipleAsync(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            var item1 = (await result.ReadAsync<T1>()).ToList();
            var item2 = (await result.ReadAsync<T2>()).ToList();
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
        }

        public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> ListAsync<T1, T2, T3>(string name, object param, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = await cnn.QueryMultipleAsync(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            var item1 = (await result.ReadAsync<T1>()).ToList();
            var item2 = (await result.ReadAsync<T2>()).ToList();
            var item3 = (await result.ReadAsync<T3>()).ToList();
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
        }

        public async Task<List<T>> ListFilterAsync<T>(string name, object param, int? commandTimeout = null) => await ListAsync<T>(name, param, commandTimeout);
    }













}

#endregion

#endregion

 

 


