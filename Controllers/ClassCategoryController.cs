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

        [HttpGet("GetAllCategory")]
        public IActionResult GetAllCategory()
        {
            try
            {
                List<ClassCategory> categories = classCategoryData.GetAllCategory();
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

        [HttpGet("GetActiveInactiveCategory")]
        public IActionResult GetActiveInactiveCategoryById(int category_id)
        {
            ClassCategory? categoryById = classCategoryData.GetActiveInactiveCategoryById(category_id);

            if (categoryById == null) return StatusCode(404, "Data Not Found");

            return StatusCode(200, categoryById);
        }

        [HttpPost("AddCategory")]
        [Authorize(Roles = "admin")]
        public IActionResult InsertNewCategory([FromBody] ClassCategoryDTO classCategoryDTO)
        {
            if (classCategoryDTO == null) return BadRequest("Data should be inputed");

            ClassCategory newCategory = new ClassCategory
            {
                categoryImg = classCategoryDTO.categoryImg,
                categoryName = classCategoryDTO.categoryName,
                categoryDescription = classCategoryDTO.categoryDescription,
                isActive = classCategoryDTO.isActive
            };

            bool result = classCategoryData.InsertNewCategory(newCategory);

            if (result) return StatusCode(200, newCategory.categoryName);
            else return StatusCode(500, "Error Occur");
        }

        [HttpPut("UpdateCategory")]
        [Authorize(Roles = "admin")]
        public IActionResult UpdateCategory(int category_id, [FromBody] ClassCategoryDTO classCategoryDTO)
        {
            if (classCategoryDTO == null) return BadRequest("Data should be inputed");

            ClassCategory newCategory = new ClassCategory
            {
                categoryImg = classCategoryDTO.categoryImg,
                categoryName = classCategoryDTO.categoryName,
                categoryDescription = classCategoryDTO.categoryDescription,
                isActive = classCategoryDTO.isActive
            };

            bool result = classCategoryData.UpdateCategory(category_id, newCategory);

            if (result) return StatusCode(200, newCategory.categoryName);
            else return StatusCode(500, "Error Occur");
        }

        /*[HttpDelete("DeleteCategory")]
        [Authorize(Roles = "admin")]
        public IActionResult DeleteCategory(int category_id)
        {
            bool result = classCategoryData.DeleteById(category_id);

            if (result) return StatusCode(200);
            else return StatusCode(500, "Error Occur");
        }*/
    }
}
