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
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;    
        }
        [HttpPost("PlaceOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> PlaceOrder(OrdersReq odrReq)
        {
            return Ok(await _orderService.PlaceOrder(odrReq));
        }

        [HttpPost("getOrdersByBuyerID")]
        [AllowAnonymous]
        public async Task<IActionResult> getOrdersByBuyerID(OrderSearchReq odrReq)
        {
            return Ok(await _orderService.getOrdersByBuyerID(odrReq));
        }

        [HttpPost("getOrdersFullByBuyerID")]
        [AllowAnonymous]
        public async Task<IActionResult> getOrdersFullByBuyerID(OrderSearchReq odrReq)
        {
            return Ok(await _orderService.getOrderFullByBuyerID(odrReq));
        }

    }
}
