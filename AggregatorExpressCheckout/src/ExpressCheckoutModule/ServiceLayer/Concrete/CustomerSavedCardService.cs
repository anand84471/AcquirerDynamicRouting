using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.Constants;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.ExternalApis.Requests;
using ExpressCheckoutContracts.ExternalApis.Responses;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ApiClients.Abstract;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Concrete
{
    internal class CustomerSavedCardService : ICustomerSavedCardService
    {
        private ICustomerSavedCardRepo _CustomerSavedCardRepo;
        private IPinePGApiClient _PinePGApiClient;
        private IMapper _mapper;
        private readonly ILogger<CustomerSavedCardService> _logger;


        public CustomerSavedCardService(ICustomerSavedCardRepo CustomerSavedCardRepo, IPinePGApiClient PinePGApiClient,IMapper mapper,
             ILogger<CustomerSavedCardService> _logger)
        {
            _CustomerSavedCardRepo = CustomerSavedCardRepo;
            _PinePGApiClient = PinePGApiClient;
            _mapper = mapper;
            this._logger = _logger;
        }

        public async Task<bool> DeleteSavedCardDetails(long customerId, long savedCardId)
        {
            bool status = await _CustomerSavedCardRepo.DeleteSavedCard(customerId, savedCardId);
            if (!status)
            {
                this._logger.LogError("Card deletion for Customer Id {0} and Saved card Id {1} failed", customerId, savedCardId);
                throw new InvalidRequestException(ResponseCodeConstants.CARD_DELETION_IS_NOT_SUCCESFUL);
            }
            return status;
        }

        public async Task<bool> UpdateSavedCardStatus(long customerId, long savedCardId, int istatus)
        {
            bool status = await _CustomerSavedCardRepo.UpdateSavedCardStatus(customerId, savedCardId,istatus);
            if (!status)
            {
                this._logger.LogError("Card Updation for Customer Id {0} and Saved card Id {1} failed", customerId, savedCardId);
                throw new InvalidRequestException(ResponseCodeConstants.CARD_UPDATION_IS_NOT_SUCCESFUL);
            }
            return status;
        }

        

        public async Task<CardDetailsDto[]> GetAllSavedCards(long customerId)
        {
            return await _CustomerSavedCardRepo.GetAllSavedCard(customerId);
        }

        public async Task<bool> InsertSavedCardDetails(SavedCardDto savedCardDto)
        {
            await GetCardAdditionalInfoFromCardNumber(savedCardDto.cardDetailsDto);
            bool status = await _CustomerSavedCardRepo.Save(savedCardDto);
            if (!status)
            {
                this._logger.LogError("Card Insertion for Customer Id {0} Failed"+ savedCardDto.customerDto.CustomerId);
                throw new InvalidRequestException(ResponseCodeConstants.SAVED_CARD_INSERTION_IS_NOT_SUCCESFUL);
            }
            return status;
        }
        private async Task GetCardAdditionalInfoFromCardNumber(CardDetailsDto cardDetailsDto)
        {
            GlobalBinCardInfoRequest globalBinCardInfoRequest = new GlobalBinCardInfoRequest();
            globalBinCardInfoRequest.bins = new List<int>();
            globalBinCardInfoRequest.bins.Add(Convert.ToInt32(cardDetailsDto.CardNumber.Substring(0, 6)));
            GlobabBinCardInfoResponse globabBinCardInfoResponse = await _PinePGApiClient.GetCardInfoData(globalBinCardInfoRequest);
            if (globabBinCardInfoResponse == null || globabBinCardInfoResponse.GlobalBinsData.Count == 0)
            {
                throw new InvalidRequestException(ResponseCodeConstants.FAILURE);
            }

            cardDetailsDto.IssuerId = Convert.ToInt32(globabBinCardInfoResponse.GlobalBinsData[0].issuerId);
            cardDetailsDto.AssociationType = Convert.ToInt32(globabBinCardInfoResponse.GlobalBinsData[0].cardSchemeId);
            
            if ( cardDetailsDto.AssociationType <= 0)
            {
                this._logger.LogError("Card Detail Association Type is invalid" + cardDetailsDto.AssociationType);
                throw new InvalidRequestException(ResponseCodeConstants.FAILURE);
            }
            cardDetailsDto.bIsDebitCard =
                globabBinCardInfoResponse.GlobalBinsData[0].cardType.Equals(CommonConstants.CARD_TYPE_DEBIT); 

        }

        public async Task<bool> CheckIsSavedCardIdMappedWithCustomer(long customerId, long savedCardId)
        {
            bool status = false;
            CardDetailsDto[] arrCardDetailsDTO = await this.GetAllSavedCards(customerId);

            if (arrCardDetailsDTO != null && arrCardDetailsDTO.Length > 0)
            {
                status = arrCardDetailsDTO.ToList().Exists(x => x.SavedCardId == savedCardId);
            }
            return status;
        }

       

    }
}