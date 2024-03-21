namespace DapperAPI.Repository
{
    using DapperAPI.dto;
    public interface IOrderRepository
    {
        Task<int> PlaceOrder(OrdersReq odrReq);
        Task<IEnumerable<OrderSearchResp>> getOrdersByBuyerID(OrderSearchReq req);
        Task<OrderDetails> getOrderFullByBuyerID(OrderSearchReq req);
    }
}
