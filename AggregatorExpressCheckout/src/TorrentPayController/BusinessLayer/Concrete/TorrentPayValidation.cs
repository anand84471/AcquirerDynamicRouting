using AutoMapper;
using Core.Constants;
using Core.Features.ExceptionHandling.Concrete;
using Core.Validation;
using ExpressCheckoutContracts.Constants.FluentValidationRuleSet;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Response.Concrete;
using ExpressCheckoutModule.ServiceLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorrentPayController.BusinessLayer.Abstract;
using TorrentPayController.Validators;

namespace TorrentPayController.BusinessLayer.Concrete
{
    public class TorrentPayValidation: ITorrentPayValidation
    {
        private readonly IMapper _mapper;
        private IAndroidSDKService _androidSDKService;


        public TorrentPayValidation(IMapper mapper, IAndroidSDKService androidSDKService
            )
        {
            _mapper = mapper;
            _androidSDKService = androidSDKService;
        }
        public async Task<BankURLDetailResponse[]> GetOtpUrlDetails(AndroidSdkGetUrlDetailsRequest objAndroidSdkGetUrlDetailsRequest)
        {
            if (objAndroidSdkGetUrlDetailsRequest == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            BankOTPUrlDTO[] bankOTPUrlDTOs = await _androidSDKService.GetBankUrlDetails(objAndroidSdkGetUrlDetailsRequest);
            var arrBankDetailsResponse = _mapper.Map<BankURLDetailResponse[]>(bankOTPUrlDTOs);
            return arrBankDetailsResponse;
        }

        public async Task<bool> InsertJsExecutionError(MobileReportJsRequest mobileReportJsRequest)
        {

            return await _androidSDKService.InsertJsExecutionError(mobileReportJsRequest);
        }

        public async Task<bool> InsertTorrentayTxnDetails(TorrentPayTransactionDetailsRequest torrentPayTransactionDetailsRequest)
        {
            if (torrentPayTransactionDetailsRequest == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var androidSessionDetailDTO = _mapper.Map<AndroidPGSdkSessionDetailDTO>(torrentPayTransactionDetailsRequest);
            ValidationHander<TorrentPayApiRequestValidator, AndroidPGSdkSessionDetailDTO>.DoValidate(androidSessionDetailDTO, ConstantRuleSetName.ANDROID_SESSION_DETAIL_INSERTION_VALIDATION);
            return await _androidSDKService.InsertAndroidSdkBrowserSessionDetails(androidSessionDetailDTO);
        }
        public async Task<bool> InsertAndroidSdkBrowserSessionDetails(AdnroidPGSdkSessionDetailsRequest objAdnroidPGSdkSessionDetailsRequest)
        {

            if (objAdnroidPGSdkSessionDetailsRequest == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var androidSessionDetailDTO = _mapper.Map<AndroidPGSdkSessionDetailDTO>(objAdnroidPGSdkSessionDetailsRequest);
            ValidationHander<TorrentPayApiRequestValidator, AndroidPGSdkSessionDetailDTO>.DoValidate(androidSessionDetailDTO, ConstantRuleSetName.ANDROID_SESSION_DETAIL_INSERTION_VALIDATION);
            return await _androidSDKService.InsertAndroidSdkBrowserSessionDetails(androidSessionDetailDTO);
        }
        public async Task<bool> ReportTransactionStatus(ReportTransactionStatusRequest objReportTransactionStatusRequest)
        {

            if (objReportTransactionStatusRequest == null)
            {
                throw new InvalidRequestException(ResponseCodeConstants.REQUEST_IS_EMPTY_OR_CONTENT_TYPE_IS_NOT_CORRECT);
            }
            var reportTransactionStatusDto = _mapper.Map<ReportTransactionStatusDto>(objReportTransactionStatusRequest);
            return await _androidSDKService.ReportTransactionStatus(reportTransactionStatusDto);
        }

    }
}
