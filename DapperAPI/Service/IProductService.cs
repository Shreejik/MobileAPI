namespace DapperAPI.Service
{
    using DapperAPI.dto;
    public interface IProductService
    {
        Task<IEnumerable<ProductSearchResp>> Search(ProductSearchReq productSearchReq);
        Task<IEnumerable<ProductSearchResp>> RecentByBuyerID(ProductSearchReq obj);
        Task<ProductRespById> GetByID(int pid);
    }

}
