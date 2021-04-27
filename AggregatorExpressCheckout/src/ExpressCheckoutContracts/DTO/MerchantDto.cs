namespace ExpressCheckoutContracts.DTO
{
    public class MerchantDto
    {
        public MerchantDto()
        {

        }
        public int MerchantId { get; set; }

        public string MerchantAccessCode { get; set; }

        public string MerchantReturnUrl { get; set; }

        public string MerchantOrderID { get; set; }

        public bool IsSavedCardEnabled { get; set; }
        public string SecureSeret { get; set; }
        public bool IsMerchatReturnUrlToBeValidated { get; set; }
    }
}