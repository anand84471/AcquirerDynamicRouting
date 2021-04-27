using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Enums;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Concrete
{
    public class MerchantService : IMerchantService
    {
        private IMerchantRepo _MerchantRepo;
        private readonly ILogger<MerchantService> _logger;
        public MerchantService(IMerchantRepo MerchantRepo , ILogger<MerchantService> _logger)
        {
            _MerchantRepo = MerchantRepo;
            this._logger = _logger;
        }

        public async Task<MerchantDto> GetMerchantData(int merchantId)
        {
            return await _MerchantRepo.GetMerchantData(merchantId);
        }

        public async Task CheckIfMerchantExistsForMerchantId(int merchantId)
        {
            var merchantDto = await GetMerchantData(merchantId);
            if (merchantDto == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.INVALID_MERCHANT_ID);
            }
        }

        public async Task<bool> IsSavedCardEnabled(int merchantId)
        {
            MerchantDto merchantDto = await this.GetMerchantData(merchantId);
            if (merchantDto == null)
            {
                throw new InvalidRequestException(-1);
            }
            return merchantDto.IsSavedCardEnabled;
        }

        public async Task<bool> IsSavedCardEnabled(MerchantDto merchantDto)
        {
            return merchantDto.IsSavedCardEnabled;
        }

        public bool IsMerchantValid(MerchantDto merchantDto, string merchantAccessCode)
        {
            return merchantDto.MerchantAccessCode.Equals(merchantAccessCode);
        }

        public async Task<bool> IsDuplicateMerchantIDAndMerchantOrderId(int merchantId, string merchantOrderID)
        {
            return await _MerchantRepo.IsDuplicateMerchantIDAndMerchantOrderId(merchantId, merchantOrderID);
        }

        public async Task<EnumPaymentMode[]> GetPaymentModesMappedWithMerchant(int merchantId)
        {

            this._logger.LogInformation("Entered GetPaymentModesMappedWithMerchant");

            int[] arrPaymentMode = await _MerchantRepo.GetPaymentModesMappedWithMerchant(merchantId);

            EnumPaymentMode[] enumPaymentMode = null;
            if (arrPaymentMode!=null)
            {
                Queue<int> ValidatedPaymentMode = new Queue<int>();
                Queue<int> EmiPaymentMode = new Queue<int>();

                for (int i = 0; i < arrPaymentMode.Length; i++)
                {
                    if ((EnumPaymentMode.EMI != (EnumPaymentMode)arrPaymentMode[i]) && (EnumPaymentMode.DEBIT_EMI != (EnumPaymentMode)arrPaymentMode[i]))
                    {
                        if (await _MerchantRepo.ValidatePaymentMode(merchantId, arrPaymentMode[i]))
                        {
                            ValidatedPaymentMode.Enqueue(arrPaymentMode[i]);
                        }
                    }
                    else
                    {
                        EmiPaymentMode.Enqueue(arrPaymentMode[i]);
                    }   
                }

                if (EmiPaymentMode.Count > 0 && (ValidatedPaymentMode.Contains((int)EnumPaymentMode.CREDIT_DEBIT)) || await _MerchantRepo.ValidatePaymentMode(merchantId, (int)EnumPaymentMode.CREDIT_DEBIT))
                {

                    while (EmiPaymentMode.Count > 0)
                    {
                         ValidatedPaymentMode.Enqueue(EmiPaymentMode.Dequeue());
                    }
                }

                int QueueLength = ValidatedPaymentMode.Count;
                if (QueueLength > 0)
                {
                    enumPaymentMode = new EnumPaymentMode[QueueLength];
                    for (int i = 0; i < QueueLength; i++)
                    {
                       enumPaymentMode[i] = (EnumPaymentMode)ValidatedPaymentMode.Dequeue();
                    }
                }
            }
            else
            {
                this._logger.LogInformation("No Payment Mode Found on the Merchant");
            }
            return enumPaymentMode;
        }

       public async Task<MerchantGatewayConfigurationMappingDto> GetMerchantPaymentGatewayConfigurationDetails(int merchantId, int gatewayId)
        {
          return await _MerchantRepo.GetMerchantPaymentGatewayConfigurationDetails(merchantId,gatewayId);
        }
    }
}