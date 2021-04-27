using Core.Features.ExceptionHandling.Abstract;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Core.Features.ExceptionHandling.Concrete
{
    public class OrderException : MasterException
    {
        public OrderException(int responseCode) : base(responseCode)
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
                return HttpStatusCode.BadRequest;
            }
        }
    }
}