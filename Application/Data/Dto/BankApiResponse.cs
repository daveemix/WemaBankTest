namespace WemaCustomer.Application.Data.Dto
{
    public class BankApiResponse
    {
        public List<BankInfo> Result { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ErrorMessages { get; set; }
        public bool HasError { get; set; }
        public string TimeGenerated { get; set; }
    }

    public class BankInfo
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }
}
