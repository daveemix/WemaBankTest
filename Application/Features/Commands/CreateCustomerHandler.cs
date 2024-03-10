using AbiaPayCollectionMiddleware.Helpers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using WemaCustomer.Application.Data;
using WemaCustomer.Helpers;

namespace WemaCustomer.Application.Features.Command
{

    public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, ApiResponse<CreateCustomerResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCustomerHandler> _logger;

        public CreateCustomerHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateCustomerHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<CreateCustomerResponse>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                

                var customer = _mapper.Map<Customer>(request);
                customer.OTP = OtpService.GenerateOtp().ToString();

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Customer created successfully.");

                var responseMessage = "Customer details submitted successfully. Provide the OTP to complete the onboarding process.";
                var response = new CreateCustomerResponse(true, responseMessage);
                return new ApiResponse<CreateCustomerResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the customer.");
                return new ApiResponse<CreateCustomerResponse>($"An error occurred while creating the customer: {ex.Message}");
            }

        }
    }

    public class CreateCustomerRequest : IRequest<ApiResponse<CreateCustomerResponse>>
    {
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "State ID is required")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "LGA ID is required")]
        public int LGAId { get; set; }
    }




    public class CreateCustomerResponse
    {
        public bool Success { get; }
        public string Message { get; }

        public CreateCustomerResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
