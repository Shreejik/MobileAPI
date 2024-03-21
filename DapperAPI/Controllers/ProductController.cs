using API;
using DapperAPI.dto;
using DapperAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Security.Claims;

namespace DapperAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;    
        }

        [HttpPost("Search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(ProductSearchReq productSearchReq)
        {

            return Ok(await _productService.Search(productSearchReq));
        }
        
        [HttpPost("RecentByBuyerID")]
        [AllowAnonymous]
        public async Task<IActionResult> RecentByBuyerID(ProductSearchReq obj)
        {

            return Ok(await _productService.RecentByBuyerID(obj));
        }

        [HttpGet("GetByID")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByID(int pID)
        {

            return Ok(await _productService.GetByID(pID));
        }


    }
}
