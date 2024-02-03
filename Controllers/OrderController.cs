using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ApelMusicAPI.Data;
using ApelMusicAPI.DTOs;
using ApelMusicAPI.Models;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderData orderData;

        public OrderController(OrderData orderData)
        {
            this.orderData = orderData;
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders(Guid user_id)
        {
            try
            {
                List<OrderResponseDTO> orders = orderData.GetOrders(user_id);
                return StatusCode(200, orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
