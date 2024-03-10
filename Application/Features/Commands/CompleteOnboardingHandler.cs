using AbiaPayCollectionMiddleware.Helpers;
using MediatR;
using System.ComponentModel.DataAnnotations;
using WemaCustomer.Application.Data;
using WemaCustomer.Application.Data.Models;

namespace WemaCustomer.Application.Features.Commands
{
    public class CompleteOnboardingHandler : IRequestHandler<CompleteOnboardingRequest, ApiResponse<CompleteOnboardingResponse>>
    {
        private readonly wemaDBContext _context;
        private readonly ILogger<CompleteOnboardingHandler> _logger;

        public CompleteOnboardingHandler(wemaDBContext context, ILogger<CompleteOnboardingHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<CompleteOnboardingResponse>> Handle(CompleteOnboardingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // Your logic to verify the OTP and complete the onboarding process
                // For example:
                var customer = await _context.Customers.FindAsync(request.CustomerId);
                if (customer == null)
                {
                    return new ApiResponse<CompleteOnboardingResponse>( "Customer not found.");
                }

                if (customer.Otp != request.Otp)
                {
                    return new ApiResponse<CompleteOnboardingResponse>("Invalid OTP.");
                }

                // Update the customer's onboarding status
                customer.IsOnboardingComplete = true;
                _context.Update(customer);
                await _context.SaveChangesAsync(cancellationToken);
                var response = new CompleteOnboardingResponse(true, "Onboarding process completed successfully.");

                return new ApiResponse<CompleteOnboardingResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while completing the onboarding process.");
                return new ApiResponse<CompleteOnboardingResponse>($"An error occurred: {ex.Message}");
            }
        }
    }



    public class CompleteOnboardingRequest : IRequest<ApiResponse<CompleteOnboardingResponse>>
    {
        [Required(ErrorMessage = "Customer Id is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "OTP is required")]
        public string Otp { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }
    }


    public class CompleteOnboardingResponse
    {
        public bool Success { get; }
        public string Message { get; }

        public CompleteOnboardingResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
