using ApelMusicAPI.Data;
using ApelMusicAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly OrderDetailData orderDetailData;

        public OrderDetailController(OrderDetailData orderDetailData)
        {
            this.orderDetailData = orderDetailData;
        }

        [HttpGet("GetOrderDetails")]
        public IActionResult GetOrderDetails(string invoice_no)
        {
            try
            {
                List<OrderDetailResponseDTO> orderDetails = orderDetailData.GetOrderDetails(invoice_no);
                return StatusCode(200, orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
