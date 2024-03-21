using DapperAPI.dto;
using DapperAPI.Repository;
using System.Collections.Generic;

namespace DapperAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<ProductSearchResp>> Search(ProductSearchReq productSearchReq)
        {
            IEnumerable<ProductSearchResp> obj = await _productRepository.Search(productSearchReq);
            return obj;
        }
        public async Task<IEnumerable<ProductSearchResp>> RecentByBuyerID(ProductSearchReq obj)
        {
            return await _productRepository.RecentByBuyerID(obj);
        }

        async Task<ProductRespById> IProductService.GetByID(int pid)
        {
            return await _productRepository.GetByID(pid);
        }
    }
}
