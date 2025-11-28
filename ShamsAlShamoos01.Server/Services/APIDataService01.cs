

using ShamsAlShamoos01.Shared.Models;
using System.Collections.Concurrent;

namespace ShamsAlShamoos01.Server.Services
{
    public class APIDataService01
    {
        // Generic thread-safe cache for lists
        private readonly ConcurrentDictionary<(string userId, string key), object> _listCache = new();

        // Generic thread-safe cache for single strings
        private readonly ConcurrentDictionary<string, string> _stringCache = new();

        // ----------------- Generic methods for lists -----------------
        public void SetList<T>(string userId, string key, List<T> data)
            => _listCache[(userId, key)] = data;

        public List<T> GetList<T>(string userId, string key)
            => _listCache.TryGetValue((userId, key), out var data) && data is List<T> list ? list : new List<T>();

        // ----------------- Generic methods for strings -----------------
        public void SetString(string userId, string key, string value)
            => _stringCache[userId + key] = value;

        public string GetString(string userId, string key)
            => _stringCache.TryGetValue(userId + key, out var value) ? value : string.Empty;

        // ----------------- Convenience Methods -----------------






        // HistoryRegisterKala01_Update
        public void SetHistoryRegisterKala01_Update(string userId, List<HistoryRegisterKala01ViewModel_Update> data)
            => SetList(userId, nameof(HistoryRegisterKala01ViewModel_Update), data);
        public List<HistoryRegisterKala01ViewModel_Update> GetHistoryRegisterKala01_Update(string userId)
            => GetList<HistoryRegisterKala01ViewModel_Update>(userId, nameof(HistoryRegisterKala01ViewModel_Update));

        // HistoryRegisterKala01
        public void SetHistoryRegisterKala01(string userId, List<HistoryRegisterKala01ViewModelcat> data)
            => SetList(userId, nameof(HistoryRegisterKala01ViewModelcat), data);
        public List<HistoryRegisterKala01ViewModelcat> GetHistoryRegisterKala01(string userId)
            => GetList<HistoryRegisterKala01ViewModelcat>(userId, nameof(HistoryRegisterKala01ViewModelcat));

        // SelectedHistoryRegisterKala01ID
        public void SetSelectedHistoryRegisterKala01ID(string userId, List<SelectedHistoryRegisterKala01ID> data)
            => SetList(userId, nameof(SelectedHistoryRegisterKala01ID), data);
        public List<SelectedHistoryRegisterKala01ID> GetSelectedHistoryRegisterKala01ID(string userId)
            => GetList<SelectedHistoryRegisterKala01ID>(userId, nameof(SelectedHistoryRegisterKala01ID));












        // ----------------- String01_Update -----------------
        public void Setstring01_Update(string userId, string value)
            => SetString(userId, "String01_Update", value);

        public string Getstring01_Update(string userId)
            => GetString(userId, "String01_Update");



 















    }
}


























 