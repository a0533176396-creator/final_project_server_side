using DAL.Functions;
using DAL.Models;
using DTO.Mapper;
using DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Functions
{
    /// <summary>
    /// Business Logic Layer for UserInsight-related operations
 /// </summary>
    public static class UserInsightBLL
    {
     /// <summary>
      /// קבלת כל התובנות של משתמש וההמרה ל-DTO
 /// </summary>
        public static List<UserInsightDTO> GetUserInsights(int userId)
        {
      var insights = UserInsightFunction.GetUserInsightsByUserId(userId);
         var dtoList = insights
        .Select(i => AppMapper.UserInsightToDto(i))
      .ToList();
      return dtoList;
        }

     /// <summary>
        /// הוספת תובנה חדשה עבור משתמש
     /// </summary>
        public static UserInsightDTO AddUserInsight(int userId, string insightText, string? category = null)
        {
           var newInsight = new UserInsight
   {
       UserId = userId,
     InsightText = insightText,
   Category = category ?? "general",
              ConfidenceLevel = 50
        };

var savedInsight = UserInsightFunction.AddNewInsight(newInsight);
          return AppMapper.UserInsightToDto(savedInsight);
        }

  /// <summary>
       /// עדכון רמת הביטחון בתובנה בהתאם לאישור או הפרכה
     /// </summary>
       public static void ConfirmInsight(int insightId, bool isCorrect)
       {
  UserInsightFunction.UpdateInsightConfidence(insightId, isCorrect);
   }

      /// <summary>
   /// מחיקת תובנה בודדת
  /// </summary>
       public static bool DeleteInsight(int insightId)
        {
       return UserInsightFunction.DeleteInsight(insightId);
     }

        /// <summary>
        /// מחיקת כל התובנות של משתמש (למשל, כשהוא מבקש reset של הזיכרון)
    /// </summary>
  public static bool ResetUserMemory(int userId)
        {
     return UserInsightFunction.DeleteAllInsightsByUserId(userId);
        }
    }
}
