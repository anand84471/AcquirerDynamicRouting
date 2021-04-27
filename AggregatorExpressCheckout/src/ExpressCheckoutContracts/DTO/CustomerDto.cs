using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace ExpressCheckoutContracts.DTO
{
    public class CustomerDto
    {
        public CustomerDto()
        {
            this.StatusId = 2;
        }

        public long CustomerId { get; set; }

        public string CustomerReferenceNumber { get; set; }

        public int MerchantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonProperty("customer_phone")]
        public string MobileNumber { get; set; }

        [JsonProperty("customer_email")]
        public string EmailId { get; set; }

        public string CountryCode { get; set; }

        public short StatusId { get; set; }

        public int RowActionCount { get; set; }

        public DateTime RowInsertionDateTime { get; set; }

        public DateTime RowUpdationDateTime { get; set; }
    }
}