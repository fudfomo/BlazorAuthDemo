using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAuthDemo.Client.Services
{
    public class ApiService
    {
        private ITokenAcquisition _tokenAquisitionService;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ApiService(HttpClient httpClient, IConfiguration configuration, ITokenAcquisition tokenAquisitionService)
        {
            _tokenAquisitionService = tokenAquisitionService;

            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["BlazorDemoApi:BaseUrl"]!);
            _httpClient.DefaultRequestHeaders.Accept.Add(new("application/json"));
        }

        public async Task<string> GetHomeAsync()
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());

            var result = (await _httpClient.GetAsync ("api/home"))!;

            var claims = await result.Content.ReadAsStringAsync();

            return claims;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var scopes = new[] { _configuration["BlazorDemoApi:Scopes"] };
            var token = await _tokenAquisitionService.GetAccessTokenForUserAsync(scopes!);
            return token;
        }
    }
}
