using Core.Features.ExceptionHandling.Abstract;
using System.Net;

namespace PinePGController.ExceptionHandling.CustomExceptions
{
    /// <summary>
    /// Custom exception for the DB exception like dataset errors
    /// </summary>
    /// <seealso cref="PinePGController.ExceptionHandling.CustomExceptions.MasterException" />
    public class DBException : MasterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBException"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        public DBException(int responseCode) : base(responseCode)
        {
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public override HttpStatusCode StatusCode
        {
            get
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}