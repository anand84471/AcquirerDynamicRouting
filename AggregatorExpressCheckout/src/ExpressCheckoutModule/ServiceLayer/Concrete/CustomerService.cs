using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly IMerchantService merchantService;
        private readonly ICustomerRepo customerRepo;
        private readonly ILogger<CustomerService> _logger;


        public CustomerService(IMerchantService merchantService, ICustomerRepo customerRepo, ILogger<CustomerService> _logger)
        {
            this.merchantService = merchantService;
            this.customerRepo = customerRepo;
            this._logger = _logger;
        }

        public async Task CreateCustomer(CustomerDto customerDto)
        {
            await merchantService.CheckIfMerchantExistsForMerchantId(customerDto.MerchantId);

            await customerRepo.CreateCustomer(customerDto);
        }

        public async Task<bool> CheckIsCustomerIdMappedWithMerchant(int merchantId, long customerId)
        {
            bool status = false;
            CustomerDto customerDto = await customerRepo.GetCustomer(customerId);
            if (customerDto != null && customerDto.MerchantId == merchantId)
            {
                
                status = true;
            }
            this._logger.LogInformation("Customer Id " + customerId + " Not mapped with Merchant " + merchantId);
            return status;
        }

        public async Task<CustomerDto> GetCustomer(long customerId)
        {
            return await customerRepo.GetCustomer(customerId);
        }

        public async Task UpdateCustomer(CustomerDto customerDto)
        {
            var existedCustomer = await customerRepo.GetCustomer(customerDto.CustomerId);
            if (existedCustomer == null)
            {
                this._logger.LogError("Customer with Customer Id "+ customerDto.CustomerId +" does not exist");
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID);
            }
            if (existedCustomer.MerchantId != customerDto.MerchantId)
            {
                this._logger.LogError("Invalid customer Id "+ customerDto.CustomerId+" and and Merchant Id"+ existedCustomer.MerchantId+ "mapping");
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING);
            }
            await customerRepo.UpdateCustomer(customerDto);
        }


        public async Task<bool> IsDuplicateCutomerIdandRefernceNumber(int merchantId, string customerReferernceNo)
        {
     
           return await customerRepo.IsDuplicateCutomerIdandRefernceNumber(merchantId, customerReferernceNo);
        }
    }
}