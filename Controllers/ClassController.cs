using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ApelMusicAPI.Data;
using ApelMusicAPI.Models;
using ApelMusicAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {

        private readonly ClassData classData;

        public ClassController(ClassData classData)
        {
            this.classData = classData;
        }


        [HttpGet("GetClasses")]
        public IActionResult GetClasses()
        {
            try
            {
                List<Class> classes = classData.GetClasses();
                return StatusCode(200, classes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetClass")]
        public IActionResult GetClassById(int id)
        {
            Class? classById = classData.GetById(id);

            if (classById == null) return StatusCode(404, "Data Not Found");

            return StatusCode(200, classById);
        }

        [HttpGet("GetClassesByCategory")]
        public IActionResult GetClassesByCategoryId(int category_id)
        {
            List<Class> classByCategoryId = classData.GetClassesByCategoryId(category_id);

            if (classByCategoryId == null) return StatusCode(404, "Data Not Found");

            return StatusCode(200, classByCategoryId);
        }

        [HttpGet("GetUserClassesPaid")]
        public IActionResult GetUserClassByUserIdPaid(Guid user_id)
        {
            try
            {
                List<UserClassesPaidUnpaidDTO> userClasses = classData.GetUserClassesByUserId(user_id, true);
                return StatusCode(200, userClasses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetUserClassesUnPaid")]
        public IActionResult GetUserClassByUserIdUnPaid(Guid user_id)
        {
            try
            {
                List<UserClassesPaidUnpaidDTO> userClasses = classData.GetUserClassesByUserId(user_id, false);
                return StatusCode(200, userClasses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("AddClass")]
        //[Authorize(Roles = "admin")]
        public IActionResult InsertNewClass([FromBody] ClassDTO classDTO)
        {
            if (classDTO == null) return BadRequest("Data should be inputed");

            bool result = classData.InsertNewClass(classDTO);

            if (result) return StatusCode(201);
            else return StatusCode(500, "Error Occur");
        }

        [HttpPut("UpdateClass")]
        //[Authorize(Roles = "admin")]
        public IActionResult UpdateClass(int class_id, [FromBody] ClassDTO classDTO)
        {
            if (classDTO == null) return BadRequest("Data should be inputed");

            bool result = classData.UpdateClass(class_id, classDTO);

            if (result) return StatusCode(201);
            else return StatusCode(500, "Error Occur");
        }

        [HttpDelete("DeleteClass")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteClass(int class_id)
        {
            bool result = classData.DeleteById(class_id);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }
    }
}
