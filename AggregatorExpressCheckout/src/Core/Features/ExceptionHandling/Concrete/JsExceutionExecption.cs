using Core.Features.ExceptionHandling.Abstract;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Core.Features.ExceptionHandling.Concrete
{
   public class JsExceutionExecption : MasterException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBException"/> class.
        /// </summary>
        /// <param name="responseCode">The response code.</param>
        public JsExceutionExecption(int responseCode) : base(responseCode)
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
