namespace PinePGController.ExceptionHandling.CustomExceptions
{
    using Core.Features.ExceptionHandling.Abstract;
    using System.Net;

    /// <summary>
    /// Custom exception for Unhandeled exceptions
    /// </summary>
    /// <seealso cref="PinePGController.ExceptionHandling.CustomExceptions.MasterException" />
    public class UnhandeledException : MasterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandeledException"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        /// <param name="message">The message.</param>
        public UnhandeledException(int responseCode)
            : base(responseCode)
        {
        }

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
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}