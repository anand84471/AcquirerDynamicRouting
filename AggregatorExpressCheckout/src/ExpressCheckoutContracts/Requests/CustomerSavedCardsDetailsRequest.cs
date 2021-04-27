namespace ExpressCheckoutContracts.Requests
{
    public class CustomerSavedCardsDetailsRequest
    {
        public int MerchantId { get; set; }
        public long CustomerId { get; set; }

        public string CardNumber { get; set; }
        public string CardExpiryYear { get; set; }
        public string CardExpiryMonth { get; set; }

        public string CardHolderName { get; set; }

        public string CVV { get; set; }
    }
}