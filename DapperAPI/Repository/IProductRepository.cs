namespace DapperAPI.Repository
{
    using DapperAPI.dto;
    public interface IProductRepository
    {
        public Task<IEnumerable<ProductSearchResp>> Search(ProductSearchReq productSearchReq);
        public Task<IEnumerable<ProductSearchResp>> RecentByBuyerID(ProductSearchReq productSearchReq);
        Task<ProductRespById> GetByID(int pid);
    }
}
