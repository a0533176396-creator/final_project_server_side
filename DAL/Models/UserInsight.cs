using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    /// <summary>
    /// מודל לשמירת תובנות שה-AI למד על המשתמש בהתאם לשיחות שלו
    /// קשר 1:N עם Users - משתמש בודד יכול להיות לו הרבה תובנות
 /// </summary>
    public class UserInsight
    {
        /// <summary>
        /// מזהה ייחודי של התובנה
        /// </summary>
 public int Id { get; set; }

        /// <summary>
   /// מפתח זר למשתמש (Users)
        /// </summary>
    [ForeignKey(nameof(User))]
   public int UserId { get; set; }

  /// <summary>
        /// התובנה עצמה - משפט שלם שה-AI ניסח על המשתמש
        /// למשל: "המשתמש מעדיף לבצע משימות טכניות בשעות הבוקר"
     /// או: "המשתמש בעל משפחה קטנה ותמיד צריך להיות זמין לאחר 16:00"
        /// </summary>
        [Column(TypeName = "text")]
     public string InsightText { get; set; }

        /// <summary>
     /// קטגוריית התובנה (עוזר בסיווג וניהול)
        /// למשל: "work_preference", "personality", "family_status", "learning_style"
 /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// רמת בטחון בתובנה זו (1-100)
        /// כמה בטוח ה-AI שהתובנה נכונה בהתאם למספר הפעמים שהוא ראה את הדפוס הזה
   /// </summary>
        public int ConfidenceLevel { get; set; } = 50;

        /// <summary>
        /// מתי ה-AI גילה/עדכן את התובנה הזו
        /// </summary>
      public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// מתי התובנה עודכנה לאחרונה
        /// </summary>
     public DateTime? LastUpdatedAt { get; set; }

        /// <summary>
        /// מספר הפעמים שהתובנה אושרה (אמת) בשיחות עוקבות
        /// </summary>
   public int ConfirmationCount { get; set; } = 0;

        /// <summary>
 /// מספר הפעמים שהתובנה הופרכה (הוכחה כשגויה)
        /// </summary>
     public int NegationCount { get; set; } = 0;

    /// <summary>
        /// קשר הניווט - המשתמש שלהם
        /// </summary>
        public Users? User { get; set; }
    }
}
