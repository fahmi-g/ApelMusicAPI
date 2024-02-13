using ApelMusicAPI.Data;
using ApelMusicAPI.DTOs;
using ApelMusicAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly PaymentMethodData paymentMethodData;

        public PaymentMethodController(PaymentMethodData paymentMethodData)
        {
            this.paymentMethodData = paymentMethodData;
        }

        [HttpGet("GetPaymentMethods")]
        public IActionResult GetPaymentMethods()
        {
            try
            {
                List<PaymentMethods> paymentMethods = paymentMethodData.GetPaymentMethods();
                return StatusCode(200, paymentMethods);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetPaymentMethod")]
        public IActionResult GetPaymentMethodById(int id)
        {
            PaymentMethods? paymentMethod = paymentMethodData.GetPaymentMethodById(id);

            if (paymentMethod == null) return StatusCode(404, "Data Not Found");

            return StatusCode(200, paymentMethod);
        }

        [HttpPost("AddPaymentMethod")]
        public IActionResult InsertNewCategory([FromBody] PaymentMethodsDTO paymentMethodsDTO)
        {
            if (paymentMethodsDTO == null) return BadRequest("Data should be inputed");

            PaymentMethods newPaymentMethod = new PaymentMethods
            {
                paymentName = paymentMethodsDTO.paymentName,
                paymentImg = paymentMethodsDTO.paymentImg,
                isActive = paymentMethodsDTO.isActive
            };

            bool result = paymentMethodData.InsertNewPaymentMethod(newPaymentMethod);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }

        [HttpPut("UpdatePaymentMethod")]
        public IActionResult UpdatePaymentMethod(int payment_id, [FromBody] PaymentMethodsDTO paymentMethodsDTO)
        {
            if (paymentMethodsDTO == null) return BadRequest("Data should be inputed");

            PaymentMethods newPaymentMethod = new PaymentMethods
            {
                paymentName = paymentMethodsDTO.paymentName,
                paymentImg = paymentMethodsDTO.paymentImg,
                isActive = paymentMethodsDTO.isActive
            };

            bool result = paymentMethodData.UpdatePaymentMethod(payment_id, newPaymentMethod);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }

        [HttpDelete("DeletePaymentMethod")]
        public IActionResult DeleteCategory(int payment_id)
        {
            bool result = paymentMethodData.DeletePaymentMethodById(payment_id);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }
    }
}
