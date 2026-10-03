using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Functions;
using DTO.Models;
using DAL.Models;

namespace tasks_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatSessionsController : ControllerBase
    {
        //-------------
        // שליפה לפי קוד משתמש
        //-------------
        [HttpGet("GetUserSessions/{userId}")]
        public IActionResult GetUserSessions(int userId)
        {
            return Ok(ChatSessionBLL.GetUserSessions(userId));
        }

        //-------------
        // הוספת שיחה חדשה
        //-------------
        [HttpPost("CreateNewSession")]
        public IActionResult CreateNewSession(int userId, string title)
        {
            return Ok(ChatSessionBLL.CreateNewSession(userId, title));
        }

        //-------------
        // מחיקה
        //-------------
        [HttpDelete("DeleteSession/{sessionId}")]
        public IActionResult DeleteSession(int sessionId)
        {
            return Ok(ChatSessionBLL.DeleteSession(sessionId));
        }
    }
}
