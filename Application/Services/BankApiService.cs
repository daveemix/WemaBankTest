using AbiaPayCollectionMiddleware.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WemaCustomer.Application.Data.Dto;
using Microsoft.Extensions.Options;
using WemaCustomer.Helpers;

namespace WemaCustomer.Application.Services
{
    public interface IBankApiService
    {
        Task<ApiResponse<BankApiResponse>> GetAllBanks();
    }

    public class BankApiService : IBankApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BankApiService> _logger;
        private readonly string _banksApiUrl;

        public BankApiService(IHttpClientFactory httpClientFactory, ILogger<BankApiService> logger, IOptions<BankApiSettings> options)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _banksApiUrl = options.Value.BanksApiUrl;
        }

        public async Task<ApiResponse<BankApiResponse>> GetAllBanks()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_banksApiUrl);

                client.Timeout = TimeSpan.FromSeconds(30); 

                var response = await client.GetAsync(""); 
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<BankApiResponse>(content);

                return new ApiResponse<BankApiResponse>(apiResponse);
            }
            catch (OperationCanceledException ex) when (ex.CancellationToken == CancellationToken.None)
            {
                _logger.LogError(ex, "Timeout occurred while retrieving bank information from the API.");
                return new ApiResponse<BankApiResponse>("Timeout occurred while retrieving bank information from the API.");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving bank information from the API.");
                return new ApiResponse<BankApiResponse>($"An error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing the API response.");
                return new ApiResponse<BankApiResponse>("An unexpected error occurred.");
            }
        }
    }
}
