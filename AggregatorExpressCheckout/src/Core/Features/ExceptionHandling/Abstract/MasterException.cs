using System;
using System.Collections.Generic;
using System.Net;

namespace Core.Features.ExceptionHandling.Abstract
{
    public abstract class MasterException : Exception
    {
        /// <summary>
        /// The response code
        /// </summary>
        public readonly int ResponseCode;

        /// <summary>
        /// The error message
        /// </summary>
        public readonly string ResponseMessage;

        public readonly List<int> ResponseCodes;

        /// <summary>Initializes a new instance of the <see cref="MasterException"/> class.</summary>
        /// <param name="responseCode">The response code.</param>
        protected MasterException(int responseCode)
        {
            ResponseMessage = string.Empty;
            ResponseCode = responseCode;
        }

        protected MasterException(List<int> responseCodes)
        {
            ResponseCodes = responseCodes;
        }

        private string GetResponseMsg(int responseCode)
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public abstract HttpStatusCode StatusCode { get; }
    }
}