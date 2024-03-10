namespace WemaCustomer.Helpers
{
    public static class OtpService
    {
        private const int OtpLength = 4;

        public static string GenerateOtp()
        {
            Random random = new Random();
            int otpValue = random.Next(1000, 10000); 
            return otpValue.ToString();
        }
    }
}
