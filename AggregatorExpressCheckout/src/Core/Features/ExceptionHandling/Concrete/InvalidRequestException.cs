using Core.Features.ExceptionHandling.Abstract;
using System.Collections.Generic;
using System.Net;

namespace Core.Features.ExceptionHandling.Concrete
{
    public class InvalidRequestException : MasterException
    {
        public InvalidRequestException(int responseCode) : base(responseCode)
        {
        }

        public InvalidRequestException(List<int> responseCodes) : base(responseCodes)
        {
        }

        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}