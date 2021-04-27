using Core.Cache.Abstract;
using Core.Constants;
using Core.Features.ExceptionHandling.Abstract;
using Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PinePGController.ExceptionHandling.CustomExceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;


        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly ICoreCache _CoreCache;

        public ExceptionMiddleware(RequestDelegate next, ICoreCache CoreCache, ILogger<ExceptionMiddleware> logger)
        {
           this. _logger = logger;
            _next = next;
            _CoreCache = CoreCache;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                 _logger.LogError($"Something went wrong: {ex}");
                await ProcessException(ex, httpContext);
            }
        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="httpContext">The HTTP context.</param>
        private async Task ProcessException(Exception exception, HttpContext httpContext)
        {
            var ex = exception as MasterException ?? this.WrapExceptionIntoGeneralException(exception);
            await SetResponseAccordingToException(ex, httpContext);
        }

        /// <summary>
        /// Sets the response according to exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="httpContext">The HTTP context.</param>
        private Task SetResponseAccordingToException(MasterException ex, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)ex.StatusCode;

            int responseCode = ResponseCodeConstants.INTERNAL_SERVER_ERROR;

            string responseMsg = String.Empty;
            if (ex.ResponseCodes != null && ex.ResponseCodes.Count > 0)
            {
                responseCode = ResponseCodeConstants.FAILURE;
                responseMsg = string.Join(",", ex.ResponseCodes.Select(s => _CoreCache.GetResponseMsg(Convert.ToInt32(s))).Where(s => !String.IsNullOrEmpty(s)).ToArray());
                if (String.IsNullOrEmpty(responseMsg))
                {
                    responseMsg = String.Empty;
                }
           

                _logger.LogError($"Validation Failed with Message "+ responseMsg);
            }
            else
            {
                responseCode = ex.ResponseCode;
                responseMsg = _CoreCache.GetResponseMsg(ex.ResponseCode);
            }

            var masterResponse = new ExceptionResponse
            {
                Code = responseCode,
                Message = responseMsg
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(masterResponse));
        }

        /// <summary>
        /// Wraps the exception into general exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        private MasterException WrapExceptionIntoGeneralException(Exception ex)
        {
            Exception exception = ex;
            //strBuilder.Append(ex.ToString());
            //LogMessage.LogDebugMessage(strBuilder);
            while (exception?.InnerException != null)
            {
                exception = exception.InnerException;
            }
            // log this exception.Message
            return new UnhandeledException(ResponseCodeConstants.INTERNAL_SERVER_ERROR);
        }
    }
}