using ExpressCheckoutDb.Entities;
using ExpressCheckoutDb.Entities.Concrete;
using ExpressCheckoutDb.Postgress.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Postgress.Concrete
{
    public class TorrentPayDBService: ITorrentPayDBService
    {
        private NpgsqlConnection m_con;
        private NpgsqlCommand m_command;
        private string m_ConnectionString;
        private readonly ILogger<TorrentPayDBService> _logger;
        private IConfiguration configuration;
        public TorrentPayDBService(ILogger<TorrentPayDBService> logger,IConfiguration configuration)
        {
            this._logger = logger;
            this.configuration = configuration;
        }
        public async Task<bool> InitDB()
        {
            bool status = false;
            try
            {
                m_ConnectionString = GetConnectionString();
                if (string.IsNullOrEmpty(m_ConnectionString))
                {
                    this._logger.LogError("PosgressSQL connection string is not present in DB");
                }
                else
                {
                    m_con = new NpgsqlConnection(m_ConnectionString);
                    var timeout = TimeSpan.FromMinutes(1);
                    var token = new CancellationToken(true);
                    await m_con.OpenAsync();
                    status = true;
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
        private string GetConnectionString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            { 
                stringBuilder.AppendFormat("server={0};", configuration.GetValue<string>("PostgressDBConnectionSetting:server"));
                stringBuilder.AppendFormat("port={0};", configuration.GetValue<string>("PostgressDBConnectionSetting:port"));
                stringBuilder.AppendFormat("database={0};", configuration.GetValue<string>("PostgressDBConnectionSetting:database"));
                stringBuilder.AppendFormat("uid={0};", configuration.GetValue<string>("PostgressDBConnectionSetting:uid"));
                stringBuilder.AppendFormat("Password={0};", configuration.GetValue<string>("PostgressDBConnectionSetting:password"));
            }
            catch(Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return stringBuilder.ToString();
        }
        public async Task<TorrentPayBankOTPUrlDetailEntity[]> GetBankOtpUrls(int cliendId, bool isNetbankingDetailsRequired)
        {
            TorrentPayBankOTPUrlDetailEntity[] lsBankOtpUrlEntity = new TorrentPayBankOTPUrlDetailEntity[] { };
            List<TorrentPayBankOTPUrlDetailEntity> lsBankOTPUrlPostgressEntity = null;
            try
            {
                if(await InitDB())
                {
                    using (var cmd =cliendId==0?new NpgsqlCommand("torrentpay_spgetbankurldetails", m_con): new NpgsqlCommand("torrentpay_spgetbankurldetailsForIos", m_con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (var m_reader = await cmd.ExecuteReaderAsync())
                        {
                            if (m_reader.HasRows)
                            {
                                lsBankOTPUrlPostgressEntity = new List<TorrentPayBankOTPUrlDetailEntity>();
                                while (m_reader.Read())
                                {
                                    if (isNetbankingDetailsRequired == true)
                                    {
                                        lsBankOTPUrlPostgressEntity.Add(new TorrentPayBankOTPUrlDetailEntity
                                        {
                                            url = Convert.ToString(m_reader["BANK_URL"]),
                                            isPageToBeResponsive = Convert.ToBoolean(m_reader["IS_PAGE_TO_BE_RESPONSIVE"]),
                                            isOTPUrl = Convert.ToBoolean(m_reader["IS_OTP_URL"]),
                                            isPageNetBankingSubmissionPage = Convert.ToBoolean(m_reader["IS_PAGE_NETBANKING_SUBMISSION_PAGE"]),
                                            strPageCustomizationJsCode = Convert.ToString(m_reader["PAGE_CUSTOMIZATION_JS_CODE"]),
                                            strNetBankingSubmissionJsCode = Convert.ToString(m_reader["NET_BANKING_PAGE_SUBMISSIOON_JS_CODE"]),
                                            iOtpLength = Convert.ToInt32(m_reader["OTP_LENGTH"]),
                                            isNetbankingLogin = Convert.ToBoolean(m_reader["IS_NETBANKING_LOGIN_PAGE"]),
                                            isNetbankingPage = Convert.ToBoolean(m_reader["IS_NETBANKING_PAGE"]),
                                            strOtpSubmissionJSCode = Convert.ToString(m_reader["OTP_PAGE_SUBMISSION_JS_CODE"]),
                                            jsCodeToCustomerId = Convert.ToString(m_reader["JS_CODE_TO_GET_CUSTOMER_ID"]),
                                            setJsCodeToCustomerId = Convert.ToString(m_reader["JS_CODE_TO_SET_CUSTOMER_ID"]),
                                            IsAutoOtpSubmitDisabled = Convert.ToBoolean(m_reader["IS_AUTO_OTP_SUBMIT_DISABLED"] == DBNull.Value ? false : m_reader["IS_AUTO_OTP_SUBMIT_DISABLED"]),
                                            IsGenricOtpFillingJsCodeDisabled = Convert.ToBoolean(m_reader["IS_GENERIC_OTP_FILLING_JS_CODE_DISABLED"] == DBNull.Value ? false : m_reader["IS_GENERIC_OTP_FILLING_JS_CODE_DISABLED"]),
                                            IsOtpCheckEnabled= Convert.ToBoolean(m_reader["is_otp_check_enabled"] == DBNull.Value ? false : m_reader["is_otp_check_enabled"]),
                                            strOtpCheckJsCode= Convert.ToString(m_reader["otp_PAGE_check_js_code"])

                                        });
                                    }
                                    else
                                    {
                                        lsBankOTPUrlPostgressEntity.Add(new TorrentPayBankOTPUrlDetailEntity
                                        {
                                            url = Convert.ToString(m_reader["BANK_URL"]),
                                            isPageToBeResponsive = Convert.ToBoolean(m_reader["IS_PAGE_TO_BE_RESPONSIVE"]),
                                            isOTPUrl = Convert.ToBoolean(m_reader["IS_OTP_URL"]),
                                            strPageCustomizationJsCode = Convert.ToString(m_reader["PAGE_CUSTOMIZATION_JS_CODE"]),
                                            iOtpLength = Convert.ToInt32(m_reader["OTP_LENGTH"]),
                                            strOtpSubmissionJSCode = Convert.ToString(m_reader["OTP_PAGE_SUBMISSION_JS_CODE"]),
                                            IsAutoOtpSubmitDisabled = Convert.ToBoolean(m_reader["IS_AUTO_OTP_SUBMIT_DISABLED"] == DBNull.Value ? false : m_reader["IS_AUTO_OTP_SUBMIT_DISABLED"]),
                                            IsGenricOtpFillingJsCodeDisabled = Convert.ToBoolean(m_reader["IS_GENERIC_OTP_FILLING_JS_CODE_DISABLED"] == DBNull.Value ? false : m_reader["IS_GENERIC_OTP_FILLING_JS_CODE_DISABLED"])
                                        });
                                    }
                                }
                            }
                        }
                    }
                    if(lsBankOTPUrlPostgressEntity != null)
                    {
                        lsBankOtpUrlEntity= lsBankOTPUrlPostgressEntity.Cast<TorrentPayBankOTPUrlDetailEntity>().ToArray();
                    }
                }
                
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());

            }
            finally
            {
                if (m_con != null)
                {
                    m_con.Dispose();
                }
                if (m_command != null)
                {
                    m_command.Dispose();
                }
            }
            return lsBankOtpUrlEntity;
        }

       
        public async Task<bool> InsertEdgeBroserSessionDetails(TorrentPaySessionDetailsEntity androidSdkSessionDetailsEntity)
        {
            bool result = false;
            try
            {
                if (await InitDB())
                {
                   using (var cmd = new NpgsqlCommand("torrentpay_spinserttorrentpaytxndetails", m_con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("imerchantid", NpgsqlDbType.Integer).Value = androidSdkSessionDetailsEntity.m_iMerchantid;
                        cmd.Parameters.Add("strmerchanttxnid", NpgsqlDbType.Varchar, 100).Value = androidSdkSessionDetailsEntity.m_strTransactionId ?? string.Empty;
                        cmd.Parameters.Add("iclienttypeid", NpgsqlDbType.Integer).Value = androidSdkSessionDetailsEntity.m_iClientId;
                        cmd.Parameters.Add("strphoneno", NpgsqlDbType.Varchar, 250).Value = androidSdkSessionDetailsEntity.m_strOrderId ?? string.Empty;
                        cmd.Parameters.Add("strcustomeremail", NpgsqlDbType.Varchar, 100).Value = androidSdkSessionDetailsEntity.m_strCustomerEmail ?? string.Empty;
                        cmd.Parameters.Add("strcustomerid", NpgsqlDbType.Varchar, 100).Value = androidSdkSessionDetailsEntity.m_strCustomeId ?? string.Empty;
                        cmd.Parameters.Add("strremarks", NpgsqlDbType.Varchar, 4000).Value = androidSdkSessionDetailsEntity.m_strRemarks ?? string.Empty;
                        cmd.Parameters.Add("strpaymentstarturl", NpgsqlDbType.Varchar, 250).Value = androidSdkSessionDetailsEntity.m_strPaymentStartUrl ?? string.Empty;
                        cmd.Parameters.Add("strreturnurl", NpgsqlDbType.Varchar, 250).Value = androidSdkSessionDetailsEntity.m_strPaymentReturnUrls ?? string.Empty;
                        cmd.Parameters.Add("strpostdata", NpgsqlDbType.Varchar, 4000).Value = androidSdkSessionDetailsEntity.m_strPostData ?? string.Empty;
                        cmd.Parameters.Add("strcustomparameter", NpgsqlDbType.Varchar, 100).Value = androidSdkSessionDetailsEntity.m_strCustomeId ?? string.Empty;
                        cmd.Parameters.Add("bispaymentsuccessful", NpgsqlDbType.Boolean).Value = androidSdkSessionDetailsEntity.m_bISPaymentSuccessful ?? false;
                        cmd.Parameters.Add("strlastvisitedurl", NpgsqlDbType.Varchar, 250).Value = androidSdkSessionDetailsEntity.m_strLastVisitedUrl ?? string.Empty;
                        cmd.Parameters.Add("bisbackpressed", NpgsqlDbType.Boolean).Value = androidSdkSessionDetailsEntity.m_bIsBackPressed ?? false;
                        cmd.Parameters.Add("bistransactionwascompleted", NpgsqlDbType.Boolean).Value = androidSdkSessionDetailsEntity.m_bIsTransactionWasCompleted ?? false;
                        cmd.Parameters.Add("bisfetchedotpurldetails", NpgsqlDbType.Boolean).Value = androidSdkSessionDetailsEntity.m_bIsFetchedOtpDetails ?? false;
                        cmd.Parameters.Add("bisotpdetected", NpgsqlDbType.Boolean).Value = androidSdkSessionDetailsEntity.m_bIsOtpDetected ?? false;
                        cmd.Parameters.Add("bhavesmspermission", NpgsqlDbType.Boolean).Value = androidSdkSessionDetailsEntity.m_bHaveSmsPermission ?? false;
                        cmd.Parameters.Add("bisotpapproved", NpgsqlDbType.Boolean).Value = androidSdkSessionDetailsEntity.m_bIsOtpApproved ?? false;
                        cmd.Parameters.Add("ierrorcode", NpgsqlDbType.Integer).Value = androidSdkSessionDetailsEntity.m_iErrorCode ?? 0;
                        cmd.Parameters.Add("strerrormessage", NpgsqlDbType.Varchar, 250).Value = androidSdkSessionDetailsEntity.m_strErrorMessage ?? string.Empty;
                        cmd.Parameters.Add("strborwesersessionstarttime", NpgsqlDbType.Varchar, 30).Value = androidSdkSessionDetailsEntity.m_strBrowserSessionStartTime ?? string.Empty;
                        cmd.Parameters.Add("strbrowsersesssionfinishtime", NpgsqlDbType.Varchar, 30).Value = androidSdkSessionDetailsEntity.m_strBrowserSessionFinishTime ?? string.Empty;
                        cmd.Parameters.Add("ithemeid", NpgsqlDbType.Integer).Value = androidSdkSessionDetailsEntity.m_iThemeId;
                        cmd.Parameters.Add("ilanguageid", NpgsqlDbType.Integer).Value = androidSdkSessionDetailsEntity.m_iLanguageId;
                        cmd.Parameters.Add("strorderid", NpgsqlDbType.Varchar, 4000).Value = androidSdkSessionDetailsEntity.m_strOrderId ?? string.Empty;
                        result = await cmd.ExecuteNonQueryAsync() > 0;
                        result = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + Ex.TargetSite);
                this._logger.LogError(Ex.ToString());
            }
            finally
            {
                if (m_con != null)
                {
                    m_con.Dispose();
                }
                if (m_command != null)
                {
                    m_command.Dispose();
                }
            }
            return result;
        }

        public async Task<bool> InertNewJsExceptionDetails(TorrentPayJsExceptionEntity edgeXceedJsExceptionEntity)
        {
            bool status = false;
            try
            {
                if (await InitDB()) {
 
                    using (var cmd = new NpgsqlCommand("torrentpay_speinsertandroidsdkjsexception", m_con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("strexceptionurl", NpgsqlDbType.Varchar, 250).Value = edgeXceedJsExceptionEntity.m_strExceptionUrl;
                        cmd.Parameters.Add("strjscodeexecuted", NpgsqlDbType.Text).Value = edgeXceedJsExceptionEntity.m_strExecutedJsCode ?? string.Empty;
                        cmd.Parameters.Add("ijscodetypeid", NpgsqlDbType.Integer).Value = edgeXceedJsExceptionEntity.m_iExecutedJsCodeTypeId;
                        cmd.Parameters.Add("strhtmlofurlofexception", NpgsqlDbType.Text).Value = edgeXceedJsExceptionEntity.m_strHtmlOfExceptionUrl;
                        cmd.Parameters.Add("strexceptionmessage", NpgsqlDbType.Text).Value = edgeXceedJsExceptionEntity.m_strExceptionMessage;
                        cmd.Parameters.Add("iclienttypeid", NpgsqlDbType.Integer).Value = edgeXceedJsExceptionEntity.m_iClientTypeId;
                        cmd.Parameters.Add("strclientversion", NpgsqlDbType.Varchar, 100).Value = edgeXceedJsExceptionEntity.m_strClientVersion;
                        using (var m_reader = await cmd.ExecuteReaderAsync())
                        {
                            if (m_reader.HasRows)
                            {

                                while (m_reader.Read())
                                {
                                    status = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + Ex.TargetSite);
                this._logger.LogError(Ex.ToString());
            }
            finally
            {
                if (m_con != null)
                {
                    m_con.Dispose();
                }
                if (m_command != null)
                {
                    m_command.Dispose();
                }
            }
            return status;
        }
        public async Task<bool> ChangeTorrentPayTxnStatus(TorrentPayChangeTxnStatusEntity changeTxnStatusEntity)
        {
            bool status = false;
            try
            {
                if (await InitDB())
                {

                    using (var cmd = new NpgsqlCommand("torrentpay_spchangetorrentpaytxnstatus", m_con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("struniquemerchanttxnid", NpgsqlDbType.Varchar, 100).Value = changeTxnStatusEntity.m_strTransactionId;
                        cmd.Parameters.Add("imerchantid", NpgsqlDbType.Integer).Value = int.Parse(changeTxnStatusEntity.m_iMerchantId);
                        cmd.Parameters.Add("ispaymentsuccessful", NpgsqlDbType.Boolean).Value = changeTxnStatusEntity.m_bTransactionStatus;
                        status = await cmd.ExecuteNonQueryAsync() > 0;
                        status = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + Ex.TargetSite);
                this._logger.LogError(Ex.ToString());
            }
            finally
            {
                if (m_con != null)
                {
                    m_con.Dispose();
                }
                if (m_command != null)
                {
                    m_command.Dispose();
                }
            }
            return status;
        }
        public  void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
