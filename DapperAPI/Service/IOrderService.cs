namespace DapperAPI.Service
{
    using DapperAPI.dto;
    public interface IOrderService
    {   
        Task<int> PlaceOrder(OrdersReq odr);
        Task<IEnumerable<OrderSearchResp>> getOrdersByBuyerID(OrderSearchReq req);
        Task<OrderDetails> getOrderFullByBuyerID(OrderSearchReq req);
    }

}
