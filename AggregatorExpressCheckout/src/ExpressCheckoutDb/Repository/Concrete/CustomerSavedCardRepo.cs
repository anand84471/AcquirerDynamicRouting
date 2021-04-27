using AggExpressCheckoutDBService;
using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PinePGController.ExceptionHandling.CustomExceptions;

namespace ExpressCheckoutDb.Repository.Concrete
{
    internal class CustomerSavedCardRepo : ICustomerSavedCardRepo
    {
        private readonly IMapper _mapper;

        private readonly IServiceProvider serviceProvider;

        private readonly ILogger<CustomerSavedCardRepo> _logger;


        /// <summary>Initializes a new instance of the <see cref="CustomerRepo"/> class.</summary>
        /// <param name="aggregatorExpressCheckoutServiceClient">The aggregator express checkout service client.</param>
        public CustomerSavedCardRepo(IMapper mapper, IServiceProvider serviceProvider, ILogger<CustomerSavedCardRepo> _logger)
        {
            this.serviceProvider = serviceProvider;
            _mapper = mapper;
            this._logger = _logger;
        }

        public async Task<bool> Save(SavedCardDto savedCardDto)
        {
            this._logger.LogInformation("CustomerSavedCardRepo Method : {0} ", "Save");
            SavedCardEntity savedCardEntity = _mapper.Map<SavedCardEntity>(savedCardDto);
            SavedCardEntity insertedSavedCardEntity = null;
            bool result = false;

            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    insertedSavedCardEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.InsertSavedCardDetailsForUserInSavedCardTblAsync(savedCardEntity);
                                 
                }

                if (insertedSavedCardEntity == null)
                {
                    this._logger.LogError("Saved Card Insertion Failed");
                    throw new DBException(ResponseCodeConstants.FAILURE);
                }
                savedCardDto.cardDetailsDto.SavedCardId = insertedSavedCardEntity.cardDetailsEntity.SavedCardId;
                result = true;
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return result;
        }

        public async Task<bool> DeleteSavedCard(long customerId, long savedCardId)
        {
            bool result = false;
            this._logger.LogInformation("CustomerSavedCardRepo Method : {0} CustomerId : {1} SavedCardId : {2} ", "DeleteSavedCard",customerId, savedCardId);
            try
            {

                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    result = await serviceClient._AggregatorExpressCheckoutServiceClient.DeleteSavedCardFromCustomerAsync(customerId, savedCardId);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
                
            }
            return result;
        }


        public async Task<bool> UpdateSavedCardStatus(long customerId, long savedCardId, int status)
        {
            this._logger.LogInformation("CustomerSavedCardRepo Method : {0} CustomerId : {1} SavedCardId : {2}  status :{3}", "UpdateSavedCardStatus", customerId, savedCardId,status);
            bool result = false;
            try
            {

                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    result = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateSavedCardStatusFromCustomerAsync(customerId, savedCardId,status);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());

            }
            return result;
        }

        public async Task<CardDetailsDto[]> GetAllSavedCard(long customerId)
        {
            this._logger.LogInformation("CustomerSavedCardRepo Method : {0} CustomerId : {1}", "GetAllSavedCard", customerId);
            CardDetailsDto[] arrCardDetailsDTO = null;
            CardDetailsEntity[] arrCardDetailsEntity;
            try
            {
                using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
                {
                    arrCardDetailsEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetSavedCardForUserAsync(customerId);
                }
                if (arrCardDetailsEntity == null || arrCardDetailsEntity.Length == 0)
                {
                    this._logger.LogError("No Card Data is mapped with customer Id "+customerId);
                    throw new InvalidRequestException(ResponseCodeConstants.NO_CARD_DATA_IS_MAPPED_WITH_CUSTOMER_ID);
                }
                arrCardDetailsDTO = _mapper.Map<CardDetailsDto[]>(arrCardDetailsEntity);
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
          
            return arrCardDetailsDTO;
        }
    }
}