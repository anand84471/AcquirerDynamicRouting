using Core.Infrastructure.Entities;
using System;

namespace ExpressCheckoutDb.Entities.Concrete
{
    public class Customer : IEntity
    {
        public long CustomerId { get; set; }

        public string CustomerReferenceNumber { get; set; }
        public long MerchantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobileNumber { get; set; }

        public string EmailId { get; set; }

        public string CountryCode { get; set; }

        public short StatusId { get; set; }

        public int RowActionCount { get; set; }

        public DateTime RowInsertionDateTime{ get; set; }

        public DateTime RowUpdationDateTime { get; set; }
    }
}