using ApelMusicAPI.Data;
using ApelMusicAPI.DTOs;
using ApelMusicAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutStateController : ControllerBase
    {
        private readonly CheckoutStateData checkoutStateData;

        public CheckoutStateController(CheckoutStateData checkoutStateData)
        {
            this.checkoutStateData = checkoutStateData;
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] UserClassesDTO userClassesDTO)
        {
            try
            {
                if (userClassesDTO == null) return BadRequest("Data should be inputed");

                UserClasses newUserClass = new UserClasses
                {
                    userId = userClassesDTO.userId,
                    classId = userClassesDTO.classId,
                    classSchedule = DateTime.Now
                };

                bool result = checkoutStateData.InsertToUserClass(newUserClass);

                if (result) return StatusCode(201);
                else return StatusCode(500, "Failed adding to cart.");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Checkout([FromBody] CheckoutDTO checkoutDTO)
        {
            bool resultOrder = false, resultOrderDetail = false;
            string invoiceNumber = "INV" + DateTime.Today.ToString("ddMMyyyyhmmss") + DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();

            try
            {
                if (checkoutDTO == null) return BadRequest("There is no order data");

                Orders order = new Orders
                {
                    orderId = Guid.NewGuid(),
                    invoiceNo =  invoiceNumber,
                    orderBy = checkoutDTO.orderBy,
                    paymentMethod = checkoutDTO.paymentMethod,
                };

                resultOrder = checkoutStateData.AddToOrders(order);

                foreach (int selectedClass in checkoutDTO.selectedClasses)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        invoiceNo = invoiceNumber,
                        classId = selectedClass
                    };

                    resultOrderDetail = checkoutStateData.AddToOrderDetail(orderDetail);

                    if (!resultOrderDetail)
                    {
                        return StatusCode(500, "Failed to checkout class by id = " + selectedClass);
                    }
                }

                if (resultOrder && resultOrderDetail) return StatusCode(201, "Success");
                else return StatusCode(500, "Error occur");
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
