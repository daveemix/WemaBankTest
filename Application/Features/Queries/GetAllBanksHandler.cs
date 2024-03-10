using AbiaPayCollectionMiddleware.Helpers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WemaCustomer.Application.Data.Dto;
using WemaCustomer.Application.Services;
using WemaCustomer.Helpers;

namespace WemaCustomer.Application.Features.Queries
{
    public class GetAllBanksHandler : IRequestHandler<GetAllBanksRequest, ApiResponse<List<BankInfo>>>
    {
        private readonly IBankApiService _bankApiService;

        public GetAllBanksHandler(IBankApiService bankApiService)
        {
            _bankApiService = bankApiService;
        }

        public async Task<ApiResponse<List<BankInfo>>> Handle(GetAllBanksRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _bankApiService.GetAllBanks();
                return new ApiResponse<List<BankInfo>>(response.Data.Result); 
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<BankInfo>>($"An error occurred: {ex.Message}");
            }
        }
    }


    public class GetAllBanksRequest : IRequest<ApiResponse<List<BankInfo>>>
    {
    }


}
