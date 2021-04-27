using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Utilities;
using Core.Validation;
using ExpressCheckout.BusinessLayer.Abstract;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressCheckout.BusinessLayer.Concrete
{
    public class MerchantValidation : IMerchantValidation
    {
        private readonly IMapper _mapper;

        private readonly IMerchantService _MerchantService;
        private readonly ILogger<MerchantValidation> _logger;

        public MerchantValidation(IMapper mapper, IMerchantService merchantService , ILogger<MerchantValidation> _logger)
        {
            _mapper = mapper;
            _MerchantService = merchantService;
            this._logger = _logger;
        }
        public async Task<EnumPaymentMode[]> GetPaymentMode(OrderDetailsRequest orderDetailsRequest)
        {
                this._logger.LogInformation("Entered GetPaymentMode");
                
                if (orderDetailsRequest == null || GenericUtility.ValidateForXSSAttackAttempt(orderDetailsRequest))
                {
                    this._logger.LogError("Order Data is Not Valid");

                    throw new OrderException(ResponseCodeConstants.ORDER_DATA_IS_NOT_VALID);
                }

                OrderDetailsDto orderDetailsDto = _mapper.Map<OrderDetailsDto>(orderDetailsRequest);

                //ValidationHander<Merc, OrderDetailsDto>.DoValidate(orderDetailsDto, ConstantRuleSetName.PURCHASE_TXN_ORDER_UPDATION_VALIDATION);
                if(orderDetailsDto.MerchantDto.MerchantId==0)
                {
                    this._logger.LogError("Merchant is Not Valid as Merchant Id is Zero");
                    throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
                }
                MerchantDto merchantDto = await _MerchantService.GetMerchantData(orderDetailsDto.MerchantDto.MerchantId);
                if (merchantDto == null)
                {
                    this._logger.LogError("Invalid Merchant as No Data Found for the Merchant");
                    throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
                }

                if (!_MerchantService.IsMerchantValid(merchantDto, orderDetailsDto.MerchantDto.MerchantAccessCode))
                {
                    this._logger.LogError("Mechant is Not Valid as Merchant Access Code not Matched");
                    throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
                }
                EnumPaymentMode[] enumPaymentModes = await _MerchantService.GetPaymentModesMappedWithMerchant(merchantDto.MerchantId);
                if (enumPaymentModes == null)
                {
                    this._logger.LogError("No Payment Mode is Found On Merchant");
                    throw new OrderException(ResponseCodeConstants.MERCHANT_IS_NOT_VALID);
                }

                return enumPaymentModes;

            
        }

    }

}

    

