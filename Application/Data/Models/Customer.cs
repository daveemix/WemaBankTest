using System;
using System.Collections.Generic;

namespace WemaCustomer.Application.Data.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int StateId { get; set; }
        public int Lgaid { get; set; }
        public string Otp { get; set; } = null!;
        public bool IsOnboardingComplete { get; set; }
    }
}
