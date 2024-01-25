﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ApelMusicAPI.Data;
using ApelMusicAPI.Models;
using ApelMusicAPI.DTOs;

namespace ApelMusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassCategoryController : ControllerBase
    {
        private readonly ClassCategoryData classCategoryData;

        public ClassCategoryController(ClassCategoryData classCategoryData)
        {
            this.classCategoryData = classCategoryData;
        }


        [HttpGet("GetCategories")]
        public IActionResult GetCategories()
        {
            try
            {
                List<ClassCategory> categories = classCategoryData.GetCategories();
                return StatusCode(200, categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetCategory")]
        public IActionResult GetCategoryById(int category_id)
        {
            ClassCategory? categoryById = classCategoryData.GetById(category_id);

            if (categoryById == null) return StatusCode(404, "Data Not Found");

            return StatusCode(200, categoryById);
        }

        [HttpPost("AddCategory")]
        public IActionResult InsertNewCategory([FromBody] ClassCategoryDTO classCategoryDTO)
        {
            if (classCategoryDTO == null) return BadRequest("Data should be inputed");

            ClassCategory newCategory = new ClassCategory
            {
                categoryImg = classCategoryDTO.categoryImg,
                categoryName = classCategoryDTO.categoryName
            };

            bool result = classCategoryData.InsertNewCategory(newCategory);

            if (result) return StatusCode(200, newCategory.categoryName);
            else return StatusCode(500, "Error Occur");
        }

        [HttpPut("UpdateCategory")]
        public IActionResult UpdateCategory(int category_id, [FromBody] ClassCategoryDTO classCategoryDTO)
        {
            if (classCategoryDTO == null) return BadRequest("Data should be inputed");

            ClassCategory newCategory = new ClassCategory
            {
                categoryImg = classCategoryDTO.categoryImg,
                categoryName = classCategoryDTO.categoryName
            };

            bool result = classCategoryData.UpdateCategory(category_id, newCategory);

            if (result) return StatusCode(200, newCategory.categoryName);
            else return StatusCode(500, "Error Occur");
        }

        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int category_id)
        {
            bool result = classCategoryData.DeleteById(category_id);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }
    }
}
