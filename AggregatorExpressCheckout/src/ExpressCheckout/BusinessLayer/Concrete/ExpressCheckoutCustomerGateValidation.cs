using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Validation;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutContracts.Validators;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Concrete
{
    public class ExpressCheckoutCustomerGateValidation : IExpressCheckoutCustomerGateValidation
    {
        private readonly IMapper mapper;
        private readonly ICustomerService customerService;
        private readonly ILogger<ExpressCheckoutCustomerGateValidation> _logger;

        public ExpressCheckoutCustomerGateValidation(IMapper mapper, ICustomerService customerService, 
            ILogger<ExpressCheckoutCustomerGateValidation> _logger)
        {
            this._logger = _logger;
            this.mapper = mapper;
            this.customerService = customerService;
        }

        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest customerRequest)
        {
            CustomerDto customerDto = null;
            if (customerRequest == null)
            {
                this._logger.LogError("CreateCustomer customerRequest is null ");
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            customerDto = mapper.Map<CustomerDto>(customerRequest);
            ValidationHander<CustomerValidator, CustomerDto>.DoValidate(customerDto, ConstantRuleSetName.CREATE_CUSTOMER_VALIDATION);
            if (await customerService.IsDuplicateCutomerIdandRefernceNumber(customerDto.MerchantId,customerDto.CustomerReferenceNumber))
            {
                this._logger.LogError("CreateCustomer Customer Refernce Number "+ customerDto.CustomerReferenceNumber +
                    " already exists for Merchant Id "+ customerDto.MerchantId);
                throw new OrderException(ResponseCodeConstants.MERCHANT_CUSTOMER_REFERENCE_NO_ALREADY_EXIST);
            }
            await this.customerService.CreateCustomer(customerDto);
            var response = mapper.Map<CustomerResponse>(customerDto);
            return response;
        }

        public async Task<CustomerResponse> GetCustomer(int merchantId, long customerId)
        {
            this._logger.LogInformation("In get customer");
            CustomerDto customerDto = null;
            if (customerId <= 0)
            {
                this._logger.LogError("GetCustomer  customerid is less than 1");
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID);
            }
            Task<bool> isCustomerIdMappedWithMerchant = IsCustomerIdMappedWithMerchant(merchantId,customerId);

            if (!await isCustomerIdMappedWithMerchant)
            {
                this._logger.LogError("Method{0} Customer Id {2} not mapped with Merchant {1}", "GetCustomer", merchantId, 
                    customerId);
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING);
            }
            customerDto = await this.customerService.GetCustomer(customerId);
            var response = mapper.Map<CustomerResponse>(customerDto);
            return response;
        }


        private async Task<bool> IsCustomerIdMappedWithMerchant(int merchantId, long customerId)
        {
          
            return await this.customerService.CheckIsCustomerIdMappedWithMerchant(merchantId, customerId);
        }


        public async Task<CustomerResponse> UpdateCustomer(long customerId, UpdateCustomerRequest customerRequest)
        {
            CustomerDto customerDto = null;
            if (customerId <= 0 || customerRequest == null)
            {
                this._logger.LogError("GetCustomer  customerid is less than 1 or Customer Request is null");
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }

           
            customerDto = mapper.Map<CustomerDto>(customerRequest);
            customerDto.CustomerId = customerId;
            ValidationHander<CustomerValidator, CustomerDto>.DoValidate(customerDto, ConstantRuleSetName.UPDATE_CUSTOMER_VALIDATION);

            Task<bool> isCustomerIdMappedWithMerchant = IsCustomerIdMappedWithMerchant(customerRequest.MerchantId, customerId);

            if (!await isCustomerIdMappedWithMerchant)
            {
                this._logger.LogError("Method{0} Customer Id {2} not mapped with Merchant {1}", "GetCustomer", customerRequest.MerchantId,
                    customerId);
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_CUSTOMER_ID_MERCHANT_ID_MAPPING);
            }
            await this.customerService.UpdateCustomer(customerDto);
            var response = mapper.Map<CustomerResponse>(customerDto);
            return response;
        }


         
}
}