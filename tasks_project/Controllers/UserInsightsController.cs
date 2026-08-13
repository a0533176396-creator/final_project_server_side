using Microsoft.AspNetCore.Mvc;
using BLL.Functions;
using DTO.Models;
using System;
using System.Collections.Generic;

namespace tasks_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInsightsController : ControllerBase
    {
       // ==========================================================
        // שליפת כל התובנות של משתמש
     // ==========================================================
   [HttpGet("user/{userId}")]
        public IActionResult GetUserInsights(int userId)
        {
       try
            {
     var insights = UserInsightBLL.GetUserInsights(userId);
   return Ok(insights);
   }
    catch (Exception ex)
      {
           return StatusCode(500, $"שגיאה בשליפת התובנות: {ex.Message}");
      }
        }

// ==========================================================
   // הוספת תובנה חדשה (בדרך כלל קוראת מ-AI)
        // ==========================================================
     [HttpPost]
        public IActionResult AddInsight([FromBody] AddInsightRequest request)
 {
         try
           {
               if (request == null || string.IsNullOrWhiteSpace(request.InsightText))
      {
          return BadRequest("הטקסט של התובנה לא יכול להיות ריק.");
           }

  var newInsight = UserInsightBLL.AddUserInsight(
    request.UserId,
      request.InsightText,
  request.Category
    );

    return Ok(newInsight);
   }
      catch (Exception ex)
             {
    return StatusCode(500, $"שגיאה בהוספת התובנה: {ex.Message}");
         }
   }

   // ==========================================================
  // עדכון בטחון בתובנה (משתמש אישר או הפריך)
   // ==========================================================
        [HttpPut("{insightId}/confirm")]
        public IActionResult ConfirmInsight(int insightId, [FromBody] ConfirmInsightRequest request)
        {
      try
           {
   UserInsightBLL.ConfirmInsight(insightId, request.IsCorrect);
      return Ok(new { message = "התובנה עודכנה בהצלחה." });
  }
           catch (Exception ex)
   {
       return StatusCode(500, $"שגיאה בעדכון התובנה: {ex.Message}");
         }
        }

  // ==========================================================
    // מחיקת תובנה בודדת
        // ==========================================================
      [HttpDelete("{insightId}")]
   public IActionResult DeleteInsight(int insightId)
    {
         try
 {
    bool isDeleted = UserInsightBLL.DeleteInsight(insightId);
       if (isDeleted)
     return Ok(new { message = "התובנה נמחקה בהצלחה." });
       
     return NotFound("התובנה לא נמצאה.");
  }
       catch (Exception ex)
         {
return StatusCode(500, $"שגיאה במחיקת התובנה: {ex.Message}");
      }
     }

        // ==========================================================
   // איפוס זיכרון של משתמש (מחיקת כל התובנות)
      // ==========================================================
  [HttpDelete("user/{userId}/reset")]
       public IActionResult ResetUserMemory(int userId)
        {
         try
         {
   bool isReset = UserInsightBLL.ResetUserMemory(userId);
         if (isReset)
      return Ok(new { message = "הזיכרון של המשתמש אופס בהצלחה." });
  
       return Ok(new { message = "אין תובנות למחוק." });
      }
 catch (Exception ex)
     {
      return StatusCode(500, $"שגיאה באיפוס הזיכרון: {ex.Message}");
    }
        }
    }

 /// <summary>
   /// בקשה להוספת תובנה חדשה
    /// </summary>
   public class AddInsightRequest
    {
 public int UserId { get; set; }
     public string InsightText { get; set; }
        public string? Category { get; set; }
    }

  /// <summary>
   /// בקשה לעדכון בטחון בתובנה
 /// </summary>
    public class ConfirmInsightRequest
   {
      public bool IsCorrect { get; set; }
    }
}
