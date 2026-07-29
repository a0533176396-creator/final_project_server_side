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
        public IActionResult GetUserById(string userId)
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

        //--------------------------------------------
        // Validate User Full Name and Password
        //--------------------------------------------
        [HttpPost("ValidateUserFullNameAndPassword")]
        public IActionResult ValidateUserFullNameAndPassword([FromBody] ValidateUserRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.FirstName) || 
       string.IsNullOrWhiteSpace(request.LastName) || string.IsNullOrWhiteSpace(request.Password))
         {
return BadRequest();
            }

  bool isValid = UsersBLL.ValidateUserFullNameAndPassword(request.FirstName, request.LastName, request.Password);
            return Ok(isValid);
        }
    }

    //--------------------------------------------
    // Request model for validation
    //--------------------------------------------
    public class ValidateUserRequest
    {
        public string FirstName { get; set; }
      public string LastName { get; set; }
        public string Password { get; set; }
    }
}
