using Microsoft.JSInterop;
using ShamsAlShamoos03.Shared.Models;
using System.Net.Http.Json;
namespace ShamsAlShamoos03.Client.Services
{
    public class HistoryRegisterService
    {
        private readonly HttpClient _http;
        private readonly IJSRuntime _js;

        public HistoryRegisterService(HttpClient http, IJSRuntime js)
        {
            _http = http;
            _js = js;

        }

        public async Task<List<HistoryRegisterKala01ViewModel_Update>> LoadDataAsync(string userId)
        {
            // پیام در کنسول
            await _js.InvokeVoidAsync("console.log", "LoadDataAsync000000000");
            var request = new LoadDataRequest
            {
                UserId = "123", // یا هر مقداری که می‌خواهید
                Skip = 0,
                Take = 50,
                Filter = "",
                Sort = ""
            };


            var response = await _http.PostAsJsonAsync("api/HistoryRegisterKala01/LoadData", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<HistoryRegisterKala01ViewModel_Update>>() ?? new();
            }
            return new();
        }
    }
}
