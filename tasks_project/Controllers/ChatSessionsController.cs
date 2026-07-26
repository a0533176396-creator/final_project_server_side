using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Functions;
using DTO.Models;

namespace tasks_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  public class ChatSessionsController : ControllerBase
    {
   //-------------
        //שליפה
        //-------------
     [HttpGet("GetAllChatSessions")]
        public IActionResult GetAllChatSessions()
     {
         return Ok(ChatSessionBLL.GetAllChatSessions());
        }

   //-------------------
        // שליפה לפי קוד
        //-------------------
   [HttpGet("GetChatSessionById/{sessionId}")]
        public IActionResult GetChatSessionById(short sessionId)
     {
   return Ok(ChatSessionBLL.GetChatSessionById(sessionId));
        }

 //-------------
        //הוספה
   //-------------
 [HttpPut("AddNewChatSession")]
        public IActionResult AddNewChatSession([FromBody] ChatSessionDTO sessionDTO)
     {
  return Ok(ChatSessionBLL.AddNewChatSession(sessionDTO));
 }

        //-------------
    //עדכון
 //-------------
[HttpPost("UpdateChatSession/{sessionId}")]
    public IActionResult UpdateChatSession(short sessionId, [FromBody] ChatSessionDTO sessionDTO)
        {
       return Ok(ChatSessionBLL.UpdateChatSession(sessionId, sessionDTO));
        }

        //-------------
        //מחיקה
        //-------------
     [HttpDelete("DeleteChatSession/{sessionId}")]
      public IActionResult DeleteChatSession(short sessionId)
        {
            return Ok(ChatSessionBLL.DeleteChatSession(sessionId));
    }
    }
}
