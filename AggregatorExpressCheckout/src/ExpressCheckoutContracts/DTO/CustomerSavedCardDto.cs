namespace ExpressCheckoutContracts.DTO
{
    public class CustomerSavedCardDto : CardDetailsDto
    {
        public int MerchantId { get; set; }
        public long CustomerId { get; set; }
    }
}