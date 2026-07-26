using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Functions;
using DTO.Models;

namespace tasks_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
{
        //-------------
        //שליפה
        //-------------
        [HttpGet("GetAllCategories")]
        public IActionResult GetAllCategories()
        {
            return Ok(categoriesBLL.GetAllCategories());
        }

      //-------------------
  // שליפה לפי קוד
        //-------------------
        [HttpGet("GetCategoryById/{categoryId}")]
        public IActionResult GetCategoryById(int categoryId)
  {
       return Ok(categoriesBLL.GetCategoryById(categoryId));
   }

        //-------------
        //הוספה
        //-------------
        [HttpPut("AddNewCategory")]
        public IActionResult AddNewCategory([FromBody] categoriesDTO categoryDTO)
        {
            return Ok(categoriesBLL.AddNewCategory(categoryDTO));
        }

     //-------------
        //עדכון
     //-------------
        [HttpPost("UpdateCategory/{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] categoriesDTO categoryDTO)
        {
    return Ok(categoriesBLL.UpdateCategory(categoryId, categoryDTO));
        }

     //-------------
        //מחיקה
        //-------------
        [HttpDelete("DeleteCategory/{categoryId}")]
        public IActionResult DeleteCategory(int categoryId)
        {
        return Ok(categoriesBLL.DeleteCategory(categoryId));
        }
    }
}
