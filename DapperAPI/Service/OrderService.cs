using DapperAPI.dto;
using DapperAPI.Repository;

namespace DapperAPI.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderSearchResp>> getOrdersByBuyerID(OrderSearchReq req)
        {
            return await _orderRepository.getOrdersByBuyerID(req);
        }

        public async Task<int> PlaceOrder(OrdersReq odr)
        {
            return await _orderRepository.PlaceOrder(odr);
        }

        public async Task<OrderDetails> getOrderFullByBuyerID(OrderSearchReq req)
        {
            return await _orderRepository.getOrderFullByBuyerID(req);
        }
    }
}
