using DAL.Data;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Functions
{
    /// <summary>
    /// Function class for UserInsight-related operations.
  /// מטפל בשמירה, שליפה ועדכון של תובנות שה-AI למד על המשתמש
    /// </summary>
    public static class UserInsightFunction
    {
        //--------------------------קבלת כל התובנות של משתמש ספציפי----------------------------
        public static List<UserInsight> GetUserInsightsByUserId(int userId)
 {
 using (AppDbContext DB = new AppDbContext())
    {
         return DB.UserInsights
          .Where(ui => ui.UserId == userId)
       .OrderByDescending(ui => ui.ConfidenceLevel) // תובנות בטוחות יותר ראשונות
  .ThenByDescending(ui => ui.LastUpdatedAt ?? ui.DiscoveredAt)
     .ToList();
            }
        }

        //--------------------------קבלת תובנות לפי קטגוריה----------------------------
        public static List<UserInsight> GetInsightsByUserAndCategory(int userId, string category)
        {
      using (AppDbContext DB = new AppDbContext())
            {
                return DB.UserInsights
  .Where(ui => ui.UserId == userId && ui.Category == category)
      .OrderByDescending(ui => ui.ConfidenceLevel)
        .ToList();
    }
        }

        //--------------------------קבלת תובנה ספציפית על פי קוד----------------------------
     public static UserInsight? GetInsightById(int id)
        {
     using (AppDbContext DB = new AppDbContext())
            {
return DB.UserInsights.FirstOrDefault(ui => ui.Id == id);
         }
    }

        //--------------------------------הוספת תובנה חדשה----------------------------------
        public static UserInsight AddNewInsight(UserInsight newInsight)
        {
       using (AppDbContext DB = new AppDbContext())
  {
       // בדיקה אם תובנה דומה כבר קיימת (מניעת כפילויות)
   var existingInsight = DB.UserInsights
      .FirstOrDefault(ui =>
     ui.UserId == newInsight.UserId &&
             ui.Category == newInsight.Category &&
      ui.InsightText.ToLower() == newInsight.InsightText.ToLower());

 if (existingInsight != null)
                {
  // עדכון תובנה קיימת במקום ליצור חדשה
           existingInsight.ConfirmationCount++;
     existingInsight.LastUpdatedAt = DateTime.UtcNow;
       existingInsight.ConfidenceLevel = Math.Min(100, existingInsight.ConfidenceLevel + 5);
 DB.SaveChanges();
              return existingInsight;
      }

    // יצירת תובנה חדשה
      newInsight.DiscoveredAt = DateTime.UtcNow;
    DB.UserInsights.Add(newInsight);
    DB.SaveChanges();
             return newInsight;
            }
        }

 //----------------------------------עדכון רמת בטחון בתובנה----------------------------------
        public static void UpdateInsightConfidence(int insightId, bool isConfirmed)
        {
       using (AppDbContext DB = new AppDbContext())
            {
                var insight = DB.UserInsights.FirstOrDefault(ui => ui.Id == insightId);
     if (insight != null)
    {
    if (isConfirmed)
       {
               insight.ConfirmationCount++;
             insight.ConfidenceLevel = Math.Min(100, insight.ConfidenceLevel + 10);
           }
          else
         {
        insight.NegationCount++;
 insight.ConfidenceLevel = Math.Max(0, insight.ConfidenceLevel - 15);
   }
       insight.LastUpdatedAt = DateTime.UtcNow;
  DB.SaveChanges();
    }
            }
        }

    //--------------------------------מחיקת תובנה בודדת----------------------------------
  public static bool DeleteInsight(int insightId)
        {
            using (AppDbContext DB = new AppDbContext())
  {
          var insightToDelete = DB.UserInsights.FirstOrDefault(ui => ui.Id == insightId);
      if (insightToDelete != null)
         {
            DB.UserInsights.Remove(insightToDelete);
  DB.SaveChanges();
        return true;
        }
         return false;
            }
        }

  //--------------------------------מחיקת כל התובנות של משתמש----------------------------------
        public static bool DeleteAllInsightsByUserId(int userId)
        {
         using (AppDbContext DB = new AppDbContext())
     {
              var insightsToDelete = DB.UserInsights
           .Where(ui => ui.UserId == userId)
   .ToList();

       if (insightsToDelete.Count > 0)
    {
        DB.UserInsights.RemoveRange(insightsToDelete);
         DB.SaveChanges();
           return true;
   }
 return false;
 }
        }

        //--------------------------------שרשור כל התובנות למחרוזת אחת לשידור ל-AI----------------------------------
     /// <summary>
     /// שרשור כל התובנות של משתמש למחרוזת אחת יפה שניתן להזריק ישירות ל-System Prompt של ה-AI
        /// </summary>
        public static string BuildUserProfilePrompt(int userId)
        {
   var insights = GetUserInsightsByUserId(userId);

            if (insights.Count == 0)
        return "אין תובנות שנאספו עדיין על המשתמש.";

     var promptBuilder = new System.Text.StringBuilder();
            promptBuilder.AppendLine("## פרופיל המשתמש:");
  promptBuilder.AppendLine();

            var groupedByCategory = insights.GroupBy(ui => ui.Category ?? "General");

            foreach (var categoryGroup in groupedByCategory)
            {
            promptBuilder.AppendLine($"### {categoryGroup.Key}:");
           foreach (var insight in categoryGroup)
       {
   var confidenceIndicator = insight.ConfidenceLevel >= 80 ? "?" :
       insight.ConfidenceLevel >= 50 ? "~" : "?";
        promptBuilder.AppendLine($"  {confidenceIndicator} {insight.InsightText}");
  }
       promptBuilder.AppendLine();
     }

            return promptBuilder.ToString();
        }
    }
}
