using AggExpressCheckoutDBService;
using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutDb.DBClients.Abstarct;
using Microsoft.Extensions.DependencyInjection;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using System.Threading.Tasks;
using PinePGController.ExceptionHandling.CustomExceptions;
using Microsoft.Extensions.Logging;

/// <summary>
///
/// </summary>
namespace ExpressCheckoutDb.Repository.Concrete
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly IMapper mapper;

        private readonly IServiceProvider serviceProvider;

        private readonly ILogger<CustomerRepo> _logger;

        /// <summary>Initializes a new instance of the <see cref="CustomerRepo"/> class.</summary>
        /// <param name="aggregatorExpressCheckoutServiceClient">The aggregator express checkout service client.</param>
        public CustomerRepo(IMapper mapper, IServiceProvider serviceProvider, ILogger<CustomerRepo> _logger)
        {
            this.mapper = mapper;
            this.serviceProvider = serviceProvider;
            this._logger = _logger;
        }

        public async Task CreateCustomer(CustomerDto customerDto)
        {
            this._logger.LogInformation("CustomerRepo Method : {0} ", "CreateCustomer");
            var entity = mapper.Map<CustomerEntity>(customerDto);
            CustomerEntity insertedCustomerEntity = null;
            using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
            {
                insertedCustomerEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.InsertCustomerDetailsInCustomerTblAsync(entity);
            }
            if (insertedCustomerEntity == null)
            {
                this._logger.LogError("CustomerRepo Method : {0} Customer Creation Failed", "CreateCustomer");
                throw new DBException(ResponseCodeConstants.FAILURE);
            }
            customerDto.CustomerId = insertedCustomerEntity.CustomerId;
            customerDto.RowActionCount = insertedCustomerEntity.RowActionCount;
            customerDto.RowInsertionDateTime = insertedCustomerEntity.RowInsertionDateTime;
            customerDto.RowUpdationDateTime = insertedCustomerEntity.RowUpdationDateTime;

        }

        public async Task<CustomerDto> GetCustomer(long customerId)
        {
            this._logger.LogInformation("CustomerRepo Method : {0} ", "GetCustomer");
            CustomerEntity customerEntity = null;
            using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
            {
               
                customerEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetCustomerAsync(customerId);
            }
            if (customerEntity == null)
            {
                this._logger.LogError("CustomerRepo Method :{0} Failed  Customer Id{1}", "GetCustomer", customerId);
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID);
            }
            var customerDto = mapper.Map<CustomerDto>(customerEntity);
            return customerDto;
        }

        public async Task UpdateCustomer(CustomerDto customerDto)
        {
            this._logger.LogInformation("CustomerRepo Method : {0} ", "UpdateCustomer");
            var entity = mapper.Map<CustomerEntity>(customerDto);
            bool updated = false;
            using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
            {
                updated = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateCustomerDetailsInCustomerTblAsync(entity);
            }
            if (!updated)
            {
                this._logger.LogError("CustomerRepo Method :{0} Failed  Customer Id{1}", "UpdateCustomer", customerDto.CustomerId);
                throw new DBException(ResponseCodeConstants.FAILURE);
            }
        }


        public async Task<bool> IsDuplicateCutomerIdandRefernceNumber(int merchantId, string customerReferernceNo)
        {
            bool status = false;
            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.IsCustomeRefNoExistsForMerchantAsync(merchantId, customerReferernceNo);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }

    }
}