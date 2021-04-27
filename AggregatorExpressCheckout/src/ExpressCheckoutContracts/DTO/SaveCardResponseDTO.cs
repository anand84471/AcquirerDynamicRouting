namespace ExpressCheckoutContracts.DTO
{
    public class SaveCardResponseDTO : ResponseCodeDto
    {
        public CardDetailsDto[] cardDetailsDto { get; set; }
    }
}