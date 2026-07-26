using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.Functions;
using DTO.Models;

namespace tasks_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        //-------------
        //שליפה
        //-------------
        [HttpGet("GetAllMessages")]
        public IActionResult GetAllMessages()
        {
            return Ok(MessageBLL.GetAllMessages());
        }

  //-------------------
        // שליפה לפי קוד
        //-------------------
        [HttpGet("GetMessageById/{messageId}")]
  public IActionResult GetMessageById(int messageId)
     {
     return Ok(MessageBLL.GetMessageById(messageId));
        }

   //-------------
        //הוספה
        //-------------
        [HttpPut("AddNewMessage")]
     public IActionResult AddNewMessage([FromBody] MessageDTO messageDTO)
        {
  return Ok(MessageBLL.AddNewMessage(messageDTO));
        }

     //-------------
  //עדכון
 //-------------
        [HttpPost("UpdateMessage/{messageId}")]
        public IActionResult UpdateMessage(int messageId, [FromBody] MessageDTO messageDTO)
        {
   return Ok(MessageBLL.UpdateMessage(messageId, messageDTO));
     }

        //-------------
        //מחיקה
        //-------------
    [HttpDelete("DeleteMessage/{messageId}")]
        public IActionResult DeleteMessage(int messageId)
   {
   return Ok(MessageBLL.DeleteMessage(messageId));
        }
    }
}
