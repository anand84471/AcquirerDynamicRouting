using AutoMapper;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.ExternalApis.Requests;
using ExpressCheckoutContracts.ExternalApis.Responses;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutDb.Repository.Abstract;
using ExpressCheckoutModule.ApiClients.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExpressCheckoutModule.ServiceLayer.Abstract
{
    public class GlobalBinDataService : IGlobalBinDataService
    {

        IPinePGApiClient _pinePGApiClient;
        IOrderRepo _orderRepo;
        private readonly IMapper _mapper;

        public GlobalBinDataService(IPinePGApiClient pinePGApiClient,IOrderRepo orderRepo, IMapper mapper)
        {
            _pinePGApiClient = pinePGApiClient;
            _orderRepo = orderRepo;
            _mapper = mapper;
        }
        
        public async Task<bool> InsertGlobalBindata(DoPaymentRequest doPaymentRequest, long OrderId)
        {
            bool status = false;
            GlobalBinCardInfoRequest globalBinCardInfoRequest=new GlobalBinCardInfoRequest();
            OrderDetailsDto orderDetailsDto = new OrderDetailsDto() ;
            GlobabBinCardInfoResponse globabBinCardInfoResponse;
            if (doPaymentRequest.CardRequest.SavedCardId == 0)
            {
                globalBinCardInfoRequest.bins = new List<int>();
                globalBinCardInfoRequest.bins.Add(Convert.ToInt32(doPaymentRequest.CardRequest.CardNumber.Substring(0, 6)));
                globabBinCardInfoResponse = await _pinePGApiClient.GetCardInfoData(globalBinCardInfoRequest);
                orderDetailsDto.BinData = _mapper.Map<GlobalBinDataDto>(globabBinCardInfoResponse.GlobalBinsData[0]);
                status = await _orderRepo.UpdateBinDataInOrderTbl(orderDetailsDto, OrderId);
            }
            else
            {
                if (doPaymentRequest.CardRequest.CardNumber != null)
                {
                    int issuerId = Convert.ToInt32(doPaymentRequest.CardRequest.IssuerId);
                    int associationType = Convert.ToInt32(doPaymentRequest.CardRequest.AssociationType);
                    List<GlobalBinsData> lsGlobalbin = new List<GlobalBinsData>();
                    lsGlobalbin.Add(
                                new GlobalBinsData
                                {
                                    issuerId = Convert.ToString(issuerId),
                                    issuerName = Convert.ToString(doPaymentRequest.CardRequest.IssuerId),
                                    cardSchemeId = Convert.ToString(associationType),
                                    bin= Convert.ToString(doPaymentRequest.CardRequest.CardNumber.Substring(0, 6))
                                }
                                );

                    orderDetailsDto.BinData = _mapper.Map<GlobalBinDataDto>(lsGlobalbin[0]);
                    status = await _orderRepo.UpdateBinDataInOrderTbl(orderDetailsDto, OrderId);
                }
            }
           
            return status;
        }
    }
}
