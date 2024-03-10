using AbiaPayCollectionMiddleware.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WemaCustomer.Application.Data;
using WemaCustomer.Application.Data.Models;
using WemaCustomer.Helpers;

namespace WemaCustomer.Application.Features.Queries
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersRequest, ApiResponse<PaginatedList<Customer>>>
    {
        private readonly wemaDBContext _context;
        private readonly ILogger<GetAllCustomersHandler> _logger;

        public GetAllCustomersHandler(wemaDBContext context, ILogger<GetAllCustomersHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<PaginatedList<Customer>>> Handle(GetAllCustomersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all customers...");

                var query = _context.Customers.AsQueryable();
                query = query.Where(x => x.IsOnboardingComplete == true);

                var customers = await PaginatedList<Customer>.CreateAsync(query, request.PageNumber, request.PageSize);

                _logger.LogInformation("Retrieved all customers successfully.");

                return new ApiResponse<PaginatedList<Customer>>(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all customers.");
                return new ApiResponse<PaginatedList<Customer>>($"An error occurred: {ex.Message}");
            }
        }
    }

    public class GetAllCustomersRequest : IRequest<ApiResponse<PaginatedList<Customer>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
