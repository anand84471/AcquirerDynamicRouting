using Core.Infrastructure.Entities;

namespace ExpressCheckoutDb.Entities.Concrete
{
    public class ResponseCodes : IEntity
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public ResponseCodes()
        {

        }

        public ResponseCodes(int key, string message)
        {
            Code = key;
            Message = message;

        }
    }
}
