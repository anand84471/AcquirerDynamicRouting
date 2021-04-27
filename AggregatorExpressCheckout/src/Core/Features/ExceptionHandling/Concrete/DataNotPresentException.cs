using Core.Features.ExceptionHandling.Abstract;
using System.Net;

namespace PinePGController.ExceptionHandling.CustomExceptions
{
    public class DataNotPresentException : MasterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        public DataNotPresentException(int responseCode) : base(responseCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRequestException"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="message">The message.</param>

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public override HttpStatusCode StatusCode
        {
            get
            {
                return HttpStatusCode.BadRequest;
            }
        }
    }
}