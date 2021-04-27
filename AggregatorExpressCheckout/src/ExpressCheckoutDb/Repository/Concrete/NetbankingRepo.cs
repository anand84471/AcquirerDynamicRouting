using ExpressCheckoutContracts.Enums;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ExpressCheckoutDb.Repository.Concrete
{
    public class NetbankingRepo : INetbankingRepo
    {
        private readonly IServiceProvider serviceProvider;

        public NetbankingRepo(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<string> GetNetbankingPaymentOptionCode(EnumGateway enumGateway, string paymentCode)
        {

            string paymentOptionCode = string.Empty;
            using (IDBServiceClient serviceClient = this.serviceProvider.GetService<IDBServiceClient>())
            {
                paymentOptionCode = await serviceClient._AggregatorExpressCheckoutServiceClient.GetNetbankingPaymentOptionCodeAsync((int)enumGateway, paymentCode);
            }
            return paymentOptionCode;
        }
    }
}
