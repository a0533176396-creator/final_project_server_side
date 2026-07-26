using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Functions;
using DTO.Models;

namespace tasks_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //-------------
        //שליפה
        //-------------
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            return Ok(UsersBLL.GetAllUsers());
        }


        //-------------------
        // שליפה לפי קוד
        //-------------------
        [HttpGet("GetUserById/{userId}")]
        public IActionResult GetUserById(int userId)
        {
            return Ok(UsersBLL.GetUserById(userId));
        }

        //-------------
        //הוספה
        //-------------
        [HttpPut("AddNewUser")]
        public IActionResult AddNewUser([FromBody] usersDTO userDTO)
        {
            return Ok(UsersBLL.AddNewUser(userDTO));
        }

        //-------------
        //עדכון
        //-------------
        [HttpPost("UpdateUser/{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] usersDTO userDTO)
        {
            return Ok(UsersBLL.UpdateUser(userId, userDTO));
        }

        //-------------
        //מחיקה
        //-------------
        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
          return Ok(UsersBLL.DeleteUser(userId));
        }
    }
}
