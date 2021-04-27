namespace ExpressCheckoutContracts.DTO
{
    public class CardDetailsDto
    {
        public long SavedCardId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiryYear { get; set; }
        public string CardExpiryMonth { get; set; }

        public string CardHolderName { get; set; }

        public string CVV { get; set; }

        public int AssociationType { get; set; }

        public int IssuerId { get; set; }

        public string CardSHA256 { get; set; }

        public int status { get; set; }

        public bool  bIsDebitCard { get; set; }
        public string CardHolderMobileNo { get; set; }
        
        public string IssuerName { get; set; }

 
        public int CardTypeId { get; set; }
    }
}