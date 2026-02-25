using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using ShamsAlShamoos01.Shared.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Repositories
{
    public class DapperGenericRepository : IDapperGenericRepository
    {
        public string ConnectionString { get; set; }

        public DapperGenericRepository(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private SqlConnection CreateConnection() => new(ConnectionString);

        #region Execute
        public void Execute(string sp) => Execute(sp, null, null);
        public void Execute(string sp, object param) => Execute(sp, param, null);
        public void Execute(string sp, object param, int? timeout)
        {
            using var cnn = CreateConnection();
            cnn.Execute(sp, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }

        public Task ExecuteAsync(string sp) => ExecuteAsync(sp, null, null);
        public Task ExecuteAsync(string sp, object param) => ExecuteAsync(sp, param, null);
        public Task ExecuteAsync(string sp, object param, int? timeout)
        {
            using var cnn = CreateConnection();
            return cnn.ExecuteAsync(sp, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
        }
        #endregion

        #region List
        public List<T> List<T>(string sp) => List<T>(sp, null, null);
        public List<T> List<T>(string sp, object param) => List<T>(sp, param, null);
        public List<T> List<T>(string sp, object param, int? timeout)
        {
            using var cnn = CreateConnection();
            return cnn.Query<T>(sp, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout).ToList();
        }

        public async Task<List<T>> ListAsync<T>(string sp) => await ListAsync<T>(sp, null, null);
        public async Task<List<T>> ListAsync<T>(string sp, object param) => await ListAsync<T>(sp, param, null);
        public async Task<List<T>> ListAsync<T>(string sp, object param, int? timeout)
        {
            using var cnn = CreateConnection();
            var result = await cnn.QueryAsync<T>(sp, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            return result.ToList();
        }
        #endregion

        #region Single
        public T Single<T>(string sp) => Single<T>(sp, null, null);
        public T Single<T>(string sp, object param) => Single<T>(sp, param, null);
        public T Single<T>(string sp, object param, int? timeout)
        {
            using var cnn = CreateConnection();
            return cnn.Query<T>(sp, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout).FirstOrDefault();
        }

        public async Task<T> SingleAsync<T>(string sp) => await SingleAsync<T>(sp, null, null);
        public async Task<T> SingleAsync<T>(string sp, object param) => await SingleAsync<T>(sp, param, null);
        public async Task<T> SingleAsync<T>(string sp, object param, int? timeout)
        {
            using var cnn = CreateConnection();
            var result = await cnn.QueryAsync<T>(sp, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout);
            return result.FirstOrDefault();
        }
        #endregion

        #region VartextAll
        public Task<VartextAllModel> ListVartextAllAsync(string sp) => ListVartextAllAsync(sp, null, null);
        public Task<VartextAllModel> ListVartextAllAsync(string sp, object param) => ListVartextAllAsync(sp, param, null);
        public async Task<VartextAllModel> ListVartextAllAsync(string sp, object param, int? timeout)
        {
            using var cnn = CreateConnection();
            using var multi = await cnn.QueryMultipleAsync(sp, param, commandType: CommandType.StoredProcedure, commandTimeout: timeout);

            var model = new VartextAllModel();
            var props = typeof(VartextAllModel).GetProperties();

            foreach (var p in props)
            {
                var list = (await multi.ReadAsync<string>()).ToList();
                p.SetValue(model, list);
            }

            return model;
        }
        #endregion
    }
}
