using AggExpressCheckoutDBService;
using AutoMapper;
using ExpressCheckoutContracts.DTO;
using ExpressCheckoutDb.DBClients.Abstarct;
using ExpressCheckoutDb.Repository.Abstract;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ExpressCheckoutContracts.Requests;
using ExpressCheckoutContracts.Enums;
using System.Text;
using Microsoft.Extensions.Logging;

namespace ExpressCheckoutDb.Repository.Concrete
{
    public class OrderRepo : IOrderRepo
    {
        private readonly IMapper _mapper;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<OrderRepo> _logger;

        /// <summary>Initializes a new instance of the <see cref="CustomerRepo"/> class.</summary>
        /// <param name="aggregatorExpressCheckoutServiceClient">The aggregator express checkout service client.</param>
        public OrderRepo(IMapper mapper, IServiceProvider serviceProvider,
            ILogger<OrderRepo> _logger)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            this._logger = _logger;
        }

        public Task<bool> CheckMerchantOrderIdExist(MerchantDto merchantDto)
        {
            throw new NotImplementedException();
        }

        public async Task<long> CreateOrder(OrderDetailsDto orderDetailsDto)
        {
            long AggOrderId = 0;
            try
            {
                OrderDetailsEntity orderDetailsEntity = _mapper.Map<OrderDetailsEntity>(orderDetailsDto);
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    AggOrderId = await serviceClient._AggregatorExpressCheckoutServiceClient.CreateOrderAsync(orderDetailsEntity);
                }


            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return AggOrderId;
        }
        public async Task<bool> UpdatePurchaseOrder(OrderDetailsDto orderDetailsDto)
        {
            bool status = false;
            OrderDetailsEntity orderDetailsEntity = _mapper.Map<OrderDetailsEntity>(orderDetailsDto);
            using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
            {
                status = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdatePurchaseOrderDetailsAsync(orderDetailsEntity);
            }
            return status;
        }
        
        public async Task<OrderDetailsDto> GetOrderDetails(long AggOrderId)
        {
            OrderDetailsDto orderDetailsDto = null;
            OrderDetailsEntity orderDetailsEntity = null;

            try
            {
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    orderDetailsEntity = await serviceClient._AggregatorExpressCheckoutServiceClient.GetOrderDetailsAsync(AggOrderId);
                }
                if (orderDetailsEntity != null)
                {
                    orderDetailsDto = _mapper.Map<OrderDetailsDto>(orderDetailsEntity);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }

            return orderDetailsDto;
        }



        public async Task<OrderPaymentDetailsDto> GetAggregatorOrderDetailsByGatewayOrderIdAsync(EnumGateway enumGateway, string gatewayOrderId)
        {
            OrderPaymentDetailsDto orderPaymentDetailsDto = null;
            OrderPaymentDetails orderPaymentDetails = null;

            try
            {
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    orderPaymentDetails = await serviceClient._AggregatorExpressCheckoutServiceClient.GetAggregatorOrderDetailsByGatewayOrderIdAsync((int)enumGateway, gatewayOrderId);
                }

                if (orderPaymentDetails != null)
                {
                    orderPaymentDetailsDto = _mapper.Map<OrderPaymentDetailsDto>(orderPaymentDetails);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }

            return orderPaymentDetailsDto;
        }

        public async Task<long> InsertOrderPaymentDetailsAndStepDetails(DoPaymentRequest doPaymentRequest, OrderDetailsDto orderDetails)
        {
            OrderPaymentDetails orderPaymentDetailsInsert = new OrderPaymentDetails
            {
                AggOrderId = orderDetails.OrderTxnInfoDto.AggOrderId,
                GatewayId = (int)doPaymentRequest.PaymentDetailsRequest.PrefferedGatewayCode,
                PaymentModeId = (int)doPaymentRequest.PaymentDetailsRequest.PaymentMode,
                RequestType = 1
            };
            long aggPaymentId = 0;

            try
            {
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    aggPaymentId = await serviceClient._AggregatorExpressCheckoutServiceClient.InsertOrderPaymentDetailsAndStepDetailsAsync(orderPaymentDetailsInsert);
                }
            }

            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }

            return aggPaymentId;
        }

        public async Task<long> InsertPaymentDetails(OrderPaymentDetailsDto orderPaymentDetailsDto)
        {
            long aggPaymentId = 0;
            try
            {
                OrderPaymentDetails orderPaymentDetailsInsert = _mapper.Map<OrderPaymentDetails>(orderPaymentDetailsDto);
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    aggPaymentId = await serviceClient._AggregatorExpressCheckoutServiceClient.InsertOrderPaymentDetailsAndStepDetailsAsync(orderPaymentDetailsInsert);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return aggPaymentId;
        }

        public async Task InsertOrderPaymentStepDetails(OrderStepDto orderStepDto)
        {
            try
            {
                OrderStepEntity orderStepEntity = _mapper.Map<OrderStepEntity>(orderStepDto);
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                     await serviceClient._AggregatorExpressCheckoutServiceClient.InsertOrderPaymentStepDetailsAsync(orderStepEntity);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
                
            }

        }



        public async Task<bool> UpdateOrderPaymentDetails(OrderPaymentDetailsDto orderPaymentDetailsDto)
        {
            bool status = false;
            try
            {
                OrderPaymentDetailsInsert orderPaymentDetailsInsert = _mapper.Map<OrderPaymentDetailsInsert>(orderPaymentDetailsDto);
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateOrderPaymentDetailsAsync(orderPaymentDetailsInsert);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }

        public async Task<bool> UpdateOrderPaymentStepDetails(OrderStepDto orderStepDto)
        {
            bool status = false;
            try
            {
                OrderStepEntity orderStepEntity = _mapper.Map<OrderStepEntity>(orderStepDto);
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateOrderPaymentStepDetailsAsync(orderStepEntity);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }

        public async Task<bool> UpdateBinDataInOrderTbl(OrderDetailsDto orderDetailsDto,long aggOrderId)
        {
            bool status = false;
            OrderDetailsEntity orderDetailsEntity = _mapper.Map<OrderDetailsEntity>(orderDetailsDto);
            using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
            {
                status = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateGlobalBinDataInOrderTblAsync(orderDetailsEntity, aggOrderId);
            }
            return status;
        }
        public async Task<bool> UpdateOrderDetails(UpdateOrderDetailsDto updateOrderDetailsDto)
        {
            bool status = false;
            try
            {
                UpdateOrderDetails updateOrderDetails = _mapper.Map<UpdateOrderDetails>(updateOrderDetailsDto);
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateOrderDetailsAsync(updateOrderDetails);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }


        public async Task<bool> UpdateDependentOrderDetails(OrderPaymentDetailsDto orderPaymentDetailsDto,
            OrderStepDto orderStepDto, UpdateOrderDetailsDto updateOrderDetailsDto)
        {
            bool status = false;
            try
            {
                OrderPaymentDetailsInsert orderPaymentDetailsInsert = _mapper.Map<OrderPaymentDetailsInsert>(orderPaymentDetailsDto);
                UpdateOrderDetails updateOrderDetails = _mapper.Map<UpdateOrderDetails>(updateOrderDetailsDto);
                OrderStepEntity orderStepEntity = _mapper.Map<OrderStepEntity>(orderStepDto);

                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateDependentOrderDetailsAsync
                        (updateOrderDetails, orderPaymentDetailsInsert, orderStepEntity);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }


        public async Task<bool> UpdateParentTransactionOrderDetails(OrderPaymentDetailsDto orderPaymentDetailsDto,
         OrderStepDto orderStepDto, UpdateOrderDetailsDto updateOrderDetailsDto, UpdateOrderDetailsDto updatePurchaseOrderDetailsDto = null)
        {
            bool status = false;
            OrderPaymentDetailsInsert orderPaymentDetailsInsert = _mapper.Map<OrderPaymentDetailsInsert>(orderPaymentDetailsDto);
            UpdateOrderDetails updateOrderDetails = _mapper.Map<UpdateOrderDetails>(updateOrderDetailsDto);
            UpdateOrderDetails updatePurchaseOrderDetails = _mapper.Map<UpdateOrderDetails>(updatePurchaseOrderDetailsDto);
            OrderStepEntity orderStepEntity = _mapper.Map<OrderStepEntity>(orderStepDto);
            try
            {
                using (IDBServiceClient serviceClient = _serviceProvider.GetService<IDBServiceClient>())
                {
                    status = await serviceClient._AggregatorExpressCheckoutServiceClient.UpdateParentTransactiontOrderDetailsAsync
                        (updateOrderDetails, updatePurchaseOrderDetails, orderPaymentDetailsInsert, orderStepEntity);
                }
            }

            catch (Exception ex)
            {                
                this._logger.LogError("\n ----------------------------Exception Stack Trace--------------------------------------");
                this._logger.LogError("Exception occured in method :" + ex.TargetSite);
                this._logger.LogError(ex.ToString());
            }
            return status;
        }






    }
}