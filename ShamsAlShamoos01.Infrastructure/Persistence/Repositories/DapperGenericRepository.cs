using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    // مدل برای نگهداری 20 مجموعه Vartext
    public class VartextAllModel
    {
        public List<string> Vartext01 { get; set; } = new();
        public List<string> Vartext02 { get; set; } = new();
        public List<string> Vartext03 { get; set; } = new();
        public List<string> Vartext04 { get; set; } = new();
        public List<string> Vartext05 { get; set; } = new();
        public List<string> Vartext06 { get; set; } = new();
        public List<string> Vartext07 { get; set; } = new();
        public List<string> Vartext08 { get; set; } = new();
        public List<string> Vartext09 { get; set; } = new();
        public List<string> Vartext10 { get; set; } = new();
        public List<string> Vartext11 { get; set; } = new();
        public List<string> Vartext12 { get; set; } = new();
        public List<string> Vartext13 { get; set; } = new();
        public List<string> Vartext14 { get; set; } = new();
        public List<string> Vartext15 { get; set; } = new();
        public List<string> Vartext16 { get; set; } = new();
        public List<string> Vartext17 { get; set; } = new();
        public List<string> Vartext18 { get; set; } = new();
        public List<string> Vartext19 { get; set; } = new();
        public List<string> Vartext20 { get; set; } = new();
    }

    public interface IDapperGenericRepository
    {
        string ConnectionString { get; set; }

        #region Sync

        void Execute(string name, object param = null, int? commandTimeout = null);
        void QueryExecute(string name, object param = null, int? commandTimeout = null);
        List<T> List<T>(string name, object param = null, int? commandTimeout = null);
        T Single<T>(string name, object param = null, int? commandTimeout = null);

        // اضافه کردن ListFilter برای سازگاری با کد قبلی
        List<T> ListFilter<T>(string name, object param, int? commandTimeout = null);

        (List<string> Vartext01, List<string> Vartext02, List<string> Vartext03, List<string> Vartext04,
         List<string> Vartext05, List<string> Vartext06, List<string> Vartext07, List<string> Vartext08,
         List<string> Vartext09, List<string> Vartext10, List<string> Vartext11, List<string> Vartext12,
         List<string> Vartext13, List<string> Vartext14, List<string> Vartext15, List<string> Vartext16,
         List<string> Vartext17, List<string> Vartext18, List<string> Vartext19, List<string> Vartext20)
         ListVartextAll(string storedProcedure, object param = null, int? commandTimeout = null);

        #endregion

        #region Async

        Task ExecuteAsync(string name, object param = null, int? commandTimeout = null);
        Task QueryExecuteAsync(string name, object param = null, int? commandTimeout = null);
        Task<List<T>> ListAsync<T>(string name, object param = null, int? commandTimeout = null);
        Task<T> SingleAsync<T>(string name, object param = null, int? commandTimeout = null);

        // اضافه کردن ListFilterAsync برای سازگاری
        Task<List<T>> ListFilterAsync<T>(string name, object param, int? commandTimeout = null);

        Task<VartextAllModel> ListVartextAllAsync(string storedProcedure, object param = null, int? commandTimeout = null);

        #endregion
    }

    public class DapperGenericRepository : IDapperGenericRepository
    {
        public string ConnectionString { get; set; }

        public DapperGenericRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #region Sync

        public void Execute(string name, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            cnn.Execute(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
        }

        public void QueryExecute(string name, object param = null, int? commandTimeout = null) => Execute(name, param, commandTimeout);

        public List<T> List<T>(string name, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = cnn.Query<T>(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            return result?.ToList() ?? new List<T>();
        }

        public T Single<T>(string name, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            return cnn.Query<T>(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout).FirstOrDefault();
        }

        public List<T> ListFilter<T>(string name, object param, int? commandTimeout = null)
        {
            // بازگردانی به List معمولی
            return List<T>(name, param, commandTimeout);
        }

        public (List<string> Vartext01, List<string> Vartext02, List<string> Vartext03, List<string> Vartext04,
                List<string> Vartext05, List<string> Vartext06, List<string> Vartext07, List<string> Vartext08,
                List<string> Vartext09, List<string> Vartext10, List<string> Vartext11, List<string> Vartext12,
                List<string> Vartext13, List<string> Vartext14, List<string> Vartext15, List<string> Vartext16,
                List<string> Vartext17, List<string> Vartext18, List<string> Vartext19, List<string> Vartext20)
            ListVartextAll(string storedProcedure, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            using var multi = cnn.QueryMultiple(storedProcedure, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);

            return (
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
        }

        #endregion

        #region Async

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

        public async Task<T> SingleAsync<T>(string name, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var result = await cnn.QueryAsync<T>(name, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);
            return result.FirstOrDefault();
        }

        public async Task<List<T>> ListFilterAsync<T>(string name, object param, int? commandTimeout = null)
        {
            return await ListAsync<T>(name, param, commandTimeout);
        }

        public async Task<VartextAllModel> ListVartextAllAsync(string storedProcedure, object param = null, int? commandTimeout = null)
        {
            using var cnn = new SqlConnection(ConnectionString);
            var multi = await cnn.QueryMultipleAsync(storedProcedure, param, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeout);

            return new VartextAllModel
            {
                Vartext01 = (await multi.ReadAsync<string>()).ToList(),
                Vartext02 = (await multi.ReadAsync<string>()).ToList(),
                Vartext03 = (await multi.ReadAsync<string>()).ToList(),
                Vartext04 = (await multi.ReadAsync<string>()).ToList(),
                Vartext05 = (await multi.ReadAsync<string>()).ToList(),
                Vartext06 = (await multi.ReadAsync<string>()).ToList(),
                Vartext07 = (await multi.ReadAsync<string>()).ToList(),
                Vartext08 = (await multi.ReadAsync<string>()).ToList(),
                Vartext09 = (await multi.ReadAsync<string>()).ToList(),
                Vartext10 = (await multi.ReadAsync<string>()).ToList(),
                Vartext11 = (await multi.ReadAsync<string>()).ToList(),
                Vartext12 = (await multi.ReadAsync<string>()).ToList(),
                Vartext13 = (await multi.ReadAsync<string>()).ToList(),
                Vartext14 = (await multi.ReadAsync<string>()).ToList(),
                Vartext15 = (await multi.ReadAsync<string>()).ToList(),
                Vartext16 = (await multi.ReadAsync<string>()).ToList(),
                Vartext17 = (await multi.ReadAsync<string>()).ToList(),
                Vartext18 = (await multi.ReadAsync<string>()).ToList(),
                Vartext19 = (await multi.ReadAsync<string>()).ToList(),
                Vartext20 = (await multi.ReadAsync<string>()).ToList(),
            };
        }

        #endregion
    }
}
