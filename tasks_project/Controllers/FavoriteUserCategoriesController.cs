using BLL.Functions;
using DAL.Models;
using DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace tasks_project.Controllers
{
  [Route("api/[controller]")]
    [ApiController]
    public class FavoriteUserCategoriesController : ControllerBase
    {
      //-------------
        //שליפה
        //-------------
        [HttpGet("GetAllFavoriteUserCategories")]
        public IActionResult GetAllFavoriteUserCategories()
        {
            return Ok(favoriet_users_categoriesBLL.GetAllFavoriteUserCategories());
        }


        [HttpGet("GetAllFavoriteUserCategoriesByUserId/{userId}")]
        public IActionResult GetFavoriteUserCategories(int userId)
        {
            return Ok(favoriet_users_categoriesBLL.GetFavoriteCategoriesByUserId(userId));
        }


        //-------------------
        // שליפה לפי קוד
        //-------------------
        [HttpGet("GetFavoriteUserCategoryById/{favoriteId}")]
 public IActionResult GetFavoriteUserCategoryById(int favoriteId)
    {
    return Ok(favoriet_users_categoriesBLL.GetFavoriteUserCategoryById(favoriteId));
     }

  //-------------
        //הוספה
        //-------------
        [HttpPut("AddNewFavoriteUserCategory")]
    public IActionResult AddNewFavoriteUserCategory([FromBody] favoriet_users_categoriesDTO favoriteDTO)
        {
       return Ok(favoriet_users_categoriesBLL.AddNewFavoriteUserCategory(favoriteDTO));
        }

        //--------------------------------------------
        // הוספת קטגוריה חדשה לחלוטין ושיוכה למשתמש
        //--------------------------------------------
        [HttpPost("CreateAndLinkNewFavoriteCategory")]
        public IActionResult CreateAndLinkNewFavoriteCategory([FromBody] CreateAndLinkFavoriteCategoryDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            try
            {
                var result = favoriet_users_categoriesBLL.CreateAndLinkNewFavoriteCategory(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, errorMessage);
            }
        }

     //-------------
  //עדכון
        //-------------
        [HttpPost("UpdateFavoriteUserCategory/{favoriteId}")]
        public IActionResult UpdateFavoriteUserCategory(int favoriteId, [FromBody] favoriet_users_categoriesDTO favoriteDTO)
  {
       return Ok(favoriet_users_categoriesBLL.UpdateFavoriteUserCategory(favoriteId, favoriteDTO));
     }

        //-------------
        //מחיקה
   //-------------
 [HttpDelete("DeleteFavoriteUserCategory/{favoriteId}")]
 public IActionResult DeleteFavoriteUserCategory(int favoriteId)
        {
      return Ok(favoriet_users_categoriesBLL.DeleteFavoriteUserCategory(favoriteId));
   }
    }
}
