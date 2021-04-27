namespace ExpressCheckoutContracts.DTO
{
    public class SavedCardDto
    {
        public MerchantDto merchantDto { get; set; }

        public CustomerDto customerDto { get; set; }

        public CardDetailsDto cardDetailsDto { get; set; }
    }
}