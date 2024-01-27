﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ApelMusicAPI.Data;
using ApelMusicAPI.Models;
using ApelMusicAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
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

        [HttpPost("AddClass")]
        public IActionResult InsertNewClass([FromBody] ClassDTO classDTO)
        {
            if (classDTO == null) return BadRequest("Data should be inputed");

            Class newClass = new Class
            {
                classCategory = classDTO.classCategory,
                classImg = classDTO.classImg,
                className = classDTO.className,
                classDescription = classDTO.classDescription,
                classPrice = classDTO.classPrice,
                classStatus = classDTO.classStatus
            };

            bool result = classData.InsertNewClass(newClass);

            if (result) return StatusCode(200, newClass.className);
            else return StatusCode(500, "Error Occur");
        }

        [HttpPut("UpdateClass")]
        public IActionResult UpdateClass(int class_id, [FromBody] ClassDTO classDTO)
        {
            if (classDTO == null) return BadRequest("Data should be inputed");

            Class newClass = new Class
            {
                classCategory = classDTO.classCategory,
                classImg = classDTO.classImg,
                className = classDTO.className,
                classDescription = classDTO.classDescription,
                classPrice = classDTO.classPrice,
                classStatus = classDTO.classStatus
            };

            bool result = classData.UpdateClass(class_id, newClass);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }

        [HttpDelete("DeleteClass")]
        public IActionResult DeleteClass(int class_id)
        {
            bool result = classData.DeleteById(class_id);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }
    }
}
