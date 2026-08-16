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
        // ==========================================================
        // שליפת היסטוריית שיחה (מחזיר רשימה של DTO)
        // ==========================================================
        [HttpGet("session/{sessionId}")]
        public IActionResult GetSessionMessages(int sessionId)
        {
            try
            {
                // קריאה ל-BLL שמטפל בשליפה מה-SQL, הורדה מגוגל והמרה ל-DTO
                List<MessageDTO> messages = MessageBLL.GetSessionMessagesDTO(sessionId);

                return Ok(messages); // מחזיר JSON ללקוח
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בשליפת היסטוריית ההודעות: {ex.Message}");
            }
        }

        // ==========================================================
        // שליחת הודעה חדשה ל-AI וקבלת תשובה (מחזיר DTO בודד)
        // ==========================================================
        [HttpPost("send")]
        public async Task<IActionResult> SendMessageToAI([FromBody] SendMessageRequest request)
        {
            try
            {
                // בדיקת תקינות בסיסית (Validation)
                if (request == null || string.IsNullOrWhiteSpace(request.Text))
                {
                    return BadRequest("ההודעה אינה יכולה להיות ריקה.");
                }

                // קריאה אסינכרונית ל-BLL שמנהל את כל התהליך (שמירה, פנייה ל-AI, והמרה)
                MessageDTO aiResponse = await MessageBLL.SendMessageAndGetReplyAsync(
                    request.SessionId, 
                    request.Text,
                    request.UserId
                );

                return Ok(aiResponse); // מחזיר את תשובת ה-AI ללקוח
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"שגיאה בתקשורת עם העוזר הווירטואלי: {ex.Message}");
            }
        }
    }
}
