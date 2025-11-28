//using ShamsAlShamoos01.Shared.Models;
using ShamsAlShamoos01.Shared.Models;
using Syncfusion.Blazor;
using System.Net.Http.Json;

namespace ShamsAlShamoos01.Client.Services
{
    public class HistoryRegisterService
    {
        private readonly HttpClient _http;

        public HistoryRegisterService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<HistoryRegisterKala01ViewModel_Update>> LoadDataAsync(string userId)
        {
 


            var request = new LoadDataRequest { UserId = "123"  };
            var response = await _http.PostAsJsonAsync("api/HistoryRegisterKala01/LoadData", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<HistoryRegisterKala01ViewModel_Update>>() ?? new();
            }

            return new();
        }
    }
}
